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
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.PdfCleanup;

namespace iText.Samples.Sandbox.Parse
{
    public class RemoveContentInRectangle
    {
        public static readonly String DEST = "../../results/sandbox/parse/remove_content_in_rectangle.pdf";

        public static readonly String SRC = "../../resources/pdfs/page229.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RemoveContentInRectangle().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            IList<PdfCleanUpLocation> cleanUpLocations = new List<PdfCleanUpLocation>();

            // The arguments of the PdfCleanUpLocation constructor: the number of page to be cleaned up,
            // a Rectangle defining the area on the page we want to clean up,
            // a color which will be used while filling the cleaned area.
            PdfCleanUpLocation location = new PdfCleanUpLocation(1,
                new Rectangle(97, 405, 383, 40), ColorConstants.GRAY);
            cleanUpLocations.Add(location);

            PdfCleanUpTool cleaner = new PdfCleanUpTool(pdfDoc, cleanUpLocations);
            cleaner.CleanUp();

            pdfDoc.Close();
        }
    }
}