using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Compiler.Symbols
{
    public class Scope
    {
        public SymbolObj Elements { get; set; } = null;
        public int Count { get; private set; } = 0;

        public SymbolObj find(string name)
        {
            SymbolObj cur = Elements;
            while (cur != null)
            {
                if (cur.name == name) return cur;
                cur = cur.next;
            }
            return null;
        }

        public void Insert(SymbolObj o)
        {
            if (Elements == null) Elements = o;
            else
            {
                SymbolObj cur = Elements;
                while (cur.next != null)
                    cur = cur.next;
                cur.next = o;
            }
            this.Count++;
        }
    }
}
