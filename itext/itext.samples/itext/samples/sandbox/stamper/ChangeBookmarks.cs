/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ChangeBookmarks 
    {
        public static readonly String DEST = "results/sandbox/stamper/change_bookmarks.pdf";
        public static readonly String SRC = "../../../resources/pdfs/bookmarks.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeBookmarks().ManipulatePdf(DEST);
        }

        public void ChangeList(IList<PdfOutline> list) 
        {
            foreach (PdfOutline entry in list) 
            {
                PdfArray array = ((PdfArray)entry.GetContent().Get(PdfName.Dest));
                for (int i = 0; i < array.Size(); i++) 
                {
                    if (PdfName.Fit.Equals(array.Get(i)))
                    {
                        array.Set(i, PdfName.FitV);
                        array.Add(new PdfNumber(60));
                    }
                }
            }
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfOutline outlines = pdfDoc.GetOutlines(false);
            IList<PdfOutline> children = outlines.GetAllChildren()[0].GetAllChildren();
            ChangeList(children);
            pdfDoc.Close();
        }
    }
}
