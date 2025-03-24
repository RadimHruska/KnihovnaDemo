# Knihovna Demo

Aplikace pro správu knihovny s odděleným backendem a frontendem.

## Struktura projektu

- `KnihovnaDemo` - Klientská aplikace (WPF)
- `KnihovnaAPI` - Backendové API (.NET Core)

## Spuštění s Dockerem

### Požadavky

- Docker
- Docker Compose

### Postup

1. **Spuštění aplikace**

   Ve Windows:
   ```
   .\start-docker.bat
   ```

   V Linuxu/MacOS:
   ```
   chmod +x ./start-docker.sh
   ./start-docker.sh
   ```

2. **Ověření běhu aplikace**

   - SQL Server běží na: `localhost:1433`
   - API běží na: `http://localhost:5000`
   - Swagger dokumentace: `http://localhost:5000/swagger`

3. **Konfigurace klienta**

   Pro připojení klienta k API v Dockeru upravte soubor `KnihovnaDemo/App.config`:
   - Nastavte `Environment` na hodnotu `Docker`
   - Ujistěte se, že `ApiBaseUrl` odpovídá URL API (výchozí: `http://localhost:5000`)

4. **Zastavení aplikace**

   Ve Windows:
   ```
   .\stop-docker.bat
   ```

   V Linuxu/MacOS:
   ```
   chmod +x ./stop-docker.sh
   ./stop-docker.sh
   ```

## Spuštění bez Dockeru

### Backend API

1. Přejděte do adresáře `KnihovnaAPI`
2. Spusťte:
   ```
   dotnet run
   ```

### Klientská aplikace

1. Přejděte do adresáře `KnihovnaDemo`
2. Spusťte:
   ```
   dotnet run
   ```

## Práce s databází a migracemi

### Vytvoření nové migrace

Ve Windows:
```
cd KnihovnaDemo
KnihovnaAPI\create-migration.bat "NazevMigrace"
```

V Linuxu/MacOS:
```
cd KnihovnaDemo
chmod +x KnihovnaAPI/create-migration.sh
./KnihovnaAPI/create-migration.sh "NazevMigrace"
```

### Aplikace migrací do databáze

Ve Windows:
```
cd KnihovnaDemo
KnihovnaAPI\update-database.bat
```

V Linuxu/MacOS:
```
cd KnihovnaDemo
chmod +x KnihovnaAPI/update-database.sh
./KnihovnaAPI/update-database.sh
```

### Migrace v Dockeru

Při spuštění pomocí Docker Compose je migrace aplikována automaticky ve speciálním kontejneru (`migrations`), který se spustí před API serverem.

## Přihlašovací údaje

Výchozí uživatelé:
- Admin: `Admin` / `admin`
- Radim: `Radim` / `heslo`
- Uživatel: `Uživatel` / `heslo`

## Databáze

Databáze obsahuje tabulky:
- `Users` - Uživatelé knihovny
- `Books` - Knihy v knihovně
- `Lends` - Výpůjčky knih

Při použití Dockeru je databáze automaticky vytvořena a naplněna testovacími daty pomocí migrací a inicializačního kódu. 