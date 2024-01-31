using DecompilableLanguage.Compiler;
using DecompilableLanguage.Decompiler;
using DecompilableLanguage.Instructions;
using DecompilableLanguage.Runtime;

namespace DecompilableLanguage 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var compiler = new DeLaCompiler(args[0]);
            compiler.Compile(args[1]);

            Console.WriteLine($"------------Deassembled------------");
            Console.WriteLine(DeLaDeassembler.Deassemble(compiler.GenerateCode()));


            Console.WriteLine($"------------Running------------");
            var runtime = new DeLaRuntime(compiler.GenerateCode(), compiler.GetDataSize()) { Debug=true};
            runtime.Run();

            var symtable = DecompilerSymbolTable.FromFile(args[1] + ".sym");

            var decompiler = new DeLaDecompiler(compiler.GenerateCode(), symtable);
            string decompilerResult = decompiler.Decompile();
            Console.WriteLine("\n------------Decompiled------------");
            Console.WriteLine(decompilerResult);
            using (StreamWriter sw = new StreamWriter("Decompiled_" + args[1]))
                sw.Write(decompilerResult);
            Console.ReadKey();
        }
    }
}