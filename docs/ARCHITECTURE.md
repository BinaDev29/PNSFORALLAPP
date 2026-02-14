# System Architecture: Push Notification System (PNS)

The PNS is built using a **Clean Layered Architecture** combined with the **CQRS (Command Query Responsibility Segregation)** and **Mediator** patterns. This ensures high scalability, maintainability, and clear separation of concerns.

## 1. Architectural Layers

### 1.1 Presentation Layer (API)
- **Role:** Handles incoming HTTP requests and returns JSON responses.
- **Components:** `NotificationController`, `ClientController`, etc.
- **Responsibility:** Maps request payloads to Commands or Queries and dispatches them via `IMediator`.

### 1.2 Application Layer
- **Role:** The "Brain" of the system.
- **Pattern:** Uses **MediatR** for CQRS.
- **Components:**
  - **Commands:** (e.g., `CreateNotificationCommand`) for write operations.
  - **Queries:** (e.g., `GetUnseenNotificationsQuery`) for read operations.
  - **Handlers:** Logic for processing commands and queries.
- **Responsibility:** Orchestrates business flow without being tied to specific DB or UI technologies.

### 1.3 Domain Layer
- **Role:** Enterprise-wide business rules and models.
- **Responsibility:** Contains core entities (`Notification`, `Client`, `Priority`, `NotificationHistory`). It is independent of all other layers.

### 1.4 Infrastructure Layer
- **Role:** Implements external concerns.
- **Components:** `EmailService` (SMTP integration).
- **Responsibility:** Sending emails and injecting the **Tracking Pixel** for open-rate analytics.

### 1.5 Persistence Layer
- **Role:** Specialized infrastructure for data storage.
- **Tech Stack:** Entity Framework Core, SQL Server.
- **Responsibility:** Implements repositories and manages the database context.

---

## 2. Key Patterns & Mechanisms

### 2.1 CQRS & Mediator Pattern
By using the **MediatR** library, the system decouples the Controller from the Business Logic handlers. 
- **Write Path (Commands):** Validates and processes requests that change state (e.g., sending a notification).
- **Read Path (Queries):** Optimized for data retrieval (e.g., fetching notification history).

### 2.2 Email Open Tracking
A core innovation of the PNS is the **Enhanced Email Tracking** mechanism:
1. When an email is dispatched via `EmailService`, a unique 1x1 transparent GIF image URL is injected into the HTML body:  
   `<img src='[API_URL]/api/Notification/[ID]/track' width='1' height='1' />`
2. When the user opens the email, the client (Outlook/Gmail) requests the image.
3. The API endpoint handles the request, updates the `IsSeen` status in the database, and returns a transparent pixel image.

### 2.3 Database Schema (Normalized)
- **Clients:** Stores application credentials and metadata.
- **Notifications:** Main log of sent/pending messages.
- **Priorities:** Defines delivery urgency levels.
- **NotificationHistory:** Tracks events like "Sent", "Opened", or "Failed".

---

## 3. Technology Stack
- **Backend:** .NET 9 (ASP.NET Core)
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Documentation:** Swagger/OpenAPI
- **Frontend Dashboard:** React with Tailwind CSS
