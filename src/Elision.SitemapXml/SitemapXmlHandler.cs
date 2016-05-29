using System;
using System.Linq;
using System.Text;
using System.Web;
using Elision.SitemapXml.Pipelines;
using Sitecore.Pipelines;

namespace Elision.SitemapXml
{
    public class SitemapXmlHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            var args = new GetSitemapXmlEntriesArgs
            {
                HttpContext = context
            };
            CorePipeline.Run("elision.getSitemapXmlEntries", args);

            args.HttpContext.Response.Headers.Add("X-Sitemap-Index-Used", args.IndexName.Or("None"));
            args.HttpContext.Response.Headers.Add("X-Sitemap-Root-Item", args.SitemapRootItem?.Paths?.FullPath?.Or(null) ?? "None");
            args.HttpContext.Response.Headers.Add("X-Sitemap-Site", args.Site?.Name ?? "None");

            if (!args.Entries.Any() && (args.Site == null || args.SitemapRootItem == null || string.IsNullOrWhiteSpace(args.IndexName)))
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.ContentType = "text/xml";
            context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                                   "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" + Environment.NewLine);

            var sb = new StringBuilder();
            foreach (var node in args.Entries)
            {
                sb.Append("<url>")
                    .AppendFormat("<loc>{0}</loc>", XmlEscapeString(node.Location))
                    .AppendFormat("<lastmod>{0}</lastmod>", node.LastModified.ToString("yyyy-MM-dd"));

                if (node.ChangeFrequency != PageUpdateFrequency.Unknown)
                    sb.AppendFormat("<changefreq>{0}</changefreq>", node.ChangeFrequency.ToString().ToLowerInvariant());

                if (node.Priority >= 0 && node.Priority <= 1.0 && Math.Abs(node.Priority - 0.5) > 0.09)
                    sb.AppendFormat("<priority>{0:0.0}</priority>", node.Priority);

                sb.Append("</url>" + Environment.NewLine);
                context.Response.Write(sb.ToString());
                sb.Clear();
            }
            context.Response.Write("</urlset>");
        }

        protected virtual string XmlEscapeString(string value)
        {
            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }
    }
}