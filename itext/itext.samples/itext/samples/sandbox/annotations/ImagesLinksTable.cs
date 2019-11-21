/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Annotations
{
    public class ImagesLinksTable
    {
        public static readonly String DEST = "results/sandbox/annotations/images_links_table.pdf";

        public static readonly String IMG = "../../resources/img/info.png";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ImagesLinksTable().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image img = new Image(ImageDataFactory.Create(IMG));
            Paragraph anchor = new Paragraph().Add(img);
            anchor.SetProperty(Property.ACTION, PdfAction.CreateURI("https://lowagie.com/"));
            
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddCell(anchor);
            table.AddCell("A");
            table.AddCell("B");
            table.AddCell("C");

            img = new Image(ImageDataFactory.Create(IMG));
            img.SetNextRenderer(new LinkImageRenderer(img));
            table.AddCell(img);

            doc.Add(table);

            doc.Close();
        }

        protected class LinkImageRenderer : ImageRenderer
        {
            public LinkImageRenderer(Image image)
                : base(image)
            {
            }

            public override IRenderer GetNextRenderer()
            {
                return new LinkImageRenderer((Image) modelElement);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                PdfAnnotation annotation = new PdfLinkAnnotation(GetOccupiedAreaBBox())
                    .SetAction(PdfAction.CreateURI("https://lowagie.com/bio"));
                drawContext.GetDocument().GetLastPage().AddAnnotation(annotation);
            }
        }
    }
}