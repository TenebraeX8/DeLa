using DecompilableLanguage.Compiler.Symbols;
using DecompilableLanguage.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Decompiler
{
    public class DeLaDecompiler
    {
        public byte[] code { get; set; }

        private DecompilerSymbolTable table { get; set; }

        public DeLaDecompiler(byte[] code, DecompilerSymbolTable table = null)
        {
            this.code = code;
            this.table = table;
        }

        private int ReadImmediate(ref int pc, int size = 4)
        {
            switch (size)
            {
                case 1: return code[pc++];
                case 4: return code[pc++] + (code[pc++] << 8) + (code[pc++] << 16) + (code[pc++] << 24);
                default: throw new DecompilerException($"Cannot read immediate value of size {size}");
            }
        }

        private string ResolveAdress(int adr) => table == null ? $"DS:{adr}" : table[adr];

        private string BinaryOpToString(byte op)
        {
            switch(op)
            {
                case Instruction.ADD: return "+";
                case Instruction.SUB: return "-";
                case Instruction.MUL: return "*";
                case Instruction.DIV: return "/";
                case Instruction.MOD: return "%";
                case Instruction.SHR: return ">>";
                case Instruction.SHL: return "<<";
                default: throw new DecompilerException($"Opcode {op} does not describe a binary operator!");
            }
        }

        private string Generate(DecompilerNode node)
        {
            if (node == null)
                throw new DecompilerException("Node was null!");
            switch(node.Instr)
            {
                case Instruction.PUSH: return node.Value.ToString();        //Immediate Value
                case Instruction.LOAD: return ResolveAdress(node.Value);    //Variable
                case Instruction.STORE: return $"{ResolveAdress(node.Value)} = {this.Generate(node.LeftChild)}";
                case Instruction.ADD:
                case Instruction.SUB:
                case Instruction.MUL:
                case Instruction.DIV:
                case Instruction.MOD:
                case Instruction.SHR:
                case Instruction.SHL:
                    return $"({Generate(node.LeftChild)} {BinaryOpToString(node.Instr)} {Generate(node.RightChild)})";
                case Instruction.NEG: return $"-({Generate(node.LeftChild)})";
                case Instruction.INC: return $"INC ({Generate(node.LeftChild)})";
                case Instruction.DEC: return $"DEC ({Generate(node.LeftChild)})";
                case Instruction.OUT: return $"OUT {Generate(node.LeftChild)}";
                default:
                    throw new DecompilerException($"Unknown opcode {node.Instr}");
            }
        }

        public string Decompile()
        {
            int pc = 0;
            DecompilerNode root = null;
            Stack<DecompilerNode> ExprStack = new Stack<DecompilerNode>();

            DecompilerNode node;
            while (pc < code.Length)
            {
                switch (code[pc++])
                {
                    case Instruction.PUSH:
                    case Instruction.LOAD:
                        ExprStack.Push(new DecompilerNode(code[pc - 1], ReadImmediate(ref pc)));
                        break;
                    case Instruction.POP:
                        ExprStack.Pop();
                        break;
                    case Instruction.STORE:
                        node = new DecompilerNode(code[pc - 1], ReadImmediate(ref pc));
                        node.LeftChild = ExprStack.Pop();
                        if (root == null) root = node;
                        else
                        {
                            DecompilerNode cur = root;
                            while (cur.Next != null) cur = cur.Next;
                            cur.Next = node;
                        }
                        break;
                    case Instruction.ADD:
                    case Instruction.SUB:
                    case Instruction.MUL:
                    case Instruction.DIV:
                    case Instruction.MOD:
                    case Instruction.SHR:
                    case Instruction.SHL:
                        node = new DecompilerNode(code[pc - 1]);
                        node.RightChild = ExprStack.Pop();
                        node.LeftChild = ExprStack.Pop();
                        ExprStack.Push(node);
                        break;
                    case Instruction.NEG:
                    case Instruction.INC:
                    case Instruction.DEC:
                        node = new DecompilerNode(code[pc - 1]);
                        node.LeftChild = ExprStack.Pop();
                        ExprStack.Push(node);
                        break;
                    case Instruction.OUT:
                        node = new DecompilerNode(code[pc - 1]);
                        node.LeftChild = ExprStack.Pop();
                        if (root == null) root = node;
                        else
                        {
                            DecompilerNode cur = root;
                            while (cur.Next != null) cur = cur.Next;
                            cur.Next = node;
                        }
                        break;
                }
            }
            string name = table?.Name ?? "<Unnamed>";
            StringBuilder sb = new StringBuilder($"SCRIPT {name}\n");
            if(table != null && table.Symbols.Count > 0)
            {
                sb.Append("VAR ");
                bool first = true;
                foreach (var sym in table.Symbols.Keys.OrderBy(x => x))
                {
                    if (first) first = false;
                    else sb.Append(", ");
                    sb.Append(table[sym]);
                }
                sb.AppendLine(";\n");                
            }

            while (root != null)
            {
                sb.Append(this.Generate(root) + ";\n");
                root = root.Next;
            }
            sb.Append($"\nEND {name}.");
            return sb.ToString();
        }

        public class DecompilerException : Exception
        {
            public DecompilerException(string msg) : base(msg)
            {

            }
        }
    }
}
