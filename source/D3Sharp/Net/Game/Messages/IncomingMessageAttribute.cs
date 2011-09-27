using System;
using System.Collections.Generic;

namespace D3Sharp.Net.Game.Messages
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class IncomingMessageAttribute : Attribute
    {
        public List<Opcodes> Opcodes { get; private set; }

        public IncomingMessageAttribute(Opcodes opcode)
        {
            this.Opcodes = new List<Opcodes> {opcode};
        }

        public IncomingMessageAttribute(Opcodes[] opcodes)
        {
            this.Opcodes = new List<Opcodes>();
            foreach(var opcode in opcodes)
            {
                this.Opcodes.Add(opcode);
            }
        }
    }
}
