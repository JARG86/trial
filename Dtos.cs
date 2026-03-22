name: Build & Deploy ScaffoldingSystem

on:
  push:
    branches: [ main, master ]
  pull_request:
    branches: [ main, master ]

env:
  DOTNET_VERSION: '8.0.x'
  PROJECT_PATH: './ScaffoldingSystem.WebAPI/ScaffoldingSystem.WebAPI.csproj'
  SOLUTION_PATH: './ScaffoldingSystem.sln'

jobs:

  # ── JOB 1: BUILD & TEST ─────────────────────────────────────────────────────
  build:
    name: 🔨 Build & Compile
    runs-on: ubuntu-latest

    steps:
      - name: 📥 Checkout código
        uses: actions/checkout@v4

      - name: ⚙️ Instalar .NET ${{ env.DOTNET_VERSION }}
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: 📦 Restaurar paquetes NuGet
        run: dotnet restore ${{ env.SOLUTION_PATH }}

      - name: 🔨 Compilar solución
        run: dotnet build ${{ env.SOLUTION_PATH }} --configuration Release --no-restore

      - name: 📤 Publicar artefacto
        run: dotnet publish ${{ env.PROJECT_PATH }} --configuration Release --no-build --output ./publish

      - name: 💾 Subir artefacto compilado
        uses: actions/upload-artifact@v4
        with:
          name: scaffolding-api-release
          path: ./publish
          retention-days: 7

  # ── JOB 2: DOCKER (opcional) ─────────────────────────────────────────────────
  docker:
    name: 🐳 Build Docker Image
    runs-on: ubuntu-latest
    needs: build
    if: github.ref == 'refs/heads/main' || github.ref == 'refs/heads/master'

    steps:
      - name: 📥 Checkout código
        uses: actions/checkout@v4

      - name: 🐳 Build Docker image
        run: docker build -t scaffolding-system:latest .

      # Descomenta esto si quieres subir a Docker Hub:
      # - name: 🔐 Login Docker Hub
      #   uses: docker/login-action@v3
      #   with:
      #     username: ${{ secrets.DOCKERHUB_USERNAME }}
      #     password: ${{ secrets.DOCKERHUB_TOKEN }}
      #
      # - name: 📤 Push imagen
      #   run: |
      #     docker tag scaffolding-system:latest ${{ secrets.DOCKERHUB_USERNAME }}/scaffolding-system:latest
      #     docker push ${{ secrets.DOCKERHUB_USERNAME }}/scaffolding-system:latest
