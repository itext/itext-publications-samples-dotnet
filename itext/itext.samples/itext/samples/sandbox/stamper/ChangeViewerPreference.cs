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

namespace iText.Samples.Sandbox.Stamper 
{
    public class ChangeViewerPreference 
    {
        public static readonly String DEST = "results/sandbox/stamper/change_viewer_preference.pdf";
        public static readonly String SRC = "../../../resources/pdfs/united_states.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeViewerPreference().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            PdfViewerPreferences viewerPreferences = pdfDoc.GetCatalog().GetViewerPreferences();
            if (viewerPreferences == null) 
            {
                viewerPreferences = new PdfViewerPreferences();
                pdfDoc.GetCatalog().SetViewerPreferences(viewerPreferences);
            }
            
            // Setting printing mode on the both sides of the pdf document (duplex mode) along with "flip on long edge" mode
            viewerPreferences.SetDuplex(PdfViewerPreferences.PdfViewerPreferencesConstants.DUPLEX_FLIP_LONG_EDGE);
            
            pdfDoc.Close();
        }
    }
}
