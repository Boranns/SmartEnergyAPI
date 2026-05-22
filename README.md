# SmartEnergyAPI
A full-stack energy trading platform built with ASP.NET Core 10 and React. Uses real spot price data from Energi Data Service Denmark — users can register, trade energy products, and track their portfolio in real time.
Built to simulate how a real energy trading platform works using actual Danish market data.

Features

JWT authentication with refresh tokens
Role-based access — Trader and Admin roles
Real energy prices from Energi Data Service Denmark (Solar, Wind, Gas, Nuclear)
Buy/sell energy products with real-time portfolio tracking
Profit/loss calculation per position
Full trade history per user
Admin panel — manage users, toggle account status, platform statistics
Real-time price updates via SignalR
26 unit tests across all service layers


Tech Stack
Backend: ASP.NET Core 10, Entity Framework Core, SQL Server, JWT + Refresh Tokens, SignalR, Repository Pattern + Service Layer, xUnit + Moq
Frontend: React 18, React Router v6, Axios, Context API
DevOps: Docker + Docker Compose, GitHub Actions CI/CD

Getting Started
Prerequisites

.NET 10 SDK
Node.js 20+
Docker (recommended)


Option 1 — Docker (recommended)
bashgit clone https://github.com/Boranns/SmartEnergyAPI.git
cd SmartEnergyAPI
docker-compose up --build
ServiceURLFrontendhttp://localhost:3000Backend APIhttp://localhost:8080/swaggerDatabaselocalhost:1433

Option 2 — Without Docker
Backend
bashgit clone https://github.com/Boranns/SmartEnergyAPI.git
cd SmartEnergyAPI/SmartEnergyAPI

# Update connection string in appsettings.json, then:
dotnet ef database update
dotnet run
API: https://localhost:7034 — Swagger: https://localhost:7034/swagger
Frontend
bashcd smart-energy-frontend
npm install
npm run dev
Frontend: http://localhost:5173

Update API_URL in src/services/api.js to match your backend URL.


API Endpoints
Auth
MethodEndpointDescriptionAuthPOST/api/Auth/registerRegister new userPublicPOST/api/Auth/loginLogin and get tokensPublicPOST/api/Auth/refreshRefresh access tokenPublic
Energy Products
MethodEndpointDescriptionAuthGET/api/EnergyProductGet all products with pricesBearerGET/api/EnergyProduct/{id}Get product by IDBearerPOST/api/EnergyProduct/seedSeed initial productsAdminPOST/api/EnergyProduct/update-pricesFetch latest prices from Energi Data ServiceAdmin
Trades
MethodEndpointDescriptionAuthPOST/api/TradeExecute a buy or sell tradeBearerGET/api/Trade/my-tradesGet current user's trade historyBearerGET/api/Trade/allGet all tradesAdmin
Portfolio
MethodEndpointDescriptionAuthGET/api/PortfolioGet current user's portfolioBearerGET/api/Portfolio/total-valueGet total portfolio value in DKKBearer
Admin
MethodEndpointDescriptionAuthGET/api/Admin/usersGet all usersAdminGET/api/Admin/users/{id}Get user by IDAdminPUT/api/Admin/users/{id}/toggle-statusActivate or deactivate userAdminGET/api/Admin/dashboardPlatform statisticsAdmin

Tests
bashcd SmartEnergyAPI.Tests
dotnet test
Passed! - Failed: 0, Passed: 26, Skipped: 0, Total: 26
Test ClassTestsAuthServiceTests5TradeServiceTests5PortfolioServiceTests4AdminServiceTests6EnergyProductServiceTests5

CI/CD
GitHub Actions runs on every push to main:

Build and restore dependencies
Run all 26 unit tests
Build frontend (React + Vite)
Build Docker images and run smoke test

See .github/workflows/ci.yml.

Project Structure
SmartEnergyAPI/
├── SmartEnergyAPI/              # ASP.NET Core backend
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── DTOs/
│   ├── Hubs/                    # SignalR
│   └── Dockerfile
├── SmartEnergyAPI.Tests/        # xUnit tests
├── smart-energy-frontend/       # React frontend
│   ├── src/
│   │   ├── pages/
│   │   ├── components/
│   │   ├── services/
│   │   └── context/
│   └── Dockerfile
└── docker-compose.yml
