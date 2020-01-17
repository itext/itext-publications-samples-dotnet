/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */
/*
* This class is part of the white paper entitled
* "Digital Signatures for PDF documents"
* written by Bruno Lowagie
*
* For more info, go to: http://itextpdf.com/learn
*/

using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Chapter01
{
    public class C1_03_EncryptDecrypt
    {
        public static readonly String DEST = "results/signatures/chapter01/";

        protected static readonly String KEYSTORE = "../../resources/encryption/ks";

        protected static readonly String PASSWORD = "password";
        protected Pkcs12Store ks;

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            EncryptDecrypt();
        }

        public static void EncryptDecrypt()
        {
            C1_03_EncryptDecrypt app = new C1_03_EncryptDecrypt();
            app.InitKeyStore(KEYSTORE, PASSWORD);
            AsymmetricKeyParameter publicKey = app.getPublicKey("demo");
            AsymmetricKeyParameter privateKey = app.getPrivateKey("demo");

            // Encrypt the message with the public key and then decrypt it with the private key
            Console.WriteLine("Let's encrypt 'secret message' with a public key");
            byte[] encrypted = app.Encrypt(publicKey, "secret message");
            Console.WriteLine("Encrypted message: " + app.GetDigestAsHexString(encrypted));
            Console.WriteLine("Let's decrypt it with the corresponding private key");
            String decrypted = app.Decrypt(privateKey, encrypted);
            Console.WriteLine(decrypted);

            // Encrypt the message with the private key and then decrypt it with the public key
            Console.WriteLine("You can also encrypt the message with a private key");
            encrypted = app.Encrypt(privateKey, "secret message");
            Console.WriteLine("Encrypted message: " + app.GetDigestAsHexString(encrypted));
            Console.WriteLine("Now you need the public key to decrypt it");
            decrypted = app.Decrypt(publicKey, encrypted);
            Console.WriteLine(decrypted);
        }

        private void InitKeyStore(String keystore, String ks_pass)
        {
            ks = new Pkcs12Store(new FileStream(keystore, FileMode.Open, FileAccess.Read),
                ks_pass.ToCharArray());
        }

        private String GetDigestAsHexString(byte[] digest)
        {
            return new BigInteger(1, digest).ToString(16);
        }

        private X509Certificate GetCertificate(String alias)
        {
            return ks.GetCertificate(alias).Certificate;
        }

        private AsymmetricKeyParameter getPublicKey(String alias)
        {
            return GetCertificate(alias).GetPublicKey();
        }

        private AsymmetricKeyParameter getPrivateKey(String alias)
        {
            return ks.GetKey(alias).Key;
        }

        // This method encrypts the message (using RSA algorithm) with the key, got as the 1st argument
        public byte[] Encrypt(AsymmetricKeyParameter key, String message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            Pkcs1Encoding encryptEngine = new Pkcs1Encoding(new RsaEngine());

            // The first parameter defines whether to encrypt or decrypt
            encryptEngine.Init(true, key);

            return encryptEngine.ProcessBlock(messageBytes, 0, messageBytes.Length);
        }

        // This method decrypts the message (using RSA algorithm) with the key, got as the 1st argument
        public String Decrypt(AsymmetricKeyParameter key, byte[] message)
        {
            Pkcs1Encoding decryptEngine = new Pkcs1Encoding(new RsaEngine());

            // The first parameter defines whether to encrypt or decrypt
            decryptEngine.Init(false, key);

            return Encoding.UTF8.GetString(decryptEngine.ProcessBlock(message, 0, message.Length));
        }
    }
}