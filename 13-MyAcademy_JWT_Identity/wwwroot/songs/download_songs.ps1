# Refresh PATH
$env:PATH = [System.Environment]::GetEnvironmentVariable("PATH", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("PATH", "User")

$placeholderSize = 8945229
$songsDir = $PSScriptRoot
$tmpDir = Join-Path $songsDir "_tmp_dl"
if (-not (Test-Path $tmpDir)) { New-Item -ItemType Directory -Path $tmpDir | Out-Null }

$songs = [ordered]@{
    "simarik"         = "Tarkan Simarik"
    "kuzukuzu"        = "Tarkan Kuzu Kuzu"
    "dudu"            = "Tarkan Dudu"
    "gulumse"         = "Sezen Aksu Gulumse"
    "hadibakalim"     = "Sezen Aksu Hadi Bakalim"
    "gesibaglari"     = "Baris Manco Gesi Baglari"
    "donence"         = "Baris Manco Donence"
    "allabeni"        = "Baris Manco Alla Beni Pulla Beni"
    "bounce"          = "Ajda Pekkan Bounce"
    "dedikodu"        = "Ajda Pekkan Dedikodu"
    "superstar"       = "Sertab Erener Superstar"
    "bambaska"        = "Sertab Erener Bambaska Biri"
    "tamirci"         = "Cem Karaca Tamirci Ciragi"
    "resimdeki"       = "Cem Karaca Resimdeki Gozyaslari"
    "istanbul"        = "Teoman Istanbulda Sonbahar"
    "papatya"         = "Teoman Papatya"
    "everyway"        = "Sertab Erener Every Way That I Can"
    "leave"           = "Hadise Leave"
    "dumtek"          = "Hadise Dum Tek Tek"
    "superman"        = "Hadise Superman"
    "kagithelva"      = "Muslum Gurses Kagit Helva"
    "affet"           = "Muslum Gurses Affet"
    "holocaust"       = "Duman Holocaust"
    "neyimvarki"      = "Duman Neyim Var Ki"
    "sendendahaguzel" = "Sagopa Kajmer Senden Daha Guzel"
    "yurekten"        = "Sagopa Kajmer Yurekten"
    "baytar"          = "Mor ve Otesi Baytar"
    "dusunki"         = "Mor ve Otesi Dusun Ki"
    "cambaz"          = "Nilufer Cambaz"
    "dunyayalan"      = "Nilufer Dunya Yalan Soyluyor"
    "dunyadonuyor"    = "Yalin Dunya Donuyor"
    "geceler"         = "Yalin Geceler"
    "ellerinesaglik"  = "Gripin Ellerine Saglik"
    "asklaftan"       = "Gripin Ask Laftan Anlamaz"
    "boylekahpe"      = "MFO Boyle Kahpedir Dunya"
    "durmayagmur"     = "MFO Durma Yagmur Durma"
    "elegunekarsi"    = "Manga Ele Gune Karsi"
    "alidesidero"     = "Manga Ali Desidero"
    "dunyaninsonu"    = "Ceza Dunyanin Sonuna Dogmusum"
    "birkadin"        = "Ceza Bir Kadin Cizeceksin"
}

$total = $songs.Count
$i = 0; $ok = 0; $fail = 0

foreach ($entry in $songs.GetEnumerator()) {
    $i++
    $filename = $entry.Key
    $searchTerm = $entry.Value
    $finalFile = Join-Path $songsDir "$filename.mp3"
    $tmpFile = Join-Path $tmpDir    "$filename.mp3"

    # Zaten gercek dosya varsa atla
    if (Test-Path $finalFile) {
        $sz = (Get-Item $finalFile).Length
        if ($sz -ne $placeholderSize) {
            Write-Host "[$i/$total] SKIP: $filename.mp3 ($([math]::Round($sz/1MB,2)) MB)"
            $ok++
            continue
        }
    }

    # Temp klasore indir (dosya kilidi sorununu atar)
    Write-Host "[$i/$total] Downloading: $searchTerm"

    # Temp'teki eski dosyayi temizle
    if (Test-Path $tmpFile) { Remove-Item $tmpFile -Force -ErrorAction SilentlyContinue }

    yt-dlp `
        --default-search "ytsearch1:" `
        --extract-audio `
        --audio-format mp3 `
        --audio-quality 5 `
        --output "$tmpFile" `
        --no-playlist `
        --quiet `
        --no-warnings `
        "$searchTerm"

    if ($LASTEXITCODE -eq 0 -and (Test-Path $tmpFile)) {
        # Orjinal placeholder dosyayi sil ve gecici dosyayi tasi
        try {
            if (Test-Path $finalFile) { Remove-Item $finalFile -Force }
            Move-Item -Path $tmpFile -Destination $finalFile -Force
            $sizeMB = [math]::Round((Get-Item $finalFile).Length / 1MB, 2)
            Write-Host "   OK - $sizeMB MB"
            $ok++
        }
        catch {
            Write-Host "   FAILED (move error): $_"
            $fail++
        }
    }
    else {
        Write-Host "   FAILED: $searchTerm"
        $fail++
    }

    Start-Sleep -Milliseconds 600
}

# Temizlik
if (Test-Path $tmpDir) {
    Get-ChildItem $tmpDir | Remove-Item -Force -ErrorAction SilentlyContinue
    Remove-Item $tmpDir -Force -ErrorAction SilentlyContinue
}

Write-Host ""
Write-Host "DONE: $ok OK, $fail FAILED (total $i)"
