using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace FootParser.ContentParsers;

public static class PaginationHandler
{
    public static int GetTotalPageNumber(HtmlDocument htmlDocument)
    {
        var pagination = htmlDocument.DocumentNode.SelectNodes("//ul[contains(@class, 'pagination-section')]");

        if (pagination == null)
        {
            return 1;
        }

        var arrowNodes = htmlDocument.DocumentNode.SelectNodes("//li[contains(@class, 'pagination-section__item--arrow')]");
        var lastLiNode =
            htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='pagination-section']/li[last()]");
        var aNode = lastLiNode.SelectSingleNode(".//a");
        var liNodes = htmlDocument.DocumentNode.SelectNodes("//li[contains(@class, 'pagination-section__item')]");
        int pageNumber;

        if (arrowNodes.Count == 4)
        {
            var dataPageValue = aNode.GetAttributeValue("data-page", "");
            int.TryParse(dataPageValue, out pageNumber);
            pageNumber++;
        }
        else
        {
            pageNumber = liNodes.Count - 2;
        }

        return pageNumber;
    }

    public static int GetCurrentPageNumber(HtmlDocument htmlDocument)
    {
        var currentPage =
            htmlDocument.DocumentNode.SelectSingleNode("//li[contains(@class, 'pagination-section__item--active')]");

        return currentPage == null ? 1 : int.Parse(currentPage.InnerText);
    }
}