/*
 * Copyright (C) 2011 D3Sharp Project
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

namespace D3Sharp.Core.Helpers
{
    public class FieldKeyHelper
    {
        public enum Program : uint
        {
            BNet = 16974,
            D3 = 17459
        }

        public static bnet.protocol.presence.FieldKey Create(Program program, uint group, uint field, uint index)
        {
            return
                bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram((uint) program).SetGroup(group).SetField(
                    field).SetIndex(index).Build();
        }
    } 
}