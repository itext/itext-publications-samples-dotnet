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

namespace iText.Samples.Sandbox.Typography.Hebrew
{
    public class HebrewHiddenTextForm
    {
        public const String DEST = "results/sandbox/typography/HebrewHiddenTextForm.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";
        public const String RESOURCE_FOLDER = "../../../resources/pdfs/";
        public const String INPUT_FILE = "hebrewAppearance.pdf";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HebrewHiddenTextForm().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a new pdf based on the resource one
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(RESOURCE_FOLDER + INPUT_FILE),
                    new PdfWriter(dest));

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            //רע ומר היה להם ליהודים
            String text = "\u05E8\u05E2\u0020\u05D5\u05DE\u05E8\u0020\u05D4\u05D9\u05D4\u0020\u05DC\u05D4\u05DD" +
                          "\u0020\u05DC\u05D9\u05D4\u05D5\u05D3\u05D9\u05DD";

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