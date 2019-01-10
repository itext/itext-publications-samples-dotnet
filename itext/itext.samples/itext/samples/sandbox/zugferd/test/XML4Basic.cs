/*

This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV

*/
/*
* This class creates an XML that has the structure needed to conform with
* the BASIC level, but the content isn't valid. Instead we used test values
* that make it easy for us to detect errors.
*/
using System;
using iText.License;
using iText.Zugferd;
using iText.Zugferd.Profiles;
using iText.Zugferd.Validation.Basic;

namespace iText.Samples.Sandbox.Zugferd.Test {
    /// <author>iText</author>
    public class XML4Basic {
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="Javax.Xml.Transform.TransformerException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        /// <exception cref="Java.Text.ParseException"/>
        public static void Main(String[] args) {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-multiple-products.xml");
            BasicProfileImp data = new BasicProfileImp(true);
            // SpecifiedExchangedDocumentContext
            data.SetTest(true);
            // HeaderExchangedDocument
            data.SetId("HeaderExchangedDocument.ID");
            data.SetName("HeaderExchangedDocument.Name");
            data.SetTypeCode(DocumentTypeCode.COMMERCIAL_INVOICE);
            data.SetDate(new DateTime(2015, 04, 01), DateFormatCode.YYYYMMDD);
            data.AddNote(new String[] { "HeaderExchangedDocument.Note[0][0]", "HeaderExchangedDocument.Note[0][1]" });
            data.AddNote(new String[] { "HeaderExchangedDocument.Note[1][0]", "HeaderExchangedDocument.Note[1][1]" });
            data.AddNote(new String[] { "HeaderExchangedDocument.Note[2][0]", "HeaderExchangedDocument.Note[2][1]" });
            // SpecifiedSupplyChainTradeTransaction>
            // Seller
            data.SetSellerName("SellerTradeParty.Name");
            data.SetSellerPostcode("SellerTradeParty.PostalTradeAddress.Postcode");
            data.SetSellerLineOne("SellerTradeParty.PostalTradeAddress.LineOne");
            data.SetSellerLineTwo("SellerTradeParty.PostalTradeAddress.LineTwo");
            data.SetSellerCityName("SellerTradeParty.PostalTradeAddress.CityName");
            data.SetSellerCountryID("BE");
            data.AddSellerTaxRegistration(TaxIDTypeCode.VAT, "SellerTradeParty.SpecifiedTaxRegistration.SchemeID[0]");
            data.AddSellerTaxRegistration(TaxIDTypeCode.FISCAL_NUMBER, "SellerTradeParty.SpecifiedTaxRegistration.SchemeID[1]"
                );
            // Buyer
            data.SetBuyerName("BuyerTradeParty.Name");
            data.SetBuyerPostcode("BuyerTradeParty.PostalTradeAddress.Postcode");
            data.SetBuyerLineOne("BuyerTradeParty.PostalTradeAddress.LineOne");
            data.SetBuyerLineTwo("BuyerTradeParty.PostalTradeAddress.LineTwo");
            data.SetBuyerCityName("BuyerTradeParty.PostalTradeAddress.CityName");
            data.SetBuyerCountryID("FR");
            data.AddBuyerTaxRegistration(TaxIDTypeCode.VAT, "BuyerTradeParty.SpecifiedTaxRegistration.SchemeID[0]");
            data.AddBuyerTaxRegistration(TaxIDTypeCode.FISCAL_NUMBER, "BuyerTradeParty.SpecifiedTaxRegistration.SchemeID[1]"
                );
            // ApplicableSupplyChainTradeDelivery
            data.SetDeliveryDate(new DateTime(2015, 04, 01), DateFormatCode.YYYYMMDD);
            // ApplicableSupplyChainTradeSettlement
            data.SetPaymentReference("ApplicableSupplyChainTradeSettlement.PaymentReference");
            data.SetInvoiceCurrencyCode("EUR");
            data.AddPaymentMeans("SpecifiedTradeSettlementPaymentMeans.schemeAgencyID[0]", "SpecifiedTradeSettlementPaymentMeans.ID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.IBANID[0]", "SpecifiedTradeSettlementPaymentMeans.AccountName[0]"
                , "SpecifiedTradeSettlementPaymentMeans.ProprietaryID[0]", "SpecifiedTradeSettlementPaymentMeans.BICID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.GermanBankleitzahlID[0]", "SpecifiedTradeSettlementPaymentMeans.Name[0]"
                );
            data.AddPaymentMeans("SpecifiedTradeSettlementPaymentMeans.schemeAgencyID[1]", "SpecifiedTradeSettlementPaymentMeans.ID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.IBANID[1]", "SpecifiedTradeSettlementPaymentMeans.AccountName[1]"
                , "SpecifiedTradeSettlementPaymentMeans.ProprietaryID[1]", "SpecifiedTradeSettlementPaymentMeans.BICID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.GermanBankleitzahlID[1]", "SpecifiedTradeSettlementPaymentMeans.Name[1]"
                );
            // ram:ApplicableTradeTax
            data.AddApplicableTradeTax("6.00", "EUR", "VAT", "100.00", "EUR", "6.00");
            data.AddApplicableTradeTax("21.00", "EUR", "VAT", "100.00", "EUR", "21.00");
            // SpecifiedTradeSettlementMonetarySummation
            data.SetMonetarySummation("1000.00", "EUR", "0.00", "EUR", "0.00", "EUR", "1000.00", "EUR", "210.00", "EUR"
                , "1210.00", "EUR");
            data.AddIncludedSupplyChainTradeLineItem("1.0000", MeasurementUnitCode.DAY, "IncludedSupplyChainTradeLineItem.SpecifiedTradeProduct.Name[0]"
                );
            data.AddIncludedSupplyChainTradeLineItem("2.0000", MeasurementUnitCode.HR, "IncludedSupplyChainTradeLineItem.SpecifiedTradeProduct.Name[1]"
                );
            data.AddIncludedSupplyChainTradeLineItem("3.0000", MeasurementUnitCode.MIN, "IncludedSupplyChainTradeLineItem.SpecifiedTradeProduct.Name[2]"
                );
            // Create the XML
            InvoiceDOM dom = new InvoiceDOM(data);
            byte[] xml = dom.ToXML();
            System.Console.Out.WriteLine(iText.IO.Util.JavaUtil.GetStringForBytes(xml));
        }
    }
}
