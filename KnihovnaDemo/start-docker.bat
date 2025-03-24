@echo off
echo Kontrola, zda je Docker nainstalován...
where docker >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Docker není nainstalován. Nainstalujte Docker a zkuste to znovu.
    exit /b 1
)

echo Kontrola, zda je Docker Compose nainstalován...
where docker-compose >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Docker Compose není nainstalován. Nainstalujte Docker Compose a zkuste to znovu.
    exit /b 1
)

echo Spouštění Docker kontejnerů pro Knihovnu...
docker-compose up -d

echo Čekání na spuštění služeb...
timeout /t 10 /nobreak >nul

echo Kontrola, zda služby běží...
docker ps | findstr "knihovna-sqlserver" >nul
set SQL_RUNNING=%ERRORLEVEL%
docker ps | findstr "knihovna-api" >nul
set API_RUNNING=%ERRORLEVEL%

if %SQL_RUNNING% equ 0 if %API_RUNNING% equ 0 (
    echo ==========================================
    echo Všechny služby byly úspěšně spuštěny!
    echo SQL Server běží na: localhost:1433
    echo API běží na: http://localhost:5000
    echo Swagger dokumentace: http://localhost:5000/swagger
    echo ==========================================
) else (
    echo Něco se nepodařilo. Zkontrolujte logy pomocí 'docker-compose logs'.
)

pause 