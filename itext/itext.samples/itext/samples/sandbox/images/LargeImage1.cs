/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using System.Linq;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class LargeImage1
    {
        public static readonly String DEST = "results/sandbox/images/large_image1.pdf";

        public static readonly String SRC = "../../resources/pdfs/large_image.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LargeImage1().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument resultDoc = new PdfDocument(new PdfWriter(dest));
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));

            // Assume that there is a single XObject in the source document
            // and this single object is an image.
            PdfDictionary pageDict = srcDoc.GetFirstPage().GetPdfObject();
            PdfDictionary pageResources = pageDict.GetAsDictionary(PdfName.Resources);
            PdfDictionary pageXObjects = pageResources.GetAsDictionary(PdfName.XObject);
            PdfName imgRef = pageXObjects.KeySet().First();
            PdfStream imgStream = pageXObjects.GetAsStream(imgRef);
            PdfImageXObject imgObject = new PdfImageXObject((PdfStream) imgStream.CopyTo(resultDoc));
            Image image = new Image(imgObject);
            image.ScaleToFit(14400, 14400);
            image.SetFixedPosition(0, 0);

            srcDoc.Close();
            
            PageSize pageSize = new PageSize(image.GetImageScaledWidth(), image.GetImageScaledHeight());
            Document doc = new Document(resultDoc, pageSize);
            doc.Add(image);
            
            doc.Close();
        }
    }
}