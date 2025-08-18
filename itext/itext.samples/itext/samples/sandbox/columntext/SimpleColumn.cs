using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Columntext
{
   
    // SimpleColumn.cs
    // 
    // This class demonstrates how to create text in a restricted column width.
    // It creates a small PDF with a single paragraph of specified width and position,
    // showing how to control text alignment and spacing in a constrained area.
 
    public class SimpleColumn
    {
        public static readonly String DEST = "results/sandbox/columntext/simple_column.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleColumn().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(100, 120));

            Paragraph paragraph = new Paragraph("REALLLLLLLLLLY LONGGGGGGGGGG text").SetFontSize(4.5f);

            paragraph.SetWidth(61);
            doc.ShowTextAligned(paragraph, 9, 85, TextAlignment.LEFT);

            doc.Close();
        }
    }
}