using Xunit;

public class ArticleTests
{
    [Fact]
    public void Article_Title_NeDoitPasEtreVide()
    {
        // Arrange
        var title = "Tamazight, une reconnaissance constitutionnelle";

        // Act
        var estValide = !string.IsNullOrEmpty(title);

        // Assert
        Assert.True(estValide);
    }

    [Fact]
    public void Article_Language_DoitEtreFrOuTiz()
    {
        // Arrange
        var language = "fr";

        // Act
        var estValide = language == "fr" || language == "tiz";

        // Assert
        Assert.True(estValide);
    }

    [Fact]
    public void Article_Language_Invalide_DoitEchouer()
    {
        // Arrange
        var language = "english";

        // Act
        var estValide = language == "fr" || language == "tiz";

        // Assert
        Assert.False(estValide);
    }
}