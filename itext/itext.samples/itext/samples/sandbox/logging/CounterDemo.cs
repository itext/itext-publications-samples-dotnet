/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Counter;
using iText.Kernel.Counter.Context;
using iText.Kernel.Counter.Event;
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
            IEventCounterFactory myCounterFactory = 
                    new SimpleEventCounterFactory(new ToLogCounter(UnknownContext.PERMISSIVE));
            EventCounterHandler.GetInstance().Register(myCounterFactory);
            
            // Generate 2 events by creating 2 pdf documents
            for (int i = 0; i < 2; i++) 
            {
                CreatePdf();
            }
            
            EventCounterHandler.GetInstance().Unregister(myCounterFactory);
        }
        
        private static void CreatePdf() 
        {
            Document document = new Document(new PdfDocument(new PdfWriter(DEST_PDF)));
            document.Add(new Paragraph("Hello World!"));
            document.Close();
        }
        
        private class ToLogCounter : EventCounter {
            public ToLogCounter(IContext fallback)
                    : base(fallback) 
            {
            }

            // Triggering registered factories to produce events and count them
            protected override void OnEvent(IEvent eventType, IMetaInfo metaInfo) 
            {
                try {
                    using (StreamWriter writer = new StreamWriter(DEST, true)) 
                    {
                        writer.Write(eventType.GetEventType() + "\n");
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