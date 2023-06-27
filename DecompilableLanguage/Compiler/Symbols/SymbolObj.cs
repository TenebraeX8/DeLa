using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Compiler.Symbols
{
    public class SymbolObj
    {
        public const int Con = 0, Type = 1, Var = 2, Script = 10;
        public string name { get; set; }
        public int kind { get; set; }
        public SymbolType type { get; set; }
        public SymbolObj next { get; set; }
        public int val { get; set; }
        public int adr { get; set; }


        public SymbolObj(int kind, string name, SymbolType type)
        {
            this.kind = kind;
            this.name = name;
            this.type = type;
        }
    }
}
