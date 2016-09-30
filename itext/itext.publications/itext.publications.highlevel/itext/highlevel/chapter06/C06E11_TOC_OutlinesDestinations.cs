/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;

using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter06 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C06E11_TOC_OutlinesDestinations {
        public const String SRC = "../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "../../results/chapter06/jekyll_hyde_outline2.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E11_TOC_OutlinesDestinations().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            pdf.GetCatalog().SetPageMode(PdfName.UseOutlines);
            // Initialize document
            Document document = new Document(pdf);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3))
                .SetFont(font).SetFontSize(11);
            StreamReader sr = File.OpenText(SRC);
            String line;
            Paragraph p;
            bool title = true;
            PdfOutline outline = null;
            while ((line = sr.ReadLine()) != null) {
                p = new Paragraph(line);
                p.SetKeepTogether(true);
                if (title) {
                    outline = CreateOutline(outline, pdf, line, p);
                    p.SetFont(bold).SetFontSize(12).SetKeepWithNext(true);
                    title = false;
                    document.Add(p);
                }
                else {
                    p.SetFirstLineIndent(36);
                    if (String.IsNullOrEmpty(line)) {
                        p.SetMarginBottom(12);
                        title = true;
                    }
                    else {
                        p.SetMarginBottom(0);
                    }
                    document.Add(p);
                }
            }
            //Close document
            document.Close();
        }

        public virtual PdfOutline CreateOutline(PdfOutline outline, PdfDocument pdf, String title, Paragraph p) {
            if (outline == null) {
                outline = pdf.GetOutlines(false);
                outline = outline.AddOutline(title);
                return outline;
            }
            OutlineRenderer renderer = new OutlineRenderer(p, title, outline);
            p.SetNextRenderer(renderer);
            return outline;
        }

        protected internal class OutlineRenderer : ParagraphRenderer {
            protected internal PdfOutline parent;

            protected internal String title;

            public OutlineRenderer(Paragraph modelElement, String title, PdfOutline
                 parent)
                : base(modelElement) {
                this.title = title;
                this.parent = parent;
            }

            public override void Draw(DrawContext drawContext) {
                base.Draw(drawContext);
                Rectangle rect = this.GetOccupiedAreaBBox();
                PdfDestination dest = PdfExplicitDestination.CreateFitH(drawContext.GetDocument().GetLastPage(), rect.GetTop
                    ());
                PdfOutline outline = this.parent.AddOutline(this.title);
                outline.AddDestination(dest);
            }
        }
    }
}
