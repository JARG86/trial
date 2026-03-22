{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_SERVER};Database=ScaffoldingDB;User Id=${DB_USER};Password=${DB_PASSWORD};TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "${JWT_KEY}",
    "Issuer": "ScaffoldingSystem",
    "Audience": "ScaffoldingSystemUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
