# Pull Request: Step 03 - US-SEC-03: Self-Service Password Recovery & Email Reset

## 🎯 Modernization Overview & Intent
This PR modernizes the legacy monolithic ASP.NET MVC password recovery workflows from `AccountController.cs` (`ForgotPassword` and `ResetPassword` actions) and legacy Razor views into a decoupled, modern multi-service architecture:
1. **Backend**: ASP.NET Core 7.0 Web API following **Clean Architecture** and the **CQRS Pattern** via MediatR with FluentValidation and ASP.NET Core Identity token providers.
2. **Frontend**: Angular 22 SPA with **AdminLTE Skin-Blue application layout parity**, standalone components, reactive forms, query token binding, and full route navigation.

---

## 🛠️ Summary of Changes

### 1. Backend (`PharmAPI`)
- **Application Layer**:
  - `IEmailService`: Decoupled abstraction for transactional password recovery email dispatch.
  - `IIdentityService`: Added `GeneratePasswordResetTokenAsync` and `ResetPasswordAsync` with security stamp invalidation.
  - `ForgotPasswordCommand`, `ForgotPasswordCommandHandler`, `ForgotPasswordCommandValidator`: CQRS command with anti-enumeration protection (returns generic confirmation even for non-existent users).
  - `ResetPasswordCommand`, `ResetPasswordCommandHandler`, `ResetPasswordCommandValidator`: CQRS command enforcing password complexity rules and rotating security stamp.
  - `ForgotPasswordResponseDto`, `ResetPasswordResponseDto`: Response DTOs.
- **Infrastructure Layer**:
  - `EmailService`: Concrete `IEmailService` implementation with structured email logging.
  - `IdentityService`: Concrete token generation and password reset implementations.
  - `DependencyInjection.cs`: Registered `IEmailService` and configured `DataProtectionTokenProviderOptions.TokenLifespan` to 24 hours.
- **API Presentation Layer**:
  - `AuthController.cs`: Exposed `POST /api/auth/forgot-password` and `POST /api/auth/reset-password` endpoints.
- **Unit Testing**:
  - `PharmAPI.Application.UnitTests`: 4 xUnit tests verifying command handlers, validators, and anti-enumeration behavior.

### 2. Frontend (`PharmaUI`)
- **Models & Services**:
  - `auth.models.ts`: Added request/response interfaces for password reset.
  - `auth.service.ts`: Added `forgotPassword` and `resetPassword` observable methods.
- **Components & Layout**:
  - `MainLayoutComponent`: Full AdminLTE Skin-Blue application shell with collapsible sidebar, notifications badge, user profile greeting, and footer.
  - `ForgotPasswordComponent`: Standalone reactive form for requesting reset link.
  - `ResetPasswordComponent`: Standalone reactive form extracting query parameters (`email`, `token`), validating password matching and submitting updates.
  - `DashboardComponent`: Operational dashboard with stats widgets and quick action buttons.
  - `PlaceholderFeatureComponent`: Routed placeholder interface for all legacy navigation routes.
  - `app.routes.ts`: Configured auth routes and protected shell routes with `authGuard`.

---

## 🧪 Verification & Test Results
- **Backend Tests**: `dotnet test PharmAPI\PharmAPI.sln` -> **4 Passed, 0 Failed (100%)**
- **Frontend Build**: `npm run build` -> **0 Errors, 0 Warnings**
- **Security Validation**:
  - Anti-enumeration protection confirmed.
  - 24-hour token expiration verified.
  - Session revocation via `UpdateSecurityStampAsync` confirmed.
- **UI Layout Parity**: Legacy AdminLTE layout verified against `_LayoutAdminLte.cshtml`.

---

## 📋 Migration Mappings
- `AccountController.ForgotPassword` -> `AuthController (POST /api/auth/forgot-password)` (`TRANSFORMED_TO`)
- `AccountController.ResetPassword` -> `AuthController (POST /api/auth/reset-password)` (`TRANSFORMED_TO`)
- `ForgotPasswordViewModel` -> `ForgotPasswordCommand.cs` (`SPLIT_INTO`)
- `ResetPasswordViewModel` -> `ResetPasswordCommand.cs` (`SPLIT_INTO`)
- `IdentityConfig.cs (EmailService)` -> `IEmailService.cs` + `EmailService.cs` (`MIGRATED_TO`)
- `Views/Account/ForgotPassword.cshtml` -> `ForgotPasswordComponent` (`REPLACED_BY`)
- `Views/Account/ResetPassword.cshtml` -> `ResetPasswordComponent` (`REPLACED_BY`)
