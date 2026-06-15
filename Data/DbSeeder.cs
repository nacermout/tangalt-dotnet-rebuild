using TangaltAPI.Models;
namespace TangaltAPI.Data
{
    public static class DbSeeder
    {
        public static void  Seed(TangaltContext context)
        {
            // Créer l'admin par défaut si aucun user n'existe
if (!context.Users.Any())
{
    context.Users.Add(new User
    {
        Email = "admin@tangalt.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tangalt2026!"),
        Role = "admin"
    });
    context.SaveChanges();
}
            if (context.Articles.Any()) return;
            var author = new Author
            {
                Name ="Maitre Hakim",
                LastName = "Saheb",
                Email = "hakaim@tangalt.com",
                Bio= "Maitre Hakim Saheb est un avocat chevronné spécialisé dans le droit des affaires et le droit de la propriété intellectuelle. Avec plus de 15 ans d'expérience, il a représenté avec succès de nombreuses entreprises et individus dans des litiges complexes. ",
                AvatarUrl = "https://tangalt.com/images/maitre-hakim-saheb.jpg",
            };
            var category = new Category
            {
                Name = "Tribune",
                Slug = "tribune",
                Description = "Opinions et prises de positions politiques",
                Color = "#8B5CF6",
            };
            context.Authors.Add(author);
            context.Categories.Add(category);
            context.SaveChanges();
            context.Articles.Add(new Article
            {
                Title = "Tamazight, une reconnaissance constitutionnelle à l'épreuve des faits",
                Content = "En cette Journée internationale de la langue maternelle...",
                Language = "fr",
                Slug = "tamazight-reconnaissance-constitutionnelle",
                IsPublished = true,
                PublishedAt = DateTime.Now,
                AuthorId = author.Id,
                CategoryId = category.Id
            });
            context.SaveChanges();
            context.Articles.AddRange(
    new Article
    {
        Title = "Asfel-iw — Mon retour au pays",
        Content = "Aqlaɣ-d seld yiwen waseggʷas n tɣellist i yiwen n tama...",
        Language = "tiz",
        Slug = "asfel-iw-mon-retour",
        IsPublished = true,
        PublishedAt = DateTime.Now,
        AuthorId = author.Id,
        CategoryId = category.Id
    },
    new Article
    {
        Title = "Mohammed Dib — Ou l'immense veilleur des mots",
        Content = "Mohammed Dib est l'une des figures les plus marquantes de la littérature algérienne...",
        Language = "fr",
        Slug = "mohammed-dib-veilleur-des-mots",
        IsPublished = true,
        PublishedAt = DateTime.Now,
        AuthorId = author.Id,
        CategoryId = category.Id
    },
    new Article
    {
        Title = "Si Emer Uhemza — Ameskar ur nettwassen",
        Content = "Yiwen n yimɣaren yellan d amaru amaziɣ ameqqran...",
        Language = "tiz",
        Slug = "si-emer-uhemza",
        IsPublished = true,
        PublishedAt = DateTime.Now,
        AuthorId = author.Id,
        CategoryId = category.Id
    }
);
context.SaveChanges();
        }
    }
}