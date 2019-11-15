/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
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

namespace iText.Samples.Sandbox.Typography.Telugu
{
    public class TeluguWordSpacing
    {
        public const String DEST = "../../results/sandbox/typography/TeluguWordSpacing.pdf";
        public const String FONTS_FOLDER = "../../itext/samples/sandbox/typography/telugu/resources/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TeluguWordSpacing().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // తెలుగు గుణింతాలు
            String text =
                    "\u0C24\u0C46\u0C32\u0C41\u0C17\u0C41\u0020\u0C17\u0C41\u0C23\u0C3F\u0C02\u0C24\u0C3E\u0C32\u0C41";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansTelugu-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // Create paragraph, add string to it with the default word spacing and add all to the document
            document.Add(new Paragraph(text));

            // Add text with a word spacing of 10 points
            document.Add(new Paragraph(text).SetWordSpacing(10));

            // Set word spacing on the document. It will be applied to the next paragraph
            document.SetWordSpacing(20);
            document.Add(new Paragraph(text));

            document.Close();
        }
    }
}