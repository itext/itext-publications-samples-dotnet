/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace iText.Samples.Sandbox.Parse
{
    public class ParseCustom
    {
        public static readonly String DEST = "../../results/txt/parse_custom.txt";

        public static readonly String SRC = "../../resources/pdfs/nameddestinations.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ParseCustom().ManipulatePdf(DEST);
        }

        public virtual void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC));

            Rectangle rect = new Rectangle(36, 750, 523, 56);
            CustomFontFilter fontFilter = new CustomFontFilter(rect);
            FilteredEventListener listener = new FilteredEventListener();

            // Create a text extraction renderer
            LocationTextExtractionStrategy extractionStrategy = listener
                .AttachEventListener(new LocationTextExtractionStrategy(), fontFilter);

            // Note: If you want to re-use the PdfCanvasProcessor, you must call PdfCanvasProcessor.reset()
            new PdfCanvasProcessor(listener).ProcessPageContent(pdfDoc.GetFirstPage());

            // Get the resultant text after applying the custom filter
            String actualText = extractionStrategy.GetResultantText();

            pdfDoc.Close();

            // See the resultant text in the console
            Console.Out.WriteLine(actualText);

            using (StreamWriter writer = new StreamWriter(dest))
            {
                writer.Write(actualText);
            }
        }

        // The custom filter filters only the text of which the font name ends with Bold or Oblique.
        protected class CustomFontFilter : TextRegionEventFilter
        {
            public CustomFontFilter(Rectangle filterRect)
                : base(filterRect)
            {
            }

            public override bool Accept(IEventData data, EventType type)
            {
                if (type.Equals(EventType.RENDER_TEXT))
                {
                    TextRenderInfo renderInfo = (TextRenderInfo) data;
                    PdfFont font = renderInfo.GetFont();
                    if (null != font)
                    {
                        String fontName = font.GetFontProgram().GetFontNames().GetFontName();
                        return fontName.EndsWith("Bold") || fontName.EndsWith("Oblique");
                    }
                }

                return false;
            }
        }
    }
}