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
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class RepeatImage
    {
        public static readonly String DEST = "results/sandbox/images/repeat_image.pdf";

        public static readonly String SRC = "../../../resources/pdfs/chinese.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RepeatImage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfPage firstPage = pdfDoc.GetFirstPage();
            Rectangle pageSize = firstPage.GetPageSize();

            // Assume that there is a single XObject in the source document
            // and this single object is an image.
            PdfDictionary pageDict = firstPage.GetPdfObject();
            PdfDictionary pageResources = pageDict.GetAsDictionary(PdfName.Resources);
            PdfDictionary pageXObjects = pageResources.GetAsDictionary(PdfName.XObject);
            PdfName imgRef = pageXObjects.KeySet().First();
            PdfStream imgStream = pageXObjects.GetAsStream(imgRef);
            PdfImageXObject imgObject = new PdfImageXObject(imgStream);
            Image image = new Image(imgObject);
            image.SetFixedPosition(0, 0);
            image.SetBorder(new SolidBorder(ColorConstants.BLACK, 5));
            image.ScaleAbsolute(pageSize.GetWidth(), pageSize.GetHeight());
            doc.Add(image);

            doc.Close();
        }
    }
}