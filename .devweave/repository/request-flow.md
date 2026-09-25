# Concrete End-to-End Request Flow

## Request Flow Sequence

```mermaid
sequenceDiagram
    autonumber
    actor User as User / Browser
    participant UI as PharmaUI (Angular)
    participant Interceptor as JwtInterceptor
    participant API as AuthController (PharmAPI.Api)
    participant MediatR as MediatR Pipeline
    participant Handler as LoginCommandHandler
    participant Identity as IdentityService / UserManager
    participant TokenSvc as JwtTokenService
    participant DB as SQLite (ApplicationDbContext)

    User->>UI: Submit Login Form (Email, Password)
    UI->>Interceptor: HTTP POST /api/auth/login
    Interceptor->>API: Forward HTTP Request with Headers
    API->>MediatR: Send(LoginCommand)
    MediatR->>Handler: Handle(LoginCommand, CancellationToken)
    Handler->>Identity: AuthenticateAsync(Email, Password)
    Identity->>DB: FindByEmailAsync & VerifyPassword
    DB-->>Identity: Return ApplicationUser / Roles
    Identity-->>Handler: Return Success / User Details
    Handler->>TokenSvc: GenerateJwtTokenAsync(ApplicationUser)
    TokenSvc-->>Handler: Return JWT Token & Expiration
    Handler-->>MediatR: Return LoginResponseDto
    MediatR-->>API: Return Result DTO
    API-->>Interceptor: HTTP 200 OK (JSON Payload)
    Interceptor-->>UI: Deliver Response to Component
    UI-->>User: Store Token & Navigate to Dashboard
```

## Detailed Pipeline Phases
1. **Frontend Initiation**:
   - `LoginComponent` captures credentials via reactive form `FormGroup`.
   - Dispatches call to `AuthService.login()`.
2. **HTTP Pipeline & Middleware**:
   - ASP.NET Core receives HTTP request.
   - `UseCors("AllowAngularApp")` verifies allowed origins (`http://localhost:4200`).
   - `UseAuthentication()` and `UseAuthorization()` evaluate security policies.
3. **Controller to Mediator**:
   - `AuthController.Login([FromBody] LoginCommand command)` delegates execution to MediatR pipeline via `ISender.Send(command)`.
4. **Application CQRS Execution**:
   - `LoginCommandHandler` processes the command.
   - Invokes `IIdentityService` abstraction.
5. **Infrastructure & Persistence**:
   - `IdentityService` interacts with `UserManager<ApplicationUser>` backed by `ApplicationDbContext` (SQLite).
   - Generates signed JWT security tokens using `IJwtTokenService`.
6. **Response Serialization**:
   - Result wrapped in `LoginResponseDto` returned through HTTP 200 OK JSON response.
