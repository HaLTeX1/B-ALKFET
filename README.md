# BookFlow – Könyvkezelő rendszer

## Résztvevők

- Réz Levente László - RTL7M
  - Backend elkészítése, Docker 
- Horváth Ádám - EIY77Z
  - Frontend elkészítése, Adatbázis
- Józsa Sándor - AY29K6
  - GHCR, CI/CD

## Projekt leírás

A BookFlow egy egyszerű, full-stack webalkalmazás könyvek nyilvántartására és kezelésére.  
A rendszer lehetővé teszi könyvek létrehozását, módosítását, törlését és listázását.

A projekt célja egy end-to-end DevOps megoldás bemutatása, amely tartalmazza:

- frontend és backend alkalmazás fejlesztését
- konténerizációt Docker segítségével
- CI pipeline-t GitHub Actions használatával
- automatizált deploy-t Docker Compose segítségével

---

## Architektúra

A rendszer három fő komponensből áll:

- Frontend – Angular (TypeScript)
- Backend – ASP.NET (C#)
- Adatbázis – MongoDB

A komponensek Docker konténerekben futnak, és egy közös hálózaton kommunikálnak.

---

## Használt technológiák

| Komponens | Technológia |
|----------|------------|
| Frontend | Angular |
| Backend | ASP.NET |
| Adatbázis | MongoDB |
| Konténerizáció | Docker |
| CI/CD | GitHub Actions |
| Registry | GitHub Container Registry (GHCR) |
| Orchestration | Docker Compose |

---

## Telepítési útmutató

### 1. Követelmények

- Docker
- Docker Compose
- Git

---

### 2. Projekt klónozása

```bash
git clone https://github.com/HaLTeX1/B-ALKFET.git
cd B-ALKFET
```

### 3. Projekt indítása
```bash
With newer Docker CLI
docker compose --env-file .env -f docker-compose.prod.yml pull
docker compose --env-file .env -f docker-compose.prod.yml up -d

With older Docker
docker-compose --env-file .env -f docker-compose.prod.yml pull
docker-compose --env-file .env -f docker-compose.prod.yml up -d

```
### 4. Elérés
Frontend:
http://<szerver-ip>:4200

Backend:
http://<szerver-ip>:5245/health

## Fejlesztői mód

Fejlesztésre az alábbi paranccsal indítható az alkalmazás:
```bash
With newer Docker CLI
docker compose up --build

With older Docker
docker-compose up --build

```

## CI / CD Pipeline

A projekt GitHub Actions-t használ:

automatikus build minden push után
Docker image-ek készítése frontend és backend számára
image-ek feltöltése GHCR-be

Elérhető image-ek:

ghcr.io/<felhasználónév>/bookflow-backend

ghcr.io/<felhasználónév>/bookflow-frontend


## API végpontok

| Művelet    | Endpoint               |
| ---------- | ---------------------- |
| Lista      | GET /api/books         |
| Egy könyv  | GET /api/books/{id}    |
| Létrehozás | POST /api/books        |
| Módosítás  | PUT /api/books/{id}    |
| Törlés     | DELETE /api/books/{id} |


## Felhasználói útmutató

A rendszer használatával a felhasználó képes:

- könyvek listázására
- új könyv létrehozására
- meglévő könyv módosítására
- könyv törlésére

A frontend két fő nézetből áll:

- Könyvlista
- Könyv létrehozása / szerkesztése

## Megjegyzések

A .env fájl demó célokra készült, nem tartalmaz érzékeny adatokat
A rendszer helyi hálózaton történő futtatásra optimalizált

## Projekt struktóra
```bash
bookflow/
├── backend/
├── frontend/
├── docker-compose.yml
├── docker-compose.prod.yml
├── .env
├── .env.example
├── k8s/
└── scripts/
```
