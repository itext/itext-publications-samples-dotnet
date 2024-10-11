using System;
using System.Collections.Generic;
using System.Text;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Tagging
{
    public class ExtractContentBasedOnTags
    {
        public static readonly string DEST = "results/sandbox/tagging/starter_pdfua1.pdf"; 
        public static readonly string SRC = "../../../resources/tagging/starter_pdfua1.pdf";

        public static void Main(string[] args)
        {
            new ExtractContentBasedOnTags().ExtractContentText();
        }

        public void ExtractContentText()
        {
            var reader = new PdfReader(SRC);
            // Add a dummy writer to the output stream if iText needs to repair the document.
            var doc = new PdfDocument(reader, new PdfWriter(DEST));
            var contentLookup = new TagContentLookup(doc);
            Console.WriteLine("Example 1: Print all tag content");
            ExamplePrintAllTagContent(doc, contentLookup);
            Console.WriteLine("Example 2: Print all tag content and collapsed leafs");
            ExamplePrintAllTagContentAndCollapsedLeafs(doc, contentLookup);
            Console.WriteLine("Example 3: Only Get H1 tags");
            exampleOnlyGetH1Tags(doc, contentLookup);
            Console.WriteLine("Example 4: Extract alternate description from all images");
            ExtractAlternateDescriptionFromAllImages(doc, contentLookup);
            Console.WriteLine("Example 5: Extract specific MCID");
            ExtractSpecificMcid(doc, contentLookup);
            doc.Close();
        }


        private static void ExamplePrintAllTagContent(PdfDocument doc, TagContentLookup contentLookup)
        {
            var tagPointer = doc.GetTagStructureContext().GetAutoTaggingPointer();
            var iterator = new TagTreePointerIterator(tagPointer);
            while (iterator.HasNext())
            {
                var currentPointer = iterator.Next();
                var content = contentLookup.GetContent(currentPointer, false);
                Console.WriteLine("Tag: " + currentPointer.GetRole() + " - " + content);
            }
        }

        private static void ExamplePrintAllTagContentAndCollapsedLeafs(PdfDocument doc, TagContentLookup contentLookup)
        {
            var tagPointer = doc.GetTagStructureContext().GetAutoTaggingPointer();
            var iterator = new TagTreePointerIterator(tagPointer);
            while (iterator.HasNext())
            {
                var currentPointer = iterator.Next();
                var content = contentLookup.GetContent(currentPointer, true);
                Console.WriteLine("Tag: " + currentPointer.GetRole() + " - " + content);
            }
        }

        private static void exampleOnlyGetH1Tags(PdfDocument doc, TagContentLookup contentLookup)
        {
            var tagPointer = doc.GetTagStructureContext().GetAutoTaggingPointer();
            var iterator = new TagTreePointerIterator(tagPointer);
            while (iterator.HasNext())
            {
                var currentPointer = iterator.Next();
                if (currentPointer.GetRole().Equals("H1"))
                {
                    var content = contentLookup.GetContent(currentPointer, true);
                    Console.WriteLine("Tag: " + currentPointer.GetRole() + " - " + content);
                }
            }
        }

        private static void ExtractAlternateDescriptionFromAllImages(PdfDocument doc, TagContentLookup contentLookup)
        {
            var tagPointer = doc.GetTagStructureContext().GetAutoTaggingPointer();
            var iterator = new TagTreePointerIterator(tagPointer);
            while (iterator.HasNext())
            {
                var currentPointer = iterator.Next();
                if (currentPointer.GetRole().Equals("Figure"))
                {
                    Console.WriteLine("Tag: " + currentPointer.GetRole() + " - " +
                                      currentPointer.GetProperties().GetAlternateDescription());
                }
            }
        }

        private static void ExtractSpecificMcid(PdfDocument doc, TagContentLookup contentLookup)
        {
            var tagPointer = doc.GetTagStructureContext().GetAutoTaggingPointer();
            var iterator = new TagTreePointerIterator(tagPointer);
            var pageNumber = 1;
            var mcid = 6;
            while (iterator.HasNext())
            {
                var currentPointer = iterator.Next();
                var content = contentLookup.GetContent(currentPointer, false);
                foreach (var content1 in content.Content)
                {
                    if (content1.McId == mcid && content1.PageNumber == pageNumber)
                    {
                        Console.WriteLine("Tag: " + currentPointer.GetRole() + " - " + content1);
                    }
                }
            }
        }


        private class TagTreePointerIterator
        {
            private readonly List<TagTreePointer> data = new List<TagTreePointer>();
            private int currentIndex;

            public TagTreePointerIterator(TagTreePointer pointer)
            {
                Traverse(new TagTreePointer(pointer), data);
            }


            public bool HasNext()
            {
                return currentIndex < data.Count;
            }

            public TagTreePointer Next()
            {
                return data[currentIndex++];
            }

            private static void Traverse(TagTreePointer pointer, List<TagTreePointer> pointers)
            {
                pointers.Add(new TagTreePointer(pointer));
                for (var i = 0; i < pointer.GetKidsRoles().Count; i++)
                {
                    if (pointer.GetKidsRoles()[i] == "MCR")
                    {
                        continue;
                    }

                    pointer.MoveToKid(i);
                    Traverse(pointer, pointers);
                    pointer.MoveToParent();
                }
            }
        }

        private class TagContentLookup
        {
            private readonly List<Content> mcidToContent;

            public TagContentLookup(PdfDocument doc)
            {
                var listener = new TagContentLookupListener();
                var processor = new PdfCanvasProcessor(listener);
                for (var i = 1; i <= doc.GetNumberOfPages(); i++)
                {
                    listener.NewPage(i);
                    processor.ProcessPageContent(doc.GetPage(i));
                }

                listener.Finish();
                //We are using a simple list to store the content, you could use a more advanced data structure to store the content
                //to allow for faster lookups for example a map with HashMap<Integer/*page*/,HashMap<Integer/*mcid*/, List<Content>>
                mcidToContent = listener.McidToContent;
            }


            /*
            Collapsing nodes mean let's say you have some structure on like this
             - caption
                - paragraph
             With collapsedLeaf node set to true you will Get the content of the caption and the paragraph in the same list
             Which could make sense in some cases as the paragraph is part of the caption
             */
            public ExtractionContent GetContent(TagTreePointer pointer, bool collapseLeafNodes)
            {
                var elem = pointer.GetDocument().GetTagStructureContext().GetPointerStructElem(pointer);
                return GetContent(pointer, elem, collapseLeafNodes);
            }

            private ExtractionContent GetContent(TagTreePointer pointer, PdfStructElem elem, bool collapseLeafNodes)
            {
                var extractionContent = new ExtractionContent();
                extractionContent.Content = new List<Content>();
                extractionContent.Pointer = pointer;

                foreach (var kid in elem.GetKids())
                {
                    if (kid is PdfMcrNumber || kid is PdfMcrDictionary)
                    {
                        var mcr = (PdfMcr)kid;
                        var pageNumber = GetPageNumberFromPointer(pointer.GetDocument(), elem);
                        foreach (var s in mcidToContent)
                        {
                            if (s.McId == mcr.GetMcid() && s.PageNumber == pageNumber)
                            {
                                extractionContent.Content.Add(s);
                            }
                        }
                    }
                    else if (kid is PdfStructElem && collapseLeafNodes)
                    {
                        var cCollapsed = GetContent(pointer, (PdfStructElem)kid, true);
                        if (cCollapsed.Content != null)
                        {
                            extractionContent.Content.AddRange(cCollapsed.Content);
                        }
                    }
                }

                return extractionContent;
            }
        }

        private static int GetPageNumberFromPointer(PdfDocument doc, PdfStructElem elem)
        {
            PdfObject elemPage = elem.GetPdfObject().GetAsDictionary(PdfName.Pg);
            for (var i = 1; i <= doc.GetNumberOfPages(); i++)
            {
                PdfObject page = doc.GetPage(i).GetPdfObject();
                if (page.Equals(elemPage))
                {
                    return i;
                }
            }

            return -1;
        }

        public abstract class Content
        {
            public int McId;
            public int PageNumber;
        }

        private class TextContent : Content
        {
            public TextContent(string content)
            {
                this.Content = content;
            }

            protected string Content;


            public override string ToString()
            {
                return $"{nameof(Content)}: {Content}";
            }
        }

        private class ImageContent : Content
        {
            public PdfImageXObject Image;
            public PdfName ImageName;
            public Vector StartPoint;
            public bool IsInline;

            public override string ToString()
            {
                return
                    $"{nameof(Image)}: {Image}, {nameof(ImageName)}: {ImageName}, {nameof(StartPoint)}: {StartPoint}, {nameof(IsInline)}: {IsInline}";
            }
        }

        private class TagContentLookupListener : IEventListener
        {
            public List<Content> McidToContent { get; } = new List<Content>();
            private LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
            private int pageNumber;
            private int currentMcid = -1;

            public void EventOccurred(IEventData data, EventType type)
            {
                if (type == EventType.RENDER_TEXT)
                {
                    var renderInfo = (TextRenderInfo)data;
                    if (currentMcid != renderInfo.GetMcid())
                    {
                        TextTagFinished();
                        currentMcid = renderInfo.GetMcid();
                    }

                    strategy.EventOccurred(data, type);
                }
                else if (type == EventType.RENDER_IMAGE)
                {
                    var imageRenderInfo = (ImageRenderInfo)data;
                    //Inline images might be part of a text block
                    if (currentMcid != imageRenderInfo.GetMcid())
                    {
                        TextTagFinished();
                        currentMcid = imageRenderInfo.GetMcid();
                    }

                    var imageContent = new ImageContent();
                    imageContent.Image = imageRenderInfo.GetImage();
                    imageContent.ImageName = imageRenderInfo.GetImageResourceName();
                    imageContent.StartPoint = imageRenderInfo.GetStartPoint();
                    imageContent.PageNumber = pageNumber;
                    imageContent.McId = currentMcid;
                    McidToContent.Add(imageContent);
                }
            }

            public ICollection<EventType> GetSupportedEvents()
            {
                return null;
            }


            public void NewPage(int pageNumber)
            {
                //We are starting a new page, so we should finish the current tag as the page is a new context
                TextTagFinished();
                this.pageNumber = pageNumber;
                //Reset the current Mcid as we are starting a new page
                currentMcid = -1;
            }

            public void Finish()
            {
                TextTagFinished();
            }

            private void TextTagFinished()
            {
                if (currentMcid == -1)
                {
                    return;
                }

                Content content = new TextContent(strategy.GetResultantText());
                content.McId = currentMcid;
                content.PageNumber = pageNumber;
                McidToContent.Add(content);
                strategy = new LocationTextExtractionStrategy();
                currentMcid = -1;
            }
        }

        private class ExtractionContent
        {
            public List<Content> Content;
            public TagTreePointer Pointer;

            public override string ToString()
            {
                var sb = new StringBuilder();
                foreach (var content in Content)
                {
                    sb.Append(content).Append(", ");
                }

                return sb.ToString();
            }
        }
    }
}