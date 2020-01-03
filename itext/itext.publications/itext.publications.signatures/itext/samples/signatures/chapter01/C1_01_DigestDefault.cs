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
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Math;

namespace iText.Samples.Signatures.Chapter01
{
    public class C1_01_DigestDefault
    {
        public static readonly String DEST = "results/signatures/chapter01/";

        public static readonly String EXPECTED_OUTPUT = "Digest using MD5: 16\n" +
                                                        "Digest: 5f4dcc3b5aa765d61d8327deb882cf99\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "SHA-1 digest is not available\n" +
                                                        "SHA-224 digest is not available\n" +
                                                        "Digest using SHA-256: 32\n" +
                                                        "Digest: 5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using SHA-384: 48\n" +
                                                        "Digest: a8b64babd0aca91a59bdbb7761b421d4f2bb38280d3a75ba0f21f" +
                                                        "2bebc45583d446c598660c94ce680c47d19c30783a7\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using SHA-512: 64\n" +
                                                        "Digest: b109f3bbbc244eb82441917ed06d618b9008dd09b3befd1b5e07394" +
                                                        "c706a8bb980b1d7785e5976ec049b46df5f1326af5a2ea6d103fd07c95385ffab0cacbc86\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "RIPEMD128 digest is not available\n" +
                                                        "Digest using RIPEMD160: 20\n" +
                                                        "Digest: 2c08e8f5884750a7b99f6f2f342fc638db25ff31\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "RIPEMD256 digest is not available\n";

        protected byte[] digest;
        protected HashAlgorithm messageDigest;

        protected C1_01_DigestDefault(String password, String algorithm)
        {
            messageDigest = HashAlgorithm.Create(algorithm);
            if (null == messageDigest)
            {
                throw new ArgumentException(algorithm + " digest is not available\n");
            }

            digest = messageDigest.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static C1_01_DigestDefault GetInstance(String password, String algorithm)
        {
            return new C1_01_DigestDefault(password, algorithm);
        }

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            TestAll();
        }

        public static void TestAll()
        {
            ShowTest("MD5");
            ShowTest("SHA-1");
            ShowTest("SHA-224");
            ShowTest("SHA-256");
            ShowTest("SHA-384");
            ShowTest("SHA-512");
            ShowTest("RIPEMD128");
            ShowTest("RIPEMD160");
            ShowTest("RIPEMD256");
        }

        public static void ShowTest(String algorithm)
        {
            try
            {
                C1_01_DigestDefault app = GetInstance("password", algorithm);
                Console.WriteLine("Digest using " + algorithm + ": " + app.GetDigestSize());
                Console.WriteLine("Digest: " + app.GetDigestAsHexString());
                Console.WriteLine("Is the password 'password'? " + app.CheckPassword("password"));
                Console.WriteLine("Is the password 'secret'? " + app.CheckPassword("secret"));
            }
            catch (ArgumentException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public int GetDigestSize()
        {
            return digest.Length;
        }

        public String GetDigestAsHexString()
        {
            return new BigInteger(1, digest).ToString(16);
        }

        // This method checks if the digest of the password is equal
        // to the digest of the text line which is passed as argument
        public bool CheckPassword(String password)
        {
            return digest.SequenceEqual(messageDigest.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}