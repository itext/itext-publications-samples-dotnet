using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    
    // SetRotationAngleAndSetTransform.cs
    //
    // This sample shows the difference between setRotationAngle and setTransform.
    //
    // setRotationAngle rotates the layout element itself, so iText takes the rotation into account when
    // calculating the occupied area. setTransform changes how the element is drawn and can also combine
    // rotation with scaling, moving, or other transformations.

    public class SetRotationAngleAndSetTransform
    {
        public static readonly string DEST = "results/sandbox/objects/setRotationAngleAndSetTransform.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new SetRotationAngleAndSetTransform().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            AddFlowLayoutExample(document);
            document.Add(new AreaBreak());
            AddFixedPositionExample(document);

            document.Close();
        }

        private static void AddFlowLayoutExample(Document document)
        {
            document.Add(new Paragraph("Flow layout")
                .SetFontSize(16));

            document.Add(new Paragraph("Gray rectangles mark the occupied areas calculated by the layout engine."));

            // The next paragraph is positioned after the area occupied by the rotated element.
            document.Add(new Paragraph("setRotationAngle changes the layout element rotation. "
                + "The occupied area used by the layout engine is calculated for the rotated element."));
            document.Add(CreateSampleParagraph("SetRotationAngle(Math.PI / 15)")
                .SetRotationAngle(Math.PI / 15));
            document.Add(CreateAfterParagraph());

            // The transform changes the rendered appearance, not the flow layout box reserved for it.
            document.Add(new Paragraph("setTransform changes the drawing while rendering. "
                + "The following content is laid out after the original, non-transformed element box."));
            document.Add(CreateSampleParagraph("SetTransform(new Transform().Rotate(...))")
                .SetTransform(new Transform().Rotate((float) Math.PI / 15)));
            document.Add(CreateAfterTransformedParagraph());

            document.Add(new Paragraph("setTransform can also rotate around a point other than the element center. "
                + "Here -50% and -50% move the rotation point to the lower-left corner."));
            document.Add(CreateSampleParagraph("SetTransform(new Transform().Rotate(..., -50%, -50%))")
                .SetTransform(new Transform().Rotate((float) Math.PI / 15,
                    UnitValue.CreatePercentValue(-50), UnitValue.CreatePercentValue(-50))));
            document.Add(CreateAfterTransformedParagraph());

            // Reference rectangles for flow layout elements. The rectangles are drawn in the PDF coordinate system.
            PdfCanvas pdfCanvas = new PdfCanvas(document.GetPdfDocument().GetLastPage());
            AddReferenceRectangle(pdfCanvas, 36f, 618.15f, 232, 74);
            AddReferenceRectangle(pdfCanvas, 36f, 388.18f, 232, 74);
            AddReferenceRectangle(pdfCanvas, 36f, 188.31f, 232, 74);
        }

        private static void AddFixedPositionExample(Document document)
        {
            document.Add(new Paragraph("Fixed position")
                .SetFontSize(16)
                .SetFixedPosition(36, 760, 520));

            // Fixed-position coordinates still use the PDF coordinate system.
            document.Add(CreateFixedPositionParagraph("setRotationAngle: rotation is part of layout positioning.",
                    ColorConstants.ORANGE)
                .SetFixedPosition(75, 560, 180)
                .SetRotationAngle(Math.PI / 6));

            // setTransform changes the drawing and may extend outside the reference rectangle.
            document.Add(CreateFixedPositionParagraph("setTransform.rotate: rendered around the occupied area's center.",
                    ColorConstants.CYAN)
                .SetFixedPosition(330, 560, 180)
                .SetTransform(new Transform().Rotate((float) Math.PI / 6)));

            document.Add(CreateFixedPositionParagraph("setRotationAngle keeps the element model simple: only rotation.",
                    ColorConstants.ORANGE)
                .SetFixedPosition(75, 360, 180)
                .SetRotationAngle(-Math.PI / 6));

            // Unlike setRotationAngle, setTransform can combine several transformations.
            document.Add(CreateFixedPositionParagraph("setTransform can combine rotation with scaling and other transformations.",
                    ColorConstants.CYAN)
                .SetFixedPosition(330, 360, 180)
                .SetTransform(new Transform()
                    .Rotate((float) -Math.PI / 6)
                    .ScaleX(1.15f)
                    .SkewX((float) Math.PI / 22.5f)));

            document.Add(new Paragraph("Gray rectangles mark the fixed-position boxes before rotation or transform.")
                .SetFontSize(9)
                .SetFixedPosition(75, 240, 435));

            // Reference rectangles for fixed-position elements. The rectangles are drawn in the PDF coordinate system.
            PdfCanvas pdfCanvas = new PdfCanvas(document.GetPdfDocument().GetLastPage());
            AddReferenceRectangle(pdfCanvas, 74, 559, 182, 38);
            AddReferenceRectangle(pdfCanvas, 329, 559, 182, 56);
            AddReferenceRectangle(pdfCanvas, 74, 359, 182, 56);
            AddReferenceRectangle(pdfCanvas, 329, 359, 182, 56);
        }

        private static Paragraph CreateSampleParagraph(string text)
        {
            return new Paragraph(text + " - text wraps inside a bordered 230 pt paragraph. "
                    + "This makes the difference in occupied area visible.")
                .AddStyle(CreateSampleStyle())
                .SetWidth(230);
        }

        private static Paragraph CreateAfterParagraph()
        {
            return new Paragraph("Next paragraph after the transformed element.")
                .SetFontColor(ColorConstants.DARK_GRAY)
                .SetMarginBottom(20);
        }

        private static Paragraph CreateAfterTransformedParagraph()
        {
            return CreateAfterParagraph()
                .SetMarginTop(20);
        }

        private static Paragraph CreateFixedPositionParagraph(string text, Color color)
        {
            return new Paragraph(text)
                .AddStyle(CreateSampleStyle())
                .SetBackgroundColor(color)
                .SetTextAlignment(TextAlignment.CENTER);
        }

        private static Style CreateSampleStyle()
        {
            return new Style()
                .SetBorder(new SolidBorder(ColorConstants.BLUE, 1))
                .SetMarginTop(12)
                .SetMarginBottom(12);
        }

        private static void AddReferenceRectangle(PdfCanvas canvas, float x, float y, float width, float height)
        {
            canvas.SaveState()
                .SetStrokeColor(ColorConstants.LIGHT_GRAY)
                .Rectangle(new Rectangle(x, y, width, height))
                .Stroke()
                .RestoreState();
        }
    }
}
