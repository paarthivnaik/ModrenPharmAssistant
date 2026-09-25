# Testing Strategies & Commands

## Frontend Testing (`Vitest`)

### Runner & Configuration
- **Test Runner**: Vitest 4.0.8 (`vitest`)
- **Environment**: JSDOM (`jsdom 28.0.0`)
- **TypeScript Spec Config**: `PharmaUI/tsconfig.spec.json`
- **Spec Files**: `*.spec.ts` (e.g., `PharmaUI/src/app/app.spec.ts`)

### Commands
- **Run All Frontend Tests**:
  ```powershell
  cd PharmaUI
  npm test
  ```
- **Run Specific Test File**:
  ```powershell
  cd PharmaUI
  npx vitest run src/app/app.spec.ts
  ```
- **Run with Coverage**:
  ```powershell
  cd PharmaUI
  npx vitest run --coverage
  ```

---

## Backend Testing (`dotnet test`)

### Strategy
- Backend tests can be added via xUnit / NUnit test projects referencing `PharmAPI.Application`, `PharmAPI.Domain`, and `PharmAPI.Infrastructure`.
- **Run All Backend Tests**:
  ```powershell
  dotnet test PharmAPI/PharmAPI.sln
  ```
- **Run Specific Backend Test Filter**:
  ```powershell
  dotnet test PharmAPI/PharmAPI.sln --filter FullyQualifiedName~FeatureName
  ```
