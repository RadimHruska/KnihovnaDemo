@echo off
echo Zastavování Docker kontejnerů pro Knihovnu...
docker-compose down

echo Kontejnery zastaveny.
echo Pro úplné vymazání včetně databáze spusťte: docker-compose down -v

pause 