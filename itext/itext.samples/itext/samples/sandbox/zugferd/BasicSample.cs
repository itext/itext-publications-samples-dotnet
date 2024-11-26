using System;
using System.IO;
using System.Xml;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.XMP;
using iText.Kernel.XMP.Options;
using iText.Pdfa;
using iText.Layout.Element;
using iText.Layout;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Zugferd
{
    public class BasicSample
    {
        public const String DEST = "results/sandbox/zugferd/invoice_with_zugferd.pdf";

        public const String ZUGFERD_XML = "../../../resources/xml/factur-x.xml";

        public const String ICC_PROFILE = "../../../resources/data/sRGB_CS_profile.icm";

        public const String FONT = "../../../resources/font/FreeSans.ttf";


        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            CreateZugferdInvoice();
        }


        private static void CreateZugferdInvoice()
        {
            FileStream inputStream = new FileStream(ICC_PROFILE, FileMode.Open, FileAccess.Read);
            PdfADocument pdfDoc = new PdfADocument(new PdfWriter(DEST), PdfAConformance.PDF_A_3B,
                new PdfOutputIntent("Custom", "",
                    null, "sRGB IEC61966-2.1", inputStream));

            Document document = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(FONT, "WinAnsi", PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

            XMPMeta xmp = CreateValidXmp(pdfDoc);
            pdfDoc.SetXmpMetadata(xmp);
            document.SetFont(font);
            
            PdfDictionary parameters = new PdfDictionary();
            parameters.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
            PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDoc, File.ReadAllBytes(ZUGFERD_XML),
                "ZUGFeRD invoice", "factur-x.xml", new PdfName("application/xml"), parameters,
                PdfName.Alternative);
            pdfDoc.AddFileAttachment("ZUGFeRD invoice", fileSpec);
            PdfArray array = new PdfArray();
            array.Add(fileSpec.GetPdfObject().GetIndirectReference());
            pdfDoc.GetCatalog().Put(PdfName.AF, array);

            FillDocumentFromXml(document);
            document.Close();
        }
        
        private static void FillDocumentFromXml(Document document)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(ZUGFERD_XML);
            XmlNodeList idList = xmlDocument.GetElementsByTagName("ram:ID");
            XmlNodeList nameList = xmlDocument.GetElementsByTagName("ram:Name");
            XmlNodeList lineOneList = xmlDocument.GetElementsByTagName("ram:LineOne");

            document.Add(new Paragraph("Invoice with ZUGFeRD").SetFontSize(18));
            document.Add(
                new Paragraph("Ensuring the readability of XML data of electronic invoices").SetBackgroundColor(
                    ColorConstants.GRAY));
            Table xmlInfoTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            xmlInfoTable.AddCell("Stylesheet-Version:");
            xmlInfoTable.AddCell("2.5 vom 10.01.2023");
            xmlInfoTable.StartNewRow();
            xmlInfoTable.AddCell("Invoice Standard:");
            xmlInfoTable.AddCell("Factur-x 1.0 Profil BASIC");
            xmlInfoTable.StartNewRow();
            xmlInfoTable.AddCell("URN ID");
            xmlInfoTable.AddCell(idList[0].InnerText);
            xmlInfoTable.StartNewRow();
            xmlInfoTable.AddCell("The XML input file:");
            xmlInfoTable.AddCell("factur-x.xml");
            document.Add(xmlInfoTable);

            document.Add(new Paragraph("Seller").SetBackgroundColor(ColorConstants.GRAY));
            Table sellerTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            sellerTable.AddCell("Name:");
            sellerTable.AddCell(nameList[1].InnerText);
            sellerTable.StartNewRow();
            sellerTable.AddCell("Address:");
            sellerTable.AddCell(lineOneList[0].InnerText);
            sellerTable.StartNewRow();
            sellerTable.AddCell("Tax number:");
            sellerTable.AddCell(idList[2].InnerText);
            document.Add(sellerTable);

            document.Add(new Paragraph("Buyer / Beneficiary").SetBackgroundColor(ColorConstants.GRAY));
            Table buyerTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            buyerTable.AddCell("Name:");
            buyerTable.AddCell(nameList[2].InnerText);
            buyerTable.StartNewRow();
            buyerTable.AddCell("Address:");
            buyerTable.AddCell(lineOneList[1].InnerText);

            document.Add(buyerTable);
            document.Add(new Paragraph("Position data").SetBackgroundColor(ColorConstants.GRAY));

            Table positionTable = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();
            positionTable.AddCell("Pos number");
            positionTable.AddCell("Reference");
            positionTable.AddCell("Description");
            positionTable.AddCell("Net price");
            positionTable.AddCell("Quantity");
            positionTable.AddCell("Name");
            positionTable.AddCell("Tax rate");
            positionTable.AddCell("Net amount");
            positionTable.StartNewRow();
            positionTable.AddCell(xmlDocument.GetElementsByTagName("ram:LineID")[0].InnerText);
            positionTable.AddCell(xmlDocument.GetElementsByTagName("ram:GlobalID")[0].InnerText);
            positionTable.AddCell(nameList[0].InnerText);
            positionTable.AddCell(xmlDocument.GetElementsByTagName("ram:ChargeAmount")[0].InnerText);
            positionTable.AddCell(xmlDocument.GetElementsByTagName("ram:BilledQuantity")[0].InnerText);
            positionTable.AddCell("Stk");
            positionTable.AddCell(xmlDocument.GetElementsByTagName("ram:RateApplicablePercent")[0].InnerText);
            positionTable.AddCell(xmlDocument.GetElementsByTagName("ram:LineTotalAmount")[0].InnerText);

            document.Add(positionTable);
        }

        private static XMPMeta CreateValidXmp(PdfADocument pdfDoc)
        {
            XMPMeta xmp = pdfDoc.GetXmpMetadata(true);
            String zugferdNamespace = "urn:ferd:pdfa:CrossIndustryDocument:invoice:1p0#";
            String zugferdPrefix = "fx";
            XMPMetaFactory.GetSchemaRegistry().RegisterNamespace(zugferdNamespace, zugferdPrefix);

            xmp.SetProperty(zugferdNamespace, "DocumentType", "INVOICE");
            xmp.SetProperty(zugferdNamespace, "Version", "1.0");
            xmp.SetProperty(zugferdNamespace, "ConformanceLevel", "BASIC");
            xmp.SetProperty(zugferdNamespace, "DocumentFileName", "factur-x.xml");

            PropertyOptions bagOptions = new PropertyOptions(PropertyOptions.ARRAY);
            xmp.SetProperty(XMPConst.NS_PDFA_EXTENSION, "schemas", null, bagOptions);

            String bagPath = "pdfaExtension:schemas";

            int newItemIndex = xmp.CountArrayItems(XMPConst.NS_PDFA_EXTENSION, bagPath) + 1;
            String newItemPath = bagPath + "[" + newItemIndex + "]";

            PropertyOptions structOptions = new PropertyOptions(PropertyOptions.STRUCT);
            xmp.SetProperty(XMPConst.NS_PDFA_EXTENSION, newItemPath, null, structOptions);

            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, newItemPath, XMPConst.NS_PDFA_SCHEMA, "schema",
                "Factur-X PDFA Extension Schema");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, newItemPath, XMPConst.NS_PDFA_SCHEMA, "namespaceURI",
                zugferdNamespace);
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, newItemPath, XMPConst.NS_PDFA_SCHEMA, "prefix", "fx");

            String seqPath = newItemPath + "/pdfaSchema:property";
            PropertyOptions seqOptions = new PropertyOptions(PropertyOptions.ARRAY_ORDERED);
            xmp.SetProperty(XMPConst.NS_PDFA_EXTENSION, seqPath, null, seqOptions);

            String firstSeqItemPath = seqPath + "[1]";
            String secondSeqItemPath = seqPath + "[2]";
            String thirdSeqItemPath = seqPath + "[3]";
            String fourthSeqItemPath = seqPath + "[4]";

            xmp.SetProperty(XMPConst.NS_PDFA_EXTENSION, firstSeqItemPath, null, structOptions);
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, firstSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "name",
                "DocumentFileName");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, firstSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "valueType",
                "Text");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, firstSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "category",
                "external");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, firstSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "description",
                "The name of the embedded XML document");

            xmp.SetProperty(XMPConst.NS_PDFA_EXTENSION, secondSeqItemPath, null, structOptions);
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, secondSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "name",
                "DocumentType");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, secondSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "valueType",
                "Text");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, secondSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "category",
                "external");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, secondSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "description",
                "The type of the hybrid document in capital letters, e.g. INVOICE or ORDER");

            xmp.SetProperty(XMPConst.NS_PDFA_EXTENSION, thirdSeqItemPath, null, structOptions);
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, thirdSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "name",
                "Version");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, thirdSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "valueType",
                "Text");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, thirdSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "category",
                "external");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, thirdSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "description",
                "The actual version of the standard applying to the embedded XML document");

            xmp.SetProperty(XMPConst.NS_PDFA_EXTENSION, fourthSeqItemPath, null, structOptions);
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, fourthSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "name",
                "ConformanceLevel");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, fourthSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "valueType",
                "Text");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, fourthSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "category",
                "external");
            xmp.SetStructField(XMPConst.NS_PDFA_EXTENSION, fourthSeqItemPath, XMPConst.NS_PDFA_PROPERTY, "description",
                "The conformance level of the embedded XML document");

            return xmp;
        }
    }
}
