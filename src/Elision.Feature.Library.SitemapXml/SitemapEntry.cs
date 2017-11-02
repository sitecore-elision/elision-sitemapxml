using System;

namespace Elision.Feature.Library.SitemapXml
{
    public class SitemapEntry
    {
        public string Location { get; set; }
        public DateTime LastModified { get; set; }
        public double Priority { get; set; }
        public PageUpdateFrequency ChangeFrequency { get; set; }
    }
}