/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
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
    public class GujaratiChoiceFormField
    {
        public const String DEST = "results/sandbox/typography/GujaratiChoiceFormField.pdf";
        public const String FONTS_FOLDER = "../../itext/samples/sandbox/typography/gujarati/resources/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new GujaratiChoiceFormField().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

            // Embedded parameter indicates whether the font is to be embedded into the target document.
            // We set it to make sure that the resultant document looks the same within different environments
            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansGujarati-Regular.ttf",
                    PdfEncodings.IDENTITY_H, true);

            // Embed entire font without any subsetting. Please note that without subset it's impossible to edit a form field
            // with the predefined font
            font.SetSubset(false);

            // વાઈસરૉયને
            String line1 = "\u0AB5\u0ABE\u0A88\u0AB8\u0AB0\u0AC9\u0AAF\u0AA8\u0AC7";

            // રાજ્યમાં
            String line2 = "\u0AB0\u0ABE\u0A9C\u0ACD\u0AAF\u0AAE\u0ABE\u0A82";

            // વસતા
            String line3 = "\u0AB5\u0AB8\u0AA4\u0ABE";

            // Create an array with text lines
            String[] options = new String[] {line1, line2, line3};

            Rectangle rect = new Rectangle(50, 650, 120, 70);

            // Create choice form field with parameters and set values
            PdfChoiceFormField choice = PdfFormField.CreateList(pdfDocument, rect, "List", "Test", options);
            choice
                    .SetMultiSelect(true)
                    .SetFont(font)
                    .SetFontSize(10);

            form.AddField(choice);

            pdfDocument.Close();
        }
    }
}