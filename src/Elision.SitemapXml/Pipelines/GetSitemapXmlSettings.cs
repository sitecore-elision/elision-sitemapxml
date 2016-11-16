namespace Elision.SitemapXml.Pipelines
{
    public class GetSitemapXmlSettings : IGetSitemapXmlEntriesProcessor
    {
        public void Process(GetSitemapXmlEntriesArgs args)
        {
            args.IndexName = null;
            args.Site = Sitecore.Sites.SiteContextFactory.GetSiteContext(Sitecore.Web.WebUtil.GetRequestUri().Host,
                args.HttpContext.Request.FilePath.ToLowerInvariant(), args.HttpContext.Request.Url.Port)?.SiteInfo;            

            if (args.Site == null || (args.Site.Port > 0 && args.Site.Port != args.HttpContext.Request.Url.Port))
            {
                Sitecore.Diagnostics.Log.Error("Error getting sitemap - could not resolve site.", this);
                return;
            }
                

            Sitecore.Context.SetActiveSite(args.Site.Name);

            args.SitemapRootItem = Sitecore.Context.Database.GetItem(args.Site.RootPath + args.Site.StartItem);
            args.Language = Sitecore.Context.Language;
        }
    }
}