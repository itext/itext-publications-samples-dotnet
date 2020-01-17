/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Tamil
{
    public class TamilDropDownBox
    {
        public const String DEST = "results/sandbox/typography/TamilDropDownBox.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TamilDropDownBox().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansTamil-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // தமிழ்
            String line1 = "\u0BA4\u0BAE\u0BBF\u0BB4\u0BCD";

            // மனித
            String line2 = "\u0BAE\u0BA9\u0BBF\u0BA4";

            // உரிமைகள்
            String line3 = "\u0B89\u0BB0\u0BBF\u0BAE\u0BC8\u0B95\u0BB3\u0BCD";

            // Initialize the array with 3 lines of text. These lines will be used as combo box options
            String[] comboText = new String[] {line1, line2, line3};

            // Create a form field and apply the properties on it
            PdfFormField formField = PdfTextFormField.CreateComboBox(document.GetPdfDocument(), new Rectangle(50, 750,
                    80, 15), "test", line1, comboText);
            formField
                    .SetBorderWidth(1)
                    .SetJustification(1)
                    .SetFont(font)
                    .SetFontSizeAutoScale();

            form.AddField(formField);

            document.Close();
        }
    }
}