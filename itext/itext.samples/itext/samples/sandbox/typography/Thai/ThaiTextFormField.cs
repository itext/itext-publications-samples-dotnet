/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
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

namespace iText.Samples.Sandbox.Typography.Thai
{
    public class ThaiTextFormField
    {
        public const String DEST = "../../results/sandbox/typography/ThaiTextFormField.pdf";
        public const String FONTS_FOLDER = "../../itext/samples/sandbox/typography/thai/resources/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ThaiTextFormField().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // ผม เรา ฉัน
            String fieldValue = "\u0E1C\u0E21\u0020\u0E40\u0E23\u0E32\u0020\u0E09\u0E31\u0E19";

            String fieldName = "Field name";

            PdfAcroForm form = PdfAcroForm.GetAcroForm(document.GetPdfDocument(), true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansThai-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // Create a form field and set some of the properties
            PdfFormField formField = PdfTextFormField.CreateText(document.GetPdfDocument(), 
                    new Rectangle(50, 750, 60, 25));
            formField
                    .SetValue(fieldValue)
                    .SetBorderWidth(2)
                    .SetFont(font)
                    .SetFontSize(10)
                    .SetJustification(1)
                    .SetFieldName(fieldName);

            form.AddField(formField);

            document.Close();
        }
    }
}