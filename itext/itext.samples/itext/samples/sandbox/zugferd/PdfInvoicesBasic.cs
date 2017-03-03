/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV

*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using iText.IO;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.XMP;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.License;
using iText.Samples.Sandbox.Zugferd.Data;
using iText.Samples.Sandbox.Zugferd.Pojo;
using iText.Test.Attributes;
using iText.Zugferd;
using iText.Zugferd.Profiles;
using NUnit.Framework;

namespace iText.Samples.Sandbox.Zugferd
{
    /// <summary>
    /// Reads invoice data from a test database and creates ZUGFeRD invoices
    /// (Basic profile).
    /// </summary>
    /// <author>Bruno Lowagie</author>
    public class PdfInvoicesBasic : GenericTest {
        public static readonly String DEST = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/test/resources/zugferd/pdfa/basic00000.pdf";

        public static readonly String DEST_PATTERN = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/test/resources/zugferd/pdfa/basic{0:00000}.pdf";

        public static readonly String ICC = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/data/sRGB_CS_profile.icm";

        public static readonly String FONT = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/font/OpenSans-Regular.ttf";

        public static readonly String FONTB = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/font/OpenSans-Bold.ttf";

        protected internal PdfFont font;

        protected internal PdfFont fontb;

        // Since all the document are technically almost the same
        // we will check only the first one
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="ParserConfigurationException"/>
        /// <exception cref="SQLException"/>
        /// <exception cref="SAXException"/>
        /// <exception cref="TransformerException"/>
        /// <exception cref="iText.Kernel.XMP.XMPException"/>
        /// <exception cref="ParseException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        public static void Main(String[] args) {
            new PdfInvoicesBasic().ManipulatePdf(DEST_PATTERN);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="ParserConfigurationException"/>
        /// <exception cref="SQLException"/>
        /// <exception cref="SAXException"/>
        /// <exception cref="TransformerException"/>
        /// <exception cref="iText.Kernel.XMP.XMPException"/>
        /// <exception cref="ParseException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        protected override void ManipulatePdf(String dest) {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-multiple-products.xml");
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Directory.CreateDirectory(Directory.GetParent(DEST_PATTERN).FullName);
            PdfInvoicesBasic app = new PdfInvoicesBasic();
            PojoFactory factory = PojoFactory.GetInstance();
            IList<Invoice> invoices = factory.GetInvoices();
            foreach (Invoice invoice in invoices) {
                app.CreatePdf(invoice);
            }
            factory.Close();
        }

        /// <exception cref="ParserConfigurationException"/>
        /// <exception cref="SAXException"/>
        /// <exception cref="TransformerException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="iText.Kernel.XMP.XMPException"/>
        /// <exception cref="ParseException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        public virtual void CreatePdf(Invoice invoice) {
            font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, true);
            fontb = PdfFontFactory.CreateFont(FONTB, PdfEncodings.WINANSI, true);
            String dest = String.Format(DEST_PATTERN, invoice.GetId());
            InvoiceData invoiceData = new InvoiceData();
            IBasicProfile basic = invoiceData.CreateBasicProfileData(invoice, true);
            FileStream @is = new FileStream(ICC, FileMode.Open, FileAccess.Read);
            ZugferdDocument pdfDoc = new ZugferdDocument(new PdfWriter(dest), new PdfOutputIntent("Custom", "", "http://www.color.org"
                , "sRGB IEC61966-2.1", @is));
            Document document = new Document(pdfDoc, new PageSize(PageSize.A4));
            XMPMeta xmp = XMPMetaFactory.ParseFromBuffer(pdfDoc.GetXmpMetadata(true));
            xmp.SetProperty(ZugferdXMPUtil.ZUGFERD_SCHEMA_NS, ZugferdXMPUtil.ZUGFERD_DOCUMENT_FILE_NAME, "ZUGFeRD-invoice.xml"
                );
            pdfDoc.SetXmpMetadata(xmp);
            // header
            Paragraph p;
            p = new Paragraph(basic.GetName() + " " + basic.GetId()).SetFont(font).SetFontSize(14);
            p.SetTextAlignment(TextAlignment.RIGHT);
            document.Add(p);
            p = new Paragraph(ConvertDate(basic.GetDateTime(), "MMM dd, yyyy")).SetFont(font).SetFontSize(12);
            p.SetTextAlignment(TextAlignment.RIGHT);
            document.Add(p);
            // Address seller / buyer
            Table table = new Table(2);
            table.SetWidthPercent(100);
            Cell seller = GetPartyAddress("From:", basic.GetSellerName(), basic.GetSellerLineOne(), basic.GetSellerLineTwo
                (), basic.GetSellerCountryID(), basic.GetSellerPostcode(), basic.GetSellerCityName());
            table.AddCell(seller);
            Cell buyer = GetPartyAddress("To:", basic.GetBuyerName(), basic.GetBuyerLineOne(), basic.GetBuyerLineTwo()
                , basic.GetBuyerCountryID(), basic.GetBuyerPostcode(), basic.GetBuyerCityName());
            table.AddCell(buyer);
            seller = GetPartyTax(basic.GetSellerTaxRegistrationID(), basic.GetSellerTaxRegistrationSchemeID());
            table.AddCell(seller);
            buyer = GetPartyTax(basic.GetBuyerTaxRegistrationID(), basic.GetBuyerTaxRegistrationSchemeID());
            table.AddCell(buyer);
            document.Add(table);
            // line items
            table = new Table(UnitValue.CreatePercentArray(new float[] { 7, 2, 1, 2, 2, 2 }));
            table.SetWidthPercent(100);
            table.SetMarginTop(10);
            table.SetMarginBottom(10);
            table.AddCell(GetCell("Item:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("Price:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("Qty:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("Subtotal:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("VAT:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("Total:", TextAlignment.LEFT, fontb, 12));
            Product product;
            foreach (Item item in invoice.GetItems()) {
                product = item.GetProduct();
                table.AddCell(GetCell(product.GetName(), TextAlignment.LEFT, font, 12));
                table.AddCell(GetCell(InvoiceData.Format2dec(InvoiceData.Round(product.GetPrice())), TextAlignment.RIGHT, 
                    font, 12));
                table.AddCell(GetCell(item.GetQuantity().ToString(), TextAlignment.RIGHT, font, 12));
                table.AddCell(GetCell(InvoiceData.Format2dec(InvoiceData.Round(item.GetCost())), TextAlignment.RIGHT, font
                    , 12));
                table.AddCell(GetCell(InvoiceData.Format2dec(InvoiceData.Round(product.GetVat())), TextAlignment.RIGHT, font
                    , 12));
                table.AddCell(GetCell(InvoiceData.Format2dec(InvoiceData.Round(item.GetCost() + ((item.GetCost() * product
                    .GetVat()) / 100))), TextAlignment.RIGHT, font, 12));
            }
            document.Add(table);
            // grand totals
            document.Add(GetTotalsTable(basic.GetTaxBasisTotalAmount(), basic.GetTaxTotalAmount(), basic.GetGrandTotalAmount
                (), basic.GetGrandTotalAmountCurrencyID(), basic.GetTaxTypeCode(), basic.GetTaxApplicablePercent(), basic
                .GetTaxBasisAmount(), basic.GetTaxCalculatedAmount(), basic.GetTaxCalculatedAmountCurrencyID()));
            // payment info
            document.Add(GetPaymentInfo(basic.GetPaymentReference(), basic.GetPaymentMeansPayeeFinancialInstitutionBIC
                (), basic.GetPaymentMeansPayeeAccountIBAN()));
            // XML version
            InvoiceDOM dom = new InvoiceDOM(basic);
            PdfDictionary parameters = new PdfDictionary();
            parameters.Put(PdfName.ModDate, new PdfDate().GetPdfObject());
            // platform-independent newlines
            byte[] xml = Encoding.UTF8.GetBytes(JavaUtil.GetStringForBytes(dom.ToXML()).Replace("\r\n", "\n"));
            PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDoc, xml, "ZUGFeRD invoice", "ZUGFeRD-invoice.xml"
                , new PdfName("application/xml"), parameters, PdfName.Alternative, false);
            pdfDoc.AddFileAttachment("ZUGFeRD invoice", fileSpec);
            PdfArray array = new PdfArray();
            array.Add(fileSpec.GetPdfObject().GetIndirectReference());
            pdfDoc.GetCatalog().Put(PdfName.AF, array);
            document.Close();
        }

        public virtual Cell GetPartyAddress(String who, String name, String line1, String line2, String countryID, 
            String postcode, String city) {
            Cell cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            if (null != who) {
                cell.Add(new Paragraph(who).SetFont(fontb).SetFontSize(12));
            }
            if (null != name) {
                cell.Add(new Paragraph(name).SetFont(font).SetFontSize(12));
            }
            if (null != line1) {
                cell.Add(new Paragraph(line1).SetFont(font).SetFontSize(12));
            }
            if (null != line2) {
                cell.Add(new Paragraph(line2).SetFont(font).SetFontSize(12));
            }
            if (null != countryID && null != postcode && null != city) {
                cell.Add(new Paragraph(String.Format("{0}-{1} {2}", countryID, postcode, city)).SetFont(font).SetFontSize(12)
                    );
            }
            return cell;
        }

        public virtual Cell GetPartyTax(String[] taxId, String[] taxSchema) {
            Cell cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            cell.Add(new Paragraph("Tax ID(s):").SetFont(fontb).SetFontSize(10));
            if (taxId.Length == 0) {
                cell.Add(new Paragraph("Not applicable").SetFont(font).SetFontSize(10));
            }
            else {
                int n = taxId.Length;
                for (int i = 0; i < n; i++) {
                    cell.Add(new Paragraph(String.Format("{0}: {1}", taxSchema[i], taxId[i])).SetFont(font).SetFontSize(10));
                }
            }
            return cell;
        }

        public virtual Table GetTotalsTable(String tBase, String tTax, String tTotal, String tCurrency, String[] type
            , String[] percentage, String[] @base, String[] tax, String[] currency) {
            Table table = new Table(new float[]{1f*100/12, 1f*100/12, 3f*100/12, 3f*100/12, 3f*100/12, 1f*100/12});
            table.AddCell(GetCell("TAX", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("%", TextAlignment.RIGHT, fontb, 12));
            table.AddCell(GetCell("Base amount:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("Tax amount:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("Total:", TextAlignment.LEFT, fontb, 12));
            table.AddCell(GetCell("", TextAlignment.LEFT, fontb, 12));
            int n = type.Length;
            for (int i = 0; i < n; i++) {
                table.AddCell(GetCell(type[i], TextAlignment.RIGHT, font, 12));
                table.AddCell(GetCell(percentage[i], TextAlignment.RIGHT, font, 12));
                table.AddCell(GetCell(@base[i], TextAlignment.RIGHT, font, 12));
                table.AddCell(GetCell(tax[i], TextAlignment.RIGHT, font, 12));
                double total = double.Parse(@base[i], CultureInfo.InvariantCulture) + double.Parse
                    (tax[i], CultureInfo.InvariantCulture);
                table.AddCell(GetCell(InvoiceData.Format2dec(InvoiceData.Round(total)), TextAlignment.RIGHT, font, 12));
                table.AddCell(GetCell(currency[i], TextAlignment.LEFT, font, 12));
            }
            Cell cell = GetCell(1, 2, "", TextAlignment.LEFT, font, 12);
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);
            table.AddCell(GetCell(tBase, TextAlignment.RIGHT, fontb, 12));
            table.AddCell(GetCell(tTax, TextAlignment.RIGHT, fontb, 12));
            table.AddCell(GetCell(tTotal, TextAlignment.RIGHT, fontb, 12));
            table.AddCell(GetCell(tCurrency, TextAlignment.LEFT, fontb, 12));
            return table;
        }

        public virtual Cell GetCell(int rowspan, int colspan, String value, TextAlignment? alignment, PdfFont font
            , int fontSize) {
            Cell cell = new Cell(rowspan, colspan);
            Paragraph p = new Paragraph(value).SetFont(font).SetFontSize(fontSize);
            p.SetTextAlignment(alignment);
            cell.Add(p);
            return cell;
        }

        public virtual Cell GetCell(String value, TextAlignment? alignment, PdfFont font, int fontSize) {
            return GetCell(1, 1, value, alignment, font, fontSize);
        }

        public virtual Paragraph GetPaymentInfo(String @ref, String[] bic, String[] iban) {
            Paragraph p = new Paragraph(String.Format("Please wire the amount due to our bank account using the following reference: {0}"
                , @ref)).SetFont(font).SetFontSize(12);
            int n = bic.Length;
            for (int i = 0; i < n; i++) {
                p.Add(String.Format("BIC: {0} - IBAN: {1}", bic[i], iban[i]));
            }
            return p;
        }

        /// <exception cref="ParseException"/>
        public virtual String ConvertDate(DateTime d, String newFormat) {
            return d.ToString(newFormat);
        }

        [LogMessage(LogMessageConstant.FONT_HAS_INVALID_GLYPH, Ignore = true)]
        [LogMessage(ZugferdLogMessageConstant.WRONG_OR_NO_CONFORMANCE_LEVEL, Ignore = true)]
        [LogMessage(ZugferdLogMessageConstant.NO_ZUGFERD_PROFILE_TYPE_SPECIFIED, Ignore = true)]
        [Test]
        public override void Test() {
            base.Test();
        }
    }
}
