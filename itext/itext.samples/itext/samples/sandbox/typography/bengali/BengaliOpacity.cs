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

namespace iText.Samples.Sandbox.Typography.Bengali
{
    public class BengaliOpacity
    {
        public const String DEST = "results/sandbox/typography/BengaliOpacity.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BengaliOpacity().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // আমার কানে লাগে
            Text text = new Text(
                    "\u0986\u09AE\u09BE\u09B0\u0020\u0995\u09BE\u09A8\u09C7\u0020\u09B2\u09BE\u0997\u09C7");
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansBengali-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // Wrap the text with paragraphs of different opacity and add them to the document: at first with 0.1, then with
            // 0.5 and then with the default opacity
            document.Add(new Paragraph(text).SetOpacity(0.1f));
            document.Add(new Paragraph(text).SetOpacity(0.5f));
            document.Add(new Paragraph(text));

            document.Close();
        }
    }
}