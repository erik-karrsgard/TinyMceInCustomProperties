@using System.Web.Optimization
@using EPiServer.Framework.Web.Mvc.Html
@model IPageViewModel<SitePageData>
<!DOCTYPE html>
<html lang="@Model.CurrentPage.LanguageBranch">
<html>
    
    <head>
        <meta charset="utf-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=10" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>@Model.CurrentPage.MetaTitle</title>
        @Styles.Render("~/bundles/css")
        @Scripts.Render("~/bundles/js")
        @Html.RequiredClientResources("Header") @*Enable components to require resources. For an example, see the view for VideoBlock.*@
    </head>

    <body>
        @Html.RenderEPiServerQuickNavigator()
        @Html.FullRefreshPropertiesMetaData()
        <div class="container">
            @if(!Model.Layout.HideHeader)
            {
                Html.RenderPartial("Header", Model);
            }
            @RenderBody()
            @if(!Model.Layout.HideHeader)
            {
                Html.RenderPartial("Footer", Model);
            }
        </div>
        @Html.RequiredClientResources("Footer")
    </body>
</html>
