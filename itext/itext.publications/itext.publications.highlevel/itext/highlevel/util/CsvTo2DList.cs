/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using iText.IO.Util;

namespace iText.Highlevel.Util {
    /// <author>iText</author>
    public class CsvTo2DList {
        /// <exception cref="System.IO.IOException"/>
        public static IList<IList<String>> Convert(String src, String separator) {
            IList<IList<String>> resultSet = new List<IList<String>>();
            StreamReader sr = File.OpenText(src);
            String line;
            IList<string> record;
            while ((line = sr.ReadLine()) != null) {
                StringTokenizer tokenizer = new StringTokenizer(line, separator);
                record = new List<string>();
                while (tokenizer.HasMoreTokens()) {
                    record.Add(tokenizer.NextToken());
                }
                resultSet.Add(record);
            }
            return resultSet;
        }
    }
}
