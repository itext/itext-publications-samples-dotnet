using System;
using System.Collections.Generic;
using iText.Bouncycastle.Asn1.Tsp;
using iText.Commons.Bouncycastle.Cert;
using iText.Forms;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Signatures;

namespace iText.Samples.Signatures.Chapter05
{
    public class C5_02_SignatureInfo
    {
        public static readonly string DEST = "signatures/chapter05/";

        public static readonly string EXAMPLE1 = "../../../resources/pdfs/step_4_signed_by_alice_bob_carol_and_dave.pdf";
        public static readonly string EXAMPLE2 = "../../../resources/pdfs/hello_signed4.pdf";
        public static readonly string EXAMPLE3 = "../../../resources/pdfs/field_metadata.pdf";

        public const String EXPECTED_OUTPUT = "../../../resources/pdfs/step_4_signed_by_alice_bob_carol_and_dave.pdf\n"
                                              + "===== sig1 =====\n"
                                              + "Field on page 1; llx: 36, lly: 728.02, urx: 559; ury: 779.02\n"
                                              + "Signature covers whole document: False\n"
                                              + "Document revision: 1 of 4\n"
                                              + "Integrity check OK? True\n"
                                              + "Digest algorithm: SHA256\n"
                                              + "Encryption algorithm: RSA\n"
                                              + "Filter subtype: /adbe.pkcs7.detached\n"
                                              + "Name of the signer: Alice Specimen\n"
                                              + "Signed on: 2016-02-23\n"
                                              + "Location: \n"
                                              + "Reason: \n"
                                              + "Contact info: \n"
                                              + "Signature type: certification\n"
                                              + "Filling out fields allowed: True\n"
                                              + "Adding annotations allowed: False\n"
                                              + "===== sig2 =====\n"
                                              + "Field on page 1; llx: 36, lly: 629.04, urx: 559; ury: 680.04\n"
                                              + "Signature covers whole document: False\n"
                                              + "Document revision: 2 of 4\n"
                                              + "Integrity check OK? True\n"
                                              + "Digest algorithm: SHA256\n"
                                              + "Encryption algorithm: RSA\n"
                                              + "Filter subtype: /adbe.pkcs7.detached\n"
                                              + "Name of the signer: Bob Specimen\n"
                                              + "Signed on: 2016-02-23\n"
                                              + "Location: \n"
                                              + "Reason: \n"
                                              + "Contact info: \n"
                                              + "Signature type: approval\n"
                                              + "Filling out fields allowed: True\n"
                                              + "Adding annotations allowed: False\n"
                                              + "Lock: /Include[sig1 approved_bob sig2 ]\n"
                                              + "===== sig3 =====\n"
                                              + "Field on page 1; llx: 36, lly: 530.05, urx: 559; ury: 581.05\n"
                                              + "Signature covers whole document: False\n"
                                              + "Document revision: 3 of 4\n"
                                              + "Integrity check OK? True\n"
                                              + "Digest algorithm: SHA256\n"
                                              + "Encryption algorithm: RSA\n"
                                              + "Filter subtype: /adbe.pkcs7.detached\n"
                                              + "Name of the signer: Carol Specimen\n"
                                              + "Signed on: 2016-02-23\n"
                                              + "Location: \n"
                                              + "Reason: \n"
                                              + "Contact info: \n"
                                              + "Signature type: approval\n"
                                              + "Filling out fields allowed: True\n"
                                              + "Adding annotations allowed: False\n"
                                              + "Lock: /Include[sig1 approved_bob sig2 ]\n"
                                              + "Lock: /Exclude[approved_dave sig4 ]\n"
                                              + "===== sig4 =====\n"
                                              + "Field on page 1; llx: 36, lly: 431.07, urx: 559; ury: 482.07\n"
                                              + "Signature covers whole document: True\n"
                                              + "Document revision: 4 of 4\n"
                                              + "Integrity check OK? True\n"
                                              + "Digest algorithm: SHA256\n"
                                              + "Encryption algorithm: RSA\n"
                                              + "Filter subtype: /adbe.pkcs7.detached\n"
                                              + "Name of the signer: Dave Specimen\n"
                                              + "Signed on: 2016-02-23\n"
                                              + "Location: \n"
                                              + "Reason: \n"
                                              + "Contact info: \n"
                                              + "Signature type: approval\n"
                                              + "Filling out fields allowed: False\n"
                                              + "Adding annotations allowed: False\n"
                                              + "Lock: /Include[sig1 approved_bob sig2 ]\n"
                                              + "Lock: /Exclude[approved_dave sig4 ]\n"
                                              + "../../../resources/pdfs/hello_signed4.pdf\n"
                                              + "===== sig =====\n"
                                              + "Field on page 1; llx: 36, lly: 648, urx: 236; ury: 748\n"
                                              + "Signature covers whole document: True\n"
                                              + "Document revision: 1 of 1\n"
                                              + "Integrity check OK? True\n"
                                              + "Digest algorithm: RIPEMD160\n"
                                              + "Encryption algorithm: RSA\n"
                                              + "Filter subtype: /ETSI.CAdES.detached\n"
                                              + "Name of the signer: Bruno Specimen\n"
                                              + "Signed on: 2016-02-23\n"
                                              + "Location: Ghent\n"
                                              + "Reason: Test 4\n"
                                              + "Contact info: \n"
                                              + "Signature type: approval\n"
                                              + "Filling out fields allowed: True\n"
                                              + "Adding annotations allowed: True\n"
                                              + "../../../resources/pdfs/field_metadata.pdf\n"
                                              + "===== Signature1 =====\n"
                                              + "Field on page 1; llx: 46.0674, lly: 472.172, urx: 332.563; ury: 726.831\n"
                                              + "Signature covers whole document: True\n"
                                              + "Document revision: 1 of 1\n"
                                              + "Integrity check OK? True\n"
                                              + "Digest algorithm: SHA256\n"
                                              + "Encryption algorithm: RSA\n"
                                              + "Filter subtype: /adbe.pkcs7.detached\n"
                                              + "Name of the signer: Bruno Specimen\n"
                                              + "Alternative name of the signer: Bruno L. Specimen\n"
                                              + "Signed on: 2016-02-23\n"
                                              + "Location: Ghent\n"
                                              + "Reason: Test metadata\n"
                                              + "Contact info: 555 123 456\n"
                                              + "Signature type: approval\n"
                                              + "Filling out fields allowed: True\n"
                                              + "Adding annotations allowed: True\n";

        public SignaturePermissions InspectSignature(PdfDocument pdfDoc, SignatureUtil signUtil, PdfAcroForm form,
            String name, SignaturePermissions perms)
        {
            IList<PdfWidgetAnnotation> widgets = form.GetField(name).GetWidgets();

            // Check the visibility of the signature annotation
            if (widgets != null && widgets.Count > 0)
            {
                Rectangle pos = widgets[0].GetRectangle().ToRectangle();
                int pageNum = pdfDoc.GetPageNumber(widgets[0].GetPage());

                if (pos.GetWidth() == 0 || pos.GetHeight() == 0)
                {
                    Console.Out.WriteLine("Invisible signature");
                }
                else
                {
                    Console.Out.WriteLine(String.Format("Field on page {0}; llx: {1}, lly: {2}, urx: {3}; ury: {4}",
                        pageNum, pos.GetLeft(), pos.GetBottom(), pos.GetRight(), pos.GetTop()));
                }
            }

            /* Find out how the message digest of the PDF bytes was created,
             * how these bytes and additional attributes were signed
             * and how the signed bytes are stored in the PDF
             */
            PdfPKCS7 pkcs7 = VerifySignature(signUtil, name);
            Console.Out.WriteLine("Digest algorithm: " + pkcs7.GetDigestAlgorithmName());
            Console.Out.WriteLine("Encryption algorithm: " + pkcs7.GetSignatureAlgorithmName());
            Console.Out.WriteLine("Filter subtype: " + pkcs7.GetFilterSubtype());

            // Get the signing certificate to find out the name of the signer.
            IX509Certificate cert = pkcs7.GetSigningCertificate();
            Console.Out.WriteLine("Name of the signer: "
                                  + iText.Signatures.CertificateInfo.GetSubjectFields(cert).GetField("CN"));
            if (pkcs7.GetSignName() != null)
            {
                Console.Out.WriteLine("Alternative name of the signer: " + pkcs7.GetSignName());
            }

            /* Get the signing time.
             * Mind that the getSignDate() method is not that secure as timestamp
             * because it's based only on signature author claim. I.e. this value can only be trusted
             * if signature is trusted and it cannot be used for signature verification.
             */
            Console.Out.WriteLine("Signed on: " + pkcs7.GetSignDate().ToUniversalTime().ToString("yyyy-MM-dd"));

            /* If a timestamp was applied, retrieve information about it.
             * Timestamp is a secure source of signature creation time,
             * because it's based on Time Stamping Authority service.
             */
            if (TimestampConstants.UNDEFINED_TIMESTAMP_DATE != pkcs7.GetTimeStampDate())
            {
                Console.Out.WriteLine("TimeStamp: " +
                                      pkcs7.GetTimeStampDate().ToUniversalTime().ToString("yyyy-MM-dd"));
                TstInfoBC ts = (TstInfoBC)pkcs7.GetTimeStampTokenInfo();
                Console.Out.WriteLine("TimeStamp service: " + ts.GetTstInfo().Tsa);
                Console.Out.WriteLine("Timestamp verified? " + pkcs7.VerifyTimestampImprint());
            }

            Console.Out.WriteLine("Location: " + pkcs7.GetLocation());
            Console.Out.WriteLine("Reason: " + pkcs7.GetReason());

            /* If you want less common entries than PdfPKCS7 object has, such as the contact info,
             * you should use the signature dictionary and get the properties by name.
             */
            PdfDictionary sigDict = signUtil.GetSignatureDictionary(name);
            PdfString contact = sigDict.GetAsString(PdfName.ContactInfo);
            if (contact != null)
            {
                Console.Out.WriteLine("Contact info: " + contact);
            }

            /* Every new signature can add more restrictions to a document, but it can’t take away previous restrictions.
             * So if you want to retrieve information about signatures restrictions, you need to pass
             * the SignaturePermissions instance of the previous signature, or null if there was none.
             */
            perms = new SignaturePermissions(sigDict, perms);
            Console.Out.WriteLine("Signature type: " + (perms.IsCertification() ? "certification" : "approval"));
            Console.Out.WriteLine("Filling out fields allowed: " + perms.IsFillInAllowed());
            Console.Out.WriteLine("Adding annotations allowed: " + perms.IsAnnotationsAllowed());
            foreach (SignaturePermissions.FieldLock Lock in perms.GetFieldLocks())
            {
                Console.Out.WriteLine("Lock: " + Lock);
            }

            return perms;
        }

        public PdfPKCS7 VerifySignature(SignatureUtil signUtil, String name)
        {
            PdfPKCS7 pkcs7 = signUtil.ReadSignatureData(name);

            Console.Out.WriteLine("Signature covers whole document: " + signUtil.SignatureCoversWholeDocument(name));
            Console.Out.WriteLine("Document revision: " + signUtil.GetRevision(name) + " of "
                                  + signUtil.GetTotalRevisions());
            Console.Out.WriteLine("Integrity check OK? " + pkcs7.VerifySignatureIntegrityAndAuthenticity());
            return pkcs7;
        }

        public virtual void InspectSignatures(String path)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, false);
            SignaturePermissions perms = null;
            SignatureUtil signUtil = new SignatureUtil(pdfDoc);
            IList<String> names = signUtil.GetSignatureNames();

            Console.WriteLine(path);
            foreach (String name in names)
            {
                Console.Out.WriteLine("===== " + name + " =====");
                perms = InspectSignature(pdfDoc, signUtil, form, name, perms);
            }
        }

        public static void Main(String[] args)
        {
            C5_02_SignatureInfo app = new C5_02_SignatureInfo();
            app.InspectSignatures(EXAMPLE1);
            app.InspectSignatures(EXAMPLE2);
            app.InspectSignatures(EXAMPLE3);
        }
    }
}