# Modernization Implementation Plan: Work Item #100

**Work Item**: [Step 03 - US-SEC-03: Self-Service Password Recovery & Email Reset](https://balajinaik.visualstudio.com/5976a5b1-4d57-4ed1-870d-4370825e9b67/_apis/wit/workItems/100)  
**Phase**: `PLAN`  
**Gate**: `HARD GATE #2 (Pending Approval)`  

---

## 1. Plan Overview & Objectives

Decompose approved architectural analysis into file-anchored implementation tasks across ASP.NET Core 7.0 Web API (`PharmAPI`) and Angular 22 SPA (`PharmaUI`):
1. **CQRS Command Pipeline**: Implement `ForgotPasswordCommand` and `ResetPasswordCommand` with MediatR and FluentValidation.
2. **Identity & Security Integration**: Support cryptographic token generation, 24-hour expiration via `DataProtectionTokenProviderOptions`, password hashing updates, and `SecurityStamp` renewal.
3. **Pluggable Email Infrastructure**: Implement `IEmailService` in `PharmAPI.Infrastructure` for password reset link delivery.
4. **Angular Standalone UI**: Create standalone `ForgotPasswordComponent` and `ResetPasswordComponent` with reactive forms and URL query parameter binding.

---

## 2. File-Anchored Implementation Breakdown

### Task 1: Backend Application Layer Abstractions & DTOs
- **Files to Create / Modify**:
  - `PharmAPI.Application/Common/Interfaces/IEmailService.cs` *(Create)*:
    ```csharp
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken cancellationToken = default);
    }
    ```
  - `PharmAPI.Application/Common/Interfaces/IIdentityService.cs` *(Modify)*:
    - Add `Task<string?> GeneratePasswordResetTokenAsync(ApplicationUser user);`
    - Add `Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);`
  - `PharmAPI.Application/Features/Auth/DTOs/ForgotPasswordResponseDto.cs` *(Create)*
  - `PharmAPI.Application/Features/Auth/DTOs/ResetPasswordResponseDto.cs` *(Create)*

---

### Task 2: Backend CQRS Commands, Handlers & Validators
- **Files to Create**:
  - `PharmAPI.Application/Features/Auth/Commands/ForgotPassword/ForgotPasswordCommand.cs`
  - `PharmAPI.Application/Features/Auth/Commands/ForgotPassword/ForgotPasswordCommandHandler.cs`
    - *Anti-enumeration behavior*: If user does not exist or is inactive, log silently and return success response.
    - *Token generation*: Call `IIdentityService.GeneratePasswordResetTokenAsync`.
    - *Email dispatch*: Call `IEmailService.SendPasswordResetEmailAsync`.
  - `PharmAPI.Application/Features/Auth/Commands/ForgotPassword/ForgotPasswordCommandValidator.cs`
    - FluentValidation rule: `Email` must be not empty and valid email format.
  - `PharmAPI.Application/Features/Auth/Commands/ResetPassword/ResetPasswordCommand.cs`
  - `PharmAPI.Application/Features/Auth/Commands/ResetPassword/ResetPasswordCommandHandler.cs`
    - Call `IIdentityService.ResetPasswordAsync`.
    - Enforce security stamp refresh on successful reset.
  - `PharmAPI.Application/Features/Auth/Commands/ResetPassword/ResetPasswordCommandValidator.cs`
    - FluentValidation rules: `Email` valid format, `Token` not empty, `NewPassword` matches complexity constraints (min 8 chars, uppercase, lowercase, digit, special character).

---

### Task 3: Backend Infrastructure Implementation & DI Configuration
- **Files to Create / Modify**:
  - `PharmAPI.Infrastructure/Services/EmailService.cs` *(Create)*:
    - Concrete implementation of `IEmailService` logging formatted reset links / dispatching emails.
  - `PharmAPI.Infrastructure/Identity/IdentityService.cs` *(Modify)*:
    - Implement `GeneratePasswordResetTokenAsync` using `UserManager<ApplicationUser>.GeneratePasswordResetTokenAsync`.
    - Implement `ResetPasswordAsync` using `UserManager<ApplicationUser>.ResetPasswordAsync` and `UserManager<ApplicationUser>.UpdateSecurityStampAsync`.
  - `PharmAPI.Infrastructure/DependencyInjection.cs` *(Modify)*:
    - Register `IEmailService` -> `EmailService` as scoped/transient.
    - Configure `DataProtectionTokenProviderOptions.TokenLifespan = TimeSpan.FromHours(24)`.

---

### Task 4: Backend Web API Presentation Endpoints
- **Files to Modify**:
  - `PharmAPI.Api/Controllers/AuthController.cs`:
    - Add `[HttpPost("forgot-password")]` accepting `ForgotPasswordCommand` and returning `ActionResult<ForgotPasswordResponseDto>`.
    - Add `[HttpPost("reset-password")]` accepting `ResetPasswordCommand` and returning `ActionResult<ResetPasswordResponseDto>`.

---

### Task 5: Frontend Core Services & Models
- **Files to Modify**:
  - `PharmaUI/src/app/core/models/auth.models.ts`:
    - Add `ForgotPasswordRequest`, `ForgotPasswordResponse`, `ResetPasswordRequest`, `ResetPasswordResponse`.
  - `PharmaUI/src/app/core/services/auth.service.ts`:
    - Add `forgotPassword(request: ForgotPasswordRequest): Observable<ForgotPasswordResponse>`
    - Add `resetPassword(request: ResetPasswordRequest): Observable<ResetPasswordResponse>`

---

### Task 6: Frontend Standalone Components & Routing
- **Files to Create / Modify**:
  - `PharmaUI/src/app/features/auth/forgot-password/forgot-password.component.ts` *(Create)*
  - `PharmaUI/src/app/features/auth/forgot-password/forgot-password.component.html` *(Create)*
  - `PharmaUI/src/app/features/auth/forgot-password/forgot-password.component.css` *(Create)*
  - `PharmaUI/src/app/features/auth/reset-password/reset-password.component.ts` *(Create)*
  - `PharmaUI/src/app/features/auth/reset-password/reset-password.component.html` *(Create)*
  - `PharmaUI/src/app/features/auth/reset-password/reset-password.component.css` *(Create)*
  - `PharmaUI/src/app/app.routes.ts` *(Modify)*:
    - Add `{ path: 'forgot-password', loadComponent: () => import('./features/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent) }`
    - Add `{ path: 'reset-password', loadComponent: () => import('./features/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) }`

---

## 3. Database Migration Strategy
- No schema alteration required. ASP.NET Core Identity's standard `AspNetUsers` schema contains `PasswordHash`, `SecurityStamp`, and token handling primitives.
- Token lifespan configuration is handled at runtime via DI configuration (`DataProtectionTokenProviderOptions`).

---

## 4. Test Specifications & Verification Commands

| Scope | Test Description | Verification Target |
| :--- | :--- | :--- |
| **Unit Test** | `ForgotPasswordCommandHandler` sends email if user exists | `IEmailService.SendPasswordResetEmailAsync` invoked |
| **Unit Test** | `ForgotPasswordCommandHandler` returns generic success if user does not exist | No enumeration / silent success |
| **Unit Test** | `ResetPasswordCommandHandler` successfully resets password | `IIdentityService.ResetPasswordAsync` succeeds |
| **Unit Test** | `ResetPasswordCommandValidator` validates password complexity | FluentValidation failure on weak password |
| **E2E / Build** | Backend compilation and test execution | `dotnet test PharmAPI/PharmAPI.sln` |
| **UI Build** | Frontend compilation | `ng build` in `PharmaUI` |

---

## 5. Verification Commands
- **Backend Build**: `dotnet build PharmAPI/PharmAPI.sln`
- **Backend Tests**: `dotnet test PharmAPI/PharmAPI.sln`
- **Frontend Typecheck & Build**: `npm run build` inside `PharmaUI`
