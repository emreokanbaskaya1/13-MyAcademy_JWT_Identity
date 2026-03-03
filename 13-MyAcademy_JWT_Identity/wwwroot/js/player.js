/**
 * Bepop Player — Package-gated song playback
 *
 * Handles the "Play Song" flow:
 *   1. User clicks a play button → playSong(id) called
 *   2. POST /api/songs/play/{id} with JWT
 *   3. 200 → audio playback via stream URL + custom player bar
 *   4. 401 → redirect to login
 *   5. 403 + PACKAGE_UPGRADE_REQUIRED → SweetAlert upgrade popup
 */
(function () {
    'use strict';

    var API_BASE = '/api/songs';

    /* ── Play Icon Injector ────────────────────────────────────────────── */
    var PLAY_SVG =
        '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" ' +
        'fill="currentColor" style="color:#6366f1;pointer-events:none;">' +
        '<polygon points="5,3 19,12 5,21"/>' +
        '</svg>';

    function injectPlayIcons() {
        document.querySelectorAll('.btn-play').forEach(function (btn) {
            if (!btn.querySelector('svg')) {
                btn.innerHTML = PLAY_SVG;
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', injectPlayIcons);
    } else {
        injectPlayIcons();
    }

    if (window.MutationObserver) {
        new MutationObserver(function (mutations) {
            if (mutations.some(function (m) { return m.addedNodes.length > 0; })) {
                injectPlayIcons();
            }
        }).observe(document.body, { childList: true, subtree: true });
    }

    document.addEventListener('pjaxEnd', injectPlayIcons);

    /* ── Custom Player Bar ─────────────────────────────────────────────── */
    var playerBar   = null;
    var audio       = null;
    var playBtn     = null;
    var iconPlay    = null;
    var iconPause   = null;
    var progressFill= null;
    var progressWrap= null;
    var currentEl   = null;
    var durationEl  = null;
    var volumeInput = null;
    var coverEl     = null;
    var titleEl     = null;
    var artistEl    = null;

    function fmt(sec) {
        sec = Math.floor(sec || 0);
        return Math.floor(sec / 60) + ':' + ('0' + (sec % 60)).slice(-2);
    }

    function initPlayerBar() {
        playerBar    = document.getElementById('bepop-player-bar');
        audio        = document.getElementById('bp-audio');
        playBtn      = document.getElementById('bp-playpause');
        iconPlay     = document.getElementById('bp-icon-play');
        iconPause    = document.getElementById('bp-icon-pause');
        progressFill = document.getElementById('bp-progress-fill');
        progressWrap = document.getElementById('bp-progress-wrap');
        currentEl    = document.getElementById('bp-current');
        durationEl   = document.getElementById('bp-duration');
        volumeInput  = document.getElementById('bp-volume');
        coverEl      = document.getElementById('bp-cover');
        titleEl      = document.getElementById('bp-title');
        artistEl     = document.getElementById('bp-artist');

        if (!audio) return;

        // Play / Pause button
        playBtn && playBtn.addEventListener('click', function () {
            if (audio.paused) { audio.play(); } else { audio.pause(); }
        });

        // Sync icons with audio state
        audio.addEventListener('play', function () {
            if (iconPlay)  iconPlay.style.display  = 'none';
            if (iconPause) iconPause.style.display = '';
        });
        audio.addEventListener('pause', function () {
            if (iconPlay)  iconPlay.style.display  = '';
            if (iconPause) iconPause.style.display = 'none';
        });

        // Time update → progress bar + current time
        audio.addEventListener('timeupdate', function () {
            if (!audio.duration) return;
            var pct = (audio.currentTime / audio.duration) * 100;
            if (progressFill) progressFill.style.width = pct + '%';
            if (currentEl)    currentEl.textContent    = fmt(audio.currentTime);
        });

        // Duration loaded
        audio.addEventListener('loadedmetadata', function () {
            if (durationEl) durationEl.textContent = fmt(audio.duration);
        });

        // Seek on progress bar click
        progressWrap && progressWrap.addEventListener('click', function (e) {
            if (!audio.duration) return;
            var rect = progressWrap.getBoundingClientRect();
            audio.currentTime = ((e.clientX - rect.left) / rect.width) * audio.duration;
        });

        // Volume
        volumeInput && volumeInput.addEventListener('input', function () {
            audio.volume = parseFloat(volumeInput.value);
        });

        // Prev / Next (single-song player — restart / no-op)
        var prevBtn = document.getElementById('bp-prev');
        var nextBtn = document.getElementById('bp-next');
        prevBtn && prevBtn.addEventListener('click', function () { audio.currentTime = 0; });
        nextBtn && nextBtn.addEventListener('click', function () { /* playlist not implemented */ });
    }

    document.addEventListener('DOMContentLoaded', initPlayerBar);

    /* ── playSong ─────────────────────────────────────────────────────── */
    window.playSong = function (songId) {
        var token = localStorage.getItem('jwtToken');

        if (!token) {
            Swal.fire({
                icon: 'warning',
                title: 'Giriş Yapın',
                text: 'Şarkı dinlemek için giriş yapmanız gerekiyor.',
                confirmButtonText: 'Giriş Yap',
                confirmButtonColor: '#6366f1',
                background: '#1a1a2e',
                color: '#e0e0e0',
                showCancelButton: true,
                cancelButtonText: 'İptal',
                cancelButtonColor: '#374151'
            }).then(function (r) {
                if (r.isConfirmed) window.location.href = '/Home/SignIn';
            });
            return;
        }

        fetch(API_BASE + '/play/' + songId, {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json'
            }
        })
        .then(function (response) {
            if (response.ok) {
                return response.json().then(function (data) {
                    try {
                        startPlayback(data, token);
                        recordHistory(songId, token);
                    } catch (e) {
                        console.error('Playback error:', e);
                    }
                });
            }

            if (response.status === 401) {
                localStorage.removeItem('jwtToken');
                Swal.fire({
                    icon: 'warning',
                    title: 'Oturum Süresi Doldu',
                    text: 'Lütfen tekrar giriş yapın.',
                    confirmButtonText: 'Giriş Yap',
                    confirmButtonColor: '#6366f1',
                    background: '#1a1a2e',
                    color: '#e0e0e0'
                }).then(function () { window.location.href = '/Home/SignIn'; });
                return;
            }

            if (response.status === 403) {
                return response.json().then(function (data) {
                    if (data.error === 'PACKAGE_UPGRADE_REQUIRED') {
                        showUpgradeAlert(data);
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Erişim Engellendi',
                            text: 'Bu içeriğe erişim izniniz yok.',
                            confirmButtonColor: '#6366f1',
                            background: '#1a1a2e',
                            color: '#e0e0e0'
                        });
                    }
                });
            }

            Swal.fire({
                icon: 'error',
                title: 'Hata',
                text: 'Bir hata oluştu. Lütfen tekrar deneyin.',
                confirmButtonColor: '#6366f1',
                background: '#1a1a2e',
                color: '#e0e0e0'
            });
        })
        .catch(function (err) {
            console.error('Play request failed:', err);
            Swal.fire({
                icon: 'error',
                title: 'Bağlantı Hatası',
                text: 'Sunucuya bağlanılamadı. (' + (err.message || err) + ')',
                confirmButtonColor: '#6366f1',
                background: '#1a1a2e',
                color: '#e0e0e0'
            });
        });
    };

    /* ── showUpgradeAlert ─────────────────────────────────────────────── */
    function showUpgradeAlert(data) {
        Swal.fire({
            icon: 'info',
            title: 'Paket Yükseltme Gerekli',
            html:
                '<div style="text-align:center;">' +
                '<p style="margin-bottom:12px;">' + (data.message || 'Please upgrade your package') + '</p>' +
                '<div style="display:flex;justify-content:center;gap:20px;margin-top:16px;">' +
                '<div style="padding:10px 16px;border-radius:8px;background:rgba(255,255,255,0.06);border:1px solid rgba(255,255,255,0.1);">' +
                '<div style="font-size:0.7rem;color:#9ca3af;margin-bottom:4px;">Mevcut Paket</div>' +
                '<div style="font-weight:600;color:#f87171;">' + (data.currentPackage || '—') + '</div>' +
                '</div>' +
                '<div style="display:flex;align-items:center;color:#6b7280;font-size:1.2rem;">→</div>' +
                '<div style="padding:10px 16px;border-radius:8px;background:rgba(99,102,241,0.1);border:1px solid rgba(99,102,241,0.3);">' +
                '<div style="font-size:0.7rem;color:#9ca3af;margin-bottom:4px;">Gerekli Paket</div>' +
                '<div style="font-weight:600;color:#818cf8;">' + (data.requiredPackage || '—') + '</div>' +
                '</div>' +
                '</div>' +
                '</div>',
            confirmButtonText: 'Paketleri İncele',
            confirmButtonColor: '#6366f1',
            showCancelButton: true,
            cancelButtonText: 'Kapat',
            cancelButtonColor: '#374151',
            background: '#1a1a2e',
            color: '#e0e0e0'
        }).then(function (r) {
            if (r.isConfirmed) window.location.href = '/Home/Packages';
        });
    }

    /* ── startPlayback ────────────────────────────────────────────────── */
    function startPlayback(data, token) {
        var streamUrl = data.streamUrl;
        var song = data.song;

        if (!streamUrl || !song) {
            console.error('startPlayback: missing streamUrl or song', data);
            return;
        }

        // Ensure player bar is initialized (covers PJAX re-renders)
        if (!audio) initPlayerBar();
        if (!audio) return;

        // Load new source
        audio.src = streamUrl + '?t=' + encodeURIComponent(token);
        audio.load();
        audio.play().catch(function (e) { console.warn('Autoplay blocked:', e); });

        // Update metadata in bar
        if (titleEl)  titleEl.textContent  = song.title      || '';
        if (artistEl) artistEl.textContent = song.artistName || '';

        // Cover: use artist image or album cover or fallback
        if (coverEl) {
            var img = song.artistImageUrl || song.coverImageUrl || '/bepop/assets/img/c0.jpg';
            coverEl.style.backgroundImage = 'url("' + img + '")';
        }

        // Reset progress
        if (progressFill) progressFill.style.width = '0%';
        if (currentEl)    currentEl.textContent    = '0:00';
        if (durationEl)   durationEl.textContent   = '0:00';

        // Show bar
        if (playerBar) playerBar.style.display = 'flex';

        // Toast
        if (typeof Swal !== 'undefined') {
            Swal.mixin({
                toast: true,
                position: 'bottom-end',
                showConfirmButton: false,
                timer: 2500,
                timerProgressBar: true,
                background: '#1a1a2e',
                color: '#e0e0e0'
            }).fire({ icon: 'success', title: '\u25B6 ' + (song.title || 'Şarkı') + ' çalınıyor...' });
        }
    }

    /* ── recordHistory ────────────────────────────────────────────────── */
    function recordHistory(songId, token) {
        fetch('/api/usersonghistories', {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ songId: songId })
        }).catch(function (err) { console.warn('History record failed:', err); });
    }

})();
