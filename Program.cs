using Microsoft.EntityFrameworkCore;
using TangaltAPI.Data;
using TangaltAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("TangaltFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => 
                !string.IsNullOrEmpty(origin) && 
                Uri.TryCreate(origin, UriKind.Absolute, out var uri) && 
                (uri.Host == "localhost" || uri.Host == "127.0.0.1"))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Configuration JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<TangaltContext>(options =>
    options.UseSqlite("Data Source=tangalt.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("TangaltFrontend");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Remplir le frigo au démarrage
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TangaltContext>();
    // BDD automatique 
    context.Database.EnsureCreated();
    DbSeeder.Seed(context);
}


// Lire un article
app.MapGet("/api/articles", async (TangaltContext context) =>
{
    var articles = await context.Articles
        .Include(a => a.Author)
        .Include(a => a.Category)
        .ToListAsync();
    return Results.Ok(articles);
});
app.MapGet("/api/articles/{id}", async (int id, TangaltContext context) =>
{
    var article = await context.Articles
        .Include(a => a.Author)
        .Include(a => a.Category)
        .FirstOrDefaultAsync(a => a.Id == id);

    if (article == null)
        return Results.NotFound();

    return Results.Ok(article);
});

// Créer un article
app.MapPost("/api/articles", async (Article article, TangaltContext context) =>
{
    article.PublishedAt = DateTime.Now;
    context.Articles.Add(article);
    await context.SaveChangesAsync();
    return Results.Created($"/api/articles/{article.Id}", article);
}).RequireAuthorization();

// Modifier un article
app.MapPut("/api/articles/{id}", async (int id, Article articleModifie, TangaltContext context) =>
{
    var article = await context.Articles.FindAsync(id);
    if (article == null) return Results.NotFound();

    article.Title = articleModifie.Title;
    article.Content = articleModifie.Content;
    article.Language = articleModifie.Language;
    article.IsPublished = articleModifie.IsPublished;
    article.Slug = articleModifie.Slug;
    article.ImageUrl = articleModifie.ImageUrl;

    await context.SaveChangesAsync();
    return Results.Ok(article);
}).RequireAuthorization();

// Supprimer un article
app.MapDelete("/api/articles/{id}", async (int id, TangaltContext context) =>
{
    var article = await context.Articles.FindAsync(id);
    if (article == null) return Results.NotFound();

    context.Articles.Remove(article);
    await context.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();


// Lire tous les auteurs
app.MapGet("/api/authors", async (TangaltContext context) =>
{
    var authors = await context.Authors.ToListAsync();
    return Results.Ok(authors);
});

// Lire un auteur précis
app.MapGet("/api/authors/{id}", async (int id, TangaltContext context) =>
{
    var author = await context.Authors.FindAsync(id);
    if (author == null) return Results.NotFound();
    return Results.Ok(author);
});

// Créer un auteur
app.MapPost("/api/authors", async (Author author, TangaltContext context) =>
{
    context.Authors.Add(author);
    await context.SaveChangesAsync();
    return Results.Created($"/api/authors/{author.Id}", author);
}).RequireAuthorization();

// Modifier un auteur
app.MapPut("/api/authors/{id}", async (int id, Author authorModifie, TangaltContext context) =>
{
    var author = await context.Authors.FindAsync(id);
    if (author == null) return Results.NotFound();

    author.Name = authorModifie.Name;
    author.LastName = authorModifie.LastName;
    author.Email = authorModifie.Email;
    author.Bio = authorModifie.Bio;
    author.AvatarUrl = authorModifie.AvatarUrl;

    await context.SaveChangesAsync();
    return Results.Ok(author);
}).RequireAuthorization();

// Supprimer un auteur
app.MapDelete("/api/authors/{id}", async (int id, TangaltContext context) =>
{
    var author = await context.Authors.FindAsync(id);
    if (author == null) return Results.NotFound();

    context.Authors.Remove(author);
    await context.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();


// Lire tous les auteurs
app.MapGet("/api/categories", async (TangaltContext context) =>
{
    var categories = await context.Categories.ToListAsync();
    return Results.Ok(categories);
});

// Lire une catégorie précise
app.MapGet("/api/categories/{id}", async (int id, TangaltContext context) =>
{
    var category = await context.Categories.FindAsync(id);
    if (category == null) return Results.NotFound();
    return Results.Ok(category);
});

// Créer une catégorie
app.MapPost("/api/categories", async (Category category, TangaltContext context) =>
{
    context.Categories.Add(category);
    await context.SaveChangesAsync();
    return Results.Created($"/api/categories/{category.Id}", category);
}).RequireAuthorization();

// Modifier une catégorie
app.MapPut("/api/categories/{id}", async (int id, Category categoryModifie, TangaltContext context) =>
{
    var category = await context.Categories.FindAsync(id);
    if (category == null) return Results.NotFound();

    category.Name = categoryModifie.Name;
    category.Description = categoryModifie.Description;
    category.Color = categoryModifie.Color;     
    await context.SaveChangesAsync();
    return Results.Ok(category);
}).RequireAuthorization();

// Supprimer une catégorie
app.MapDelete("/api/categories/{id}", async (int id, TangaltContext context) =>
{
    var category = await context.Categories.FindAsync(id);
    if (category == null) return Results.NotFound();

    context.Categories.Remove(category);
    await context.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

// Login — POST /api/auth/login
app.MapPost("/api/auth/login", async (LoginRequest request, TangaltContext context) =>
{
    var user = await context.Users
        .FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        return Results.Unauthorized();

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: new[] { new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role) },
        expires: DateTime.Now.AddHours(24),
        signingCredentials: creds
    );

    return Results.Ok(new {
        token = new JwtSecurityTokenHandler().WriteToken(token),
        email = user.Email,
        role = user.Role
    });
});
app.Run();
record LoginRequest(string Email, string Password);