# Task Management Backend API

This is a clean, DDD-structured Task Management Backend API built with .NET 8.

## Features

- **Clean Architecture / DDD**: Separation of concerns into Domain, Application, Infrastructure, and Presentation layers.
- **RESTful API**: Endpoints for authentication, user management (admin only), and tasks.
- **Authentication**: JWT-based authentication and authorization.
- **Redis Caching**: Caches tasks requested by ID.
- **Background Processing**: Simple background worker queue using `System.Threading.Channels` and `.NET BackgroundService`.
- **Database**: Entity Framework Core with PostgreSQL.
- **Database Seeding**: Default Admin user seeded on startup.
- **Swagger Documentation**: Interactive API documentation.

## Requirements

- Docker and Docker Compose
- .NET 8 SDK (for local development without Docker)

## Setup and Run

1. **Start the Infrastructure and API using Docker Compose**
   ```bash
   docker-compose up --build
   ```
   This will start:
   - PostgreSQL on port `5432`
   - Redis on port `6379`
   - The .NET Web API on port `8080` / `8081`
2. **Access Swagger**
   Open your browser and navigate to:
   [http://localhost:8080/swagger](http://localhost:8080/swagger)
   or
   [https://localhost:8081/swagger](https://localhost:8081/swagger)

## Admin Credentials

The application will automatically seed a default admin user:

- **Email**: `admin@example.com`
- **Password**: `Admin@123`
  You can use these credentials to get a JWT token and access the `/api/Admin` endpoints.

## Assumptions Made

1. **Migrations**: Database migrations are applied automatically at startup for convenience during development.
2. **Caching**: Redis is configured to cache `GetTaskById` for 10 minutes.
3. **Background Processing**: Used a native .NET `Channel<TaskItem>` as an in-memory queue to simulate sending tasks to a worker, instead of standing up an external broker like RabbitMQ.
4. **User Entity**: Instead of using the full ASP.NET Core Identity system which brings in a lot of tables and complexity, a lightweight custom `User` entity was implemented alongside BCrypt for hashing, to keep the solution clean and focused.
