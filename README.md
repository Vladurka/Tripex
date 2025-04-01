# 🌍 Tripex — Mini Instagram Clone

**Tripex** is a social media platform inspired by Instagram, designed with microservices, modern architectural patterns, and cloud-native technologies to ensure scalability, flexibility, and high performance.

---

## 🚀 Features

- User registration & authentication (JWT)
- Create posts with images, or videos
- Like, comment, and follow functionality
- Personalized user news feeds
- Profiles search
- Real-time notifications
- Profile management
- API Gateway with REST & GraphQL

---

## ⚙️ Architecture & Technologies

- **Architecture**: Microservices, Domain-Driven Design (DDD), CQRS, Outbox Pattern
- **Backend**: ASP.NET Core 8 (Minimal APIs)
- **Communication**:
  - REST (external/client-facing)
  - gRPC (internal high-performance communication)
  - RabbitMQ (message broker)
- **Databases**:
  - PostgreSQL 
  - Cassandra
  - Elasticsearch(search)
  - Redis(caching)
- **Media Storage**: Azure Blob Storage
- **Logging**: Serilog
- **Containerization**: Docker

---

## 🧩 Microservices Overview

Each microservice is independently deployable and maintains its own data and bounded context.

### 🔐 Auth Service
- **Handles**: User registration, login, JWT
- **Storage**: PostgreSQL
- **Interface**: REST API
- **Notes**: Publishes events for profile setup

---

### 👤 Profile Service
- **Handles**: User profile management (bio, avatar, etc.)
- **Storage**: PostgreSQL + Azure Blob Storage(for avatars)
- **Interface**: REST API
- **Notes**: Subscribes to Auth events for user onboarding

---

### 🖼️ Post Service
- **Handles**: Post creation and retrieval (text, images, videos)
- **Storage**: Cassandra + Azure Blob Storage
- **Interface**: REST API
- **Notes**: Emits events to Feed, Search, and Interaction services

---

### 📰 Feed Service
- **Handles**: Personalized user news feeds
- **Interface**: gRPC
- **Notes**: Builds feeds based on events (posts, follows, etc.)

---

### 💬 Interaction Service
- **Handles**: Likes, comments, followers
- **Storage**: Cassandra
- **Interface**: REST API
- **Notes**: Emits events for notifications and feed updates

---

### 🔍 Search Service
- **Handles**: Full-text search and filtering 
- **Storage**: Elasticsearch
- **Interface**: REST API
- **Notes**: Indexes data from Post and Profile services

---

### 🔔 Notification Service
- **Handles**: Push notifications for interactions (likes, follows, comments)
- **Interface**: gRPC
- **Notes**: Subscribes to Interaction and Post events

---

### 🌐 Gateway Service (API Aggregator)
- **Handles**: Unified entry point for clients
- **Interface**: REST
- **Notes**: Aggregates data from other services
