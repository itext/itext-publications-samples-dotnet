/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class NestedLists
    {
        public static readonly String DEST = "results/sandbox/objects/nested_list.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new NestedLists().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);

            List topLevel = new List();
            ListItem topLevelItem = new ListItem();
            topLevelItem.Add(new Paragraph().Add("Item 1"));
            topLevel.Add(topLevelItem);

            List secondLevel = new List();
            secondLevel.Add("Sub Item 1");
            ListItem secondLevelItem = new ListItem();
            secondLevelItem.Add(new Paragraph("Sub Item 2"));
            secondLevel.Add(secondLevelItem);
            topLevelItem.Add(secondLevel);

            List thirdLevel = new List();
            thirdLevel.Add("Sub Sub Item 1");
            thirdLevel.Add("Sub Sub Item 2");
            secondLevelItem.Add(thirdLevel);

            document.Add(topLevel);

            document.Close();
        }
    }
}