# projekt – Skladová aplikace

Tato aplikace slouží pro správu a evidenci materiálů ve firmě projekt. Systém umožňuje technikům odebírat materiál, administrátorům sledovat pohyby a superadminům řídit více poboček (např. Plzeň, Praha, Beroun).

## Funkce

- Přihlášení a správa uživatelů (technik, admin, superadmin)
- Přehledný dashboard se statistikami (odepsané materiály, top technici, průměrná spotřeba)
- CRUD operace pro:
  - Materiály
  - Typy materiálů
  - Uživatele
  - Odepsaný materiál
- Automatické hlídání minimálního stavu na skladě
- Upozornění na překročení měsíčního limitu odepsání
- Responzivní rozhraní s Bootstrapem (Bootswatch Slate)
- Možnost filtrování podle období (týden, měsíc, kvartál, rok)

## Použité technologie

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (SQL Server)
- Identity (správa uživatelů a rolí)
- AutoMapper
- Chart.js
- Bootstrap (Bootswatch)

## Role uživatelů

- **Technik** – může odebírat materiál, vidí pouze své odepsané položky a dostupný sklad.
- **Admin** – spravuje materiály, typy materiálů a techniky ve svém departmentu.

## Struktura projektu

- `/Controllers` – logika zpracování požadavků
- `/Models` – datové modely (Entity)
- `/ViewModels` – modely pro zobrazení
- `/Services` – logika aplikace a práce s daty
- `/Repositories` – přístup k databázi
- `/Views` – razor pohledy
- `/wwwroot/js` – skripty včetně `site.js` pro grafy

## Testovací přihlašovací údaje

| Role         | Email            | Heslo      |
|--------------|------------------|------------|
| Admin        | admin@admin.cz   | 12345678   |
| Technik      | nutné vytvořit   |            |
	
## Jak projekt spustit

1. Načti projekt v **Visual Studiu 2022+**
2. Ujisti se, že máš připojený **SQL Server LocalDB**
3. Spusť migrace (nebo inicializační skript, pokud je přiložen)
4. Spusť aplikaci (`F5`)

## Statistiky

Dashboard zobrazuje:

- Top 5 techniků podle odepsaných kusů
- Top 5 materiálů
- Průměrná spotřeba materiálu za zvolené období
- Grafické přehledy pomocí Chart.js

## Autor

Projekt vytvořen v rámci studia ASP.NET Core – Šimon Durák (2025)
