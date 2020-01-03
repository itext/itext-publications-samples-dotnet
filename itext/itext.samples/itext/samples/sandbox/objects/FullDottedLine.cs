/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class FullDottedLine
    {
        public static readonly string DEST = "results/sandbox/objects/full_dotted_line.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new FullDottedLine().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Before dotted line"));
            doc.Add(new LineSeparator(new CustomDottedLine(pdfDoc.GetDefaultPageSize())));
            doc.Add(new Paragraph("After dotted line"));

            doc.Close();
        }


        protected class CustomDottedLine : DottedLine
        {
            private Rectangle pageSize;

            public CustomDottedLine(Rectangle pageSize)
            {
                this.pageSize = pageSize;
            }

            public override void Draw(PdfCanvas canvas, Rectangle drawArea)
            {
                // Dotted line from the left edge of the page to the right edge.
                base.Draw(canvas, new Rectangle(pageSize.GetLeft(), drawArea.GetBottom(),
                    pageSize.GetWidth(), drawArea.GetHeight()));
            }
        }
    }
}