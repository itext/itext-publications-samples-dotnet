using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Utils;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Properties;
using iText.Licensing.Base;
using iText.Samples.Util;

namespace iText.Samples.Sandbox.Typography.Arabic
{
    public class ArabicHiddenTextForm
    {
        public const String DEST = "results/sandbox/typography/ArabicHiddenTextForm.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";
        public const String RESOURCE_FOLDER = "../../../resources/pdfs/";
        public const String INPUT_FILE = "arabicAppearance.pdf";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            String licensePath = LicenseUtil.GetPathToLicenseFileWithITextCoreAndPdfCalligraphProducts();
            using (Stream license = FileUtil.GetInputStreamForFile(licensePath))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ArabicHiddenTextForm().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a new pdf based on the resource one
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(RESOURCE_FOLDER + INPUT_FILE),
                    new PdfWriter(dest));

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoNaskhArabic-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // في القيام بنشاط
            String text = "\u0641\u064A\u0020\u0627\u0644\u0642\u064A\u0627\u0645\u0020\u0628\u0646\u0634\u0627\u0637";

            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDocument, true);

            // Set needAppearance value to false in order to hide the text of the form fields
            form.SetNeedAppearances(false);

            // Update the value and some other properties of all the pdf document's form fields
            foreach (KeyValuePair<String, PdfFormField> entry in form.GetAllFormFields())
            {
                PdfFormField field = entry.Value;
                field.SetValue(text);
                field.SetJustification(TextAlignment.RIGHT).SetFont(font);
            }

            pdfDocument.Close();
        }
    }
}