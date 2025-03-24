#!/bin/bash

# Kontrola, zda je nainstalován dotnet EF tools
if ! command -v dotnet-ef &> /dev/null; then
    echo "dotnet-ef nástroje nejsou nainstalovány. Instaluji je..."
    dotnet tool install --global dotnet-ef
fi

# Kontrola parametrů
if [ -z "$1" ]; then
    echo "Použití: $0 <název-migrace>"
    echo "Příklad: $0 InitialCreate"
    exit 1
fi

# Zobrazení aktuálního adresáře pro kontrolu
echo "Aktuální adresář: $(pwd)"

# Vytvoření migrace
echo "Vytvářím migraci '$1'..."
dotnet ef migrations add $1

# Kontrola výsledku
if [ $? -eq 0 ]; then
    echo "Migrace '$1' byla úspěšně vytvořena."
    echo "Pro aplikaci migrace spusťte: dotnet ef database update"
else
    echo "Při vytváření migrace došlo k chybě."
fi 