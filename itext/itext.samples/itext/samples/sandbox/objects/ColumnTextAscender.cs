/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ColumnTextAscender
    {
        public static readonly string DEST = "results/sandbox/objects/column_text_ascender.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ColumnTextAscender().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Rectangle[] areas = new Rectangle[]
            {
                new Rectangle(50, 750, 200, 50),
                new Rectangle(300, 750, 200, 50)
            };

            // For canvas usage one should create a page
            pdfDoc.AddNewPage();
            foreach (Rectangle rect in areas)
            {
                new PdfCanvas(pdfDoc.GetFirstPage()).SetLineWidth(0.5f).SetStrokeColor(ColorConstants.RED)
                    .Rectangle(rect).Stroke();
            }

            doc.SetRenderer(new ColumnDocumentRenderer(doc, areas));
            AddColumn(doc, false);
            AddColumn(doc, true);
            doc.Close();
        }

        private static void AddColumn(Document doc, bool useAscender)
        {
            Paragraph p = new Paragraph("This text is added at the top of the column.");
            
            // SetUseAscender (boolean) - this is the approach used in iText5.
            // We can change leading instead, the result will be the same
            if (useAscender)
            {
                p.SetMargin(0);
                
                // The Leading is a spacing between lines of text
                p.SetMultipliedLeading(1);
            }

            doc.Add(p);
        }
    }
}