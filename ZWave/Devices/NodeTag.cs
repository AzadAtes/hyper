﻿using System;

namespace ZWave.Devices
{
    public struct NodeTag : IEquatable<NodeTag>, IComparable<NodeTag>, IComparable
    {
        public byte Id;
        public byte EndPointId;
        public bool IsBitAddress;

        public NodeTag(byte node, byte endpoint, bool isBitAddress)
        {
            Id = node;
            EndPointId = endpoint;
            IsBitAddress = isBitAddress;
        }

        public NodeTag(byte node, byte endpoint)
        {
            Id = node;
            EndPointId = endpoint;
            IsBitAddress = false;
        }

        public NodeTag(byte nodeId)
        {
            Id = nodeId;
            EndPointId = 0;
            IsBitAddress = false;
        }

        public NodeTag Parent
        {
            get { return new NodeTag(Id); }
        }

        public static NodeTag FF = new NodeTag(0xFF);

        public override int GetHashCode()
        {
            return (Id << 8) + EndPointId;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeTag))
                return false;

            NodeTag nodeId = (NodeTag)obj;
            return Equals(nodeId);
        }

        public bool Equals(NodeTag other)
        {
            return Id == other.Id && EndPointId == other.EndPointId;
        }

        public static bool operator !=(NodeTag nodeId1, NodeTag nodeId2)
        {
            return !nodeId1.Equals(nodeId2);
        }

        public static bool operator ==(NodeTag nodeId1, NodeTag nodeId2)
        {
            return nodeId1.Equals(nodeId2);
        }

        public static implicit operator byte(NodeTag node)
        {
            return node.Id;
        }

        public override string ToString()
        {
            if (EndPointId == 0)
            {
                return Id.ToString();
            }
            else
            {
                return Id.ToString() + "." + EndPointId.ToString();
            }
        }

        public int CompareTo(NodeTag other)
        {
            if (Id < other.Id)
                return -1;
            else if (Id > other.Id)
                return 1;
            else if (EndPointId < other.EndPointId)
                return -1;
            else if (EndPointId > other.EndPointId)
                return 1;
            else
                return 0;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is NodeTag))
                return 0;

            NodeTag nodeId = (NodeTag)obj;
            return CompareTo(nodeId);
        }
    }
}
