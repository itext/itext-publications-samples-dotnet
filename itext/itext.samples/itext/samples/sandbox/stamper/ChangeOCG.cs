/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Layer;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ChangeOCG {
        public static readonly String DEST = "results/sandbox/stamper/change_ocg.pdf";
        public static readonly String SRC = "../../resources/pdfs/ocg.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeOCG().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            IList<PdfLayer> layers = pdfDoc.GetCatalog().GetOCProperties(true).GetLayers();
            foreach (PdfLayer layer in layers) 
            {
                if ("Nested layer 1".Equals(layer.GetPdfObject().Get(PdfName.Name).ToString())) 
                {
                    layer.SetOn(false);
                    break;
                }
            }
            
            pdfDoc.Close();
        }
    }
}
