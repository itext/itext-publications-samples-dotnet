using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Annotations
{
   
    // RenameDestinations.cs
    // 
    // This class demonstrates how to rename named destinations in a PDF document.
    // The code opens a PDF with existing named destinations, creates new names by
    // prefixing "new" to each original name, updates the name tree, and then
    // modifies all link annotations to point to the renamed destinations.
    // This example shows how to maintain document navigation integrity while
    // changing the internal destination references.
 
    public class RenameDestinations
    {
        public static readonly String DEST = "results/sandbox/annotations/rename_destinations.pdf";

        public static readonly String SRC = "../../../resources/pdfs/nameddestinations.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RenameDestinations().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Dictionary<String, PdfString> renamed = new Dictionary<String, PdfString>();

            PdfNameTree nameTree = pdfDoc.GetCatalog().GetNameTree(PdfName.Dests);
            IDictionary<PdfString, PdfObject> names = nameTree.GetNames();

            // Loop over all named destinations and rename its string values with new names
            foreach (PdfString key in nameTree.GetKeys())
            {
                String oldName = key.ToUnicodeString();
                PdfString newName = new PdfString("new" + oldName);

                names.Add(newName, names[key]);
                names.Remove(key);
                renamed.Add(oldName, newName);
            }

            // Specify that the name tree has been modified
            // This implies that the name tree will be rewritten on close() method.
            nameTree.SetModified();

            PdfDictionary page = pdfDoc.GetPage(1).GetPdfObject();
            PdfArray annotations = page.GetAsArray(PdfName.Annots);

            // Loop over all link annotations of the first page and change their destinations.
            for (int i = 0; i < annotations.Size(); i++)
            {
                PdfDictionary annotation = annotations.GetAsDictionary(i);
                PdfDictionary action = annotation.GetAsDictionary(PdfName.A);
                if (action == null)
                {
                    continue;
                }

                PdfString n = action.GetAsString(PdfName.D);
                if (n != null && renamed.ContainsKey(n.ToString()))
                {
                    action.Put(PdfName.D, renamed[n.ToString()]);
                }
            }

            pdfDoc.Close();
        }
    }
}