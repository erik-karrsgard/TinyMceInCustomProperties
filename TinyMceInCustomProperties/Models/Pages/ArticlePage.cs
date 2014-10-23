using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace TinyMceInCustomProperties.Models.Pages
{
    /// <summary>
    /// Used primarily for publishing news articles on the website
    /// </summary>
    [SiteContentType(
        GroupName = Global.GroupNames.News,
        GUID = "AEECADF2-3E89-4117-ADEB-F8D43565D2F4")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    public class ArticlePage : StandardPage
    {
        [Display(Order = 305, GroupName = Global.GroupNames.MetaData)]
        [UIHint(Global.SiteUIHints.ExtendedXhtml)]
        [CultureSpecific]
        public virtual XhtmlString ExtendedXhtml { get; set; }
    }
}
