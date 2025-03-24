@echo off
setlocal

:: Kontrola, zda je nainstalován dotnet EF tools
dotnet ef >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo dotnet-ef nástroje nejsou nainstalovány. Instaluji je...
    dotnet tool install --global dotnet-ef
    if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%
)

:: Zobrazení aktuálního adresáře pro kontrolu
echo Aktuální adresář: %CD%

:: Výpis existujících migrací
echo Existující migrace:
dotnet ef migrations list --project KnihovnaAPI

:: Aplikace migrace
echo Aplikuji migrace do databáze...
dotnet ef database update --project KnihovnaAPI

:: Kontrola výsledku
if %ERRORLEVEL% equ 0 (
    echo Migrace byly úspěšně aplikovány.
) else (
    echo Při aplikaci migrací došlo k chybě.
)

endlocal
pause 