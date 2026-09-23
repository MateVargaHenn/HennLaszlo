using Microsoft.AspNetCore.Routing;
using Modules.Content.Presentation.ContentPages.Upsert;
using Modules.Content.Presentation.ContentPages.GetAdminByKey;
using Modules.Content.Presentation.ContentPages.Publish;
using Modules.Content.Presentation.ContentPages.Unpublish;
using Modules.Content.Presentation.ContentPages.GetPublishedByKey;
using Modules.Content.Presentation.ContentPages.GetAdminList;
using Modules.Content.Presentation.Articles.Create;
using Modules.Content.Presentation.Articles.Publish;
using Modules.Content.Presentation.Articles.Unpublish;
using Modules.Content.Presentation.Articles.GetPublishedList;
using Modules.Content.Presentation.Articles.GetPublishedBySlug;
using Modules.Content.Presentation.Articles.GetAdminList;
using Modules.Content.Presentation.Articles.GetAdminById;
using Modules.Content.Presentation.Articles.Update;
using Modules.Content.Presentation.Articles.Delete;


namespace Modules.Content.Presentation;

public static class ContentEndpoints
{
    public static IEndpointRouteBuilder
        MapContentEndpoints(
            this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapUpsertContentPage();
        endpoints.MapGetAdminContentPages();
		endpoints.MapGetAdminContentPageByKey();
        endpoints.MapPublishContentPage();
        endpoints.MapUnpublishContentPage();
        endpoints.MapGetPublishedContentPageByKey();
        endpoints.MapCreateArticle();
        endpoints.MapPublishArticle();
        endpoints.MapUnpublishArticle();
        endpoints.MapGetPublishedArticles();
        endpoints.MapGetPublishedArticleBySlug();
        endpoints.MapGetAdminArticles();
        endpoints.MapGetAdminArticleById();
        endpoints.MapUpdateArticle();
        endpoints.MapDeleteArticle();

        return endpoints;
    }
}