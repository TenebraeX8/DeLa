using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DecompilableLanguage.Runtime.DeLaRuntime;

namespace DecompilableLanguage.Instructions
{
    public static class DeLaDeassembler
    {

        private static int ReadImmediate(byte[] code, ref int pc, int size = 4)
        {
            switch (size)
            {
                case 1: return code[pc++];
                case 4: return code[pc++] + (code[pc++] << 8) + (code[pc++] << 16) + (code[pc++] << 24);
                default: throw new RuntimeException($"Cannot read immediate value of size {size}");
            }
        }


        public static string Deassemble(byte[] code)
        {
            StringBuilder sb = new StringBuilder();
            int pc = 0;
            while (pc < code.Length)
            {
                switch (code[pc++])
                {
                    case Instruction.PUSH:  sb.Append($"PUSH {ReadImmediate(code, ref pc)}\n"); break;
                    case Instruction.POP:   sb.Append("POP\n"); break;
                    case Instruction.LOAD:  sb.Append($"LOAD {ReadImmediate(code, ref pc)}\n"); break;
                    case Instruction.STORE: sb.Append($"STORE {ReadImmediate(code, ref pc)}\n"); break;

                    case Instruction.ADD: sb.Append("ADD\n"); break;
                    case Instruction.SUB: sb.Append("SUB\n"); break;
                    case Instruction.MUL: sb.Append("MUL\n"); break;
                    case Instruction.DIV: sb.Append("DIV\n"); break;
                    case Instruction.MOD: sb.Append("MOD\n"); break;    
                    case Instruction.NEG: sb.Append("NEG\n"); break;
                    case Instruction.INC: sb.Append("INC\n"); break;
                    case Instruction.DEC: sb.Append("DEC\n"); break;
                    case Instruction.SHR: sb.Append("SHR\n"); break;
                    case Instruction.SHL: sb.Append("SHL\n"); break;
                    case Instruction.NOT: sb.Append("NOT\n"); break;
                    case Instruction.AND: sb.Append("AND\n"); break;
                    case Instruction.OR:  sb.Append("OR\n"); break;
                    case Instruction.XOR: sb.Append("XOR\n"); break;
                    case Instruction.OUT: sb.Append("OUT\n"); break;
                    case Instruction.COND_JMP: sb.Append($"COND_JMP {ReadImmediate(code, ref pc)}\n"); break;

                    default: throw new RuntimeException($"Illegal opcode: {code[pc - 1]}\n");
                }
            }
            return sb.ToString();

        }
    }
}
