/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
    public class MergeAndAddFont
    {
        public static readonly String FONT = "../../resources/font/GravitasOne.ttf";

        public static readonly String[] FILE_A =
        {
            "results/sandbox/fonts/testA1.pdf",
            "results/sandbox/fonts/testA2.pdf",
            "results/sandbox/fonts/testA3.pdf"
        };

        public static readonly String[] FILE_B =
        {
            "results/sandbox/fonts/testB1.pdf",
            "results/sandbox/fonts/testB2.pdf",
            "results/sandbox/fonts/testB3.pdf"
        };

        public static readonly String[] FILE_C =
        {
            "results/sandbox/fonts/testC1.pdf",
            "results/sandbox/fonts/testC2.pdf",
            "results/sandbox/fonts/testC3.pdf"
        };

        public static readonly String[] CONTENT =
        {
            "abcdefgh", "ijklmnopq", "rstuvwxyz"
        };

        public static readonly Dictionary<String, String> DEST_NAMES = new Dictionary<string, string>();

        static MergeAndAddFont()
        {
            DEST_NAMES.Add("A1", "testA_merged1.pdf");
            DEST_NAMES.Add("A2", "testA_merged2.pdf");
            DEST_NAMES.Add("B1", "testB_merged1.pdf");
            DEST_NAMES.Add("B2", "testB_merged2.pdf");
            DEST_NAMES.Add("C1", "testC_merged1.pdf");
            DEST_NAMES.Add("C2", "testC_merged2.pdf");
        }

        public static readonly String DEST = "results/sandbox/fonts/";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MergeAndAddFont().ManipulatePdf(DEST);
        }

        public void CreatePdf(String filename, String text, bool embedded, bool subset)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filename));
            Document doc = new Document(pdfDoc);

            // The 3rd parameter indicates whether the font is to be embedded into the target document.
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, embedded);

            // When set to true, only the used glyphs will be included in the font.
            // When set to false, the full font will be included and all subset ranges will be removed.
            font.SetSubset(subset);
            doc.Add(new Paragraph(text).SetFont(font));

            doc.Close();
        }

        public void MergeFiles(String[] files, String result, bool isSmartModeOn)
        {
            PdfWriter writer = new PdfWriter(result);

            // In smart mode when resources (such as fonts, images,...) are encountered,
            // a reference to these resources is saved in a cache and can be reused.
            // This mode reduces the file size of the resulting PDF document.
            writer.SetSmartMode(isSmartModeOn);
            PdfDocument pdfDoc = new PdfDocument(writer);

            // This method initializes an outline tree of the document and sets outline mode to true.
            pdfDoc.InitializeOutlines();

            for (int i = 0; i < files.Length; i++)
            {
                PdfDocument addedDoc = new PdfDocument(new PdfReader(files[i]));
                addedDoc.CopyPagesTo(1, addedDoc.GetNumberOfPages(), pdfDoc);
                addedDoc.Close();
            }

            pdfDoc.Close();
        }

        protected void ManipulatePdf(String dest)
        {
            for (int i = 0; i < FILE_A.Length; i++)
            {
                
                // Create pdf files with font, which will be embedded into the target document,
                // and only the used glyphs will be included in the font.
                CreatePdf(FILE_A[i], CONTENT[i], true, true);
            }

            MergeFiles(FILE_A, dest + DEST_NAMES["A1"], false);
            MergeFiles(FILE_A, dest + DEST_NAMES["A2"], true);

            for (int i = 0; i < FILE_B.Length; i++)
            {
                
                // Create pdf files with font, which will embedded into the target document.
                // Full font will be included and all subset ranges will be removed
                CreatePdf(FILE_B[i], CONTENT[i], true, false);
            }

            MergeFiles(FILE_B, dest + DEST_NAMES["B1"], false);
            MergeFiles(FILE_B, dest + DEST_NAMES["B2"], true);

            for (int i = 0; i < FILE_C.Length; i++)
            {
                
                // Create pdf files with font, which won't be embedded into the target document.
                // Full font will be included and all subset ranges will be removed
                CreatePdf(FILE_C[i], CONTENT[i], false, false);
            }

            MergeFiles(FILE_C, dest + DEST_NAMES["C1"], true);

            // Embed the font manually
            EmbedFont(dest + DEST_NAMES["C1"], FONT, dest + DEST_NAMES["C2"]);
        }

        protected void EmbedFont(String merged, String fontfile, String result)
        {
            
            // The font file
            FileStream raf = new FileStream(fontfile, FileMode.Open, FileAccess.Read);
            byte[] fontbytes = new byte[(int) raf.Length];
            raf.Read(fontbytes, 0, fontbytes.Length);
            raf.Close();

            // Create a new stream for the font file
            PdfStream stream = new PdfStream(fontbytes);
            stream.SetCompressionLevel(CompressionConstants.DEFAULT_COMPRESSION);
            stream.Put(PdfName.Length1, new PdfNumber(fontbytes.Length));

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(merged), new PdfWriter(result));
            int numberOfPdfObjects = pdfDoc.GetNumberOfPdfObjects();

            // Search for the font dictionary
            for (int i = 0; i < numberOfPdfObjects; i++)
            {
                PdfObject pdfObject = pdfDoc.GetPdfObject(i);
                if (pdfObject == null || !pdfObject.IsDictionary())
                {
                    continue;
                }

                PdfDictionary fontDictionary = (PdfDictionary) pdfObject;
                PdfFont font = PdfFontFactory.CreateFont(fontfile, PdfEncodings.WINANSI);
                PdfName fontname = new PdfName(font.GetFontProgram().GetFontNames().GetFontName());
                if (PdfName.FontDescriptor.Equals(fontDictionary.Get(PdfName.Type))
                    && fontname.Equals(fontDictionary.Get(PdfName.FontName)))
                {
                    
                    // Embed the passed font to the pdf document
                    fontDictionary.Put(PdfName.FontFile2, stream.MakeIndirect(pdfDoc).GetIndirectReference());
                }
            }

            pdfDoc.Close();
        }
    }
}