# KnihovnaDemo

Tento projekt je ukázkový knihovní systém vytvořený v .NET s využitím ADO.NET a SQL dotazů. Poskytuje funkce pro správu knih a čtenářů, včetně možnosti evidovat výpůjčky a vrácení knih.

## Funkcionalita

- **Správa knih**: Možnost přidávání, editace a mazání knih.
- **Správa čtenářů**: Registrace nových čtenářů, aktualizace jejich údajů.
- **Evidence výpůjček**: Sledování, kdo si knihu vypůjčil, a kdy byla vrácena.

## Požadavky

- .NET 6 nebo novější
- SQL Server

## Instalace

1. Naklonujte si tento repozitář:

   ```bash
   git clone https://github.com/RadimHruska/KnihovnaDemo.git
   ```

2. Otevřete projekt v aplikaci Visual Studio.

3. Nastavte připojení k databázi v souboru `appsettings.json`.

4. Vytvořte databázi a naimportujte SQL skripty pro vytvoření tabulek (nachází se ve složce `Scripts`).

5. Spusťte aplikaci.

## Použití

Po spuštění aplikace se otevře hlavní rozhraní, kde je možné:

- Vyhledávat knihy.
- Přidávat čtenáře.
- Evidovat nové výpůjčky.
- Spravovat seznam knih.

## Plány do budoucna

1. **Redesign aplikace**:

   - Modernizace uživatelského rozhraní pro lepší uživatelský zážitek.

2. **Přesunutí databáze na server**:

   - Migrace z lokální databáze na vzdálený SQL server pro lepší přístupnost a spolehlivost.

## Licence

Tento projekt je licencován pod licencí MIT. Podrobnosti naleznete v souboru `LICENSE`.

---

