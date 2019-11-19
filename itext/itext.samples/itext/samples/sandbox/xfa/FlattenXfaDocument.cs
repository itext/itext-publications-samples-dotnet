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
using iTextSharp.tool.xml.xtra.xfa;

namespace iText.Samples.Sandbox.Xfa
{
    public class FlattenXfaDocument
    {
        public static readonly String DEST = "results/sandbox/xfa/flattened.pdf";

        public static readonly String XFA = "../../resources/xfa/xfa.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FlattenXfaDocument().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            MetaData metaData = new MetaData()
                .SetAuthor("iText Samples")
                .SetLanguage("EN")
                .SetSubject("Showing off our flattening skills")
                .SetTitle("Flattened XFA");

            XFAFlattenerProperties flattenerProperties = new XFAFlattenerProperties()
                .SetPdfVersion(XFAFlattenerProperties.PDF_1_7)
                .CreateXmpMetaData()
                .SetTagged()
                .SetMetaData(metaData);

            List<String> javascriptEvents = new List<string>()
            {
                "click"
            };

            XFAFlattener xfaFlattener = new XFAFlattener()
                .SetExtraEventList(javascriptEvents)
                .SetFlattenerProperties(flattenerProperties)
                .SetViewMode(XFAFlattener.ViewMode.SCREEN);

            xfaFlattener.Flatten(new FileStream(XFA, FileMode.Open), new FileStream(dest, FileMode.Create));
        }
    }
}