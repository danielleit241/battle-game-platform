# BattleGamePlatform

## 📖 Giới thiệu

BattleGamePlatform là một dự án microservices pattern được xây dựng với mục đích luyện tập và áp dụng các best practices trong kiến trúc phân tán. Dự án này triển khai một nền tảng game trận chiến với các services độc lập, giao tiếp qua message bus, và áp dụng các patterns như:

- **Clean Architecture**: Phân tách rõ ràng giữa Domain, Application, Infrastructure layers
- **CQRS (Command Query Responsibility Segregation)**: Tách biệt logic đọc và ghi
- **Event-Driven Architecture**: Giao tiếp bất đồng bộ giữa các services thông qua RabbitMQ
- **Outbox Pattern**: Đảm bảo tính nhất quán khi publish events
- **API Gateway Pattern**: Điểm vào duy nhất cho clients
- **Health Checks**: Giám sát trạng thái services
- **Distributed Tracing**: Theo dõi requests xuyên suốt các services với OpenTelemetry
- **Repository Pattern**: Trừu tượng hóa data access layer
- **Dependency Injection**: Loose coupling giữa các components

## 🏗️ Kiến trúc

### Microservices

Hệ thống bao gồm các services sau:

1. **UserService**: Quản lý người dùng, authentication/authorization
2. **GameService**: Quản lý thông tin games, game logic
3. **MatchService**: Xử lý các trận đấu, matchmaking
4. **TournamentService**: Quản lý giải đấu
5. **LeaderboardService**: Bảng xếp hạng người chơi
6. **SearchService**: Tìm kiếm games với Elasticsearch

### Infrastructure

- **PostgreSQL**: Primary database cho các services
- **MongoDB**: Document storage
- **Redis**: Caching layer
- **RabbitMQ**: Message broker cho event-driven communication
- **Elasticsearch**: Full-text search engine
- **Aspire Dashboard**: Monitoring và observability
- **PgWeb**: PostgreSQL web interface

## 🚀 Yêu cầu hệ thống

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

## 📦 Cài đặt và chạy

### 1. Clone repository

```bash
git clone https://github.com/yourusername/BattleGamePlatform.git
cd BattleGamePlatform
```

### 2. Tạo file `.env`

Tạo file `.env` trong thư mục `Docker/` với nội dung sau:

```env
# PostgreSQL
POSTGRES_VERSION=17
POSTGRES_USER=postgres
POSTGRES_PASSWORD=your_secure_password
POSTGRES_DB=battlegamedb
POSTGRES_PORT=5432

# MongoDB
MONGO_VERSION=latest
MONGO_ROOT_USERNAME=root
MONGO_ROOT_PASSWORD=your_secure_password
MONGO_PORT=27017

# Redis
REDIS_VERSION=latest
REDIS_PASSWORD=your_secure_password
REDIS_PORT=6379

# RabbitMQ
RABBITMQ_VERSION=3-management
RABBITMQ_USER=admin
RABBITMQ_PASSWORD=your_secure_password
RABBITMQ_PORT=5672
RABBITMQ_MANAGEMENT_PORT=15672

# JWT
JWT__KEY=your_jwt_secret_key_minimum_32_characters_long

# Aspire Dashboard
ASPIRE_DASHBOARD_OTLP_API_KEY=your_api_key
ASPIRE_DASHBOARD_UI_PORT=18888
ASPIRE_DASHBOARD_OTLP_PORT=18889

# Services
ASPNETCORE_ENVIRONMENT=Development
USERSERVICE_PORT=5001
GAMESERVICE_PORT=5002
MATCHSERVICE_PORT=5003
TOURNAMENTSERVICE_PORT=5004
LEADERBOARDSERVICE_PORT=5005
SEARCHSERVICE_PORT=5006

# PgWeb
PGWEB_PORT=5050

# OpenTelemetry
OTEL_SERVICE_VERSION=1.0.0
```

### 3. Chạy Docker Compose

```bash
cd Docker
docker-compose up -d
```

Lệnh này sẽ:

- Build tất cả các services từ source code
- Khởi động các infrastructure services (PostgreSQL, MongoDB, Redis, RabbitMQ)
- Chạy migrations cho databases
- Khởi động tất cả microservices

### 4. Kiểm tra services

Sau khi khởi động, các services sẽ available tại:

- **Aspire Dashboard**: http://localhost:18888 - Monitoring và distributed tracing
- **RabbitMQ Management**: http://localhost:15672 - Message broker UI (user: admin)
- **PgWeb**: http://localhost:5050 - PostgreSQL web interface
- **User Service API**: http://localhost:5001
- **Game Service API**: http://localhost:5002
- **Match Service API**: http://localhost:5003
- **Tournament Service API**: http://localhost:5004
- **Leaderboard Service API**: http://localhost:5005
- **Search Service API**: http://localhost:5006

### 5. Xem logs

```bash
# Xem logs tất cả services
docker-compose logs -f

# Xem logs của một service cụ thể
docker-compose logs -f battlegame-userservice
```

### 6. Dừng services

```bash
docker-compose down

# Dừng và xóa volumes (xóa toàn bộ data)
docker-compose down -v
```

## 🧪 Chạy Tests

```bash
# Chạy unit tests
dotnet test BattleGamePlatform.Tests/BattleGame.UnitTests/

# Chạy integration tests
dotnet test BattleGamePlatform.Tests/BattleGame.IntergrationTests/
```

## 📚 API Documentation

Mỗi service đều có Swagger UI để explore APIs:

- UserService: http://localhost:5001/swagger
- GameService: http://localhost:5002/swagger
- MatchService: http://localhost:5003/swagger
- TournamentService: http://localhost:5004/swagger
- LeaderboardService: http://localhost:5005/swagger

## 🛠️ Development

### Chạy local (không dùng Docker)

1. Start infrastructure services:

```bash
cd Docker
docker-compose up -d postgres redis rabbitmq mongo dashboard
```

2. Run services từ Visual Studio hoặc command line:

```bash
cd BattleGamePlatform.AppHost
dotnet run
```

Aspire AppHost sẽ orchestrate tất cả services và mở dashboard tự động.

### Database Migrations

Migrations được tự động chạy khi services khởi động. Để tạo migration mới:

```bash
cd BattleGamePlatform.Services/User/BattleGame.UserService.DataAccessLayer
dotnet ef migrations add YourMigrationName
```

## 🏛️ Clean Architecture Structure

```
BattleGame.{ServiceName}/
├── Domain/                    # Entities, Value Objects, Domain Events
├── Application/              # Use Cases, DTOs, Interfaces
├── Infrastructure/           # Data Access, External Services
└── API/                     # Controllers, Middleware, Configuration

BattleGame.{ServiceName}.BusinessLogicLayer/  # Application layer
BattleGame.{ServiceName}.DataAccessLayer/      # Infrastructure layer
BattleGame.{ServiceName}.Common/               # Shared contracts
```

## 📝 Coding Standards

Dự án tuân thủ các coding standards được định nghĩa trong [.github/copilot-instructions.md](.github/copilot-instructions.md), bao gồm:

- Naming conventions (PascalCase, camelCase)
- Clean Architecture principles
- RESTful API design
- Error handling với ProblemDetails
- Logging best practices
- Validation và security

## 🤝 Contributing

Đây là dự án học tập cá nhân, nhưng mọi góp ý và suggestions đều được welcome!

## 📄 License

This project is for educational purposes.

## 🔗 Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microservices Pattern by Chris Richardson](https://microservices.io/)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Docker Documentation](https://docs.docker.com/)

---

**Made with ❤️ for learning and practicing microservices architecture**
