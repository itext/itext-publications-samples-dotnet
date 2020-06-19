using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Colors.Gradients;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.StyledXmlParser.Css.Util;

namespace iText.Samples.Sandbox.Graphics
{
    public class LinearGradientsInKernel
    {
        public static readonly string DEST = "results/sandbox/graphics/linearGradientsInKernel.pdf";
        
        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LinearGradientsInKernel().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            AddLinearGradientITextAPIApproach(pdfDoc);
            AddLinearGradientCssApproach(pdfDoc);
            AddLinearGradientDirectCoordinatesApproach(pdfDoc);

            pdfDoc.Close();
        }

        private void AddLinearGradientITextAPIApproach(PdfDocument pdfDoc)
        {
            AbstractLinearGradientBuilder gradientBuilder = new StrategyBasedLinearGradientBuilder()
                    .SetGradientDirectionAsStrategy(StrategyBasedLinearGradientBuilder.GradientStrategy.TO_TOP_RIGHT)
                    .SetSpreadMethod(GradientSpreadMethod.PAD)
                    .AddColorStop(new GradientColorStop(ColorConstants.CYAN.GetColorValue()))
                    .AddColorStop(new GradientColorStop(ColorConstants.GREEN.GetColorValue()))
                    .AddColorStop(new GradientColorStop(new float[] {1f, 0f, 0f}, 0.5f, GradientColorStop.OffsetType.RELATIVE));

            AffineTransform canvasTransform = AffineTransform.GetTranslateInstance(50, -50);
            canvasTransform.Scale(0.8, 1.1);
            canvasTransform.Rotate(Math.PI/3, 400f, 550f);
            
            Rectangle rectangleToDraw = new Rectangle(50f, 450f, 500f, 300f);

            GeneratePdf(pdfDoc, canvasTransform, gradientBuilder, rectangleToDraw);
        }

        private void AddLinearGradientCssApproach(PdfDocument pdfDoc)
        {
            String gradientValue = "linear-gradient(to left, #ff0000, #008000, #0000ff)";

            if (CssGradientUtil.IsCssLinearGradientValue(gradientValue))
            {
                StrategyBasedLinearGradientBuilder gradientBuilder = CssGradientUtil

                        // "em/rem" parameters are mandatory but don't have effect in case of such parameters aren't used
                        // within passing "gradientValue" variable
                        .ParseCssLinearGradient(gradientValue, 12, 12);
                Rectangle rectangleToDraw = new Rectangle(50f, 450f, 500f, 300f);

                GeneratePdf(pdfDoc, null, gradientBuilder, rectangleToDraw);
            }
            else
            {
                Console.Out.WriteLine("The passed parameter: " + "\n" + gradientValue + "\n" +
                                      " is not a linear gradient or repeating linear gradient function");
            }
        }

        private void AddLinearGradientDirectCoordinatesApproach(PdfDocument pdfDoc) 
        {
            Rectangle targetBoundingBox = new Rectangle(50f, 450f, 300f, 300f);
            AbstractLinearGradientBuilder gradientBuilder = new LinearGradientBuilder()
                    .SetGradientVector(targetBoundingBox.GetLeft() + 100f, targetBoundingBox.GetBottom() + 100f,
                            targetBoundingBox.GetRight() - 100f, targetBoundingBox.GetTop() - 100f)
                    .SetSpreadMethod(GradientSpreadMethod.REPEAT)

                    // For the RELATIVE offset type "0" value means the target vector start and the "1" value means the target vector end
                    .AddColorStop(new GradientColorStop(ColorConstants.BLUE.GetColorValue(), 0.5, GradientColorStop.OffsetType.RELATIVE))
                    .AddColorStop(new GradientColorStop(ColorConstants.GREEN.GetColorValue(), 1, GradientColorStop.OffsetType.RELATIVE));

            GeneratePdf(pdfDoc, null, gradientBuilder, targetBoundingBox);
        }

        private void GeneratePdf(PdfDocument pdfDocument, AffineTransform transform,
                AbstractLinearGradientBuilder gradientBuilder, Rectangle rectangleToDraw)
        {
            PdfCanvas canvas = new PdfCanvas(pdfDocument.AddNewPage());

            if (transform != null)
            {
                canvas.ConcatMatrix(transform);
            }

            canvas.SetFillColor(gradientBuilder.BuildColor(rectangleToDraw, transform, pdfDocument))
                    .SetStrokeColor(ColorConstants.BLACK)
                    .Rectangle(rectangleToDraw)
                    .FillStroke();
        }
    }
}