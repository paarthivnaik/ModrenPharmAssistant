# Repository Profile: ModernPharmAssistant

## Executive Summary
- **Repository Name**: `ModrenPharmAssistant`
- **Topology**: Monorepo / Multi-Service (Backend API + Frontend SPA)
- **Primary Domain**: Pharmacy Management System Modernization (PharmAssistant)
- **Architectural Style**: Clean Architecture with CQRS (Backend) & Component-Driven Modular SPA (Frontend)

## 5-Layer Stack Classification

| Layer | Classification | Details | Confidence |
| :--- | :--- | :--- | :--- |
| **Layer 1: Topology** | Monorepo / Multi-Service | `PharmAPI/` (.NET 7 Web API) + `PharmaUI/` (Angular 22 SPA) | `HIGH` |
| **Layer 2: Languages** | Polyglot (C#, TypeScript) | C# 11 (.NET 7.0), TypeScript 6.0.2, HTML5/CSS3 | `HIGH` |
| **Layer 3: Frameworks** | ASP.NET Core 7 + Angular 22 | ASP.NET Core Web API, MediatR 12.0.1, EF Core 7.0.20, Angular 22.1.0 | `HIGH` |
| **Layer 4: Libraries & Tooling** | CQRS, ORM, Build, Test | MediatR, FluentValidation, EF Core SQLite, Swagger, Angular CLI, Vitest | `HIGH` |
| **Layer 5: Versions** | Runtimes & Dependencies | .NET 7.0.20 SDK, Node/NPM 11.17.0, TypeScript 6.0.2, Vitest 4.0.8 | `HIGH` |

## Subsystems Layout
- **`PharmAPI/`**: Backend REST API implementing Clean Architecture:
  - `PharmAPI.Api/`: Web API host, endpoints, middleware, authentication & Swagger documentation.
  - `PharmAPI.Application/`: Business logic, CQRS commands/queries, MediatR handlers, and FluentValidation rules.
  - `PharmAPI.Domain/`: Core domain entities (`ApplicationUser`, `ApplicationRole`), value objects, and domain abstractions.
  - `PharmAPI.Infrastructure/`: Data access via EF Core (SQLite), Identity services, and JWT token provider.
- **`PharmaUI/`**: Modern standalone Angular 22 frontend application with reactive forms, JWT interceptors, and auth route guards.
