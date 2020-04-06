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
using iText.PdfCleanup;

namespace iText.Samples.Sandbox.Parse
{
    public class RemoveRedactedContent
    {
        public static readonly String DEST = "results/sandbox/parse/remove_redacted_content.pdf";

        public static readonly String SRC = "../../../resources/pdfs/page229_redacted.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RemoveRedactedContent().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            // If the second argument is true, then regions to be erased are extracted from the redact annotations
            // contained inside the given document. If the second argument is false (that's default behavior),
            // then use PdfCleanUpTool.addCleanupLocation(PdfCleanUpLocation)
            // method to set regions to be erased from the document.
            PdfCleanUpTool cleaner = new PdfCleanUpTool(pdfDoc, true);
            cleaner.CleanUp();

            pdfDoc.Close();
        }
    }
}