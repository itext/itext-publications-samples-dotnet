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
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Hebrew
{
    public class HebrewLink
    {
        public const String DEST = "results/sandbox/typography/HebrewLink.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HebrewLink().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // רע ומר היה להם ליהודים
            String text = "\u05E8\u05E2\u0020\u05D5\u05DE\u05E8\u0020\u05D4\u05D9\u05D4\u0020\u05DC\u05D4\u05DD" +
                          "\u0020\u05DC\u05D9\u05D4\u05D5\u05D3\u05D9\u05DD";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSerifHebrew-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Create an action with an URI. Use the action together with text to create a Link element
            Link link = new Link(text, PdfAction.CreateURI("http://itextpdf.com"));

            // Overwrite some default document properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10)

                    // In Hebrew text goes from right to left, that's why we need to overwrite the default iText's alignment
                    .SetTextAlignment(TextAlignment.RIGHT);

            document
                    .Add(new Paragraph(link))
                    .Add(new Paragraph(text));

            document.Close();
        }
    }
}