# Dependencies & Package Managers

## Backend Package Management (`dotnet` / NuGet)
- **Solution File**: `PharmAPI/PharmAPI.sln`
- **Package Reference Format**: Direct `<PackageReference>` in `.csproj` files
- **Key NuGet Packages**:
  - `Microsoft.AspNetCore.OpenApi` (7.0.20)
  - `Swashbuckle.AspNetCore` (6.5.0)
  - `Microsoft.AspNetCore.Authentication.JwtBearer` (7.0.20)
  - `Microsoft.EntityFrameworkCore.Sqlite` (7.0.20)
  - `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (7.0.20)
  - `System.IdentityModel.Tokens.Jwt` (7.0.3)
  - `MediatR` (12.0.1)
  - `FluentValidation` (11.9.0)
  - `FluentValidation.DependencyInjectionExtensions` (11.9.0)

## Frontend Package Management (`npm`)
- **Manifest**: `PharmaUI/package.json`
- **Lockfile**: `PharmaUI/package-lock.json`
- **Manager Target**: `npm@11.17.0`
- **Key Production Dependencies**:
  - `@angular/core`: `^22.1.0`
  - `@angular/common`: `^22.1.0`
  - `@angular/forms`: `^22.1.0`
  - `@angular/router`: `^22.1.0`
  - `@angular/platform-browser`: `^22.1.0`
  - `rxjs`: `~7.8.0`
  - `tslib`: `^2.3.0`
- **Key Dev Dependencies**:
  - `@angular/cli`: `^22.1.8`
  - `@angular/build`: `^22.1.8`
  - `typescript`: `~6.0.2`
  - `vitest`: `^4.0.8`
  - `jsdom`: `^28.0.0`
  - `prettier`: `^3.8.1`
