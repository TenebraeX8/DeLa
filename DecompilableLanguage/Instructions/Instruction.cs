using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecompilableLanguage.Instructions
{
    public static class Instruction
    {
        public const byte PUSH = 0x00;
        public const byte POP = 0x01;
        public const byte LOAD = 0x02;
        public const byte STORE = 0x03;

        public const byte ADD = 0x0A;
        public const byte SUB = 0x0B;
        public const byte MUL = 0x0C;
        public const byte DIV = 0x0D;
        public const byte MOD = 0x0E;
        public const byte NEG = 0x0F;

        public const byte INC = 0x10;
        public const byte DEC = 0x11;
        public const byte SHR = 0x12;
        public const byte SHL = 0x13;
        public const byte NOT = 0x14;
        public const byte AND = 0x15;
        public const byte OR = 0x16;
        public const byte XOR = 0x17;

        public const byte OUT = 0x20;

    }
}
