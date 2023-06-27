using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Compiler.Symbols
{
    public class SymbolType
    {
        public const int None = 0, Int = 1;
        public int kind { get; set; }
        public int size { get; set; }

        public SymbolType(int kind, int size)
        {
            this.kind = kind;
            this.size = size;
        }
    }
}
