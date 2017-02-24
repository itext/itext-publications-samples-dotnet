/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV

*/
/*
* This class creates an XML that has the structure needed to conform with
* the COMFORT level, but the content isn't valid. Instead we used test values
* that make it easy for us to detect errors.
*/
using System;
using iText.License;
using iText.Zugferd;
using iText.Zugferd.Profiles;
using iText.Zugferd.Validation.Basic;
using iText.Zugferd.Validation.Comfort;

namespace iText.Samples.Sandbox.Zugferd.Test {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class XML4Comfort {
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="Javax.Xml.Transform.TransformerException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        /// <exception cref="Java.Text.ParseException"/>
        public static void Main(String[] args) {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-multiple-products.xml");
            ComfortProfileImp data = new ComfortProfileImp(true);
            // SpecifiedExchangedDocumentContext
            data.SetTest(true);
            // HeaderExchangedDocument
            data.SetId("HeaderExchangedDocument.ID");
            data.SetName("HeaderExchangedDocument.Name");
            data.SetTypeCode(DocumentTypeCode.DEBIT_NOTE_FINANCIAL_ADJUSTMENT);
            data.SetDate(new DateTime(2016, 04, 01), DateFormatCode.YYYYMMDD);
            data.AddNote(new String[] { "HeaderExchangedDocument.Note[0][0]", "HeaderExchangedDocument.Note[0][1]" }, 
                FreeTextSubjectCode.REGULATORY_INFORMATION);
            data.AddNote(new String[] { "HeaderExchangedDocument.Note[1][0]", "HeaderExchangedDocument.Note[1][1]" }, 
                FreeTextSubjectCode.PRICE_CONDITIONS);
            data.AddNote(new String[] { "HeaderExchangedDocument.Note[2][0]", "HeaderExchangedDocument.Note[2][1]" }, 
                FreeTextSubjectCode.PAYMENT_INFORMATION);
            // SpecifiedSupplyChainTradeTransaction
            data.SetBuyerReference("BuyerReference");
            // Seller
            data.SetSellerID("SellerTradeParty.ID");
            data.AddSellerGlobalID(GlobalIdentifierCode.DUNS, "SellerTradeParty.GlobalID[0]");
            data.AddSellerGlobalID(GlobalIdentifierCode.GTIN, "SellerTradeParty.GlobalID[1]");
            data.AddSellerGlobalID(GlobalIdentifierCode.ODETTE, "SellerTradeParty.GlobalID[2]");
            data.SetSellerName("SellerTradeParty.Name");
            data.SetSellerPostcode("SellerTradeParty.PostalTradeAddress.Postcode");
            data.SetSellerLineOne("SellerTradeParty.PostalTradeAddress.LineOne");
            data.SetSellerLineTwo("SellerTradeParty.PostalTradeAddress.LineTwo");
            data.SetSellerCityName("SellerTradeParty.PostalTradeAddress.CityName");
            data.SetSellerCountryID("DE");
            data.AddSellerTaxRegistration(TaxIDTypeCode.VAT, "SellerTradeParty.SpecifiedTaxRegistration.SchemeID[0]");
            data.AddSellerTaxRegistration(TaxIDTypeCode.FISCAL_NUMBER, "SellerTradeParty.SpecifiedTaxRegistration.SchemeID[1]"
                );
            // Buyer
            data.SetBuyerID("BuyerTradeParty.ID");
            data.AddBuyerGlobalID(GlobalIdentifierCode.DUNS, "BuyerTradeParty.GlobalID[0]");
            data.AddBuyerGlobalID(GlobalIdentifierCode.GTIN, "BuyerTradeParty.GlobalID[1]");
            data.AddBuyerGlobalID(GlobalIdentifierCode.ODETTE, "BuyerTradeParty.GlobalID[2]");
            data.SetBuyerName("BuyerTradeParty.Name");
            data.SetBuyerPostcode("BuyerTradeParty.PostalTradeAddress.Postcode");
            data.SetBuyerLineOne("BuyerTradeParty.PostalTradeAddress.LineOne");
            data.SetBuyerLineTwo("BuyerTradeParty.PostalTradeAddress.LineTwo");
            data.SetBuyerCityName("BuyerTradeParty.PostalTradeAddress.CityName");
            data.SetBuyerCountryID("BE");
            data.AddBuyerTaxRegistration(TaxIDTypeCode.VAT, "BuyerTradeParty.SpecifiedTaxRegistration.SchemeID[0]");
            data.AddBuyerTaxRegistration(TaxIDTypeCode.FISCAL_NUMBER, "BuyerTradeParty.SpecifiedTaxRegistration.SchemeID[1]"
                );
            // ApplicableSupplyChainTradeAgreement
            data.SetBuyerOrderReferencedDocumentIssueDateTime(new DateTime(2016, 04, 02), DateFormatCode.YYYYMMDD);
            data.SetBuyerOrderReferencedDocumentID("ApplicableSupplyChainTradeAgreement.BuyerOrderReferencedDocumentID"
                );
            data.SetContractReferencedDocumentIssueDateTime(new DateTime(2016, 04, 03), DateFormatCode.YYYYMMDD);
            data.SetContractReferencedDocumentID("ApplicableSupplyChainTradeAgreement.ContractReferencedDocument");
            data.SetCustomerOrderReferencedDocumentIssueDateTime(new DateTime(2016, 04, 04), DateFormatCode.YYYYMMDD);
            data.SetCustomerOrderReferencedDocumentID("ApplicableSupplyChainTradeAgreement.CustomerOrderReferencedDocument"
                );
            // ApplicableSupplyChainTradeDelivery
            data.SetDeliveryDate(new DateTime(2016, 04, 05), DateFormatCode.YYYYMMDD);
            data.SetDeliveryNoteReferencedDocumentIssueDateTime(new DateTime(2016, 04, 06), DateFormatCode.YYYYMMDD);
            data.SetDeliveryNoteReferencedDocumentID("ApplicableSupplyChainTradeAgreement.DeliveryNoteReferencedDocument"
                );
            // ApplicableSupplyChainTradeSettlement
            data.SetPaymentReference("ApplicableSupplyChainTradeSettlement.PaymentReference");
            data.SetInvoiceCurrencyCode("USD");
            data.SetInvoiceeID("InvoiceeTradeParty.ID");
            data.AddInvoiceeGlobalID(GlobalIdentifierCode.DUNS, "InvoiceeTradeParty.GlobalID[0]");
            data.AddInvoiceeGlobalID(GlobalIdentifierCode.GTIN, "InvoiceeTradeParty.GlobalID[1]");
            data.AddInvoiceeGlobalID(GlobalIdentifierCode.ODETTE, "InvoiceeTradeParty.GlobalID[2]");
            data.SetInvoiceeName("InvoiceeTradeParty.Name");
            data.SetInvoiceePostcode("InvoiceeTradeParty.PostalTradeAddress.Postcode");
            data.SetInvoiceeLineOne("InvoiceeTradeParty.PostalTradeAddress.LineOne");
            data.SetInvoiceeLineTwo("InvoiceeTradeParty.PostalTradeAddress.LineTwo");
            data.SetInvoiceeCityName("InvoiceeTradeParty.PostalTradeAddress.CityName");
            data.SetInvoiceeCountryID("BE");
            data.AddInvoiceeTaxRegistration(TaxIDTypeCode.VAT, "InvoiceeTradeParty.SpecifiedTaxRegistration.SchemeID[0]"
                );
            data.AddInvoiceeTaxRegistration(TaxIDTypeCode.FISCAL_NUMBER, "InvoiceeTradeParty.SpecifiedTaxRegistration.SchemeID[1]"
                );
            String[] information0 = new String[] { "SpecifiedTradeSettlementPaymentMeans.Information[0][0]", "SpecifiedTradeSettlementPaymentMeans.Information[0][1]"
                , "SpecifiedTradeSettlementPaymentMeans.Information[0][2]" };
            data.AddPaymentMeans(PaymentMeansCode.PAYMENT_TO_BANK_ACCOUNT, information0, "SpecifiedTradeSettlementPaymentMeans.schemeAgencyID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.ID[0]", "SpecifiedTradeSettlementPaymentMeans.Payer.IBANID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.Payer.ProprietaryID[0]", "SpecifiedTradeSettlementPaymentMeans.Payee.IBANID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.Payee.AccountName[0]", "SpecifiedTradeSettlementPaymentMeans.Payee.ProprietaryID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.Payer.BICID[0]", "SpecifiedTradeSettlementPaymentMeans.Payer.GermanBankleitzahlID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.Payer.Name[0]", "SpecifiedTradeSettlementPaymentMeans.Payee.BICID[0]"
                , "SpecifiedTradeSettlementPaymentMeans.Payee.GermanBankleitzahlID[0]", "SpecifiedTradeSettlementPaymentMeans.Payee.Name[0]"
                );
            String[] information1 = new String[] { "SpecifiedTradeSettlementPaymentMeans.Information[1][0]", "SpecifiedTradeSettlementPaymentMeans.Information[1][1]"
                 };
            data.AddPaymentMeans(PaymentMeansCode.CASH, information1, "SpecifiedTradeSettlementPaymentMeans.schemeAgencyID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.ID[1]", "SpecifiedTradeSettlementPaymentMeans.Payer.IBANID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.Payer.ProprietaryID[1]", "SpecifiedTradeSettlementPaymentMeans.Payee.IBANID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.Payee.AccountName[1]", "SpecifiedTradeSettlementPaymentMeans.Payee.ProprietaryID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.Payer.BICID[1]", "SpecifiedTradeSettlementPaymentMeans.Payer.GermanBankleitzahlID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.Payer.Name[1]", "SpecifiedTradeSettlementPaymentMeans.Payee.BICID[1]"
                , "SpecifiedTradeSettlementPaymentMeans.Payee.GermanBankleitzahlID[1]", "SpecifiedTradeSettlementPaymentMeans.Payee.Name[1]"
                );
            // ram:ApplicableTradeTax
            data.AddApplicableTradeTax("6.00", "USD", "VAT", "ApplicableTradeTax.ExemptionReason[0]", "100.00", "USD", 
                "S", "6.00");
            data.AddApplicableTradeTax("21.00", "USD", "VAT", "ApplicableTradeTax.ExemptionReason[1]", "100.00", "USD"
                , "S", "21.00");
            data.SetBillingStartEnd(new DateTime(2016, 04, 01), DateFormatCode.YYYYMMDD, new DateTime(2016, 04, 30), DateFormatCode
                .YYYYMMDD);
            data.AddSpecifiedTradeAllowanceCharge(true, "0.1234", "USD", "TradeAllowanceCharge.Reason[0]", new String[
                ] { "VAT", "VAT" }, new String[] { "S", "S" }, new String[] { "6.00", "21.00" });
            data.AddSpecifiedTradeAllowanceCharge(false, "0.0120", "USD", "TradeAllowanceCharge.Reason[1]", new String
                [] { "VAT", "VAT" }, new String[] { "S", "S" }, new String[] { "0.00", "8.00" });
            data.AddSpecifiedLogisticsServiceCharge(new String[] { "SpecifiedLogisticsServiceCharge.Description[0][0]"
                , "SpecifiedLogisticsServiceCharge.Description[0][1]" }, "0.4321", "EUR", new String[] { "VAT", "VAT" }
                , new String[] { "S", "S" }, new String[] { "6.00", "21.00" });
            data.AddSpecifiedLogisticsServiceCharge(new String[] { "SpecifiedLogisticsServiceCharge.Description[1][0]"
                , "SpecifiedLogisticsServiceCharge.Description[1][1]" }, "0.1234", "EUR", new String[] { "VAT", "VAT" }
                , new String[] { "S", "S" }, new String[] { "0.00", "8.00" });
            data.AddSpecifiedTradePaymentTerms(new String[] { "SpecifiedTradePaymentTerms.Information[0][0]", "SpecifiedTradePaymentTerms.Information[0][1]"
                 }, new DateTime(2016, 05, 01), DateFormatCode.YYYYMMDD);
            data.AddSpecifiedTradePaymentTerms(new String[] { "SpecifiedTradePaymentTerms.Information[1][0]", "SpecifiedTradePaymentTerms.Information[1][1]"
                 }, new DateTime(2016, 05, 02), DateFormatCode.YYYYMMDD);
            // SpecifiedTradeSettlementMonetarySummation       
            data.SetMonetarySummation("1000.00", "USD", "0.00", "USD", "0.00", "USD", "1000.00", "USD", "210.00", "USD"
                , "1210.00", "USD");
            data.SetTotalPrepaidAmount("0.00", "USD");
            data.SetDuePayableAmount("1210.00", "USD");
            String[][] notes0 = new String[][] { new String[] { "IncludedSupplyChainTradeLineItem[0].AssociatedDocumentLineDocument.IncludedNote[0].Content[0]"
                , "IncludedSupplyChainTradeLineItem[0].AssociatedDocumentLineDocument.IncludedNote[0].Content[1]", "IncludedSupplyChainTradeLineItem[0].AssociatedDocumentLineDocument.IncludedNote[0].Content[2]"
                 }, new String[] { "IncludedSupplyChainTradeLineItem[0].AssociatedDocumentLineDocument.IncludedNote[1].Content[0]"
                , "IncludedSupplyChainTradeLineItem[0].AssociatedDocumentLineDocument.IncludedNote[1].Content[1]" }, new 
                String[] { "IncludedSupplyChainTradeLineItem[0].AssociatedDocumentLineDocument.IncludedNote[2].Content[0]"
                 } };
            bool[] indicator = new bool[] { true, false, true };
            String[] actualamount = new String[] { "1.0000", "2.0000", "3.0000" };
            String[] actualamountCurr = new String[] { "USD", "USD", "USD" };
            String[] reason = new String[] { "IncludedSupplyChainTradeLineItem[0].SpecifiedSupplyChainTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge[0].Reason"
                , "IncludedSupplyChainTradeLineItem[0].SpecifiedSupplyChainTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge[1].Reason"
                , "IncludedSupplyChainTradeLineItem[0].SpecifiedSupplyChainTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge[2].Reason"
                 };
            String[] taxTC = new String[] { "VAT", "VAT" };
            String[] taxER = new String[] { "IncludedSupplyChainTradeLineItem[0].SpecifiedSupplyChainTradeSettlement.SpecifiedTradeSettlementMonetarySummation.ApplicableTradeTax[0].ExemptionReason"
                , "IncludedSupplyChainTradeLineItem[0].SpecifiedSupplyChainTradeSettlement.SpecifiedTradeSettlementMonetarySummation.ApplicableTradeTax[1].ExemptionReason"
                 };
            String[] taxCC = new String[] { "S", "S" };
            String[] taxAP = new String[] { "12.00", "18.00" };
            data.AddIncludedSupplyChainTradeLineItem("LINE 1", notes0, "10.0000", "USD", "1.0000", MeasurementUnitCode
                .ITEM, indicator, actualamount, actualamountCurr, reason, "6.0000", "USD", "80.0001", MeasurementUnitCode
                .L, "1.0000", MeasurementUnitCode.HR, taxTC, taxER, taxCC, taxAP, "5.00", "USD", "IncludedSupplyChainTradeLineItem[0].SpecifiedTradeProduct.GlobalID"
                , GlobalIdentifierCode.GTIN, "IncludedSupplyChainTradeLineItem[0].SpecifiedTradeProduct.SellerAssignedID"
                , "IncludedSupplyChainTradeLineItem[0].SpecifiedTradeProduct.BuyerAssignedID", "IncludedSupplyChainTradeLineItem[0].SpecifiedTradeProduct.Name"
                , "IncludedSupplyChainTradeLineItem[0].SpecifiedTradeProduct.Description");
            String[][] notes1 = new String[][] { new String[] { "IncludedSupplyChainTradeLineItem.ram:AssociatedDocumentLineDocument[1].IncludedNote[0].Content[0]"
                , "IncludedSupplyChainTradeLineItem.ram:AssociatedDocumentLineDocument[1].IncludedNote[0].Content[1]" }
                , new String[] { "IncludedSupplyChainTradeLineItem.ram:AssociatedDocumentLineDocument[1].IncludedNote[1].Content[0]"
                 } };
            bool[] indicator1 = new bool[] { false, true, false };
            String[] actualamount1 = new String[] { "4.0000", "5.0000", "6.0000" };
            String[] actualamountCurr1 = new String[] { "USD", "USD", "USD" };
            String[] reason1 = new String[] { "IncludedSupplyChainTradeLineItem[1].SpecifiedSupplyChainTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge[0].Reason"
                , "IncludedSupplyChainTradeLineItem[1].SpecifiedSupplyChainTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge[1].Reason"
                , "IncludedSupplyChainTradeLineItem[1].SpecifiedSupplyChainTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge[2].Reason"
                 };
            String[] taxER1 = new String[] { "IncludedSupplyChainTradeLineItem[1].SpecifiedSupplyChainTradeSettlement.SpecifiedTradeSettlementMonetarySummation.ApplicableTradeTax[0].ExemptionReason"
                , "IncludedSupplyChainTradeLineItem[1].SpecifiedSupplyChainTradeSettlement.SpecifiedTradeSettlementMonetarySummation.ApplicableTradeTax[1].ExemptionReason"
                 };
            String[] taxAP1 = new String[] { "13.00", "17.00" };
            data.AddIncludedSupplyChainTradeLineItem("LINE 2", notes1, "20.0000", "USD", "10.0000", MeasurementUnitCode
                .KG, indicator1, actualamount1, actualamountCurr1, reason1, "30.0000", "USD", "15.0000", MeasurementUnitCode
                .M, "57.0000", MeasurementUnitCode.DAY, taxTC, taxER1, taxCC, taxAP1, "15.00", "USD", "IncludedSupplyChainTradeLineItem[1].SpecifiedTradeProduct.GlobalID"
                , GlobalIdentifierCode.ODETTE, "IncludedSupplyChainTradeLineItem[1].SpecifiedTradeProduct.SellerAssignedID"
                , "IncludedSupplyChainTradeLineItem[1].SpecifiedTradeProduct.BuyerAssignedID", "IncludedSupplyChainTradeLineItem[1].SpecifiedTradeProduct.Name"
                , "IncludedSupplyChainTradeLineItem[1].SpecifiedTradeProduct.Description");
            InvoiceDOM dom = new InvoiceDOM(data);
            byte[] xml = dom.ToXML();
            System.Console.Out.WriteLine(iText.IO.Util.JavaUtil.GetStringForBytes(xml));
        }
    }
}
