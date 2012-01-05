/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections.Generic;
using Mooege.Core.MooNet.Helpers;
using Google.ProtocolBuffers;
using System.Linq;
using System.Text;

namespace Mooege.Core.MooNet.Objects
{
    public class EntityIdPresenceFieldList
    {
        public List<bnet.protocol.EntityId> Value = new List<bnet.protocol.EntityId>();

        protected FieldKeyHelper.Program _program;
        protected FieldKeyHelper.OriginatingClass _originatingClass;
        protected uint _fieldNumber;
        protected uint _index;

        public EntityIdPresenceFieldList(FieldKeyHelper.Program Program, FieldKeyHelper.OriginatingClass OriginatingClass, uint FieldNumber, uint Index)
        {
            _fieldNumber = FieldNumber;
            _index = Index;
            _program = Program;
            _originatingClass = OriginatingClass;
        }

        public List<bnet.protocol.presence.FieldOperation> GetFieldOperationList()
        {
            var operationList = new List<bnet.protocol.presence.FieldOperation>();

            foreach (var id in Value)
            {
                var Key = FieldKeyHelper.Create(FieldKeyHelper.Program.BNet, FieldKeyHelper.OriginatingClass.Account, 4, id.High);
                var Field = bnet.protocol.presence.Field.CreateBuilder().SetKey(Key).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetEntityidValue(id).Build()).Build();
                operationList.Add(bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(Field).Build());
            }
            return operationList;
        }
    }

    public class BoolPresenceField : PresenceField<bool>, IPresenceField
    {
        public BoolPresenceField(FieldKeyHelper.Program Program, FieldKeyHelper.OriginatingClass OriginatingClass, uint FieldNumber, uint Index)
            : base(Program, OriginatingClass, FieldNumber, Index)
        {
        }

        public bnet.protocol.presence.Field GetField()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(Value).Build()).Build();
            return field;
        }

        public bnet.protocol.presence.FieldOperation GetFieldOperation()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(Value).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();
        }
    }

    public class UintPresenceField : PresenceField<uint>, IPresenceField
    {
        public UintPresenceField(FieldKeyHelper.Program Program, FieldKeyHelper.OriginatingClass OriginatingClass, uint FieldNumber, uint Index)
            : base(Program, OriginatingClass, FieldNumber, Index)
        {
        }

        public bnet.protocol.presence.Field GetField()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetUintValue(Value).Build()).Build();
            return field;
        }

        public bnet.protocol.presence.FieldOperation GetFieldOperation()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetUintValue(Value).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();
        }
    }

    public class IntPresenceField : PresenceField<int>, IPresenceField
    {
        public IntPresenceField(FieldKeyHelper.Program Program, FieldKeyHelper.OriginatingClass OriginatingClass, uint FieldNumber, uint Index)
            : base(Program, OriginatingClass, FieldNumber, Index)
        {
        }

        public bnet.protocol.presence.Field GetField()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(Value).Build()).Build();
            return field;
        }

        public bnet.protocol.presence.FieldOperation GetFieldOperation()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(Value).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();
        }
    }

    public class FourCCPresenceField : PresenceField<String>, IPresenceField
    {
        public FourCCPresenceField(FieldKeyHelper.Program Program, FieldKeyHelper.OriginatingClass OriginatingClass, uint FieldNumber, uint Index)
            : base(Program, OriginatingClass, FieldNumber, Index)
        {
        }

        public bnet.protocol.presence.Field GetField()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetFourccValue(Value).Build()).Build();
            return field;
        }

        public bnet.protocol.presence.FieldOperation GetFieldOperation()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetFourccValue(Value).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();
        }
    }

    public class StringPresenceField : PresenceField<String>, IPresenceField
    {
        public StringPresenceField(FieldKeyHelper.Program Program, FieldKeyHelper.OriginatingClass OriginatingClass, uint FieldNumber, uint Index)
            :base(Program, OriginatingClass, FieldNumber, Index)
        {
        }

        public bnet.protocol.presence.Field GetField()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(Value).Build()).Build();
            return field;
        }

        public bnet.protocol.presence.FieldOperation GetFieldOperation()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetStringValue(Value).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();
        }
    }

    public class ByteStringPresenceField<T> : PresenceField<T>, IPresenceField where T : IMessageLite<T> //Used IMessageLite to get ToByteString(), might need refactoring later
    {
        public ByteStringPresenceField(FieldKeyHelper.Program Program, FieldKeyHelper.OriginatingClass OriginatingClass, uint FieldNumber, uint Index)
            :base(Program, OriginatingClass, FieldNumber, Index)
        {
        }

        public bnet.protocol.presence.Field GetField()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(Value.ToByteString()).Build()).Build();
            return field;
        }

        public bnet.protocol.presence.FieldOperation GetFieldOperation()
        {
            var fieldKey = FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
            var field = bnet.protocol.presence.Field.CreateBuilder().SetKey(fieldKey).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(Value.ToByteString()).Build()).Build();
            return bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field).Build();
        }
    }

    public abstract class PresenceField<T>
    {
        public T Value;

        public FieldKeyHelper.Program Program { get; private set; }
        public FieldKeyHelper.OriginatingClass OriginatingClass { get; private set; }
        public uint FieldNumber { get; private set; }
        public uint Index { get; private set; }

        public PresenceField(FieldKeyHelper.Program program, FieldKeyHelper.OriginatingClass originatingClass , uint fieldNumber, uint index)
        {
            Value = default(T);
            FieldNumber = fieldNumber;
            Index = index;
            Program = program;
            OriginatingClass = originatingClass;
        }

        public bnet.protocol.presence.FieldKey GetFieldKey()
        {
            return FieldKeyHelper.Create(Program, OriginatingClass, FieldNumber, Index);
        }
    }

    public interface IPresenceField
    {
        bnet.protocol.presence.Field GetField();
        bnet.protocol.presence.FieldOperation GetFieldOperation();
        bnet.protocol.presence.FieldKey GetFieldKey();
    }
}
