using EPiServer.Core;

namespace TinyMceInCustomProperties.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
