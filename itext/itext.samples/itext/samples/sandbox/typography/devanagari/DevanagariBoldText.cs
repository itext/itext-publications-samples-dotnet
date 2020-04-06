/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Devanagari
{
    public class DevanagariBoldText
    {
        public const String DEST = "results/sandbox/typography/DevanagariBoldText.pdf";
        public const String FONTS_FOLDER = "../../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DevanagariBoldText().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansDevanagari-Regular.ttf",
                    PdfEncodings.IDENTITY_H);
            PdfFont fontBold = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansDevanagari-Bold.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document.SetFont(font);
            document.SetFontSize(10);

            // मैथिली का प्रथम प्रमाण रामायण में मिलता
            Text devanagariText = new Text(
                    "\u092E\u0948\u0925\u093F\u0932\u0940\u0020\u0915\u093E\u0020\u092A\u094D\u0930"
                    + "\u0925\u092E\u0020\u092A\u094D\u0930\u092E\u093E\u0923\u0020\u0930\u093E\u092E\u093E\u092F\u0923\u0020"
                    + "\u092E\u0947\u0902\u0020\u092E\u093F\u0932\u0924\u093E");

            // Add paragraphs with text to the document:
            // Text without thickness
            document.Add(new Paragraph(devanagariText));

            // Add a paragraph with a set bold font to the paragraph
            document.Add(new Paragraph(devanagariText).SetFont(fontBold));

            // We don't suggest usage of setBold() method to reach text thickness since the result is written with the usual
            // rather than the bold font: we only emulate "thickness". It's recommended to use an actual bold font instead.
            // For example NotoSansDevanagari-Bold
            document.Add(new Paragraph(devanagariText).SetBold());

            document.Close();
        }
    }
}