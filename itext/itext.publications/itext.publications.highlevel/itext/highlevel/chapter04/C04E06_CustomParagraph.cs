/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter04 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C04E06_CustomParagraph {
        internal class MyParagraphRenderer : ParagraphRenderer {
            public MyParagraphRenderer(C04E06_CustomParagraph _enclosing, Paragraph modelElement)
                : base(modelElement) {
                this._enclosing = _enclosing;
            }

            public override void DrawBackground(DrawContext drawContext) {
                Background background = this.GetProperty<Background>(Property.BACKGROUND);
                if (background != null) {
                    Rectangle bBox = this.GetOccupiedAreaBBox();
                    bool isTagged = drawContext.IsTaggingEnabled() && this.GetModelElement() is IAccessibleElement;
                    if (isTagged) {
                        drawContext.GetCanvas().OpenTag(new CanvasArtifact());
                    }
                    Rectangle bgArea = this.ApplyMargins(bBox, false);
                    if (bgArea.GetWidth() <= 0 || bgArea.GetHeight() <= 0) {
                        return;
                    }
                    drawContext.GetCanvas().SaveState().SetFillColor(background.GetColor()).RoundRectangle((double)bgArea.GetX
                        () - background.GetExtraLeft(), (double)bgArea.GetY() - background.GetExtraBottom(), (double)bgArea.GetWidth
                        () + background.GetExtraLeft() + background.GetExtraRight(), (double)bgArea.GetHeight() + background.GetExtraTop
                        () + background.GetExtraBottom(), 5).Fill().RestoreState();
                    if (isTagged) {
                        drawContext.GetCanvas().CloseTag();
                    }
                }
            }

            private readonly C04E06_CustomParagraph _enclosing;
        }

        public const String DEST = "results/chapter04/custom_paragraph.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E06_CustomParagraph().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p1 = new Paragraph("The Strange Case of Dr. Jekyll and Mr. Hyde");
            p1.SetBackgroundColor(Color.ORANGE);
            document.Add(p1);
            Paragraph p2 = new Paragraph("The Strange Case of Dr. Jekyll and Mr. Hyde");
            p2.SetBackgroundColor(Color.ORANGE);
            p2.SetNextRenderer(new C04E06_CustomParagraph.MyParagraphRenderer(this, p2));
            document.Add(p2);
            document.Close();
        }
    }
}
