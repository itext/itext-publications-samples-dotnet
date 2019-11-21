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
    public class HebrewWithEnglish
    {
        public const String DEST = "results/sandbox/typography/HebrewWithEnglish.pdf";
        public const String FONTS_FOLDER = "../../itext/samples/sandbox/typography/hebrew/resources/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HebrewWithEnglish().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Be aware that from now on this font size will be used for all the child elements unless it's overwritten in them
            document.SetFontSize(10);

            // Create another font
            PdfFont freeSansFont = PdfFontFactory.CreateFont(FONTS_FOLDER + "FreeSans.ttf",
                    PdfEncodings.IDENTITY_H);

            // ראשון
            Text text1 = new Text("\u05E8\u05D0\u05E9\u05D5\u05DF\u0020").SetFont(font);

            Text text2 = new Text("B.A").SetFont(freeSansFont);

            //  מהאוניברסיטה העברית בירושלים
            Text text3 = new Text("\u0020\u05DE\u05D4\u05D0\u05D5\u05E0\u05D9\u05D1\u05E8\u05E1\u05D9\u05D8\u05D4\u0020"
                                  + "\u05D4\u05E2\u05D1\u05E8\u05D9\u05EA\u0020\u05D1\u05D9\u05E8\u05D5\u05E9\u05DC\u05D9" +
                                  "\u05DD\u0020").SetFont(font);

            Text text4 = new Text("23").SetFont(freeSansFont);

            // Wrap text with a paragraph, then set its direction and alignment
            Paragraph paragraph = new Paragraph();
            paragraph
                    .Add(text1)
                    .Add(text2)
                    .Add(text3)
                    .Add(text4);
            paragraph
                    // In Hebrew text goes from right to left, that's why we need to overwrite the default iText's alignment
                    // and direction
                    .SetBaseDirection(BaseDirection.RIGHT_TO_LEFT)
                    .SetTextAlignment(TextAlignment.RIGHT);

            document.Add(paragraph);

            document.Close();
        }
    }
}