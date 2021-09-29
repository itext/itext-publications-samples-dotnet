using System;
using System.IO;
using iText.Commons.Actions;
using iText.Commons.Actions.Confirmations;
using iText.Commons.Actions.Contexts;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Logging
{
    public class CounterDemo
    {
        public const String DEST_PDF = "results/sandbox/logging/helloCounterDemo.pdf";
        public const String DEST = "results/sandbox/logging/CounterDemo.txt";
        
        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new CounterDemo().ManipulatePdf();
        }
        
        protected virtual void ManipulatePdf() 
        {
            
            // Implement and register custom factory
            ToLogCounter logCounter = new ToLogCounter(UnknownContext.PERMISSIVE);
            EventManager.GetInstance().Register(logCounter);
            
            // Generate 2 events by creating 2 pdf documents
            for (int i = 0; i < 2; i++) 
            {
                CreatePdf();
            }
            
            EventManager.GetInstance().Unregister(logCounter);
        }
        
        private static void CreatePdf() 
        {
            Document document = new Document(new PdfDocument(new PdfWriter(DEST_PDF)));
            document.Add(new Paragraph("Hello World!"));
            document.Close();
        }
        
        private class ToLogCounter : AbstractContextBasedEventHandler {
            public ToLogCounter(IContext fallback)
                    : base(fallback) 
            {
            }

            // Triggering registered factories to produce events and count them
            protected override void OnAcceptedEvent(AbstractContextBasedITextEvent iTextEvent) 
            {
                try {
                    using (StreamWriter writer = new StreamWriter(DEST, true)) 
                    {
                        if (iTextEvent is ConfirmEvent) {
                            ConfirmEvent confirmEvent = (ConfirmEvent) iTextEvent;
                            writer.Write(String.Format("{0}\n", confirmEvent.GetEventType()));
                        }
                    }
                }
                catch (IOException) 
                {
                    Console.Error.WriteLine("IOException occured.");
                }
            }
        }
    }
}