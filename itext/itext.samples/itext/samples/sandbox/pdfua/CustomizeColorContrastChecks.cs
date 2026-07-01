using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Contrast;
using iText.Kernel.Exceptions;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Validation;
using iText.Layout;
using iText.Layout.Element;
using iText.Pdfua;

namespace iText.Samples.Sandbox.Pdfua
{
    /*
     * CustomizeColorContrastChecks.cs
     *
     * Example showing how to replace the default ColorContrastChecker
     * with a custom one that throws an exception on failure and checks
     * WCAG AA instead of AAA.
     */
    public class CustomizeColorContrastChecks
    {
        public static readonly string DEST = "results/sandbox/pdfua/pdf_ua_customize_color_contrast_checks.pdf";
        public static readonly string FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            try
            {
                new CustomizeColorContrastChecks().ManipulatePdf(DEST);
            }
            catch (PdfException e)
            {
                Console.WriteLine("Color contrast validation failed:");
                Console.WriteLine(e.Message);
            }
        }

        protected void ManipulatePdf(string dest)
        {
            PdfUAConfig config = new PdfUAConfig(PdfUAConformance.PDF_UA_2,
                "Custom color contrast checks", "en-US");

            using (PdfUADocument pdfDocument = new CustomPdfUADocument(
                       new PdfWriter(dest, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)), config))
            using (Document document = new Document(pdfDocument))
            {
                PdfFont font = PdfFontFactory.CreateFont(FONT);
                document.SetFont(font);

                document.Add(new Paragraph("This paragraph intentionally uses low contrast.")
                    .SetFontColor(ColorConstants.LIGHT_GRAY)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetFontSize(12));
            }
        }

        private class CustomPdfUADocument : PdfUADocument
        {
            public CustomPdfUADocument(PdfWriter writer, PdfUAConfig config)
                : base(writer, config)
            {
            }

            protected override IList<IValidationChecker> CreateCheckers(PdfUAConformance uaConformance)
            {
                IList<IValidationChecker> checkers = base.CreateCheckers(uaConformance);

                for (int i = checkers.Count - 1; i >= 0; i--)
                {
                    if (checkers[i] is ColorContrastChecker)
                    {
                        checkers.RemoveAt(i);
                    }
                }
                
                // Register color contrast checker.
                // The second parameter controls whether an exception is thrown on failure.
                // Set it to false to allow PDF creation even if contrast issues are found.
                ColorContrastChecker contrastChecker = new ColorContrastChecker(true, false);
                contrastChecker.SetCheckWcagAA(true);
                contrastChecker.SetCheckWcagAAA(false);

                checkers.Add(contrastChecker);
                return checkers;
            }
        }
    }
}