using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Compiler.Code
{
    public class CodeJump
    {
        private CodeGenerator Generator { get; set; }
        private int Source { get; set; }

        public CodeJump(CodeGenerator gen)
        {
            Generator = gen;
            gen.CondJmp();
            Source = gen.pc - 4;
        }

        public void SetTarget()
        {
            Generator.PutImmediate(this.Source, Generator.pc - this.Source - 4);
        }

    }
}
