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
using iText.License;

namespace iText.Samples.Sandbox.Typography.Tamil
{
    public class TamilPushButton
    {
        public const String DEST = "results/sandbox/typography/TamilPushButton.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TamilPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            // Embedded parameter indicates whether the font is to be embedded into the target document.
            // We set it to make sure that the resultant document looks the same within different environments
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansTamil-Regular.ttf",
                    PdfEncodings.IDENTITY_H, true);

            // என்னை தள்ளி விடு
            String text =
                    "\u0B8E\u0BA9\u0BCD\u0BA9\u0BC8\u0020\u0BA4\u0BB3\u0BCD\u0BB3\u0BBF\u0020\u0BB5\u0BBF\u0B9F\u0BC1";

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);
            Rectangle rect = new Rectangle(50, 650, 140, 30);

            // Create a button for the form field, set its font and size
            PdfButtonFormField pushButton = PdfFormField.CreatePushButton(pdfDocument, rect, "Name", text);
            pushButton
                    .SetFont(font)
                    .SetFontSize(10);

            // Add the button to the form
            form.AddField(pushButton);

            pdfDocument.Close();
        }
    }
}