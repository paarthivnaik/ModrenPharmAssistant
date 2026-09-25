# Build System & Commands

## Backend Build (`dotnet`)

### Prerequisites
- .NET 7.0 SDK installed and on `PATH`

### Build Commands
- **Restore Dependencies**:
  ```powershell
  dotnet restore PharmAPI/PharmAPI.sln
  ```
- **Compile Solution (Debug)**:
  ```powershell
  dotnet build PharmAPI/PharmAPI.sln --configuration Debug
  ```
- **Compile Solution (Release)**:
  ```powershell
  dotnet build PharmAPI/PharmAPI.sln --configuration Release --no-restore
  ```
- **Run Backend API locally**:
  ```powershell
  dotnet run --project PharmAPI/PharmAPI.Api/PharmAPI.Api.csproj
  ```

---

## Frontend Build (`npm` / `@angular/cli`)

### Prerequisites
- Node.js & npm (npm 11.17.0+)

### Build Commands
- **Install Dependencies**:
  ```powershell
  cd PharmaUI
  npm install
  ```
- **Development Server**:
  ```powershell
  cd PharmaUI
  npm start
  # or: ng serve
  ```
- **Production Compilation**:
  ```powershell
  cd PharmaUI
  npm run build
  # or: ng build --configuration production
  ```
- **Watch Build Mode**:
  ```powershell
  cd PharmaUI
  npm run watch
  ```
