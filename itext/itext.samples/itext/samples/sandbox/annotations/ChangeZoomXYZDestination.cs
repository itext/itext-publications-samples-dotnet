/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Annotations
{
    public class ChangeZoomXYZDestination
    {
        public static readonly String DEST = "results/sandbox/annotations/change_zoom_xyz_destination.pdf";

        public static readonly String SRC = "../../../resources/pdfs/xyz_destination.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ChangeZoomXYZDestination().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            PdfDictionary pageDict = pdfDoc.GetPage(11).GetPdfObject();
            PdfArray annots = pageDict.GetAsArray(PdfName.Annots);

            // Loop over the annotations
            for (int i = 0; i < annots.Size(); i++)
            {
                PdfDictionary annotation = annots.GetAsDictionary(i);
                if (PdfName.Link.Equals(annotation.GetAsName(PdfName.Subtype)))
                {
                    PdfArray d = annotation.GetAsArray(PdfName.Dest);
                    if (d != null && d.Size() == 5 && PdfName.XYZ.Equals(d.GetAsName(1)))
                    {
                        
                        // Change the zoom factor of the current link to 0
                        d.Set(4, new PdfNumber(0));
                    }
                }
            }

            pdfDoc.Close();
        }
    }
}