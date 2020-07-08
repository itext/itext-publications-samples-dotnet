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

namespace iText.Highlevel.Chapter04 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C04E06_CustomParagraph {
        internal class MyParagraphRenderer : ParagraphRenderer {
            public MyParagraphRenderer(Paragraph modelElement)
                : base(modelElement) {
            }

            public override void DrawBackground(DrawContext drawContext) {
                Background background = this.GetProperty<Background>(Property.BACKGROUND);
                if (background != null) {
                    Rectangle bBox = this.GetOccupiedAreaBBox();
                    bool isTagged = drawContext.IsTaggingEnabled();
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
        }

        public const String DEST = "../../../results/chapter04/custom_paragraph.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E06_CustomParagraph().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p1 = new Paragraph("The Strange Case of Dr. Jekyll and Mr. Hyde");
            p1.SetBackgroundColor(ColorConstants.ORANGE);
            document.Add(p1);
            Paragraph p2 = new Paragraph("The Strange Case of Dr. Jekyll and Mr. Hyde");
            p2.SetBackgroundColor(ColorConstants.ORANGE);
            p2.SetNextRenderer(new C04E06_CustomParagraph.MyParagraphRenderer(p2));
            document.Add(p2);
            document.Close();
        }
    }
}
