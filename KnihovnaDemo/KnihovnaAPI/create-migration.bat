@echo off
setlocal

:: Kontrola, zda je nainstalován dotnet EF tools
dotnet ef >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo dotnet-ef nástroje nejsou nainstalovány. Instaluji je...
    dotnet tool install --global dotnet-ef
    if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%
)

:: Kontrola parametrů
if "%~1"=="" (
    echo Použití: %0 ^<název-migrace^>
    echo Příklad: %0 InitialCreate
    exit /b 1
)

:: Zobrazení aktuálního adresáře pro kontrolu
echo Aktuální adresář: %CD%

:: Vytvoření migrace
echo Vytvářím migraci '%1'...
dotnet ef migrations add %1 --project KnihovnaAPI

:: Kontrola výsledku
if %ERRORLEVEL% equ 0 (
    echo Migrace '%1' byla úspěšně vytvořena.
    echo Pro aplikaci migrace spusťte: dotnet ef database update --project KnihovnaAPI
) else (
    echo Při vytváření migrace došlo k chybě.
)

endlocal 