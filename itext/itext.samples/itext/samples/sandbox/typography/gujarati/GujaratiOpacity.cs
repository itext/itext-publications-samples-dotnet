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

namespace iText.Samples.Sandbox.Typography.Gujarati
{
    public class GujaratiOpacity
    {
        public const String DEST = "results/sandbox/typography/GujaratiOpacity.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new GujaratiOpacity().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // રાજ્યમાં વસતા લોકો દ્વારા લેખનકાર્યમાં વપરાતી
            Text text = new Text(
                    "\u0AB0\u0ABE\u0A9C\u0ACD\u0AAF\u0AAE\u0ABE\u0A82\u0020\u0AB5\u0AB8\u0AA4\u0ABE\u0020\u0AB2"
                    + "\u0ACB\u0A95\u0ACB\u0020\u0AA6\u0ACD\u0AB5\u0ABE\u0AB0\u0ABE\u0020\u0AB2\u0AC7\u0A96\u0AA8\u0A95\u0ABE"
                    + "\u0AB0\u0ACD\u0AAF\u0AAE\u0ABE\u0A82\u0020\u0AB5\u0AAA\u0AB0\u0ABE\u0AA4\u0AC0");
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansGujarati-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // Wrap the text with paragraphs of different opacity and add them to the document: at first with 0.1, then
            // with 0.5 and then with the default opacity
            document.Add(new Paragraph(text).SetOpacity(0.1f));
            document.Add(new Paragraph(text).SetOpacity(0.5f));
            document.Add(new Paragraph(text));

            document.Close();
        }
    }
}