# 🏗️ ScaffoldingSystem — Sistema de Control de Andamios
## Plataforma ASP.NET Core 8 + C# + SQL Server

---

## 📁 ESTRUCTURA DEL PROYECTO

```
ScaffoldingSystem/
│
├── Domain/                          ← Núcleo del negocio (sin dependencias)
│   ├── Entities/
│   │   ├── BaseEntity.cs            ← Clase base con Id, CreatedAt, IsActive
│   │   ├── User.cs                  ← Usuario del sistema
│   │   └── Operations.cs            ← Rental, Dispatch, Reception, Inspection
│   ├── Enums/
│   │   └── Enums.cs                 ← UserRole, ScaffoldingStatus, etc.
│   └── Interfaces/
│       ├── IRepository.cs           ← Contrato genérico de repositorio
│       └── IRepositories.cs         ← Contratos específicos por entidad
│
├── Application/                     ← Lógica de negocio y casos de uso
│   ├── DTOs/
│   │   └── Dtos.cs                  ← Requests, Responses y DTOs
│   ├── Interfaces/
│   │   └── IServices.cs             ← Contratos de servicios
│   └── Services/
│       ├── AuthUserService.cs       ← Auth (JWT) + gestión de usuarios
│       └── OperationServices.cs     ← Andamios, Alquileres, Despacho, etc.
│
├── Infrastructure/                  ← Acceso a datos (EF Core + SQL Server)
│   ├── Data/
│   │   └── AppDbContext.cs          ← DbContext con configuración de tablas
│   └── Repositories/
│       └── Repositories.cs          ← Implementaciones de repositorios
│
└── WebAPI/                          ← API HTTP (Controllers + Middleware)
    ├── Controllers/
    │   └── Controllers.cs           ← Todos los endpoints REST
    ├── Middleware/
    │   └── ExceptionMiddleware.cs   ← Manejo global de errores
    ├── Program.cs                   ← Configuración y arranque
    └── appsettings.json             ← Cadena de conexión y JWT
```

---

## 👥 ROLES Y PERMISOS

| Rol            | Descripción                              | Accesos principales                        |
|----------------|------------------------------------------|--------------------------------------------|
| Administrador  | Control total del sistema                | Todo                                       |
| Arrendatario   | Cliente que alquila andamios             | Crear alquileres, ver sus contratos        |
| Despachador    | Envía los andamios al cliente            | Crear despachos, marcar entregados         |
| Receptor       | Confirma llegada de andamios             | Registrar recepciones                      |
| Inspector      | Controla el estado de los andamios       | Crear inspecciones, actualizar estado      |

---

## 🔄 FLUJO DEL SISTEMA

```
[Arrendatario]  →  Crea Alquiler (Cotizado)
[Administrador] →  Aprueba Alquiler (Aprobado)
[Despachador]   →  Crea Despacho (EnCamino)
[Receptor]      →  Registra Recepción (Entregado)
[Inspector]     →  Realiza Inspección (Aprobado/Rechazado)
```

---

## 🚀 CÓMO PONER EN MARCHA EL PROYECTO

### PASO 1 — Crear la solución en Visual Studio

```bash
# Abrir terminal (PowerShell o cmd) y ejecutar:
dotnet new sln -n ScaffoldingSystem

dotnet new classlib -n ScaffoldingSystem.Domain
dotnet new classlib -n ScaffoldingSystem.Application
dotnet new classlib -n ScaffoldingSystem.Infrastructure
dotnet new webapi   -n ScaffoldingSystem.WebAPI

dotnet sln add ScaffoldingSystem.Domain
dotnet sln add ScaffoldingSystem.Application
dotnet sln add ScaffoldingSystem.Infrastructure
dotnet sln add ScaffoldingSystem.WebAPI
```

### PASO 2 — Agregar referencias entre proyectos

```bash
dotnet add ScaffoldingSystem.Application reference ScaffoldingSystem.Domain
dotnet add ScaffoldingSystem.Infrastructure reference ScaffoldingSystem.Domain
dotnet add ScaffoldingSystem.Infrastructure reference ScaffoldingSystem.Application
dotnet add ScaffoldingSystem.WebAPI reference ScaffoldingSystem.Application
dotnet add ScaffoldingSystem.WebAPI reference ScaffoldingSystem.Infrastructure
```

### PASO 3 — Instalar paquetes NuGet

```bash
# En Infrastructure
cd ScaffoldingSystem.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# En Application
cd ../ScaffoldingSystem.Application
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.Extensions.Configuration.Abstractions

# En WebAPI
cd ../ScaffoldingSystem.WebAPI
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package Swashbuckle.AspNetCore
dotnet add package BCrypt.Net-Next
```

### PASO 4 — Copiar archivos de código

Copiar cada archivo .cs al proyecto correspondiente según la estructura mostrada arriba.

### PASO 5 — Configurar cadena de conexión

Editar `WebAPI/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=ScaffoldingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "TU_CLAVE_SECRETA_MINIMO_32_CARACTERES_AQUI!"
  }
}
```
> ⚠️ La Jwt:Key debe tener al menos 32 caracteres.

### PASO 6 — Crear y ejecutar la migración de base de datos

```bash
cd ScaffoldingSystem.WebAPI

# Crear la migración inicial
dotnet ef migrations add InitialCreate --project ../ScaffoldingSystem.Infrastructure --startup-project .

# Aplicar la migración (crea la BD en SQL Server)
dotnet ef database update --project ../ScaffoldingSystem.Infrastructure --startup-project .
```

### PASO 7 — Ejecutar la API

```bash
dotnet run --project ScaffoldingSystem.WebAPI
```

Abrir en el navegador: `https://localhost:5001`  
Swagger UI estará disponible directamente en la raíz.

---

## 🔑 ENDPOINTS PRINCIPALES

### Auth
| Método | URL              | Rol         | Descripción          |
|--------|------------------|-------------|----------------------|
| POST   | /api/auth/login  | Público     | Obtener token JWT    |

### Usuarios
| Método | URL                      | Rol          |
|--------|--------------------------|--------------|
| GET    | /api/users               | Admin        |
| POST   | /api/users               | Admin        |
| PUT    | /api/users/{id}          | Admin        |
| DELETE | /api/users/{id}          | Admin        |

### Andamios
| Método | URL                          | Rol              |
|--------|------------------------------|------------------|
| GET    | /api/scaffoldings            | Todos            |
| GET    | /api/scaffoldings/available  | Todos            |
| POST   | /api/scaffoldings            | Admin, Inspector |
| PUT    | /api/scaffoldings/{id}       | Admin, Inspector |

### Alquileres
| Método | URL                          | Rol                  |
|--------|------------------------------|----------------------|
| GET    | /api/rentals                 | Admin, Arrendatario  |
| POST   | /api/rentals                 | Admin, Arrendatario  |
| PATCH  | /api/rentals/{id}/status     | Admin                |
| GET    | /api/rentals/my-rentals      | Arrendatario         |

### Despacho
| Método | URL                           | Rol                 |
|--------|-------------------------------|---------------------|
| GET    | /api/dispatches/pending       | Admin, Despachador  |
| POST   | /api/dispatches               | Admin, Despachador  |
| PATCH  | /api/dispatches/{id}/delivered| Admin, Despachador  |

### Recepción
| Método | URL                                   | Rol             |
|--------|---------------------------------------|-----------------|
| POST   | /api/receptions                       | Admin, Receptor |
| GET    | /api/receptions/by-dispatch/{id}      | Todos           |

### Inspecciones
| Método | URL                                        | Rol              |
|--------|--------------------------------------------|------------------|
| POST   | /api/inspections                           | Admin, Inspector |
| GET    | /api/inspections/scaffolding/{id}          | Todos            |
| GET    | /api/inspections/scaffolding/{id}/last     | Todos            |

---

## 📦 TECNOLOGÍAS UTILIZADAS

| Tecnología                | Versión   | Uso                              |
|---------------------------|-----------|----------------------------------|
| ASP.NET Core              | 8.0       | Framework principal              |
| Entity Framework Core     | 8.x       | ORM - acceso a base de datos     |
| SQL Server                | 2019+     | Base de datos relacional         |
| JWT (JwtBearer)           | 8.x       | Autenticación y autorización     |
| BCrypt.Net                | Latest    | Hash seguro de contraseñas       |
| Swagger (Swashbuckle)     | 6.x       | Documentación interactiva de API |

---

## 🏛️ ARQUITECTURA — Clean Architecture

```
WebAPI ──→ Application ──→ Domain
  ↓              ↑
Infrastructure ──┘
```

- **Domain**: Entidades puras y contratos (sin dependencias externas)
- **Application**: Lógica de negocio y servicios (depende solo del Domain)
- **Infrastructure**: EF Core y repositorios (implementa contratos del Domain)
- **WebAPI**: Controllers HTTP (orquesta Application e Infrastructure)

---

## 💡 PRÓXIMOS PASOS SUGERIDOS

1. **Seed de datos iniciales** — Crear un usuario Administrador por defecto al iniciar
2. **Subida de fotos** — Integrar Azure Blob Storage o S3 para fotos de inspección
3. **Frontend** — Conectar con React, Angular o Blazor
4. **Notificaciones** — Email/SMS al cambiar estado de alquiler
5. **Dashboard** — Panel con estadísticas de andamios por estado
6. **Tests** — Pruebas unitarias con xUnit + Moq
