# TodoApi — ASP.NET Core + MongoDB + JWT

API REST sécurisée construite avec **ASP.NET Core 9**, **MongoDB** et **JWT (JSON Web Tokens)**.

## Fonctionnalités

- ✅ CRUD complet sur les Todos
- ✅ Base de données **MongoDB**
- ✅ Authentification **JWT**
- ✅ Autorisation par **rôles** (`admin` / `user`)
  - `admin` → peut créer, modifier et supprimer des Todos
  - `user` → peut uniquement lire les Todos
- ✅ Containerisation avec **Docker** et **Docker Compose**
- ✅ Documentation interactive avec **Scalar UI**

---

## Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MongoDB](https://www.mongodb.com/try/download/community) **OU** [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

## Démarrage rapide

### Option 1 — Docker Compose (recommandé)

Lance l'API et MongoDB en une seule commande :

```bash
docker-compose up --build
```

L'API sera disponible sur **http://localhost:8080**

### Option 2 — Exécution locale (sans Docker)

1. Assurez-vous que MongoDB tourne sur `localhost:27017`
2. Restaurer les dépendances et lancer l'application :

```bash
dotnet restore
dotnet run
```

L'API sera disponible sur **https://localhost:7xxx** (port affiché dans le terminal).

---

## Documentation interactive

En mode développement, accédez à **Scalar UI** :

```
https://localhost:{port}/scalar/v1
```

---

## Endpoints

### Authentification (public)

| Méthode | Route                  | Description                  |
|---------|------------------------|------------------------------|
| POST    | `/api/auth/register`   | Créer un compte              |
| POST    | `/api/auth/login`      | Se connecter, obtenir un JWT |

#### Exemple — Inscription

```json
POST /api/auth/register
{
  "username": "alice",
  "password": "MonMotDePasse123!",
  "role": "admin"
}
```

#### Exemple — Connexion

```json
POST /api/auth/login
{
  "username": "alice",
  "password": "MonMotDePasse123!"
}
```

Réponse :
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "role": "admin"
}
```

---

### Todos (authentification requise)

Ajouter le header dans chaque requête :
```
Authorization: Bearer <votre_token_jwt>
```

| Méthode | Route                  | Rôle requis | Description           |
|---------|------------------------|-------------|-----------------------|
| GET     | `/api/todoitems`       | user, admin | Lister tous les Todos |
| GET     | `/api/todoitems/{id}`  | user, admin | Obtenir un Todo       |
| POST    | `/api/todoitems`       | admin       | Créer un Todo         |
| PUT     | `/api/todoitems/{id}`  | admin       | Modifier un Todo      |
| DELETE  | `/api/todoitems/{id}`  | admin       | Supprimer un Todo     |

---

## Structure du projet

```
TodoApi/
├── Controllers/
│   ├── AuthController.cs        # Register / Login
│   └── TodoItemsController.cs   # CRUD Todos (protégé)
├── Models/
│   ├── TodoItem.cs              # Entité MongoDB
│   ├── TodoItemDTO.cs           # DTO exposé
│   ├── User.cs                  # Entité utilisateur
│   └── MongoDbSettings.cs       # Config MongoDB
├── Services/
│   ├── TodoService.cs           # Accès MongoDB Todos
│   └── UserService.cs           # Accès MongoDB Users
├── Program.cs                   # Configuration DI, JWT, Middleware
├── appsettings.json             # Configuration
├── Dockerfile                   # Image Docker
└── docker-compose.yml           # Orchestration Docker
```

---

## Variables d'environnement (Docker)

| Variable                           | Description                         |
|------------------------------------|-------------------------------------|
| `MongoDb__ConnectionString`        | URI de connexion MongoDB            |
| `MongoDb__DatabaseName`            | Nom de la base de données           |
| `Jwt__Key`                         | Clé secrète JWT (min. 32 caractères)|
| `Jwt__Issuer`                      | Issuer du token JWT                 |
| `Jwt__Audience`                    | Audience du token JWT               |

> ⚠️ **Production** : changez impérativement la valeur de `Jwt__Key` par une clé forte et unique.
