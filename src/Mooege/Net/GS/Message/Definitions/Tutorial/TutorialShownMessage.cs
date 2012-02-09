/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using System.Text;

namespace Mooege.Net.GS.Message.Definitions.Tutorial
{
    /// <summary>
    /// Sent by the client after it has shown a tutorial
    /// </summary>
    [Message(Opcodes.TutorialShownMessage)]
    public class TutorialShownMessage : GameMessage, ISelfHandler
    {
        public int SNOTutorial;

        public override void Parse(GameBitBuffer buffer)
        {
            SNOTutorial = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOTutorial);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TutorialShownMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOTutorial: 0x" + SNOTutorial.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


        /// <summary>
        /// Server only has to save what tutorials are shown, so the player
        /// does not have to see them over and over...
        /// </summary>
        /// <param name="client"></param>
        public void Handle(GameClient client)
        {
            for (int i = 0; i < client.Player.SeenTutorials.Length; i++)
                if (client.Player.SeenTutorials[i] == -1)
                {
                    client.Player.SeenTutorials[i] = SNOTutorial;
                    break;
                }
        }
    }
}