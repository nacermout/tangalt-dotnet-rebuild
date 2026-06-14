using Microsoft.EntityFrameworkCore;
using TangaltAPI.Data;
using TangaltAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TangaltContext>(options =>
    options.UseSqlite("Data Source=tangalt.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
});

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
});

// Supprimer un article
app.MapDelete("/api/articles/{id}", async (int id, TangaltContext context) =>
{
    var article = await context.Articles.FindAsync(id);
    if (article == null) return Results.NotFound();

    context.Articles.Remove(article);
    await context.SaveChangesAsync();
    return Results.NoContent();
});


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
});

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
});

// Supprimer un auteur
app.MapDelete("/api/authors/{id}", async (int id, TangaltContext context) =>
{
    var author = await context.Authors.FindAsync(id);
    if (author == null) return Results.NotFound();

    context.Authors.Remove(author);
    await context.SaveChangesAsync();
    return Results.NoContent();
});


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
});

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
});

// Supprimer une catégorie
app.MapDelete("/api/categories/{id}", async (int id, TangaltContext context) =>
{
    var category = await context.Categories.FindAsync(id);
    if (category == null) return Results.NotFound();

    context.Categories.Remove(category);
    await context.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();