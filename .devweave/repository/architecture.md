# Architecture Topology & Component Overview

## High-Level Topology

```mermaid
graph TD
    subgraph Frontend["PharmaUI (Angular 22 SPA)"]
        UI_Components["Standalone Components<br/>(Login, Register, Dashboard)"]
        UI_Services["AuthService / API Services"]
        UI_Guards["AuthGuard / Interceptors"]
        UI_Components --> UI_Services
        UI_Services --> UI_Guards
    end

    subgraph Backend["PharmAPI (.NET 7 Clean Architecture)"]
        API_Layer["PharmAPI.Api<br/>Controllers, Program.cs, Middleware, CORS, Swagger"]
        App_Layer["PharmAPI.Application<br/>Commands, Queries, MediatR Handlers, DTOs, FluentValidation"]
        Domain_Layer["PharmAPI.Domain<br/>Entities (ApplicationUser, ApplicationRole), Interfaces"]
        Infra_Layer["PharmAPI.Infrastructure<br/>ApplicationDbContext (EF Core), IdentityService, JwtTokenService"]

        API_Layer --> App_Layer
        API_Layer --> Infra_Layer
        App_Layer --> Domain_Layer
        Infra_Layer --> App_Layer
        Infra_Layer --> Domain_Layer
    end

    subgraph DataStore["Persistence"]
        SQLite_DB[("SQLite Database<br/>(pharmassistant.db)")]
        Infra_Layer --> SQLite_DB
    end

    UI_Services -->|HTTP / JSON (Port 5000/7000)| API_Layer
```

## Architectural Principles
1. **Clean Architecture Separation**:
   - `Domain` has zero external library dependencies (only Core Identity models).
   - `Application` depends only on `Domain` and core abstractions (`IIdentityService`, `IJwtTokenService`, `MediatR`, `FluentValidation`).
   - `Infrastructure` implements abstractions with concrete technologies (EF Core SQLite, ASP.NET Identity).
   - `Api` acts as composition root, hosting controllers and configuring HTTP middleware pipelines.
2. **CQRS with MediatR**:
   - Commands and queries are isolated into distinct feature folders with handlers, validators, and DTOs.
3. **Stateless Security**:
   - Authentication is performed via JSON Web Tokens (JWT) signed with secret keys, verified on subsequent requests via `JwtBearerHandler` and frontend `jwt.interceptor.ts`.
