using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.LuceneProvider.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Elision.Feature.Library.SitemapXml
{
    public class SitemapSesarchResultItem : SearchResultItem
    {
        [IndexField("_haspresentation")]
        public bool HasPresentation { get; set; }

        [TypeConverter(typeof(IndexFieldBooleanValueConverter))]
        [IndexField("HideFromSitemapXml")]
        public virtual bool HideFromSitemapXml { get; set; }

        [IndexField("SitemapXmlPriority")]
        public virtual string SitemapXmlPriorityRaw { get; set; }

        [TypeConverter(typeof(IndexFieldFloatingPointNumberValueConverter))]
        [IndexField("SitemapXmlPriority")]
        public virtual double SitemapXmlPriority { get; set; }
        
        [IndexField("SitemapXmlUpdateFrequency")]
        public virtual string SitemapXmlUpdateFrequencyRaw { get; set; }

        [IgnoreIndexField]
        public virtual PageUpdateFrequency SitemapXmlUpdateFrequency
        {
            get
            {
                PageUpdateFrequency parsed;
                return Enum.TryParse(SitemapXmlUpdateFrequencyRaw, true, out parsed)
                    ? parsed
                    : PageUpdateFrequency.Unknown;
            }
            set { SitemapXmlUpdateFrequencyRaw = value.ToString(); }
        }
    }
}