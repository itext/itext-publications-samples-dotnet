using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace iText.Samples.Sandbox.Merge
{
    public class MergeAndCount
    {
        public static readonly String DEST = "results/sandbox/merge/splitDocument1_{0}.pdf";

        public static readonly String RESOURCE = "../../../resources/pdfs/Wrong.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MergeAndCount().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(RESOURCE));

            IList<PdfDocument> splitDocuments = new CustomPdfSplitter(pdfDoc, dest).SplitBySize(200000);

            foreach (PdfDocument doc in splitDocuments)
            {
                doc.Close();
            }

            pdfDoc.Close();
        }
        
        private class CustomPdfSplitter : PdfSplitter
        {
            private String dest;
            private int partNumber = 1;
            
            public CustomPdfSplitter(PdfDocument pdfDocument, String dest) : base(pdfDocument)
            {
                this.dest = dest;
            }
            
            protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange) {
                return new PdfWriter(String.Format(dest, partNumber++));
            }
        }
    }
}