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
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Objects
{
    public class DrawRectangleAroundText
    {
        public static readonly string DEST = "results/sandbox/objects/draw_rectangle_around_text.pdf";
        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new DrawRectangleAroundText().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            Paragraph p = new Paragraph("This is a long paragraph that doesn't"
                                        + "fit the width we defined for the simple column of the"
                                        + "ColumnText object, so it will be distributed over several"
                                        + "lines (and we don't know in advance how many).");

            Rectangle firstRect = new Rectangle(120, 500, 130, 280);
            new Canvas(canvas, pdfDoc, firstRect)
                .Add(p);
            canvas.Rectangle(firstRect);
            canvas.Stroke();

            // In the lines below the comment we try to reproduce the iText5 method to achieve the result
            // However it's much more simple to use the next line
            // p.SetBorder(new SolidBorder(1));
            // Or you can implement your own ParagraphRenderer and change the behaviour of drawBorder(DrawContext)
            // or draw(DrawContext)
            Rectangle secRect = new Rectangle(300, 500, 130, 280);
            ParagraphRenderer renderer = (ParagraphRenderer) p.CreateRendererSubTree().SetParent(doc.GetRenderer());
            float height = renderer.Layout(new LayoutContext(new LayoutArea(0, secRect)))
                .GetOccupiedArea().GetBBox().GetHeight();

            new Canvas(canvas, pdfDoc, secRect)
                .Add(p);
            canvas.Rectangle(secRect.GetX(), secRect.GetY() + secRect.GetHeight() - height, secRect.GetWidth(), height);
            canvas.Stroke();

            doc.Close();
        }
    }
}