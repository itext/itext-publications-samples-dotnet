/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Telugu
{
    public class TeluguCaretAnnotation
    {
        public const String DEST = "../../results/sandbox/typography/TeluguCaretAnnotation.pdf";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TeluguCaretAnnotation().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));

            // ఆంధ్రుల గురించి
            String line1 = "\u0C06\u0C02\u0C27\u0C4D\u0C30\u0C41\u0C32\u0020\u0C17\u0C41\u0C30\u0C3F\u0C02\u0C1A\u0C3F";

            // Create a rectangle for an annotation
            Rectangle rectangleAnnot = new Rectangle(55, 750, 35, 35);

            // Create the annotation, set its contents and color
            PdfAnnotation annotation = new PdfCaretAnnotation(rectangleAnnot);
            annotation
                    .SetContents(line1)
                    .SetColor(ColorConstants.MAGENTA);

            // Add an empty page to the document, then add the annotation to the page
            PdfPage page = pdfDocument.AddNewPage();
            page.AddAnnotation(annotation);

            pdfDocument.Close();
        }
    }
}