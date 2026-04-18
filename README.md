🚀 Interno - Microservices E-Commerce System

Interno is a full-stack e-commerce platform built using a microservices architecture, designed to simulate real-world scalable systems. The project combines a modern React frontend with multiple .NET backend services, integrated using messaging and caching technologies.

🧱 Architecture Overview

The system follows a microservices-based architecture:

Frontend: React (UI Layer)
Backend Services:
Auth Service
Product Service
Order Service
Communication: RabbitMQ (Event-driven messaging)
Caching: Redis
Database: SQL Server (per service)
Containerization: Docker (optional / in progress)
⚙️ Tech Stack
🔹 Frontend
React.js
Component-based architecture
State management (Zustand / Redux)
API integration
🔹 Backend
ASP.NET Core Web API
Microservices architecture
RESTful APIs
Entity Framework / ADO.NET
🔹 Infrastructure
Redis (Caching)
RabbitMQ (Message Broker)
Docker & Docker Compose
📁 Project Structure
interno/
├── frontend/                # React application
├── services/
│   ├── auth-service/       # Authentication & Authorization
│   ├── product-service/    # Product management
│   ├── order-service/      # Orders & transactions
├── infrastructure/
│   ├── redis/              # Redis configuration
│   ├── docker-compose.yml  # Services orchestration
├── README.md
🚀 Features
🔐 User authentication & authorization
🛍️ Product management system
📦 Order processing system
⚡ Caching using Redis
🔄 Asynchronous communication via RabbitMQ
🧩 Scalable microservices architecture
▶️ Getting Started
1️⃣ Clone the repository
git clone https://github.com/abdelrahman2901/Interno.git
cd Interno
2️⃣ Run Frontend
cd frontend
npm install
npm start
3️⃣ Run Backend Services
Open each service in Visual Studio
Run individually OR via Docker (if configured)
4️⃣ Run Infrastructure (optional)
docker-compose up --build
