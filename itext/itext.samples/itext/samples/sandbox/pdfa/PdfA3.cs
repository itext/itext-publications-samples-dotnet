/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfa;
using Path = System.IO.Path;

namespace iText.Samples.Sandbox.Pdfa
{
    public class PdfA3
    {
        public static readonly string DEST = "results/sandbox/pdfa/pdf_a3.pdf";

        public static readonly String BOLD = "../../../resources/font/OpenSans-Bold.ttf";

        public static readonly String DATA = "../../../resources/data/united_states.csv";

        public static readonly String FONT = "../../../resources/font/OpenSans-Regular.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfA3().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            PdfFont bold = PdfFontFactory.CreateFont(BOLD, PdfEncodings.IDENTITY_H);

            Stream fileStream =
                new FileStream("../../../resources/data/sRGB_CS_profile.icm", FileMode.Open, FileAccess.Read);
            PdfADocument pdfDoc = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_3B,
                new PdfOutputIntent("Custom", "",
                    null, "sRGB IEC61966-2.1", fileStream));

            Document document = new Document(pdfDoc, PageSize.A4.Rotate());

            PdfDictionary parameters = new PdfDictionary();
            parameters.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
            
            // Embeds file to the document 
            PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDoc, 
                File.ReadAllBytes(DATA), 
                "united_states.csv", "united_states.csv",
                new PdfName("text/csv"), parameters, PdfName.Data);
            pdfDoc.AddAssociatedFile("united_states.csv", fileSpec);

            Table table = new Table(UnitValue.CreatePercentArray(
                new float[] {4, 1, 3, 4, 3, 3, 3, 3, 1})).UseAllAvailableWidth();

            using (StreamReader streamReader = new StreamReader(DATA))
            {
                // Reads content of csv file
                String line = streamReader.ReadLine();

                Process(table, line, bold, 10, true);
                while ((line = streamReader.ReadLine()) != null)
                {
                    Process(table, line, font, 10, false);
                }

                streamReader.Close();
            }

            document.Add(table);
            document.Close();
        }

        public virtual void Process(Table table, String line, PdfFont font, int fontSize, bool isHeader)
        {
            // Parses csv string line with specified delimiter
            StringTokenizer tokenizer = new StringTokenizer(line, ";");

            while (tokenizer.HasMoreTokens())
            {
                Paragraph content = new Paragraph(tokenizer.NextToken()).SetFont(font).SetFontSize(fontSize);

                if (isHeader)
                {
                    table.AddHeaderCell(content);
                }
                else
                {
                    table.AddCell(content);
                }
            }
        }
    }
}