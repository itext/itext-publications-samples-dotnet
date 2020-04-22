using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;

namespace iText.Samples.Sandbox.Acroforms 
{
    public class JawsFlattenedButtonFieldsTagging 
    {
        public static readonly string DEST = "results/sandbox/acroforms/jawsRecognition.pdf";
        public static readonly string SRC = "../../../resources/pdfs/jawsRecognition.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new JawsFlattenedButtonFieldsTagging().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDocument = InitializeDocument(dest, SRC);
            
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, false);
            
            // Here we handle radio buttons and checkboxes (Button fields type) but there are also other field types
            // which can be used as well, for example they are Text fields, Choice fields, Signature fields
            foreach (PdfFormField field in form.GetFormFields().Values) 
            {
                if (field.GetFieldFlag(PdfButtonFormField.FF_RADIO)) 
                {
                    AddAttributes("rb", field, pdfDocument);
                }
                else 
                {
                    // Checkbox existence should be checked by verifying if its field type is Btn and that a Push button
                    // and Radio flags are both clear
                    if (field.GetFormType().Equals(PdfName.Btn) && ((!field.GetFieldFlag(PdfButtonFormField.FF_RADIO)) && (!field
                        .GetFieldFlag(PdfButtonFormField.FF_PUSH_BUTTON)))) 
                    {
                        AddAttributes("cb", field, pdfDocument);
                    }
                }
            }
            
            form.FlattenFields();
            
            pdfDocument.Close();
        }

        private static void AddAttributes(String attributeValue, PdfFormField pdfFormField, PdfDocument pdfDocument)
        {
            foreach (PdfWidgetAnnotation widget in pdfFormField.GetWidgets()) 
            {
                PdfDictionary pdfObject = widget.GetPage().GetPdfObject();
                int i = widget.GetPdfObject().GetAsNumber(PdfName.StructParent).IntValue();
                PdfObjRef objRef = pdfDocument.GetStructTreeRoot().FindObjRefByStructParentIndex(pdfObject, i);
                if (objRef != null)
                {
                    TagTreePointer p = pdfDocument.GetTagStructureContext().CreatePointerForStructElem((PdfStructElem)objRef.GetParent());
                    IList<PdfStructureAttributes> attributes = p.GetProperties().GetAttributesList();
                    bool printFieldAttrFound = false;
                    foreach (PdfStructureAttributes attribute in attributes) 
                    {
                        if (attribute.GetAttributeAsEnum("O").Equals("PrintField")) 
                        {
                            printFieldAttrFound = true;
                            break;
                        }
                    }
                    if (!printFieldAttrFound) 
                    {
                        p.GetProperties().AddAttributes(new PdfStructureAttributes("PrintField").AddEnumAttribute("Role", attributeValue));
                    }
                }
                else 
                {
                    Console.Out.WriteLine("The object reference couldn't be found.");
                    return;
                }
            }
        }

        // Create the new document using already existed one. New document can be also created using iText, but this approach
        // will require adding custom form fields first of all
        private static PdfDocument InitializeDocument(String dest, String src) 
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            
            if (!pdfDocument.IsTagged()) 
            {
                Console.Out.WriteLine("The document should be tagged to add desired attributes to it.");
            }
            
            return pdfDocument;
        }
    }
}
