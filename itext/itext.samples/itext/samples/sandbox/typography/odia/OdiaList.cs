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

namespace iText.Samples.Sandbox.Typography.Odia
{
    public class OdiaList
    {
        public const String DEST = "results/sandbox/typography/OdiaList.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new OdiaList().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansOriya-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // ସବୁ ମନୁଷ୍ଯ ଜନ୍ମକାଳରୁ
            String text =
                    "\u0B38\u0B2C\u0B41\u0020\u0B2E\u0B28\u0B41\u0B37\u0B4D\u0B2F\u0020\u0B1C\u0B28\u0B4D\u0B2E\u0B15"
                    + "\u0B3E\u0B33\u0B30\u0B41";

            List list = new List();
            list
                    .Add(text)
                    .Add(text)
                    .Add(text);

            document.Add(list);

            document.Close();
        }
    }
}