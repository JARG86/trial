{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ScaffoldingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "TU_CLAVE_SECRETA_MINIMO_32_CARACTERES_AQUI!",
    "Issuer": "ScaffoldingSystem",
    "Audience": "ScaffoldingSystemUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
