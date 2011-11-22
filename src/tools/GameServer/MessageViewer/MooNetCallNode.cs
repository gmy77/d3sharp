using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bnet.protocol;
using Mooege.Core.MooNet.Services;
using Mooege.Net.MooNet.Packets;
using Mooege.Net.MooNet;
using Google.ProtocolBuffers;
using System.Windows.Forms;
using Google.ProtocolBuffers.Descriptors;

namespace GameMessageViewer
{
    interface ITextNode
    {
        string AsText();
    }


    class MooNetReplyNode : TreeNode, ITextNode
    {
        PacketIn packet;
        IMessage message;

        public MooNetReplyNode(PacketIn packet, IBuilder builder)
        {
            this.packet = packet;
            this.message = packet.ReadMessage(builder);
            this.Text = message.DescriptorForType.Name;
        }

        public string AsText()
        {
            return message.ToString();
        }
    }


    class MooNetCallNode:TreeNode, ITextNode
    {
        PacketIn packet;
        IMessage message;
        IMessage reply;

        public MooNetCallNode(PacketIn packet, CodedInputStream stream)
        {
            var service = Service.GetByID(packet.Header.ServiceId);
            var method = service.DescriptorForType.Methods.Single(m => MooNetRouter.GetMethodId(m) == packet.Header.MethodId);
            var proto = service.GetRequestPrototype(method);
            reply = service.GetResponsePrototype(method);
            var builder = proto.WeakCreateBuilderForType();

            try
            {
                message = builder.WeakMergeFrom(CodedInputStream.CreateInstance(packet.GetPayload(stream))).WeakBuild();
                Text = message.DescriptorForType.Name;
            }
            catch (Exception e)
            {
                message = builder.WeakBuildPartial();
                Text = "Error parsing message {0}";
            }


        }

        public string AsText()
        {
            return message.ToString();
        }

        public TreeNode ReceiveReply(PacketIn packet, bool add)
        {
            var node = new MooNetReplyNode(packet, reply.WeakCreateBuilderForType());
            this.Nodes.Add(node);
            return node;
        }
    }
}
