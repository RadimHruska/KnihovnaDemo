#!/bin/bash

# Barvy pro výstup
GREEN='\033[0;32m'
RED='\033[0;31m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

API_URL="http://localhost:5001/api"
NEW_USER_ID=0
NEW_BOOK_ID=0
NEW_LEND_ID=0

# Funkce pro testování endpointů
test_endpoint() {
    local method=$1
    local endpoint=$2
    local payload=$3
    local description=$4
    local expected_status=$5

    echo -e "${BLUE}=========================================${NC}"
    echo -e "${BLUE}TEST: $description${NC}"
    echo -e "${BLUE}$method $endpoint${NC}"
    
    if [ -n "$payload" ]; then
        echo -e "${BLUE}Payload: $payload${NC}"
    fi

    # Vykonej HTTP požadavek
    if [ "$method" = "GET" ]; then
        response=$(curl -s -o response.txt -w "%{http_code}" -X $method "$endpoint")
    else
        response=$(curl -s -o response.txt -w "%{http_code}" -X $method "$endpoint" -H "Content-Type: application/json" -d "$payload")
    fi
    
    # Zkontroluj status kód
    if [ "$response" -eq "$expected_status" ]; then
        echo -e "${GREEN}✓ Status: $response (OK)${NC}"
        cat response.txt | jq '.' 2>/dev/null || cat response.txt
        
        # Extrahuj ID z odpovědi pro další testy, pokud jde o vytvoření nového záznamu
        if [ "$method" = "POST" ] && [[ "$endpoint" == *"/users" ]] && [ "$response" -eq 200 ]; then
            NEW_USER_ID=$(cat response.txt)
            echo -e "${BLUE}Uloženo nové ID uživatele: $NEW_USER_ID${NC}"
        elif [ "$method" = "POST" ] && [[ "$endpoint" == *"/books" ]] && [ "$response" -eq 200 ]; then
            NEW_BOOK_ID=$(cat response.txt)
            echo -e "${BLUE}Uloženo nové ID knihy: $NEW_BOOK_ID${NC}"
        elif [ "$method" = "POST" ] && [[ "$endpoint" == *"/lends" ]] && [ "$response" -eq 200 ]; then
            NEW_LEND_ID=$(cat response.txt)
            echo -e "${BLUE}Uloženo nové ID výpůjčky: $NEW_LEND_ID${NC}"
        fi
    else
        echo -e "${RED}✗ Status: $response (Očekáván: $expected_status)${NC}"
        cat response.txt
    fi
    
    echo ""
    rm -f response.txt
}

echo -e "${BLUE}=================================${NC}"
echo -e "${BLUE}TESTOVÁNÍ API KNIHOVNY${NC}"
echo -e "${BLUE}=================================${NC}"

# ==============================================
# Test endpointů uživatelů
# ==============================================

# GET /api/users - Seznam všech uživatelů
test_endpoint "GET" "$API_URL/users" "" "Seznam všech uživatelů" 200

# POST /api/users - Vytvoření nového uživatele
test_endpoint "POST" "$API_URL/users" '{ "name": "Test User", "password": "test123", "isAdmin": false }' "Vytvoření nového uživatele" 200

# GET /api/users/{id} - Získání uživatele podle ID
if [ $NEW_USER_ID -ne 0 ]; then
    test_endpoint "GET" "$API_URL/users/$NEW_USER_ID" "" "Získání uživatele podle ID" 200
else 
    test_endpoint "GET" "$API_URL/users/1" "" "Získání uživatele podle ID" 200
fi

# PUT /api/users/{id} - Aktualizace uživatele
if [ $NEW_USER_ID -ne 0 ]; then
    test_endpoint "PUT" "$API_URL/users/$NEW_USER_ID" '{ "id": '"$NEW_USER_ID"', "name": "Updated Test User", "password": "test123", "isAdmin": false }' "Aktualizace uživatele" 204
else
    test_endpoint "PUT" "$API_URL/users/1" '{ "id": 1, "name": "Updated User 1", "password": "test123", "isAdmin": true }' "Aktualizace uživatele" 204
fi

# ==============================================
# Test endpointů knih
# ==============================================

# GET /api/books - Seznam všech knih
test_endpoint "GET" "$API_URL/books" "" "Seznam všech knih" 200

# POST /api/books - Vytvoření nové knihy
test_endpoint "POST" "$API_URL/books" '{ "name": "Test Book", "author": "Test Author", "inStock": 5, "publicationYear": 2023 }' "Vytvoření nové knihy" 200

# GET /api/books/{id} - Získání knihy podle ID
if [ $NEW_BOOK_ID -ne 0 ]; then
    test_endpoint "GET" "$API_URL/books/$NEW_BOOK_ID" "" "Získání knihy podle ID" 200
else
    test_endpoint "GET" "$API_URL/books/1" "" "Získání knihy podle ID" 200
fi

# GET /api/books/search/{query} - Vyhledávání knih
test_endpoint "GET" "$API_URL/books/search/Test" "" "Vyhledávání knih" 200

# GET /api/books/available - Dostupné knihy
test_endpoint "GET" "$API_URL/books/available" "" "Dostupné knihy" 200

# PUT /api/books/{id} - Aktualizace knihy
if [ $NEW_BOOK_ID -ne 0 ]; then
    test_endpoint "PUT" "$API_URL/books/$NEW_BOOK_ID" '{ "id": '"$NEW_BOOK_ID"', "name": "Updated Test Book", "author": "Test Author", "inStock": 3, "publicationYear": 2023 }' "Aktualizace knihy" 204
else
    test_endpoint "PUT" "$API_URL/books/1" '{ "id": 1, "name": "Updated Book 1", "author": "Author 1", "inStock": 10, "publicationYear": 2020 }' "Aktualizace knihy" 204
fi

# ==============================================
# Test endpointů výpůjček
# ==============================================

# GET /api/lends - Seznam všech výpůjček
test_endpoint "GET" "$API_URL/lends" "" "Seznam všech výpůjček" 200

# Vytvoření nové výpůjčky - nastavíme ID uživatele a knihy
USER_ID=$([ $NEW_USER_ID -ne 0 ] && echo $NEW_USER_ID || echo 1)
BOOK_ID=$([ $NEW_BOOK_ID -ne 0 ] && echo $NEW_BOOK_ID || echo 1)

# POST /api/lends - Vytvoření nové výpůjčky
test_endpoint "POST" "$API_URL/lends" '{ "landedDate": "'"$(date +"%Y-%m-%d")"'", "idUser": '"$USER_ID"', "idBook": '"$BOOK_ID"' }' "Vytvoření nové výpůjčky" 200

# GET /api/lends/{id} - Získání výpůjčky podle ID
if [ $NEW_LEND_ID -ne 0 ]; then
    test_endpoint "GET" "$API_URL/lends/$NEW_LEND_ID" "" "Získání výpůjčky podle ID" 200
else
    test_endpoint "GET" "$API_URL/lends/1" "" "Získání výpůjčky podle ID" 200
fi

# GET /api/lends/user/{userId} - Výpůjčky uživatele
test_endpoint "GET" "$API_URL/lends/user/$USER_ID" "" "Výpůjčky uživatele" 200

# GET /api/lends/active - Aktivní výpůjčky
test_endpoint "GET" "$API_URL/lends/active" "" "Aktivní výpůjčky" 200

# GET /api/lends/overdue - Zpožděné výpůjčky
test_endpoint "GET" "$API_URL/lends/overdue" "" "Zpožděné výpůjčky" 200

# PUT /api/lends/{id}/return - Vrácení knihy
if [ $NEW_LEND_ID -ne 0 ]; then
    test_endpoint "PUT" "$API_URL/lends/$NEW_LEND_ID/return" '{ "id": '"$NEW_LEND_ID"', "returnedDate": "'"$(date +"%Y-%m-%d")"'" }' "Vrácení knihy" 200
else
    test_endpoint "PUT" "$API_URL/lends/1/return" '{ "id": 1, "returnedDate": "'"$(date +"%Y-%m-%d")"'" }' "Vrácení knihy" 200
fi

# ==============================================
# Test endpointů autentizace
# ==============================================

# POST /api/auth/login - Přihlášení uživatele
test_endpoint "POST" "$API_URL/auth/login" '{ "username": "admin", "password": "admin" }' "Přihlášení uživatele (admin)" 200
test_endpoint "POST" "$API_URL/auth/login" '{ "username": "neplatné", "password": "neplatné" }' "Přihlášení uživatele (neplatné)" 401

# ==============================================
# Úklid - odstranění testovacích dat
# ==============================================

# DELETE endpointy - pouze pokud byly vytvořeny nové záznamy

if [ $NEW_LEND_ID -ne 0 ]; then
    test_endpoint "DELETE" "$API_URL/lends/$NEW_LEND_ID" "" "Smazání výpůjčky" 204
fi

if [ $NEW_BOOK_ID -ne 0 ]; then
    test_endpoint "DELETE" "$API_URL/books/$NEW_BOOK_ID" "" "Smazání knihy" 204
fi

if [ $NEW_USER_ID -ne 0 ]; then
    test_endpoint "DELETE" "$API_URL/users/$NEW_USER_ID" "" "Smazání uživatele" 204
fi

echo -e "${BLUE}=================================${NC}"
echo -e "${BLUE}TESTOVÁNÍ DOKONČENO${NC}"
echo -e "${BLUE}=================================${NC}" 