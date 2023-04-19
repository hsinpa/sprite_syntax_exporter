using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Hsinpa.SSE.SpriteSyntaxStatic;


namespace Hsinpa.SSE {
    public class SpriteSyntaxUtility
    {
        public static void PrepareDirectory(string store_directory)
        {
            string root = Application.streamingAssetsPath;
            string main_directory = Path.Combine(root, store_directory);

            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(root);
            }

            if (!Directory.Exists(main_directory))
            {
                Directory.CreateDirectory(main_directory);
            }
        }

        public static string ToBinary(System.Byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }
        
        public static Task SaveJSONFileToPath(string file_name, string raw_json, string json_path, string bjson_path) {
            string jsonPathFilter = string.Format(json_path, file_name);
            string bsonPathFilter = string.Format(bjson_path, file_name);

            return Task.WhenAll(
                //Pure Raw JSON Text
                File.WriteAllTextAsync(jsonPathFilter, raw_json),

                //Binary
                File.WriteAllTextAsync(bsonPathFilter, SpriteSyntaxUtility.ToBinary(Encoding.UTF8.GetBytes(raw_json)))
            );
        }

        public static Task<string> FormatJson(string json, string indent = "  ")
        {
            var indentation = 0;
            var quoteCount = 0;
            var escapeCount = 0;

            return Task.Run(() =>
            {
                var result =
                from ch in json ?? string.Empty
                let escaped = (ch == '\\' ? escapeCount++ : escapeCount > 0 ? escapeCount-- : escapeCount) > 0
                let quotes = ch == '"' && !escaped ? quoteCount++ : quoteCount
                let unquoted = quotes % 2 == 0
                let colon = ch == ':' && unquoted ? ": " : null
                let nospace = char.IsWhiteSpace(ch) && unquoted ? string.Empty : null
                let lineBreak = ch == ',' && unquoted ? ch + System.Environment.NewLine + string.Concat(Enumerable.Repeat(indent, indentation)) : null
                let openChar = (ch == '{' || ch == '[') && unquoted ? ch + System.Environment.NewLine + string.Concat(Enumerable.Repeat(indent, ++indentation)) : ch.ToString()
                let closeChar = (ch == '}' || ch == ']') && unquoted ? System.Environment.NewLine + string.Concat(Enumerable.Repeat(indent, --indentation)) + ch : ch.ToString()
                select colon ?? nospace ?? lineBreak ?? (
                    openChar.Length > 1 ? openChar : closeChar
                );

                return string.Concat(result);
            });
        }
    }
}

