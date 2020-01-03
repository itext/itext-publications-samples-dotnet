/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Kernel.Pdf.Tagging;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Headertagging
{
    public class CustomThTagWorker : TdTagWorker
    {
        public CustomThTagWorker(IElementNode element, ProcessorContext context) : base(element, context) 
        {
        }
        
        public override IPropertyContainer GetElementResult() {
            Cell cell = (Cell) base.GetElementResult();
            cell.GetAccessibilityProperties().SetRole(StandardRoles.TH);
            return base.GetElementResult();
        }
    }
}