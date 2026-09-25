# Repository Conventions & Style Guide

## C# (.NET 7)
- **Formatting**: Default dotnet format standards with implicit usings and nullable references enabled.
- **Architecture Integrity**: Clean Architecture boundaries (Domain &rarr; Application &rarr; Infrastructure &rarr; Api).
- **Naming Conventions**:
  - Types/Interfaces: PascalCase (`IIdentityService`, `LoginCommand`, `ApplicationUser`).
  - Methods: PascalCase, async methods suffixed with `Async` (`GenerateJwtTokenAsync`).
  - Private Fields: `_camelCase` (`_userManager`, `_jwtTokenService`).

## TypeScript & Angular
- **Component Style**: Standalone components (`@Component({ standalone: true, ... })`).
- **File Structure**: Feature modules organized under `src/app/features/<feature-name>/`, shared/core under `src/app/core/`.
- **Form Handling**: Reactive forms with `FormGroup`, typed controls, and clear validation feedback.
- **Formatting**: Prettier (`.prettierrc`) for TypeScript, HTML, CSS, JSON.
