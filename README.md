GLMS – Global Logistics Management System
Overview

<img width="1536" height="1024" alt="GLMS Logo" src="https://github.com/user-attachments/assets/af695e61-a695-4d83-a08b-824443c93ece" />


The Global Logistics Management System (GLMS) is a cloud-native enterprise web application developed to assist logistics and service-based organizations in managing clients, service contracts, service requests, and financial operations within a centralized platform.

The system was originally developed as a monolithic ASP.NET Core MVC application to establish core business functionality before being refactored into a Service-Oriented Architecture (SOA). The final solution separates the presentation layer, business services, and database access into independent components that communicate through RESTful APIs.

GLMS demonstrates modern software engineering practices including API-driven architecture, containerization, authentication, automated testing, dependency injection, database abstraction, and cloud-native deployment principles.

Business Problem

Organizations that manage large numbers of service contracts often struggle with:

Tracking client agreements across multiple regions
Managing contract lifecycles
Monitoring service requests against active contracts
Handling international currency conversions
Maintaining document storage for signed agreements
Scaling systems during periods of rapid business growth
Ensuring data consistency across multiple environments

Without a centralized management system, contract information becomes fragmented, operational efficiency decreases, and reporting accuracy is compromised.

GLMS was developed to solve these challenges by providing a unified platform for contract and service management.

Project Objectives

The primary objectives of GLMS are:

Centralize client information management
Track service contracts from creation to expiration
Manage service requests linked to contracts
Enforce business rules through automated validation
Support international currency conversion
Store and retrieve signed contract documents
Enable scalable service-oriented deployment
Demonstrate modern DevOps and cloud-native practices
Core Features
Client Management

GLMS allows administrators to manage organizational clients.

Features include:

Create new clients
View client information
Edit client details
Delete clients
Organize clients by region
Store contact information

Each client can own multiple service contracts.

Contract Management

Contracts form the core of the system.

Each contract contains:

Client association
Start date
End date
Service level
Contract status
Signed agreement document

Contract statuses include:

Draft
Active
On Hold
Expired

The system automatically enforces workflow rules based on contract status.

Signed Agreement Storage

For every contract, administrators can upload a signed PDF agreement.

The system:

Validates uploaded files
Restricts uploads to PDF format
Stores documents on the server
Provides download functionality through the web interface

This simulates integration with an enterprise document management system or file server.

Service Request Management

Service Requests represent work performed under a specific contract.

Each service request stores:

Linked contract
Description
Cost in USD
Exchange rate used
Converted cost in ZAR
Current status

Business rules prevent service requests from being created when:

Contract status is Expired
Contract status is On Hold

This ensures compliance with contractual agreements.

Currency Conversion

GLMS supports international business operations through live currency conversion.

When creating a service request:

User enters a USD value
System calls an external Exchange Rate API
Current USD-to-ZAR rate is retrieved
Local currency value is automatically calculated
Both values are stored for auditing purposes

This feature demonstrates external API consumption using HttpClient.

Search and Filtering

Administrators can quickly locate contracts using advanced filtering.

Supported filters include:

Contract status
Start date range
End date range

The filtering mechanism is implemented using LINQ queries and API endpoints.

System Architecture
Phase 1: Monolithic Architecture

The initial implementation used a traditional ASP.NET Core MVC architecture.

Components included:

Razor Views
Controllers
Entity Framework Core
SQL Database
Business Services

All functionality existed within a single deployable application.

While effective for rapid development, this approach limited scalability and maintainability.

Phase 2: Service-Oriented Architecture (SOA)

The application was later refactored into separate services.

GLMS.Web

Responsible for:

User interface
Authentication flow
Form submission
API communication

This project contains no direct database access.

GLMS.Api

Responsible for:

Business logic
Data validation
Database operations
Authentication
External service integration

All database communication occurs through the API.

Database Layer

The persistence layer stores:

Clients
Contracts
Service Requests
Users

The API acts as the sole gateway to the database.

Authentication and Security

GLMS uses JWT (JSON Web Token) authentication.

Features include:

User registration
User login
Password hashing using BCrypt
Token generation
Protected API endpoints
Role-based access support

This ensures only authenticated users can perform sensitive operations.

Cloud-Native Deployment

The system is fully containerized using Docker.

The deployment environment consists of:

Container 1

MySQL Database

Stores all application data.

Container 2

GLMS.Api

Provides RESTful API services.

Container 3

GLMS.Web

Provides the user-facing MVC application.

Docker Compose orchestrates the entire environment.

Benefits include:

Environment consistency
Simplified deployment
Isolation between services
Improved scalability
Cloud portability
Automated Testing

GLMS includes automated testing to improve reliability.

Testing types include:

Unit Testing

Tests individual business rules:

Currency conversion
File validation
Contract workflow validation
Integration Testing

Tests complete API functionality:

Client endpoints
Contract endpoints
Authentication endpoints
Service request endpoints

Automated testing reduces regression defects and supports CI/CD workflows.

Technology Stack
Frontend
ASP.NET Core MVC
Razor Pages
Bootstrap
JavaScript
Backend
ASP.NET Core Web API
RESTful Services
JWT Authentication
Database
MySQL
Entity Framework Core
Testing
xUnit
Integration Testing
DevOps
Docker
Docker Compose
External Services
ExchangeRate API
Future Enhancements

Potential future improvements include:

Role-based authorization
Email notifications
Audit logging
Dashboard analytics
Azure deployment
Kubernetes orchestration
CI/CD pipelines with GitHub Actions
Distributed caching with Redis
Load balancing and horizontal scaling
Conclusion

GLMS demonstrates the complete lifecycle of a modern enterprise application, beginning as a monolithic ASP.NET Core MVC solution and evolving into a cloud-native Service-Oriented Architecture.

The project showcases database management, API development, authentication, file handling, external service integration, automated testing, Docker containerization, and scalable software architecture practices. It serves as a practical example of how enterprise systems can be designed, deployed, and maintained using contemporary software engineering methodologies.
