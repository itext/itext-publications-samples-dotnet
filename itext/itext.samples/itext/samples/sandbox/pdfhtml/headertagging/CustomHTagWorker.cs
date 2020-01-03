/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Headertagging
{
    public class CustomHTagWorker : DivTagWorker
    {
        private int i;
        public CustomHTagWorker(IElementNode element, ProcessorContext context, int i) : base(element, context)
        {
            this.i = i;
        }

        public override IPropertyContainer GetElementResult()
        {
            Div div = (Div) base.GetElementResult();
            div.GetAccessibilityProperties().SetRole("H" + i);
            return base.GetElementResult();
        }
    }
}