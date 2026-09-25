# Frameworks & Libraries

## Backend Frameworks (PharmAPI)

### Web & API Framework
- **Framework**: ASP.NET Core 7.0 Web API
- **Routing & Controllers**: ASP.NET Core MVC Controllers (`ApiControllerBase`)
- **Documentation**: Swashbuckle Swagger UI (`Swashbuckle.AspNetCore 6.5.0`, `Microsoft.AspNetCore.OpenApi 7.0.20`)
- **Evidence**: `PharmAPI/PharmAPI.Api/Program.cs`, `PharmAPI/PharmAPI.Api/PharmAPI.Api.csproj`

### Messaging & CQRS
- **Mediator**: `MediatR 12.0.1`
- **Validation**: `FluentValidation 11.9.0` with Dependency Injection extensions
- **Evidence**: `PharmAPI/PharmAPI.Application/PharmAPI.Application.csproj`, `PharmAPI/PharmAPI.Application/DependencyInjection.cs`

### Data Access & ORM
- **ORM**: Entity Framework Core 7.0.20 (`Microsoft.EntityFrameworkCore.Sqlite`)
- **Identity Framework**: `Microsoft.AspNetCore.Identity.EntityFrameworkCore 7.0.20`
- **Security & Token**: `Microsoft.AspNetCore.Authentication.JwtBearer 7.0.20`, `System.IdentityModel.Tokens.Jwt 7.0.3`
- **Evidence**: `PharmAPI/PharmAPI.Infrastructure/PharmAPI.Infrastructure.csproj`, `PharmAPI/PharmAPI.Infrastructure/Persistence/ApplicationDbContext.cs`

---

## Frontend Frameworks (PharmaUI)

### UI Framework
- **Framework**: Angular 22.1.0 (`@angular/core`, `@angular/common`, `@angular/router`, `@angular/forms`)
- **Architecture**: Standalone Components, Reactive Forms, Dependency Injection
- **Reactivity & Async**: RxJS `~7.8.0`
- **Evidence**: `PharmaUI/package.json`, `PharmaUI/src/app/app.config.ts`, `PharmaUI/src/app/app.routes.ts`

### Frontend Build & Tooling
- **Build System**: `@angular/build 22.1.8`, `@angular/cli 22.1.8`
- **Code Formatting**: `prettier 3.8.1`
- **Evidence**: `PharmaUI/package.json`, `PharmaUI/angular.json`
