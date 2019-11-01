/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class LinkInTableCell
    {
        public static readonly string DEST = "../../results/sandbox/tables/link_in_table_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LinkInTableCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1)).UseAllAvailableWidth();

            // Part of the content is a link:
            Paragraph paragraph = new Paragraph();
            paragraph.Add("iText at the ");
            Link chunk = new Link("European Business Awards",
                PdfAction.CreateURI("https://itextpdf.com/en/events/itext-european-business-awards-gala-milan"));
            paragraph.Add(chunk);
            paragraph.Add(" gala in Milan");
            table.AddCell(paragraph);

            // The complete cell is a link:
            Cell cell = new Cell().Add(new Paragraph("Help us win a European Business Award!"));
            cell.SetNextRenderer(new LinkInCellRenderer(cell,
                "http://itextpdf.com/blog/help-us-win-european-business-award"));
            table.AddCell(cell);
            
            // The complete cell is a link (using SetAction() directly on cell):
            cell = new Cell().Add(new Paragraph(
                "IText becomes Belgium’s National Public Champion in the 2016 European Business Awards"));
            cell.SetAction(PdfAction.CreateURI(
                "http://itextpdf.com/en/blog/itext-becomes-belgiums-national-public-champion-2016-european-business-awards"));
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private class LinkInCellRenderer : CellRenderer
        {
            private string url;

            public LinkInCellRenderer(Cell modelElement, string url)
                : base(modelElement)
            {
                this.url = url;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new LinkInCellRenderer((Cell) modelElement, url);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);

                PdfLinkAnnotation linkAnnotation = new PdfLinkAnnotation(GetOccupiedAreaBBox());
                linkAnnotation.SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT);
                linkAnnotation.SetAction(PdfAction.CreateURI(url));
                drawContext.GetDocument().GetLastPage().AddAnnotation(linkAnnotation);
            }
        }
    }
}