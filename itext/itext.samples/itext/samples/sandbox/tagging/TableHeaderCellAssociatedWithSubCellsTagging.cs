using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Layout.Tagging;

namespace iText.Samples.Sandbox.Tagging 
{
    public class TableHeaderCellAssociatedWithSubCellsTagging 
    {
        public static readonly String DEST = "results/sandbox/tagging/TableHeaderCellAssociatedWithSubCellsTagging.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new TableHeaderCellAssociatedWithSubCellsTagging().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            pdfDocument.SetTagged();
            
            Document doc = new Document(pdfDocument);
            
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            
            // Initialize ID strings beforehand. Every ID should be unique across the document
            PdfString[] headersId = 
            { 
                        // Since '/ID' is a `byte string` according to specification we are not passing
                        // encoding to the constructor of the PdfString
                        new PdfString("header_id_0"), new PdfString("header_id_1"), new PdfString("header_id_2") 
            };
            
            for (int i = 0; i < 3; ++i) 
            {
                Cell c = new Cell().Add(new Paragraph("Header " + (i + 1)));
                AccessibilityProperties ap = c.GetAccessibilityProperties();
                ap.SetRole(StandardRoles.TH).SetStructureElementId(headersId[i].GetValueBytes());
                table.AddHeaderCell(c);
            }
            
            List<TaggingHintKey> colSpanHints = new List<TaggingHintKey>();
            for (int i = 0; i < 4; i++) 
            {
                Cell c;
                if (i < 3) 
                {
                    c = new Cell().Add(new Paragraph((i + 1).ToString()));
                }
                else 
                {
                    // Colspan creation
                    c = new Cell(1, 3).Add(new Paragraph((i + 1).ToString()));
                }
                
                if (i < 3) 
                {
                    // Correct table tagging requires marking which headers correspond to the given cell.
                    // The correspondence is defined by header cells tags IDs. For table cells without
                    // col/row spans it's easy to reference a header: just add proper
                    // PdfStructureAttributes to it. Table cells with col spans are processed below.
                    PdfStructureAttributes tableAttributes = new PdfStructureAttributes("Table");
                    PdfArray headers;
                    headers = new PdfArray(headersId[i % headersId.Length]);
                    tableAttributes.GetPdfObject().Put(PdfName.Headers, headers);
                    c.GetAccessibilityProperties().AddAttributes(tableAttributes);
                }
                else 
                {
                    // When we add PdfStructureAttributes to the element these attributes override any
                    // attributes generated for it automatically. E.g. cells with colspan require properly
                    // generated attributes which describe the colspan (e.g. which columns this cell spans).
                    // So here we will use a different approach: fetch the tag which will be created for
                    // the cell and modify attributes object directly.
                    TaggingHintKey colSpanCellHint = LayoutTaggingHelper.GetOrCreateHintKey(c);
                    colSpanHints.Add(colSpanCellHint);
                }
                
                table.AddCell(c);
            }
            
            doc.Add(table);
            
            // After table has been drawn on the page, we can modify the colspan cells tags
            foreach (TaggingHintKey colSpanHint in colSpanHints) 
            {
                WaitingTagsManager waitingTagsManager = pdfDocument.GetTagStructureContext().GetWaitingTagsManager();
                TagTreePointer p = new TagTreePointer(pdfDocument);
                
                // Move tag pointer to the colspan cell using its hint
                if (!waitingTagsManager.TryMovePointerToWaitingTag(p, colSpanHint)) 
                {
                    // It is not expected to happen ever if immediate-flush is enabled (which is by default),
                    // otherwise this should be done after the flush
                    throw new InvalidOperationException("A work-around does not work. A tag for the cell is " +
                                                        "not created or cannot be found.");
                }
                
                foreach (PdfStructureAttributes attr in p.GetProperties().GetAttributesList()) 
                {
                    if ("Table".Equals(attr.GetAttributeAsEnum("O"))) 
                    {
                        // Specify all the headers for the column spanning (all of 3)
                        PdfArray headers = new PdfArray(headersId);
                        attr.GetPdfObject().Put(PdfName.Headers, headers);
                        break;
                    }
                }
            }

            doc.Close();
        }
    }
}
