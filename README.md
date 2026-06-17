# Tangalt API — ASP.NET Core 8

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Railway](https://img.shields.io/badge/Railway-deployed-0B0D0E?logo=railway)
![SQLite](https://img.shields.io/badge/SQLite-production-003B57?logo=sqlite)

Backend REST API pour Tangalt — magazine littéraire bilingue (FR / Tamazight).
Construit en ASP.NET Core 8, déployé sur Railway.

## 🌐 Démo live

- **API** : https://tangalt-dotnet-rebuild-production.up.railway.app
- **Swagger** : https://tangalt-dotnet-rebuild-production.up.railway.app/swagger
- **Frontend** : https://tangalt-react-csharp.vercel.app

## 🛠 Stack

| Couche       | Technologie              |
|--------------|--------------------------|
| Framework    | ASP.NET Core 8 (Minimal API) |
| ORM          | Entity Framework Core 8  |
| Base de données | SQLite (production)   |
| Auth         | JWT Bearer + BCrypt      |
| Déploiement  | Railway (Docker)         |
| Documentation| Swagger / OpenAPI        |

## 🏗 Architecture

```
TangaltAPI/
├── Models/          # Article, Author, Category, User
├── Data/            # TangaltContext (EF Core) + DbSeeder
├── Program.cs       # Endpoints + DI + Auth + CORS
├── appsettings.json # Config (JWT, connexion DB)
└── Migrations/      # Historique EF Core
```

## 📡 Endpoints

### Articles
| Méthode | Route                  | Auth | Description           |
|---------|------------------------|------|-----------------------|
| GET     | /api/articles          | —    | Tous les articles     |
| GET     | /api/articles/{id}     | —    | Un article            |
| POST    | /api/articles          | JWT  | Créer un article      |
| PUT     | /api/articles/{id}     | JWT  | Modifier un article   |
| DELETE  | /api/articles/{id}     | JWT  | Supprimer un article  |

### Auteurs
| Méthode | Route                  | Auth | Description           |
|---------|------------------------|------|-----------------------|
| GET     | /api/authors           | —    | Tous les auteurs      |
| GET     | /api/authors/{id}      | —    | Un auteur             |
| POST    | /api/authors           | JWT  | Créer un auteur       |
| PUT     | /api/authors/{id}      | JWT  | Modifier un auteur    |
| DELETE  | /api/authors/{id}      | JWT  | Supprimer un auteur   |

### Catégories
| Méthode | Route                  | Auth | Description           |
|---------|------------------------|------|-----------------------|
| GET     | /api/categories        | —    | Toutes les catégories |
| POST    | /api/categories        | JWT  | Créer une catégorie   |
| PUT     | /api/categories/{id}   | JWT  | Modifier              |
| DELETE  | /api/categories/{id}   | JWT  | Supprimer             |

### Auth
| Méthode | Route                  | Auth | Description           |
|---------|------------------------|------|-----------------------|
| POST    | /api/auth/login        | —    | Login → token JWT     |

## 🚀 Lancer en local

```bash
# Prérequis : .NET SDK 8.0+
git clone https://github.com/nacermout/tangalt-dotnet-rebuild.git
cd tangalt-dotnet-rebuild

# Variables d'environnement (créer appsettings.Development.json)
# Jwt:Key, Jwt:Issuer, Jwt:Audience

dotnet restore
dotnet run

# API disponible sur http://localhost:8080
# Swagger sur http://localhost:8080/swagger
```

## 📐 Modèle de données

```
Article
  ├── Id, Title, Slug
  ├── Content, Summary
  ├── Language (fr / tiz)
  ├── IsPublished, PublishedAt
  ├── ImageUrl
  ├── AuthorId → Author
  └── CategoryId → Category

Author
  └── Id, Name, LastName, Email, Bio, AvatarUrl

Category
  └── Id, Name, Description, Color
```

## 🧑‍💻 Contexte

Projet portfolio construit en 8 semaines pour démontrer la maîtrise
de l'écosystème Microsoft (.NET / C# / Azure) dans le cadre d'une
candidature développeur full-stack.

Tangalt (tangalt.com) est un magazine littéraire bilingue franco-kabyle
fondé par l'auteur de ce projet.

---
Nacer M. · 2026 · nacermout/tangalt-dotnet-rebuild