using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Compiler.Symbols
{
    public class SymbolTable
    {
        public static SymbolType NoType = new SymbolType(SymbolType.None, 0);
        public static SymbolType IntegerType = new SymbolType(SymbolType.Int, 4);

        private Scope scope = new Scope();
        private Parser Parser { get; }
        private int NextAdr = 0;

        public SymbolTable(Parser parser)
        {
            Parser = parser;
        }

        public void ScriptName(string name)
        {
            this.Insert(name, SymbolObj.Script, NoType);
        }

        public SymbolObj Insert(string name, int kind, SymbolType type)
        {
            SymbolObj elem = null;
            if (name != null && name != "")
            {
                elem = new SymbolObj(kind, name, type);
                elem.adr = NextAdr;
                NextAdr += elem.type.size;
                if (scope.find(name) != null)
                    Parser.Error($"\"{name}\" was declared twice!");
                else
                    scope.Insert(elem);
            }
            return elem;
        }

        public SymbolObj Find(string name)
        {
            SymbolObj obj = scope.find(name);
            if (obj != null) return obj;
            else
            {
                Parser.Error($"Cannot find referenced name {name}!");
                return null;
            }
        }

        public int DataSize()
        {
            int size = 0;
            var cur = this.scope.Elements;
            while(cur != null)
            {
                size += cur.type.size;
                cur = cur.next;
            }
            return size;
        }
    
        public void Store(string file)
        {
            using(StreamWriter sw = new StreamWriter(file))
            {
                var cur = scope.Elements;
                while (cur != null)
                {
                    if (cur.kind == SymbolObj.Script) sw.Write($"SCRIPT {cur.name}");
                    else if (cur.kind == SymbolObj.Var) sw.Write($";{cur.name}:{cur.adr}");

                    cur = cur.next;
                }
            }
        }
    
    }
}
