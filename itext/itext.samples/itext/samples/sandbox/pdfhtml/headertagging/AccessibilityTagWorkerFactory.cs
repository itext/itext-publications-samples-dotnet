/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Headertagging
{
    public class AccessibilityTagWorkerFactory : DefaultTagWorkerFactory
    {
        public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
        {
            switch (tag.Name())
            {
                case "h1":
                    return new CustomHTagWorker(tag, context, 1);
                case "h2":
                    return new CustomHTagWorker(tag, context, 2);
                case "h3":
                    return new CustomHTagWorker(tag, context, 3);
                case "h4":
                    return new CustomHTagWorker(tag, context, 4);
                case "h5":
                    return new CustomHTagWorker(tag, context, 5);
                case "h6":
                    return new CustomHTagWorker(tag, context, 6);
                case "th":
                    return new CustomThTagWorker(tag, context);
                default:
                    return null;
            }
        }
    }
}