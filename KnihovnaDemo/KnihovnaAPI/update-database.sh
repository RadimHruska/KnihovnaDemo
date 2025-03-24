#!/bin/bash

# Kontrola, zda je nainstalován dotnet EF tools
if ! command -v dotnet-ef &> /dev/null; then
    echo "dotnet-ef nástroje nejsou nainstalovány. Instaluji je..."
    dotnet tool install --global dotnet-ef
fi

# Zobrazení aktuálního adresáře pro kontrolu
echo "Aktuální adresář: $(pwd)"

# Výpis existujících migrací
echo "Existující migrace:"
dotnet ef migrations list --project KnihovnaAPI

# Aplikace migrace
echo "Aplikuji migrace do databáze..."
dotnet ef database update --project KnihovnaAPI

# Kontrola výsledku
if [ $? -eq 0 ]; then
    echo "Migrace byly úspěšně aplikovány."
else
    echo "Při aplikaci migrací došlo k chybě."
fi 