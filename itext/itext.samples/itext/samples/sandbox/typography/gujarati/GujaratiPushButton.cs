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

namespace iText.Samples.Sandbox.Typography.Gujarati
{
    public class GujaratiPushButton
    {
        public const String DEST = "results/sandbox/typography/GujaratiPushButton.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new GujaratiPushButton().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansGujarati-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // મને દબાણ કરો
            String text = "\u0AAE\u0AA8\u0AC7\u0020\u0AA6\u0AAC\u0ABE\u0AA3\u0020\u0A95\u0AB0\u0ACB";

            Rectangle rect = new Rectangle(50, 650, 100, 25);

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