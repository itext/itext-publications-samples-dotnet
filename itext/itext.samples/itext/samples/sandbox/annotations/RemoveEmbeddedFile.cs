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
    public class RemoveEmbeddedFile
    {
        public static readonly String DEST = "results/sandbox/annotations/remove_embedded_file.pdf";

        public static readonly String SRC = "../../resources/pdfs/hello_with_attachment.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RemoveEmbeddedFile().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            PdfDictionary root = pdfDoc.GetCatalog().GetPdfObject();
            PdfDictionary names = root.GetAsDictionary(PdfName.Names);
            PdfDictionary embeddedFiles = names.GetAsDictionary(PdfName.EmbeddedFiles);
            PdfArray namesArray = embeddedFiles.GetAsArray(PdfName.Names);

            // Remove the description of the embedded file
            namesArray.Remove(0);

            // Remove the reference to the embedded file.
            namesArray.Remove(0);

            pdfDoc.Close();
        }
    }
}