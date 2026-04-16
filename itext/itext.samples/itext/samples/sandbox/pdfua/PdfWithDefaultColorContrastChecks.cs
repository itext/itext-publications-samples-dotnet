using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Contrast;
using iText.Kernel.Exceptions;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Validation;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Pdfua
{
    /*
     * PdfWithDefaultColorContrastChecks.cs
     *
     * Example showing default color contrast checks in a regular PdfDocument.
     */
    public class PdfWithDefaultColorContrastChecks
    {
        public static readonly string DEST = "results/sandbox/pdfua/pdf_default_color_contrast_checks.pdf";
        public static readonly string FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfWithDefaultColorContrastChecks().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            try
            {
                using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest)))
                {
                    ValidationContainer container = new ValidationContainer();
                    
                    // Register color contrast checker.
                    // The second parameter controls whether an exception is thrown on failure.
                    // Set it to false to allow PDF creation even if contrast issues are found.
                    container.AddChecker(new ColorContrastChecker(true, false));
                    pdfDocument.GetDiContainer().Register(typeof(ValidationContainer), container);

                    using (Document document = new Document(pdfDocument))
                    {
                        PdfFont font = PdfFontFactory.CreateFont(FONT);
                        document.SetFont(font);

                        document.Add(new Paragraph("This paragraph uses a clearly readable contrast.")
                            .SetFontColor(ColorConstants.BLACK)
                            .SetBackgroundColor(ColorConstants.WHITE)
                            .SetFontSize(12));

                        document.Add(new Paragraph("This paragraph intentionally uses low contrast.")
                            .SetFontColor(ColorConstants.LIGHT_GRAY)
                            .SetBackgroundColor(ColorConstants.WHITE)
                            .SetFontSize(12));
                    }
                }
            }
            catch (PdfException e)
            {
                Console.WriteLine("Color contrast validation failed:");
                Console.WriteLine(e.Message);
            }
        }
    }
}