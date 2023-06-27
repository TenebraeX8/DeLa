using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecompilableLanguage.Decompiler;

namespace DecompilableLanguage.Decompiler
{
    public class DecompilerSymbolTable
    {
       public string Name { get; set; } = String.Empty;
        public Dictionary<int, string> Symbols { get; private set; } = new Dictionary<int, string>();

        public string this[int idx] => Symbols[idx];

        private void Load(string file)
        {
            using(StreamReader sr = new StreamReader(file))
            {
                var elements = sr.ReadToEnd().Trim().Split(";");
                foreach(var element in elements)
                {
                    if (element.StartsWith("SCRIPT ")) this.Name = element.Substring("SCRIPT ".Length);
                    else
                    {
                        var splits = element.Split(":");
                        if (splits.Length != 2) throw new DeLaDecompiler.DecompilerException("Illegal Format of Symbol Table!");
                        Symbols[int.Parse(splits[1])] = splits[0];
                    }
                }
            }
        }

        public static DecompilerSymbolTable FromFile(string file)
        {
            var table = new DecompilerSymbolTable();
            table.Load(file);
            return table;
        }
    }
}
