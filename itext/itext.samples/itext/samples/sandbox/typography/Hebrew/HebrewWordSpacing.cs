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
using iText.Layout.Properties;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Hebrew
{
    public class HebrewWordSpacing
    {
        public const String DEST = "results/sandbox/typography/HebrewWordSpacing.pdf";
        public const String FONTS_FOLDER = "../../itext/samples/sandbox/typography/hebrew/resources/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HebrewWordSpacing().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // ראשון  מהאוניברסיטה העברית בירושלים
            String text =
                    "\u05E8\u05D0\u05E9\u05D5\u05DF\u0020\u0020\u05DE\u05D4\u05D0\u05D5\u05E0\u05D9\u05D1\u05E8\u05E1"
                    + "\u05D9\u05D8\u05D4\u0020\u05D4\u05E2\u05D1\u05E8\u05D9\u05EA\u0020\u05D1\u05D9\u05E8\u05D5\u05E9\u05DC"
                    + "\u05D9\u05DD\u0020";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // Create paragraph, add string to it with the default word spacing and add all to the document
            document.Add(CreateParagraph(text));

            // Add text with a word spacing of 10 points
            document.Add(CreateParagraph(text).SetWordSpacing(10));

            // Set word spacing on the document. It will be applied to the next paragraph
            document.SetWordSpacing(20);
            document.Add(CreateParagraph(text));

            document.Close();
        }

        // This method creates a paragraph with right text alignment
        private static Paragraph CreateParagraph(String text)
        {
            Paragraph paragraph = new Paragraph(text);

            // In Hebrew text goes from right to left, that's why we need to overwrite the default iText's alignment
            paragraph.SetTextAlignment(TextAlignment.RIGHT);
            return paragraph;
        }
    }
}