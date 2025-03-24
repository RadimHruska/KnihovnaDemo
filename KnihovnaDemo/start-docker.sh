#!/bin/bash

# Kontrola, zda je Docker nainstalován
if ! command -v docker &> /dev/null; then
    echo "Docker není nainstalován. Nainstalujte Docker a zkuste to znovu."
    exit 1
fi

# Kontrola, zda je Docker Compose nainstalován
if ! command -v docker-compose &> /dev/null; then
    echo "Docker Compose není nainstalován. Nainstalujte Docker Compose a zkuste to znovu."
    exit 1
fi

echo "Spouštění Docker kontejnerů pro Knihovnu..."
docker-compose up -d

# Počkejme na spuštění služeb s dostatečnou dobou čekání
echo "Čekání na spuštění služeb (60 sekund)..."
sleep 60

echo "Kontrola, zda služby běží..."
if docker ps | grep -q "knihovna-sqlserver" && docker ps | grep -q "knihovna-api"; then
    echo "=========================================="
    echo "Všechny služby byly úspěšně spuštěny!"
    echo "SQL Server běží na: localhost:1433"
    echo "API běží na: http://localhost:5001"
    echo "Swagger dokumentace: http://localhost:5001/swagger"
    echo "=========================================="
else
    echo "Něco se nepodařilo. Zkontrolujte logy pomocí 'docker-compose logs'."
fi 