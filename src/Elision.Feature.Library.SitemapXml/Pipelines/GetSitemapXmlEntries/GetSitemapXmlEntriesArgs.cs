using System.Collections.Generic;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Pipelines;
using Sitecore.Web;

namespace Elision.Feature.Library.SitemapXml.Pipelines.GetSitemapXmlEntries
{
    public class GetSitemapXmlEntriesArgs : PipelineArgs
    {
        public string IndexName { get; set; }
        public Item SitemapRootItem { get; set; }
        public HttpContext HttpContext { get; set; }
        public SiteInfo Site { get; set; }
        public Language Language { get; set; }

        public readonly List<SitemapEntry> Entries;

        public GetSitemapXmlEntriesArgs()
        {
            Entries = new List<SitemapEntry>();
        }
    }
}