# Integrations & External Dependencies

## Database Persistence
- **Database Engine**: SQLite 3
- **Data Source**: `Data Source=pharmassistant.db`
- **ORM**: Entity Framework Core 7.0 (Code-First / Auto Migration `EnsureCreatedAsync()`)
- **Seeded Entities**: Default Roles (`Admin`, `Pharmacist`, `Staff`), Default Admin User (`admin@pharmassistant.com`)

## Security & Identity
- **Authentication**: ASP.NET Core Identity with JWT Bearer Token validation
- **Token Issuer**: `PharmAssistantApi`
- **Token Audience**: `PharmAssistantClient`
- **Token Expiry**: 60 minutes
- **Default Hash Algorithm**: HMAC-SHA256

## HTTP & Cross-Origin Resource Sharing (CORS)
- **Allowed Frontend Origin**: `http://localhost:4200`
- **CORS Policy Name**: `AllowAngularApp`
- **Supported Methods**: Any HTTP method, headers, with credentials enabled
