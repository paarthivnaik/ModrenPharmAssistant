# Physical-to-Logical Layer Mapping

| Physical Directory | Logical Architectural Tier | Responsibilities & Contents | Key Dependencies |
| :--- | :--- | :--- | :--- |
| **`PharmaUI/src/app/features/`** | Presentation (UI Views) | Feature views, Angular standalone components, UI forms, CSS styles (Login, Register). | `@angular/core`, `@angular/forms`, `rxjs` |
| **`PharmaUI/src/app/core/`** | UI Core & Infrastructure | HTTP interceptors (`jwt.interceptor.ts`), route guards (`auth.guard.ts`), services (`auth.service.ts`), models. | `@angular/router`, `HttpClient` |
| **`PharmAPI/PharmAPI.Api/Controllers/`** | Presentation (REST API) | HTTP endpoints (`AuthController.cs`), route bindings, HTTP status mappings, OpenAPI documentation. | `MediatR`, `Microsoft.AspNetCore.Mvc` |
| **`PharmAPI/PharmAPI.Api/` (Program.cs)** | Application Host / Composition Root | Service registration, middleware pipeline, CORS policies, JWT configuration, DB seeding. | ASP.NET Core Web Host |
| **`PharmAPI/PharmAPI.Application/Features/`** | Application / Business Logic (CQRS) | Commands, Queries, Request Handlers (`LoginCommandHandler`, `RegisterCommandHandler`), Validators, DTOs. | `MediatR`, `FluentValidation` |
| **`PharmAPI/PharmAPI.Application/Common/`** | Application Abstractions | Service interfaces (`IIdentityService`, `IJwtTokenService`), shared behaviors, mapping contracts. | .NET BCL |
| **`PharmAPI/PharmAPI.Domain/Entities/`** | Domain Model | Core business entities (`ApplicationUser`, `ApplicationRole`), value objects, invariants. | IdentityCore |
| **`PharmAPI/PharmAPI.Infrastructure/Identity/`** | Infrastructure (Security) | Identity implementations (`IdentityService`, `JwtTokenService`), token generation and validation. | `System.IdentityModel.Tokens.Jwt` |
| **`PharmAPI/PharmAPI.Infrastructure/Persistence/`** | Infrastructure (Data Access) | EF Core `ApplicationDbContext`, entity configurations, initial database seeding (`ApplicationDbContextSeed`). | `Microsoft.EntityFrameworkCore.Sqlite` |
