using System;
using System.Linq;

namespace Elision.SitemapXml.Pipelines
{
    public class GetSitemapXmlSettings : IGetSitemapXmlEntriesProcessor
    {
        public void Process(GetSitemapXmlEntriesArgs args)
        {
            args.IndexName = null;
            args.Site = Sitecore.Configuration.Factory.GetSiteInfoList()
                .FirstOrDefault(i => string.Equals(i.HostName, args.HttpContext.Request.Url.Host, StringComparison.CurrentCultureIgnoreCase));

            if (args.Site == null || (args.Site.Port > 0 && args.Site.Port != args.HttpContext.Request.Url.Port))
                return;

            Sitecore.Context.SetActiveSite(args.Site.Name);

            args.SitemapRootItem = Sitecore.Context.Database.GetItem(args.Site.RootPath + args.Site.StartItem);
            args.Language = Sitecore.Context.Language;
        }
    }
}