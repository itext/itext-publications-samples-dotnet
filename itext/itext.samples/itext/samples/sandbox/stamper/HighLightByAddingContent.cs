/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Stamper 
{
    public class HighLightByAddingContent 
    {
        public static readonly String DEST = "results/sandbox/stamper/high_light_by_adding_content.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new HighLightByAddingContent().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            // The content, placed on a content stream before, will be rendered before the other content
            // and, therefore, could be understood as a background (bottom "layer")
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage().NewContentStreamBefore(),
                    pdfDoc.GetFirstPage().GetResources(), pdfDoc);
            
            canvas
                    .SaveState()
                    .SetFillColor(ColorConstants.YELLOW)
                    .Rectangle(36, 786, 66, 16).Fill()
                    .RestoreState();
            
            pdfDoc.Close();
        }
    }
}
