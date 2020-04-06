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
    public class ReuseImage
    {
        public static readonly String DEST = "results/sandbox/images/reuse_image.pdf";

        public static readonly String SRC = "../../../resources/pdfs/single_image.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ReuseImage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfDocument resultDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(resultDoc, PageSize.A4.Rotate());

            // Assume that there is a single XObject in the source document
            // and this single object is an image.
            PdfDictionary pageDict = srcDoc.GetFirstPage().GetPdfObject();
            PdfDictionary pageResources = pageDict.GetAsDictionary(PdfName.Resources);
            PdfDictionary pageXObjects = pageResources.GetAsDictionary(PdfName.XObject);
            PdfName imgRef = pageXObjects.KeySet().First();
            PdfStream imgStream = pageXObjects.GetAsStream(imgRef);
            PdfImageXObject imgObject = new PdfImageXObject((PdfStream) imgStream.CopyTo(resultDoc));
            
            srcDoc.Close();
            
            Image image = new Image(imgObject);
            image.ScaleToFit(PageSize.A4.GetHeight(), PageSize.A4.GetWidth());
            image.SetFixedPosition((PageSize.A4.GetHeight() - image.GetImageScaledWidth()) / 2,
                (PageSize.A4.GetWidth() - image.GetImageScaledHeight()) / 2);
            doc.Add(image);
            
            doc.Close();
        }
    }
}