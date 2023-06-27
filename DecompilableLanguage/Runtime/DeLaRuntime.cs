using DecompilableLanguage.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Runtime
{
    public class DeLaRuntime
    {
        public byte[] code { get; set; }
        public byte[] data { get; set; }
        public bool Debug { get; set; } = false;

        public Stack<int> ExpressionStack = new Stack<int>();

        public DeLaRuntime(byte[] code, int datasize)
        {
            this.data = new byte[datasize];
            this.code = code;
        }


        private int ReadImmediate(ref int pc, int size = 4)
        {
            switch(size)
            {
                case 1: return code[pc++];
                case 4: return code[pc++] + (code[pc++] << 8) + (code[pc++] << 16) + (code[pc++] << 24);
                default: throw new RuntimeException($"Cannot read immediate value of size {size}");
            }
        }

        private int ReadData(int adr, int size = 4)
        {
            if (adr < 0)
                throw new RuntimeException($"Accessing address {adr} out of bounds of data area");
            if ((adr + size) > data.Length)
                throw new RuntimeException($"Accessing address {adr + size - 1} out of bounds of data area");

            switch (size)
            {
                case 1: return data[adr];
                case 4: return data[adr] + (data[adr+1] << 8) + (data[adr+2] << 16) + (data[adr+3] << 24);
                default: throw new RuntimeException($"Cannot load data of size {size}");
            }
        }

        private void StoreData(int adr, int value, int size = 4)
        {
            if (adr < 0)
                throw new RuntimeException($"Accessing address {adr} out of bounds of data area");
            if ((adr + size) > data.Length)
                throw new RuntimeException($"Accessing address {adr + size - 1} out of bounds of data area");

            switch (size)
            {
                case 1: data[adr] = (byte)value; break;
                case 4:
                    data[adr] = (byte)(value & 0xFF);
                    data[adr + 1] = (byte)((value >> 8) & 0xFF);
                    data[adr + 2] = (byte)((value >> 16) & 0xFF);
                    data[adr + 3] = (byte)((value >> 24) & 0xFF);
                    break;
                default: throw new RuntimeException($"Cannot store data of size {size}");
            }
        }

        public void Run()
        {
            int pc = 0;
            int value;
            Report(pc);
            while (pc < code.Length)
            {
                switch (code[pc++])
                {
                    case Instruction.PUSH: ExpressionStack.Push(ReadImmediate(ref pc)); break;
                    case Instruction.POP: ExpressionStack.Pop(); break;
                    case Instruction.LOAD: ExpressionStack.Push(ReadData(ReadImmediate(ref pc))); break;
                    case Instruction.STORE: StoreData(ReadImmediate(ref pc), ExpressionStack.Pop()); break;

                    case Instruction.ADD: ExpressionStack.Push(ExpressionStack.Pop() + ExpressionStack.Pop()); break;
                    case Instruction.SUB: ExpressionStack.Push(-ExpressionStack.Pop() + ExpressionStack.Pop()); break;
                    case Instruction.MUL: ExpressionStack.Push(ExpressionStack.Pop() * ExpressionStack.Pop()); break;
                    case Instruction.DIV:
                    case Instruction.MOD:
                        value = ExpressionStack.Pop();
                        if (value == 0)
                            throw new RuntimeException($"Dividing by zero at Code:{pc}!");
                        if (code[pc - 1] == Instruction.DIV)
                            ExpressionStack.Push(ExpressionStack.Pop() / value);
                        else
                            ExpressionStack.Push(ExpressionStack.Pop() % value);
                        break;
                    case Instruction.NEG: ExpressionStack.Push(-ExpressionStack.Pop()); break;
                    case Instruction.INC: ExpressionStack.Push(ExpressionStack.Pop()+1); break;
                    case Instruction.DEC: ExpressionStack.Push(ExpressionStack.Pop()-1); break;
                    case Instruction.SHR:
                        value = ExpressionStack.Pop();
                        ExpressionStack.Push(ExpressionStack.Pop() >> value); 
                        break;
                    case Instruction.SHL:
                        value = ExpressionStack.Pop();
                        ExpressionStack.Push(ExpressionStack.Pop() << value);
                        break;

                    case Instruction.OUT: Output(ExpressionStack.Pop()); break;

                    default: throw new RuntimeException($"Illegal opcode: {code[pc - 1]}");
                }
                Report(pc);
            }
        }

        private void Report(int pc)
        {
            if (Debug)
            {
                Console.WriteLine($"PC {pc}: ");
                Console.Write("    Expr. Stack: {");
                bool first = true;
                foreach (var val in this.ExpressionStack)
                {
                    if (first) first = false;
                    else Console.Write(", ");
                    Console.Write(val.ToString());
                }
                Console.WriteLine("}");
                Console.Write("    Data: {");
                first = true;
                foreach (var val in this.data)
                {
                    if (first) first = false;
                    else Console.Write(", ");
                    Console.Write(val.ToString("X"));
                }
                Console.WriteLine("}");

            }
        }

        private void Output(int value)
        {
            if (Debug) Console.WriteLine($"Output: {value}");
            else Console.Write(value);
        }

        public class RuntimeException: Exception
        {
            public RuntimeException(string msg) : base(msg)
            {

            }
        }

    }
}
