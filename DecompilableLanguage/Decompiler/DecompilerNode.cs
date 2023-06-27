using DecompilableLanguage.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Decompiler
{
    public class DecompilerNode
    {
        public byte Instr { get; set; } = 0;
        public int Value = 0;
        public DecompilerNode Parent = null;
        public DecompilerNode Next = null;
        public DecompilerNode LeftChild = null;
        public DecompilerNode RightChild = null;

        public DecompilerNode(byte instr, int value = 0)
        {
            this.Instr = instr;
            this.Value = value;
        }
    }
}
