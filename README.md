# Push Notification System (PNS)

[![Build Status](https://github.com/BinaDev29/PNSFORALLAPP/actions/workflows/main.yml/badge.svg)](https://github.com/BinaDev29/PNSFORALLAPP/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A centralized, component-based notification hub designed to handle multi-application alerts with high reliability and embedded tracking metrics.

## 🚀 Overview

The PNS is a decoupled service built to solve code redundancy and lack of user engagement metrics in enterprise environments. It provides a single RESTful API hub for all outbound communications (Email, SMS, internal alerts).

**Developed during internship at Africom Technologies PLC.**

### Key Features
- **Clean Architecture:** Domain-driven design with CQRS and Mediator patterns.
- **Enhanced Email Tracking:** Unique tracking pixel injection to monitor real-time email open rates.
- **Bulk Notifications:** Efficient batch processing for high-volume deployments.
- **Interactive Dashboard:** Modern React-based UI for managing clients and viewing analytics.
- **Self-Documented API:** Integrated Swagger UI for easy client integration.

## 🛠️ Technology Stack
- **Backend:** .NET 9 (ASP.NET Core / C#)
- **Data Access:** Entity Framework Core
- **Database:** SQL Server
- **Messaging Pattern:** MediatR (CQRS)
- **Frontend:** React, Tailwind CSS, Shadcn UI
- **DevOps:** Docker, GitHub Actions (CI/CD)

## 📂 Documentation
- [**Architecture Overview**](./docs/ARCHITECTURE.md) - Deep dive into patterns and system design.
- [**Internship Report**](./docs/PROJECT_REPORT.md) - Full project background, objectives, and findings.
- [**Frontend Guide**](./pns-dashboard/README.md) - Setup and development for the dashboard.

## 🏁 Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js (v20+)
- SQL Server (LocalDB or Express)

### Quick Start (Docker)
```bash
docker-compose up --build
```

### Manual Setup
1. **Database:** Update connection strings in `PNS/API/appsettings.json`.
2. **Backend:** 
   ```bash
   cd PNS/API
   dotnet run
   ```
3. **Frontend:**
   ```bash
   cd pns-dashboard
   npm install
   npm run dev
   ```

## 🧪 Running Tests
```bash
cd PNS
dotnet test
```

## 📜 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
