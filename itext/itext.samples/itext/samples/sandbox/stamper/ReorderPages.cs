using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ReorderPages 
    {
        public const String DEST = "results/sandbox/stamper/reorder_pages.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ReorderPages().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(new RandomAccessSourceFactory()
                    .CreateSource(CreateBaos().ToArray()), new ReaderProperties()));
            
            PdfDocument resultDoc = new PdfDocument(new PdfWriter(dest));
            
            // One should call this method to preserve the outlines of the source pdf file, otherwise they
            // will be absent in the resultant document to which we copy pages. In this particular sample,
            // however, this line doesn't make sense, since the source pdf lacks outlines
            resultDoc.InitializeOutlines();
            
            IList<int> pages = new List<int>();
            pages.Add(1);
            for (int i = 13; i <= 15; i++) 
            {
                pages.Add(i);
            }
            for (int i = 2; i <= 12; i++) 
            {
                pages.Add(i);
            }
            pages.Add(16);
            srcDoc.CopyPagesTo(pages, resultDoc);
            
            resultDoc.Close();
            srcDoc.Close();
        }

        // Create a temporary document in memory. Then we will reopen it and change the order of its pages
        private static MemoryStream CreateBaos() 
        {
            MemoryStream baos = new MemoryStream();
            
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(baos));
            Document doc = new Document(pdfDoc);
            
            for (int i = 1; i < 17; i++) 
            {
                doc.Add(new Paragraph("Page " + i));
                if (16 != i) 
                {
                    doc.Add(new AreaBreak());
                }
            }
            doc.Close();
            
            return baos;
        }
    }
}
