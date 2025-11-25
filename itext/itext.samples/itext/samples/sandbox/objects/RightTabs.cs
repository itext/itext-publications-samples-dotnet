using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
   
    // RightTabs.cs
    //
    // Example showing how to use right-aligned tabs for text positioning.
    // Demonstrates aligning text to left and right on the same line with tabs.
 
    public class RightTabs
    {
        public static readonly String DEST = "results/sandbox/objects/right_tabs.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RightTabs().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            Rectangle pageSize = pdfDoc.GetDefaultPageSize();
            float width = pageSize.GetWidth() - document.GetLeftMargin() - document.GetRightMargin();

            List<TabStop> tabStops = new List<TabStop>();
            tabStops.Add(new TabStop(width, TabAlignment.RIGHT));

            Paragraph paragraph = new Paragraph()
                .AddTabStops(tabStops)
                .Add("ABCD")
                .Add(new Tab())
                .Add("EFGH");
            document.Add(paragraph);

            paragraph = new Paragraph()
                .AddTabStops(tabStops)
                .Add("Text to the left")
                .Add(new Tab())
                .Add("Text to the right");
            document.Add(paragraph);

            paragraph = new Paragraph()
                .AddTabStops(tabStops)
                .Add("01234")
                .Add(new Tab())
                .Add("56789");
            document.Add(paragraph);

            paragraph = new Paragraph()
                .AddTabStops(tabStops)
                .Add("iText 5 is old")
                .Add(new Tab())
                .Add("iText is new");
            document.Add(paragraph);

            document.Close();
        }
    }
}