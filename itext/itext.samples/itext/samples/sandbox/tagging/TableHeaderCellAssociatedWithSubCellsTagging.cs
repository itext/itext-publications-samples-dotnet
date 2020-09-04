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
            
            PdfName idTreeName = new PdfName("IDTree");
            PdfNameTree idTree = new PdfNameTree(pdfDocument.GetCatalog(), idTreeName);
            for (int i = 0; i < 3; ++i) 
            {
                Cell c = new Cell().Add(new Paragraph("Header " + (i + 1)));
                c.GetAccessibilityProperties().SetRole(StandardRoles.TH);
                table.AddHeaderCell(c);
                PdfString headerId = headersId[i];
                
                // Use custom renderer for cell header element in order to add ID to its tag
                CellRenderer renderer = new StructIdCellRenderer(c, doc, headerId, idTree);
                c.SetNextRenderer(renderer);
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
            
            pdfDocument.GetStructTreeRoot().GetPdfObject().Put(idTreeName, idTree.BuildTree()
                    .MakeIndirect(pdfDocument));
            
            doc.Close();
        }

        private class StructIdCellRenderer : CellRenderer 
        {
            private readonly PdfDocument pdfDocument;
            private readonly PdfNameTree idTree;
            private readonly TagStructureContext tagContext;
            private readonly PdfString headerId;

            public StructIdCellRenderer(Cell c, Document document, PdfString headerId, PdfNameTree idTree) : base(c) 
            {
                this.pdfDocument = document.GetPdfDocument();
                this.idTree = idTree;
                this.tagContext = pdfDocument.GetTagStructureContext();
                this.headerId = headerId;
            }

            public override void Draw(DrawContext drawContext) 
            {
                LayoutTaggingHelper taggingHelper = GetProperty<LayoutTaggingHelper>(Property.TAGGING_HELPER);
                
                // We want to reach the actual tag from logical structure tree, in order to set custom properties, for
                // which iText doesn't provide convenient API at the moment. Specifically we are aiming at setting /ID
                // entry in structure element dictionary corresponding to the table header cell. Here we are creating tag
                // for the current element in logical structure tree right at the beginning of #draw method.
                // If this particular instance of header cell is paging artifact it would be marked so by layouting
                // engine and it would not allow to create a tag (return value of the method would be 'false').
                // If this particular instance of header cell is the header which is to be tagged, a tag will be created.
                // It's safe to create a tag at this moment, it will be picked up and placed at correct position in the
                // logical structure tree later by layout engine.
                
                TagTreePointer p = new TagTreePointer(pdfDocument);
                if (taggingHelper.CreateTag(this, p)) 
                {
                    // After the tag is created, we can fetch low level entity PdfStructElem
                    // in order to work with it directly. These changes would be directly reflected
                    // in the PDF file inner structure.
                    PdfStructElem structElem = tagContext.GetPointerStructElem(p);
                    PdfDictionary structElemDict = structElem.GetPdfObject();
                    structElemDict.Put(PdfName.ID, headerId);
                    idTree.AddEntry(headerId.GetValue(), structElemDict);
                }
                
                base.Draw(drawContext);
            }
        }
    }
}
