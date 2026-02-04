# PNS - Push Notification Service

Enhanced Push Notification Service with .NET Core Core API and React Dashboard.

## Features
- **Clean Architecture**: Decoupled Domain, Application, Infrastructure, and Persistence layers.
- **Real-time Notifications**: SignalR integration for instant updates.
- **Docker Ready**: Run the entire stack with a single command.
- **CI/CD**: Automated build and test pipeline with GitHub Actions.
- **High Security**: JWT Authentication, Rate Limiting, and Security Headers.

## Getting Started

### Prerequisites
- Node.js (v20+)
- .NET 9 SDK
- Docker (optional)
- SQL Server

### Run with Docker (Recommended)
```bash
docker-compose up --build
```

### Manual Setup
1. **Database**: Update connection strings in `PNS/API/appsettings.json`.
2. **Backend**: 
   ```bash
   cd PNS/API
   dotnet run
   ```
3. **Frontend**:
   ```bash
   cd pns-dashboard
   npm install
   npm run dev
   ```

### Running Tests
```bash
cd PNS
dotnet test
```
