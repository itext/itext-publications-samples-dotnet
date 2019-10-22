/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Collection;
using iText.Kernel.Pdf.Filespec;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Collections
{
    public class PortableCollection
    {
        public static readonly String DEST = "../../results/sandbox/collections/portable_collection.pdf";

        public static readonly String DATA = "../../resources/data/united_states.csv";
        public static readonly String HELLO = "../../resources/pdfs/hello.pdf";
        public static readonly String IMG = "../../resources/img/berlin2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PortableCollection().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Portable collection"));
            
            PdfCollection collection = new PdfCollection();
            collection.SetView(PdfCollection.TILE);
            pdfDoc.GetCatalog().SetCollection(collection);

            AddFileAttachment(pdfDoc, DATA, "united_states.csv");
            AddFileAttachment(pdfDoc, HELLO, "hello.pdf");
            AddFileAttachment(pdfDoc, IMG, "berlin2013.jpg");

            doc.Close();
        }

        // This method adds file attachment to the pdf document
        private void AddFileAttachment(PdfDocument document, String attachmentPath, String fileName)
        {
            String embeddedFileName = fileName;
            String embeddedFileDescription = fileName;
            String fileAttachmentKey = fileName;

            // the 5th argument is the mime-type of the embedded file;
            // the 6th argument is the AFRelationship key value.
            PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(document, attachmentPath, embeddedFileDescription,
                embeddedFileName, null, null);
            document.AddFileAttachment(fileAttachmentKey, fileSpec);
        }
    }
}