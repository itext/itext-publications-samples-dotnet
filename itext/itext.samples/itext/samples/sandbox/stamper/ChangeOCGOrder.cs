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
using iText.Kernel.Pdf.Layer;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ChangeOCGOrder 
    {
        public static readonly String DEST = "results/sandbox/stamper/change_ocg_order.pdf";
        public static readonly String SRC = "../../../resources/pdfs/ocg.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeOCGOrder().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            PdfCatalog catalog = pdfDoc.GetCatalog();
            PdfOCProperties ocProps = catalog.GetOCProperties(true);
            PdfDictionary occd = (PdfDictionary)ocProps.GetPdfObject().Get(PdfName.D);
            PdfArray order = occd.GetAsArray(PdfName.Order);
            
            PdfObject nestedLayers = order.Get(0);
            PdfObject nestedLayerArray = order.Get(1);
            PdfObject groupedLayers = order.Get(2);
            PdfObject radioGroup = order.Get(3);
            order.Set(0, radioGroup);
            order.Set(1, nestedLayers);
            order.Set(2, nestedLayerArray);
            order.Set(3, groupedLayers);
            
            pdfDoc.Close();
        }
    }
}
