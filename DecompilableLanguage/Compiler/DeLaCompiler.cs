using DecompilableLanguage.Compiler.Code;
using DecompilableLanguage.Compiler.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Compiler
{
    public class DeLaCompiler
    {
        private Parser parser { get; set; }
        private SymbolTable table { get; set; }
        private CodeGenerator code { get; set; }
        public DeLaCompiler(string file)
        {
            var scanner = new Scanner(file);
            this.parser = new Parser(scanner);
            this.table = new SymbolTable(this.parser);
            this.parser.tab = this.table;
            this.code = new CodeGenerator();
            this.parser.code = this.code;
        }

        public void Compile(string target)
        {
            this.parser.Parse();
            Console.WriteLine($"Parser finshed with {this.parser.errors.count} Errors!");
            if (this.parser.errors.count == 0)
            {
                File.WriteAllBytes(target, code.Generate());
                this.table.Store(target + ".sym");
            }
        }

        public byte[] GenerateCode() => this.parser.code.Generate();

        public int GetDataSize() => this.table.DataSize();
    }
}
