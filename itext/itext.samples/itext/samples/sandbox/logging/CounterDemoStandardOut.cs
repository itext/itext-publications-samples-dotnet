using System;
using System.IO;
using iText.Html2pdf;
using iText.Kernel.Counter;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Logging
{
    public class CounterDemoStandardOut
    {
        public const String DEST = "results/sandbox/logging/CounterDemoStandardOut.pdf";
        
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new CounterDemoStandardOut().ManipulatePdf();
        }
        
        protected virtual void ManipulatePdf() 
        {
            
            // Implement default SystemOut factory and register it
            StandardOutputEventCounterFactory counterFactory = new StandardOutputEventCounterFactory();
            EventCounterHandler.GetInstance().Register(counterFactory);
            
            // Generate 3 core events by creating 3 pdf documents
            for (int i = 0; i < 3; i++) 
            {
                CreatePdf();
            }
            
            String html = "<p>iText</p>";
            
            // Generate 2 events (core and html-convert) by converting html to pdf: the first during pdf document creation,
            // the second one during conversion
            ConvertToPdf(html);
            
            EventCounterHandler.GetInstance().Unregister(counterFactory);
        }
        
        private static void CreatePdf() 
        {
            Document document = new Document(new PdfDocument(new PdfWriter(DEST)));
            document.Add(new Paragraph("Hello World!"));
            document.Close();
        }

        private static void ConvertToPdf(String html) 
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new MemoryStream()));
            HtmlConverter.ConvertToPdf(html, pdfDocument, new ConverterProperties());
        }
    }
}