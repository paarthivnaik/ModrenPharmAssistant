# Repository Technologies

## Runtimes & Languages

### 1. C# / .NET Runtime
- **Language Version**: C# 11.0 (`net7.0`)
- **Runtime Target**: .NET 7.0 (`Microsoft.NET.Sdk`, `Microsoft.NET.Sdk.Web`)
- **Nullability**: Enabled (`<Nullable>enable</Nullable>`)
- **Implicit Usings**: Enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- **Confidence**: `HIGH`
- **Evidence Citation**:
  - `PharmAPI/PharmAPI.Api/PharmAPI.Api.csproj` (Target: `net7.0`)
  - `PharmAPI/PharmAPI.Application/PharmAPI.Application.csproj`
  - `PharmAPI/PharmAPI.Domain/PharmAPI.Domain.csproj`
  - `PharmAPI/PharmAPI.Infrastructure/PharmAPI.Infrastructure.csproj`

### 2. TypeScript / JavaScript Runtime
- **Language Version**: TypeScript ~6.0.2
- **Runtime Environment**: Node.js / Browser (V8 / Modern Evergreen Browsers)
- **Package Manager**: npm @ 11.17.0
- **Confidence**: `HIGH`
- **Evidence Citation**:
  - `PharmaUI/package.json` (`typescript: "~6.0.2"`, `packageManager: "npm@11.17.0"`)
  - `PharmaUI/tsconfig.json` & `PharmaUI/tsconfig.app.json`

### 3. Markup & Styles
- **HTML**: HTML5
- **CSS**: Standard CSS3 / Modern styling
- **Confidence**: `HIGH`
- **Evidence Citation**:
  - `PharmaUI/src/index.html`
  - `PharmaUI/src/styles.css`
