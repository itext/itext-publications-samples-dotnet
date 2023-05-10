using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Acroforms
{
    public class OverrideFormFieldMethods
    {
        public static readonly String DEST = "results/sandbox/acroforms/overrideFormFieldMethods.pdf";

        public static readonly String SRC = "../../../resources/pdfs/checkboxes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new OverrideFormFieldMethods().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            try
            {
                using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest)))
                {
                    PdfFormFactory customFactory = new CustomFormFactory();
                    PdfFormCreator.SetFactory(customFactory);
                    PdfAcroForm acroForm = PdfFormCreator.GetAcroForm(pdfDoc, true);
                    acroForm.GetField("cb0").RegenerateField();
                    acroForm.GetField("cb1").RegenerateField();
                    acroForm.GetField("cb2").RegenerateField();
                    acroForm.GetField("cb3").RegenerateField();
                    acroForm.GetField("cb4").RegenerateField();
                    acroForm.GetField("cb5").RegenerateField();
                }
            }
            finally
            {
                PdfFormCreator.SetFactory(new PdfFormFactory());
            }
        }
        
        private class CustomFormFactory : PdfFormFactory
        {
            public override PdfFormAnnotation CreateFormAnnotation(PdfWidgetAnnotation widget, PdfDocument document) {
                return new CustomPdfFormAnnotation(widget, document);
            }
        }

        private class CustomPdfFormAnnotation : PdfFormAnnotation
        {
            public CustomPdfFormAnnotation(PdfWidgetAnnotation widget, PdfDocument document) : base(widget, document)
            {
                // All widgets will have 2 points red border by default.
                SetBorderWidth(2);
                SetBorderColor(ColorConstants.RED);
            }
        }
    }
}