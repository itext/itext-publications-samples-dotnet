using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Contrast;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Validation;
using iText.Layout;
using iText.Layout.Element;
using iText.Pdfua;

namespace iText.Samples.Sandbox.Pdfua
{
    /*
     * DisableColorContrastChecks.cs
     *
     * Example showing how to disable color contrast checks in PdfUADocument
     * by removing ColorContrastChecker from the validation checker list.
     */
    public class DisableColorContrastChecks
    {
        public static readonly string DEST = "results/sandbox/pdfua/pdf_ua_disable_color_contrast_checks.pdf";
        public static readonly string FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DisableColorContrastChecks().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfUAConfig config = new PdfUAConfig(PdfUAConformance.PDF_UA_2,
                "Disable color contrast checks", "en-US");

            using (PdfUADocument pdfDocument = new CustomPdfUADocument(
                       new PdfWriter(dest, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)), config))
            using (Document document = new Document(pdfDocument))
            {
                PdfFont font = PdfFontFactory.CreateFont(FONT);
                document.SetFont(font);

                document.Add(new Paragraph("This paragraph intentionally uses low contrast,")
                    .SetFontColor(ColorConstants.LIGHT_GRAY)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetFontSize(12));

                document.Add(new Paragraph("but the color contrast checker was disabled.")
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

                // remove ColorContrastChecker (no RemoveIf in .NET)
                for (int i = checkers.Count - 1; i >= 0; i--)
                {
                    if (checkers[i] is ColorContrastChecker)
                    {
                        checkers.RemoveAt(i);
                    }
                }

                return checkers;
            }
        }
    }
}