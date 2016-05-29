using System.Linq;
using Sitecore.ContentSearch;

namespace Elision.SitemapXml.Pipelines
{
    public class GetSitemapXmlEntries : IGetSitemapXmlEntriesProcessor
    {
        public void Process(GetSitemapXmlEntriesArgs args)
        {
            ISearchIndex searchIndex = null;
            if (!string.IsNullOrEmpty(args.IndexName))
                searchIndex = ContentSearchManager.GetIndex(args.IndexName);
            else if (args.SitemapRootItem != null)
                searchIndex = ContentSearchManager.GetIndex((SitecoreIndexableItem) args.SitemapRootItem);

            if (searchIndex == null)
                return;

            args.IndexName = searchIndex.Name;

            using (var ctx = searchIndex.CreateSearchContext())
            {
                var results = ctx.GetQueryable<SitemapSesarchResultItem>()
                    .Where(i => i.Paths.Contains(args.SitemapRootItem.ID) && i.Language == args.Language.Name)
                    .Where(i => !i.HideFromSitemapXml);

                args.Entries.AddRange(results.Select(x => new SitemapEntry
                {
                    Location = x.Url,
                    LastModified = x.Updated,
                    Priority = string.IsNullOrWhiteSpace(x.SitemapXmlPriorityRaw) ? 0.5 : x.SitemapXmlPriority,
                    ChangeFrequency = x.SitemapXmlUpdateFrequency
                }));
            }
        }
    }
}