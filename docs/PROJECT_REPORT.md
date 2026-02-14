# Internship Report: Push Notification System (PNS)

**Institution:** Debre Berhan University  
**College:** College of Computing  
**Department:** Department of Computer Science  
**Hosting Company:** AFRICOM TECHNOLOGIES PLC  
**Internee:** BINIYAM TEHAKELE (DBU1501070)  
**Academic Mentor:** SIRAGE Z.  
**Company Supervisor:** ROBEL  
**Date:** September 2025

---

## Part One: Introduction

### Declaration
I, the undersigned, declare that this internship report entitled “Push Notification System (PNS)”, prepared in partial fulfillment of the requirements for the degree of Bachelor of Science in Computer Science at Debre Berhan University, is my original work carried out at Africom Technologies PLC during my internship period.

### Acknowledgements
I am sincerely thankful to Africom Technologies PLC for providing me with the opportunity to undertake my internship. My deepest appreciation goes to my university mentor, Ms. Sirage Z., and my company supervisor, Mr. Robel.

### Executive Summary
This report presents the design and development of a Push Notification System (PNS) using ASP.NET Core, C#, SQL Server, and SMTP integration. The project aimed to create a reliable system that enables organizations to send notifications to their employees or customers efficiently.

Key highlights include:
- **Architecture:** CQRS and Mediator patterns.
- **Features:** Email dispatch, Bulk sending, and unique Email Open Tracking via tracking pixels.
- **Tech Stack:** .NET Core, Entity Framework Core, SQL Server, Swagger UI.

---

## Part Two: Company Background

### 2.1 Company Profile
Africom Technologies PLC is a prominent Ethiopian IT solutions company established in 2004. Located in the Ethiopian ICT Park, Addis Ababa, it focuses on custom software development, IT consultancy, and BPO services.

### 2.2 Core Services
- Custom Software Development (ERP, E-commerce)
- Business Process Outsourcing (BPO)
- IT Consultancy and Audit
- Support and Maintenance

---

## Part Three: Internship Experience

### 3.1 Department
Assigned to the **Software Development Department**, specifically the team dedicated to the Push Notification System (PNS).

### 3.2 Key Tasks Executed
1. **Database Development:** Designed relational schema in SQL Server; used EF Core for migrations.
2. **API Implementation:** Developed RESTful endpoints for CRUD operations and notification querying.
3. **Email Integration:** Configured SMTP services and implemented **Email Open Tracking** using a 1x1 tracking pixel.
4. **Quality Assurance:** Participated in GitHub collaboration, code reviews, and unit/integration testing.

### 3.3 Challenges and Solutions
- **SMTP Authentication:** Resolved by using App Passwords for Gmail/BPO accounts.
- **Architecture Learning:** Mastered CQRS and Mediator patterns through mentorship.
- **Merge Conflicts:** Adopted a strict Git branching strategy to manage collaborative development.

---

## Part Four: Project Work

### 4.1 Project Overview
The PNS is a decoupled, centralized Web API serving as a single hub for all outbound communication for Africom’s enterprise applications.

### 4.2 Problem Statement
- **Redundancy:** Multiple applications had duplicated notification logic.
- **Lack of Metrics:** No way to track if critical emails were opened.
- **High Maintenance:** Changes in SMTP required updates across all applications.

### 4.3 Objectives
- Design a scalable, decoupled PNS API.
- Implement a centralized database for notification history.
- Achieve 99%+ email delivery reliability.
- **Enable Enhanced Tracking** via tracking pixels.

### 4.4 Methodology
- **Architectural Pattern:** Layered Architecture with CQRS/MediatR.
- **Development Process:** Agile/Scrum with 2-week sprints.
- **Fact-Finding:** Desk research on SMTP protocols and existing codebase inspection.

### 4.5 System Design
The system consists of five logical layers:
1. **Presentation Layer (API):** Handles HTTP requests (Controllers).
2. **Application Layer:** Contains business logic, commands, queries, and handlers.
3. **Infrastructure Layer:** Implements external dependencies (EmailService).
4. **Persistence Layer:** Manages DB access (EF Core, Repositories).
5. **Domain Layer:** Defines core entities (Client, Notification, Priority).

---

## Part Five: Conclusion and Recommendations

### 5.1 Project Conclusion
The development of the Component-Based PNS was a complete success. It achieved a robust, decoupled architecture and provided essential user engagement metrics through email tracking.

### 5.2 Recommendations
- **To Africom:** Integrate asynchronous queuing (RabbitMQ) to improve performance and add SMS channels.
- **To DBU:** Extend internship duration to 3-4 months and include more focus on enterprise architectures (CQRS, Microservices) in the curriculum.
