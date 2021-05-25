# Commenting system using EF Core 5 and PostgreSQL

Commenting system backend where we can -
1. Register and login user
2. Create, Read, Update and Delete Posts and Comments

---
3 main entities into play -
- User
```
{
    "first_name": "John",
    "last_name": "Doe",
    "gender_abbreviation": "M",
    "date_of_birth": "1970-01-01T09:30:00",
    "phone": "1234567890",
    "username": "johndoe123",
    "email": "johndoe@email.com",
    "password": "1234567890",
    "compare_password": "1234567890"
}
```

- Post
```
{
    "title": "My first post",
    "body": "Post body",
    "user_id": 1
}
```

- Comment
```
{
    "body": "My first comment on the first post",
    "user_id": 1,
    "post_id": 1
}
```
---
**Steps to recreate**

1. Scaffold the project structure

```
mkdir src
cd src
dotnet new webapi -o Api

dotnet new classlib -o Core
dotnet new classlib -o Infrastructure

cd ..
dotnet new sln
dotnet sln add .\src\Api
dotnet sln add .\src\Core
dotnet sln add .\src\Infrastructure
```

2. Build connection string based on [`NPGSQL docs`](https://www.connectionstrings.com/npgsql/standard/)

3. Install the following nuget packages -
```
AutoMapper.Extensions.Microsoft.DependencyInjection -> For AutoMapper (Api)
Npgsql.EntityFrameworkCore.PostgreSQL -> EF Core for PostgreSQL (Infrastructure)
Microsoft.EntityFrameworkCore.Tools -> EF Core Tools (Api)
LinqKit.Microsoft.EntityFrameworkCore -> For building predicate expression trees for EF Core 5 (Infrastructure)
BCrypt.Net-Next -> For password hashing using BCrypt Algorithm (Infrastructure)
EntityFrameworkCore.Exceptions.PostgreSQL -> For catching PostgreSQL exceptions in .NET friendly way(Core)
```

4. Apply the migrations
```
Add-Migration InitialDbCreation
Update-Database
```

5. Add business logic

6. Test using [`Postman collection`](https://www.getpostman.com/collections/04ea831f0d2f22111682)