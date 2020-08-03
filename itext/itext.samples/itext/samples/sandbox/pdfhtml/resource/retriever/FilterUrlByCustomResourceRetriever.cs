using System;
using System.IO;
using iText.Html2pdf;
using iText.StyledXmlParser.Resolver.Resource;

namespace iText.Samples.Sandbox.Pdfhtml.Resource.Retriever
{
    public class FilterUrlByCustomResourceRetriever
    {
        public static readonly string SRC = "../../../resources/pdfhtml/FilterUrlByCustomResourceRetriever/";
        public static readonly string DEST = "results/sandbox/pdfhtml/FilterUrlByCustomResourceRetriever.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            string htmlSource = SRC + "FilterUrlByCustomResourceRetriever.html";

            new FilterUrlByCustomResourceRetriever().ManipulatePdf(htmlSource, DEST);
        }

        protected void ManipulatePdf(String src, String dest)
        {
            IResourceRetriever resourceRetriever = new FilterResourceRetriever();
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetResourceRetriever(resourceRetriever);

            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }

        private class FilterResourceRetriever : DefaultResourceRetriever
        {
            protected override bool UrlFilter(Uri url)
            {
                // Specify that only urls, that are containing '/imagePath' text in the path, are allowed to handle
                return url.AbsolutePath.Contains("/imagePath");
            }
        }
    }
}