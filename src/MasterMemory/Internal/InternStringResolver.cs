﻿using MessagePack;
using MessagePack.Formatters;
using System;

namespace MasterMemory.Internal
{
    internal class InternStringResolver : IFormatterResolver, IMessagePackFormatter<string>
    {
        readonly IFormatterResolver innerResolver;

        public InternStringResolver(IFormatterResolver innerResolver)
        {
            this.innerResolver = innerResolver;
        }

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            if (typeof(T) == typeof(string))
            {
                return (IMessagePackFormatter<T>)this;
            }

            return innerResolver.GetFormatter<T>();
        }

        string IMessagePackFormatter<string>.Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var str = MessagePackBinary.ReadString(bytes, offset, out readSize);
            if (str == null)
            {
                return null;
            }

            return string.Intern(str);
        }

        int IMessagePackFormatter<string>.Serialize(ref byte[] bytes, int offset, string value, IFormatterResolver formatterResolver)
        {
            throw new NotSupportedException();
        }
    }
}