# Modernization Technical Analysis: Work Item #100

**Work Item**: [Step 03 - US-SEC-03: Self-Service Password Recovery & Email Reset](https://balajinaik.visualstudio.com/5976a5b1-4d57-4ed1-870d-4370825e9b67/_apis/wit/workItems/100)  
**Phase**: `ANALYZE`  
**Gate**: `HARD GATE #1 (Pending Approval)`  

---

## 1. Executive Summary & Objective

Modernize legacy monolithic password recovery from [`AccountController.cs`](file:///D:/PharmAssistant/PharmAssistant/FYPPharmAssistant/Controllers/Account/AccountController.cs) into:
1. **Backend**: ASP.NET Core 7.0 Web API with CQRS commands (`ForgotPasswordCommand`, `ResetPasswordCommand`), FluentValidation, ASP.NET Core Identity token providers with 24-hour expiration, and modern `IEmailService`.
2. **Frontend**: Angular 22 Standalone Components (`ForgotPasswordComponent`, `ResetPasswordComponent`) with reactive form validation and password confirmation.

---

## 2. Behavioral & Business Rule Analysis

### 2.1 Legacy Behavioral Findings
- **Account Existence Protection**: Legacy implementation silently redirects to `ForgotPasswordConfirmation` even when an email does not exist or is not confirmed. This prevents account enumeration attacks.
- **Token Generation**: Uses ASP.NET Identity `GeneratePasswordResetTokenAsync(userId)`.
- **Token Consumption**: Consumes token and sets new password hash using `ResetPasswordAsync(userId, code, password)`.
- **Email Delivery**: Legacy used SendGrid through `IIdentityMessageService`.

### 2.2 Modern Target Enhancements
1. **Security Stamp Invalidation**: Upon password reset, call `UpdateSecurityStampAsync` to revoke all active JWT tokens and invalidating ongoing sessions.
2. **Token Lifespan Configuration**: Explicitly configure `DataProtectionTokenProviderOptions.TokenLifespan` to 24 hours.
3. **Email Delivery Architecture**: Decouple email dispatch into `IEmailService` registered in DI, allowing pluggable SMTP / SendGrid / Mock logging providers.
4. **Angular Reactive UI**: Build dedicated standalone components for `/forgot-password` and `/reset-password` (with query param binding for `email` and `token`).

---

## 3. Component & API Contract Mappings

### 3.1 REST API Contracts

#### Endpoint 1: Request Password Reset Link
- **Route**: `POST /api/auth/forgot-password`
- **Request Body**:
  ```json
  {
    "email": "pharmacist@example.com"
  }
  ```
- **Response**: `200 OK`
  ```json
  {
    "message": "If the email is registered, a password reset link has been dispatched."
  }
  ```

#### Endpoint 2: Reset Password with Token
- **Route**: `POST /api/auth/reset-password`
- **Request Body**:
  ```json
  {
    "email": "pharmacist@example.com",
    "token": "CfDJ8...",
    "newPassword": "SecurePassword123!"
  }
  ```
- **Response**: `200 OK`
  ```json
  {
    "message": "Password has been reset successfully. Please log in with your new credentials."
  }
  ```

---

## 4. Blast Radius & Dependency Impact

| Area | Impact | Description |
| :--- | :--- | :--- |
| **`PharmAPI.Application`** | Added Features | `ForgotPasswordCommand`, `ResetPasswordCommand`, Handlers, Validators, DTOs, and `IEmailService` |
| **`PharmAPI.Infrastructure`** | Extension | Implement reset token generation, `ResetPasswordAsync`, and `EmailService` |
| **`PharmAPI.Api`** | Controller Extension | Add `[HttpPost("forgot-password")]` and `[HttpPost("reset-password")]` to `AuthController.cs` |
| **`PharmaUI`** | Components & Routes | `ForgotPasswordComponent`, `ResetPasswordComponent`, update `AuthService` and `app.routes.ts` |
| **Database** | Identity Tables | Uses existing `AspNetUsers` table and security stamps; no schema migration required |

---

## 5. Verification Strategy & Acceptance Criteria

1. **AC-1 (Email Token Dispatch)**: Unit test verifying that valid email dispatches reset token email with encoded link.
2. **AC-2 (Expiration & Validation)**: Unit test verifying expired or tampered token returns 400 Bad Request.
3. **AC-3 (Password Hash & Session Revocation)**: Integration test asserting user can authenticate with new password and security stamp has been updated.
4. **AC-4 (Email Delivery)**: Verification of `IEmailService` sending formatted HTML email template.
