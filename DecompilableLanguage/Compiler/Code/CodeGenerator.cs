using DecompilableLanguage.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Compiler.Code
{
    public class CodeGenerator
    {
        private const int INITIAL_SIZE = 2048;

        public byte[] code = new byte[INITIAL_SIZE];
        public int pc = 0;

        private void CheckBuffer()
        {
            if(pc >= code.Length)
            {
                byte[] newBuffer = new byte[code.Length * 2];
                Array.Copy(code, 0, newBuffer, 0, code.Length); 
            }
        }

        private void Put4(int value)
        {
            code[pc++] = (byte)(value & 0xFF);
            code[pc++] = (byte)((value >> 8) & 0xFF);
            code[pc++] = (byte)((value >> 16) & 0xFF);
            code[pc++] = (byte)((value >> 24) & 0xFF);
        }

        public void Instr(byte op)
        {
            CheckBuffer();
            code[pc++] = op;
        }

        public void ImmediateInstr(byte op, int value)
        {
            Instr(op);
            Put4(value);
        }

        public void Push(int value) => ImmediateInstr(Instruction.PUSH, value);
        public void Pop() => Instr(Instruction.POP);
        public void Load(int value) => ImmediateInstr(Instruction.LOAD, value);
        public void Store(int value) => ImmediateInstr(Instruction.STORE, value);
        public void Add() => Instr(Instruction.ADD);
        public void Sub() => Instr(Instruction.SUB);
        public void Mul() => Instr(Instruction.MUL);
        public void Div() => Instr(Instruction.DIV);
        public void Mod() => Instr(Instruction.MOD);
        public void Neg() => Instr(Instruction.NEG);
        public void Out() => Instr(Instruction.OUT);
        public byte[] Generate() => this.code.Take(this.pc).ToArray();
    }
}
