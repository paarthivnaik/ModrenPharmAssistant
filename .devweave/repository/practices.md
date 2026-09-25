# Technology Practices & Coding Standards

## Target Stack: .NET 7 (Clean Architecture + CQRS) & Angular 22

### 1. CQRS & MediatR Pattern Standards
- Commands and Queries must be strictly segregated.
- Each Command/Query should reside in its own dedicated directory alongside its Handler, Validator (`AbstractValidator<T>`), and Response DTO.
- Handlers must interact with infrastructure exclusively through Application interfaces (`IIdentityService`, `IApplicationDbContext`).

### 2. Dependency Injection & Service Registration
- Each layer must expose an extension method on `IServiceCollection` (e.g., `AddApplicationServices()`, `AddInfrastructureServices()`) in `DependencyInjection.cs`.
- Avoid injecting DbContext directly into controllers or presentation layer.

### 3. Frontend Architecture (Angular 22 Standalone)
- Use standalone components (`imports: [...]`, `standalone: true`).
- Enforce strict typing in forms with `FormBuilder` / `FormGroup`.
- Protect routes with functional/class route guards (`canActivate: [authGuard]`).
- Attach authorization tokens automatically via `HttpInterceptorFn` or `HttpInterceptor`.

### 4. Code Style & Conventions
- **C#**: PascalCase for class, method, property names; camelCase with `_` prefix for private fields (e.g. `_userManager`).
- **TypeScript**: camelCase for functions/variables, PascalCase for classes/interfaces, kebab-case for component file names.
- **Zero Raw Secrets**: Connection strings, secrets, and keys must be managed via configuration or environment variables, never committed directly in plain text in production.
