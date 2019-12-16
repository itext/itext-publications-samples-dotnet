/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;

namespace iText.Samples.Sandbox.Tagging
{
    public class WalkTheTree
    {
        public static readonly String DEST = "results/txt/walk_the_tree.txt";

        public static readonly String SRC = "../../resources/pdfs/tree_test.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new WalkTheTree().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC));
            StringBuilder builder = new StringBuilder();

            Process(pdfDoc.GetStructTreeRoot(), builder);

            CreateResultTxt(dest, builder.ToString());
        }

        private static void Process(IStructureNode elem, StringBuilder builder)
        {
            if (elem == null)
            {
                return;
            }

            builder.Append("Role: " + elem.GetRole() + "\n");
            builder.Append("Class name: " + elem.GetType().FullName + "\n");
            if (elem is PdfStructElem)
            {
                ProcessStructElem((PdfStructElem) elem, builder);
            }

            if (elem.GetKids() != null)
            {
                foreach (IStructureNode structElem in elem.GetKids())
                {
                    Process(structElem, builder);
                }
            }
        }

        private static void ProcessStructElem(PdfStructElem elem, StringBuilder builder)
        {
            PdfDictionary page = elem.GetPdfObject().GetAsDictionary(PdfName.Pg);
            if (page == null)
            {
                return;
            }

            PdfStream contents = page.GetAsStream(PdfName.Contents);
            if (contents != null)
            {
                builder.Append("Content: \n" + Encoding.UTF8.GetString(contents.GetBytes()) + "\n");
            }
            else
            {
                PdfArray array = page.GetAsArray(PdfName.Contents);
                builder.Append("Contents array: " + array + "\n");
            }
        }

        private static void CreateResultTxt(String dest, String text)
        {
            using (StreamWriter writer = new StreamWriter(dest))
            {
                writer.Write(text);
            }
        }
    }
}