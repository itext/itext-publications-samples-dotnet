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
using System.Text;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ChangeInfoDictionary 
    {
        public static readonly String DEST = "results/sandbox/stamper/change_info_dictionary.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeInfoDictionary().ManipulatePdf(DEST);
        }

        protected internal virtual void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            
            IDictionary<String, String> newInfo = new Dictionary<String, String>();
            newInfo.Add("Special Character: \u00e4", "\u00e4");
            
            StringBuilder buf = new StringBuilder();
            buf.Append((char)0xc3);
            buf.Append((char)0xa4);
            newInfo.Add(buf.ToString(), "\u00e4");
            
            info.SetMoreInfo(newInfo);
            
            pdfDoc.Close();
        }
    }
}
