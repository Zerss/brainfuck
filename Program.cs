using System;
using System.Collections.Generic;
using System.Text;

namespace brainfuck
{
    class Program
    {
        ASCIIEncoding Encode;
        Memory Memory;
        String Code = "";
        Stack<int> Loops; // using a LIFO in order to keep every loops beginning index value

        static void Main(string[] args)
            => new Program();

        public Program()
        {
            Encode = new ASCIIEncoding();
            Memory = new Memory(10);
            Loops = new Stack<int>();

            for (int i = 0; i < Code.Length; i++)
            {
                char c = Code[i];

                switch (c)
                {
                    case '+':
                        Memory.Add();
                        break;
                    case '-':
                        Memory.Remove();
                        break;
                    case '>':
                        Memory.MoveForward();
                        break;
                    case '<':
                        Memory.MoveBackwards();
                        break;
                    case '.':
                        Console.Write(toChar((byte)Memory.Read()));
                        break;
                    case ',':
                        Memory.Write();
                        Console.Write("\b");
                        break;
                    case '[':
                        Loops.Push(i);
                        break;
                    case ']':
                        int ibeg = Loops.Pop();
                        if (Memory.Read() != 0)
                            i = ibeg - 1;
                        break;
                    default:
                        break;
                }
            }

            Console.ReadKey();
        }

        private char toChar(Byte b)
            => Encode.GetString(new byte[] { b })[0];
        
        private Byte toByte(char c)
            => Encode.GetBytes(new char[] { c })[0];
    }

    class Memory
    {
        public int[] Content;
        public int Size;
        int Cursor;

        public Memory(int DefaultSize)
        {
            Size = DefaultSize;
            Content = new int[Size];
            Cursor = 0;
        }

        public void Add()
        {
            Content[Cursor] += 1;
            Content[Cursor] %= 256;
        }

        public void Remove()
        {
            Content[Cursor] -= 1;
            while (Content[Cursor] < 0)
                Content[Cursor] += 256;
        }

        public int Read()
            => Content[Cursor];

        public void MoveForward()
        {
            Cursor++;
            if (Size == Cursor)
            {
                int[] NewMemory = new int[Size * 2];
                for (int i = 0; i < Size; i++)
                    NewMemory[i] = Content[i];

                Content = NewMemory;
                Size *= 2;
            }
        }

        public void Write()
            => Content[Cursor] = (int)Console.ReadKey().KeyChar;

        public void MoveBackwards()
        {
            Cursor--;
            if (Cursor < 0)
                throw new System.Exception("Cursor cannot point at a negative index");
        }
    }
}