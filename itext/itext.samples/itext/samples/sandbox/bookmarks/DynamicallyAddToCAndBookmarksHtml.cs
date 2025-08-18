using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.StyledXmlParser.Jsoup;
using iText.StyledXmlParser.Jsoup.Nodes;
using iText.StyledXmlParser.Jsoup.Select;

namespace iText.Samples.Sandbox.Bookmarks
{
   
    // DynamicallyAddToCAndBookmarksHtml.cs
    // 
    // This class demonstrates how to dynamically generate a table of contents and PDF bookmarks
    // from an HTML document. It parses H2 headings, creates navigable links and page references,
    // and adds corresponding PDF bookmarks.
 
    public class DynamicallyAddToCAndBookmarksHtml
    {
        public static readonly string DEST = "results/sandbox/bookmarks/DynamicallyAddToCAndBookmarksHtml.pdf";

        public static readonly string SRC = "../../../resources/pdfhtml/DynamicallyAddToCAndBookmarksHtml/original_file.html";

        private static readonly IList<string> IDS = new List<string>();

        static DynamicallyAddToCAndBookmarksHtml()
        {
            IDS.Add("random_id_1");
            IDS.Add("random_id_2");
            IDS.Add("random_id_3");
            IDS.Add("random_id_4");
            IDS.Add("random_id_5");
            IDS.Add("random_id_6");
        }

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DynamicallyAddToCAndBookmarksHtml().ManipulatePdf();
        }

        public void ManipulatePdf()
        {
            Document htmlDoc = Jsoup.Parse(new FileInfo(SRC), "UTF-8");

            // This is our Table of Contents aggregating element
            Element tocElement = htmlDoc.Body().PrependElement("div");
            tocElement.Append("<b>Table of contents</b>");

            // We are going to build a complex CSS
            StringBuilder tocStyles = new StringBuilder().Append("<style>");

            using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(DEST)))
            {
                PdfOutline bookmarks = pdfDocument.GetOutlines(false);
                Elements tocElements = htmlDoc.Select("h2");
                foreach (Element elem in tocElements)
                {
                    // Here we create an anchor to be able to refer to this element when generating page numbers and links
                    String id = elem.Attr("id");
                    if (string.IsNullOrEmpty(id))
                    {
                        id = generateId();
                        elem.Attr("id", id);
                    }

                    // CSS selector to show page numbers for a TOC entry
                    tocStyles.Append("*[data-toc-id=\"").Append(id)
                        .Append("\"] .toc-page-ref::after { content: target-counter(#").Append(id).Append(", page) }");

                    // Generating TOC entry as a small table to align page numbers on the right
                    Element tocEntry = tocElement.AppendElement("table");
                    tocEntry.Attr("style", "width: 100%");
                    Element tocEntryRow = tocEntry.AppendElement("tr");
                    tocEntryRow.Attr("data-toc-id", id);
                    Element tocEntryTitle = tocEntryRow.AppendElement("td");
                    tocEntryTitle.AppendText(elem.Text());
                    Element tocEntryPageRef = tocEntryRow.AppendElement("td");
                    tocEntryPageRef.Attr("style", "text-align: right");
                    // <span> is a placeholder element where target page number will be inserted
                    // It is wrapped by an <a> tag to create links pointing to the element in our document
                    tocEntryPageRef.Append("<a href=\"#" + id + "\"><span class=\"toc-page-ref\"></span></a>");

                    // Add bookmark
                    PdfOutline bookmark = bookmarks.AddOutline(elem.Text());
                    bookmark.AddAction(PdfAction.CreateGoTo(id));
                }

                tocStyles.Append("</style>");
                htmlDoc.Head().Append(tocStyles.ToString());
                String html = htmlDoc.OuterHtml();

                ConverterProperties converterProperties = new ConverterProperties().SetImmediateFlush(false);
                HtmlConverter.ConvertToDocument(html, pdfDocument, converterProperties).Close();
            }
        }

        private static String generateId()
        {
            // Usually random id can be generated, but for the purpose of testing we will use predefined ids.
            string id = IDS.Count == 0 ? null : IDS[0];
            IDS.RemoveAt(0);
            return id;
        }
    }
}