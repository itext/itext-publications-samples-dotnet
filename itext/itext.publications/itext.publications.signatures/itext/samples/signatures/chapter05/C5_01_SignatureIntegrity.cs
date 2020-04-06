/*

This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV

*/
/*
* This class is part of the white paper entitled
* "Digital Signatures for PDF documents"
* written by Bruno Lowagie
*
* For more info, go to: http://itextpdf.com/learn
*/

using System;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace iText.Samples.Signatures.Chapter05
{
    public class C5_01_SignatureIntegrity
    {
        public static readonly string DEST = "signatures/chapter05/";
        
        public static readonly string EXAMPLE1 = "../../../resources/pdfs/hello_level_1_annotated.pdf";

        public static readonly string EXAMPLE2 = "../../../resources/pdfs/step_4_signed_by_alice_bob_carol_and_dave.pdf";

        public static readonly string EXAMPLE3 = "../../../resources/pdfs/step_6_signed_by_dave_broken_by_chuck.pdf";

        public const String EXPECTED_OUTPUT = "../../../resources/pdfs/hello_level_1_annotated.pdf\n"
                                             + "===== sig =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 1 of 2\n"
                                             + "Integrity check OK? True\n"
                                             + "../../../resources/pdfs/step_4_signed_by_alice_bob_carol_and_dave.pdf\n"
                                             + "===== sig1 =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 1 of 4\n"
                                             + "Integrity check OK? True\n"
                                             + "===== sig2 =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 2 of 4\n"
                                             + "Integrity check OK? True\n"
                                             + "===== sig3 =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 3 of 4\n"
                                             + "Integrity check OK? True\n"
                                             + "===== sig4 =====\n"
                                             + "Signature covers whole document: True\n"
                                             + "Document revision: 4 of 4\n"
                                             + "Integrity check OK? True\n"
                                             + "../../../resources/pdfs/step_6_signed_by_dave_broken_by_chuck.pdf\n"
                                             + "===== sig1 =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 1 of 5\n"
                                             + "Integrity check OK? True\n"
                                             + "===== sig2 =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 2 of 5\n"
                                             + "Integrity check OK? True\n"
                                             + "===== sig3 =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 3 of 5\n"
                                             + "Integrity check OK? True\n"
                                             + "===== sig4 =====\n"
                                             + "Signature covers whole document: False\n"
                                             + "Document revision: 4 of 5\n"
                                             + "Integrity check OK? True\n";

        public void VerifySignatures(String path)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            SignatureUtil signUtil = new SignatureUtil(pdfDoc);
            IList<String> names = signUtil.GetSignatureNames();

            Console.WriteLine(path);
            foreach (String name in names)
            {
                Console.Out.WriteLine("===== " + name + " =====");
                VerifySignature(signUtil, name);
            }
            
            pdfDoc.Close();
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

        public static void Main(String[] args)
        {
            C5_01_SignatureIntegrity app = new C5_01_SignatureIntegrity();
            app.VerifySignatures(EXAMPLE1);
            app.VerifySignatures(EXAMPLE2);
            app.VerifySignatures(EXAMPLE3);
        }
    }
}