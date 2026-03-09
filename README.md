# Bepop — Music Streaming Platform

A full-stack music streaming web application built with **ASP.NET Core 8 MVC**, featuring JWT-based authentication, claim-driven package authorization, ML.NET-powered song recommendations, and real-time audio streaming.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 8 |
| Web Framework | ASP.NET Core MVC (Razor Views + REST API) |
| ORM | Entity Framework Core 8 (Code-First) |
| Database | SQL Server |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| Authorization | Custom `IAuthorizationHandler` (policy-based) |
| ML / Recommendations | ML.NET 3.0 — Matrix Factorization |
| API Docs | Swagger / OpenAPI (Swashbuckle) |
| Frontend | Bepop HTML template served as static files |

---

## Key Features

### JWT Authentication
- Users register and log in via `UsersController`.
- `JwtService` generates a signed HS256 token containing standard claims (`Name`, `Email`, `NameIdentifier`) plus custom claims: `PackageLevel` (int) and `PackageName` (string).
- Token lifetime and signing key are configured through `appsettings.json` (`Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpireInMinutes`).
- `ClockSkew` is set to `TimeSpan.Zero` for precise expiration enforcement.

### Package-Based Authorization
Content is tiered across 6 subscription packages. Authorization is enforced at the API level using a custom `IAuthorizationHandler`:

| Package | Content Level |
|---------|:---:|
| Elite | 1 |
| Premium | 2 |
| Gold | 3 |
| Standard | 4 |
| Basic | 5 |
| Free | 6 |

- `PackageAuthorizationHandler` reads the `PackageLevel` claim from the JWT and compares it against the `PackageRequirement` registered on each policy.
- A user with a lower content level number has access to all content at equal or higher level numbers (Elite can access everything; Free can only access level-6 content).
- Six named policies (`EliteContent` … `FreeContent`) are registered in `Program.cs` and applied via `[Authorize(Policy = "...")]` on controller actions.

### Audio Streaming
- Songs are streamed as `audio/mpeg` with HTTP Range support (`enableRangeProcessing: true`), enabling seek and partial-content delivery for the browser `<audio>` element.
- Because browsers cannot attach `Authorization` headers to audio src requests, the JWT is accepted from the `?t=` query string on the `/api/songs/stream/{id}` endpoint via a custom `OnMessageReceived` event in the JWT Bearer configuration.

### ML.NET Song Recommendations
- `SongRecommendationService` is registered as a **Singleton** and trains a **Matrix Factorization** model on startup using listening history data (`UserSongHistories`).
- Training input: `(UserId, SongId, PlayCount)` — play count is used as the implicit rating signal.
- Model hyperparameters: 20 iterations, rank 8, learning rate 0.1.
- Predictions are served thread-safely via a locked `PredictionEngine<SongRating, SongRatingPrediction>`.
- Graceful fallback: if fewer than 4 interaction rows exist, or the user has listened to all songs, the service returns globally popular tracks the user hasn't heard yet.
- The `/api/recommendations/train` endpoint allows on-demand retraining without restarting the application.

---

###Screenshots
1) <img width="1913" height="1038" alt="10" src="https://github.com/user-attachments/assets/643ecdac-891a-4ffc-8a03-273fa4cc9500" />
2) <img width="1915" height="1031" alt="9" src="https://github.com/user-attachments/assets/941556a4-28ef-4aac-bb47-30da63dcc3a9" />
3) <img width="1909" height="1037" alt="8" src="https://github.com/user-attachments/assets/d5ced072-fe1f-4e8e-9f6a-339e416e337c" />
4) <img width="1913" height="1038" alt="7" src="https://github.com/user-attachments/assets/9a18ef38-e22e-49bb-b122-d782bfeb8f93" />
5) <img width="1913" height="1035" alt="6" src="https://github.com/user-attachments/assets/972babf2-ce00-4fef-b355-f54ebf172091" />
6) <img width="1913" height="1039" alt="5" src="https://github.com/user-attachments/assets/afc88ebf-8b5a-410c-a79b-b756c6767d3c" />
7) <img width="1915" height="1039" alt="4" src="https://github.com/user-attachments/assets/1b4a4ede-df62-4604-a1cd-1a403f0577bc" />
8) <img width="1916" height="1037" alt="3" src="https://github.com/user-attachments/assets/9ea0964d-cc74-4a6b-8d3c-02b201c9a560" />
9) <img width="1913" height="1039" alt="2" src="https://github.com/user-attachments/assets/81839153-7d94-4093-8751-6de81cf627bf" />
10) <img width="1917" height="1038" alt="1" src="https://github.com/user-attachments/assets/73bd46e9-f6e0-425e-9e9c-ea61bcc92378" />

