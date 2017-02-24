/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV

*/
/*
* Code written by Bruno Lowagie in the context of an example.
*/
using System;
using System.Collections.Generic;
using iText.Samples.Sandbox.Zugferd.Pojo;
using iText.Zugferd.Profiles;
using iText.Zugferd.Validation.Basic;
using iText.Zugferd.Validation.Comfort;

namespace iText.Samples.Sandbox.Zugferd.Data {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class InvoiceData {
        public InvoiceData() {
        }

        public virtual IBasicProfile CreateBasicProfileData(Invoice invoice, bool testInvoice) {
            BasicProfileImp profileImp = new BasicProfileImp(testInvoice);
            ImportData(profileImp, invoice);
            ImportBasicData(profileImp, invoice);
            return profileImp;
        }

        public virtual IComfortProfile CreateComfortProfileData(Invoice invoice, bool testInvoice) {
            ComfortProfileImp profileImp = new ComfortProfileImp(testInvoice);
            ImportData(profileImp, invoice);
            ImportComfortData(profileImp, invoice);
            return profileImp;
        }

        public virtual void ImportData(BasicProfileImp profileImp, Invoice invoice) {
            profileImp.SetId(String.Format("I/{0:00000}", invoice.GetId()));
            profileImp.SetName("INVOICE");
            profileImp.SetTypeCode(DocumentTypeCode.COMMERCIAL_INVOICE);
            profileImp.SetDate(invoice.GetInvoiceDate(), DateFormatCode.YYYYMMDD);
            profileImp.SetSellerName("Das Company");
            profileImp.SetSellerLineOne("ZUG Business Center");
            profileImp.SetSellerLineTwo("Highway 1");
            profileImp.SetSellerPostcode("9000");
            profileImp.SetSellerCityName("Ghent");
            profileImp.SetSellerCountryID("BE");
            profileImp.AddSellerTaxRegistration(TaxIDTypeCode.FISCAL_NUMBER, "201/113/40209");
            profileImp.AddSellerTaxRegistration(TaxIDTypeCode.VAT, "BE123456789");
            Customer customer = invoice.GetCustomer();
            profileImp.SetBuyerName(String.Format("{0}, {1}", customer.GetLastName(), customer.GetFirstName()));
            profileImp.SetBuyerPostcode(customer.GetPostalcode());
            profileImp.SetBuyerLineOne(customer.GetStreet());
            profileImp.SetBuyerCityName(customer.GetCity());
            profileImp.SetBuyerCountryID(customer.GetCountryId());
            profileImp.SetPaymentReference(String.Format("{0:000000000}", invoice.GetId()));
            profileImp.SetInvoiceCurrencyCode("EUR");
        }

        public virtual void ImportBasicData(BasicProfileImp profileImp, Invoice invoice) {
            profileImp.AddNote(new String[] { "This is a test invoice.\nNothing on this invoice is real.\nThis invoice is part of a tutorial."
                 });
            profileImp.AddPaymentMeans("", "", "BE 41 7360 0661 9710", "", "", "KREDBEBB", "", "KBC");
            profileImp.AddPaymentMeans("", "", "BE 56 0015 4298 7888", "", "", "GEBABEBB", "", "BNP Paribas");
            IDictionary<double, double> taxes = new SortedDictionary<double, double>();
            double tax;
            foreach (Item item in invoice.GetItems()) {
                tax = item.GetProduct().GetVat();
                if (taxes.ContainsKey(tax)) {
                    taxes[tax] = taxes[tax] + item.GetCost();
                }
                else {
                    taxes[tax] = item.GetCost();
                }
                profileImp.AddIncludedSupplyChainTradeLineItem(Format4dec(item.GetQuantity()), "C62", item.GetProduct().GetName
                    ());
            }
            double total;
            double tA;
            double ltN = 0;
            double ttA = 0;
            double gtA = 0;
            foreach (KeyValuePair<double, double> t in taxes) {
                tax = t.Key;
                total = Round(t.Value);
                gtA += total;
                tA = Round((100 * total) / (100 + tax));
                ttA += (total - tA);
                ltN += tA;
                profileImp.AddApplicableTradeTax(Format2dec(total - tA), "EUR", TaxTypeCode.VALUE_ADDED_TAX, Format2dec(tA
                    ), "EUR", Format2dec(tax));
            }
            profileImp.SetMonetarySummation(Format2dec(ltN), "EUR", Format2dec(0), "EUR", Format2dec(0), "EUR", Format2dec
                (ltN), "EUR", Format2dec(ttA), "EUR", Format2dec(gtA), "EUR");
        }

        public virtual void ImportComfortData(ComfortProfileImp profileImp, Invoice invoice) {
            profileImp.AddNote(new String[] { "This is a test invoice.\nNothing on this invoice is real.\nThis invoice is part of a tutorial."
                 }, FreeTextSubjectCode.REGULATORY_INFORMATION);
            profileImp.AddPaymentMeans(PaymentMeansCode.PAYMENT_TO_BANK_ACCOUNT, new String[] { "This is the preferred bank account."
                 }, "", "", "", "", "BE 41 7360 0661 9710", "", "", "", "", "", "KREDBEBB", "", "KBC");
            profileImp.AddPaymentMeans(PaymentMeansCode.PAYMENT_TO_BANK_ACCOUNT, new String[] { "Use this as an alternative account."
                 }, "", "", "", "", "BE 56 0015 4298 7888", "", "", "", "", "", "GEBABEBB", "", "BNP Paribas");
            IDictionary<double, double> taxes = new SortedDictionary<double, double>();
            double tax;
            int counter = 0;
            foreach (Item item in invoice.GetItems()) {
                counter++;
                tax = item.GetProduct().GetVat();
                if (taxes.ContainsKey(tax)) {
                    taxes[tax] = taxes[tax] + item.GetCost();
                }
                else {
                    taxes[tax] = item.GetCost();
                }
                profileImp.AddIncludedSupplyChainTradeLineItem(counter.ToString(), null, Format4dec(item.GetProduct().GetPrice
                    ()), "EUR", null, null, null, null, null, null, null, null, null, null, Format4dec(item.GetQuantity())
                    , "C62", new String[] { TaxTypeCode.VALUE_ADDED_TAX }, new String[1], new String[] { TaxCategoryCode.STANDARD_RATE
                     }, new String[] { Format2dec(item.GetProduct().GetVat()) }, Format2dec(item.GetCost()), "EUR", null, 
                    null, item.GetProduct().GetId().ToString(), null, item.GetProduct().GetName(), null);
            }
            double total;
            double tA;
            double ltN = 0;
            double ttA = 0;
            double gtA = 0;
            foreach (KeyValuePair<double, double> t in taxes) {
                tax = t.Key;
                total = Round(t.Value);
                gtA += total;
                tA = Round((100 * total) / (100 + tax));
                ttA += (total - tA);
                ltN += tA;
                profileImp.AddApplicableTradeTax(Format2dec(total - tA), "EUR", TaxTypeCode.VALUE_ADDED_TAX, null, Format2dec
                    (tA), "EUR", TaxCategoryCode.STANDARD_RATE, Format2dec(tax));
            }
            profileImp.SetMonetarySummation(Format2dec(ltN), "EUR", Format2dec(0), "EUR", Format2dec(0), "EUR", Format2dec
                (ltN), "EUR", Format2dec(ttA), "EUR", Format2dec(gtA), "EUR");
        }

        public static double Round(double d) {
            d = d * 100;
            long tmp = (long) Math.Round(d);
            return (double)tmp / 100;
        }

        public static String Format2dec(double d) {
            return String.Format("{0:0.00}", d);
        }

        public static String Format4dec(double d) {
            return String.Format("{0:0.0000}", d);
        }
    }
}
