﻿using System.Collections.Generic;
using Utils;
using ZWave.BasicApplication.Operations;
using ZWave.CommandClasses;
using ZWave.Devices;
using ZWave.Enums;

namespace ZWave.BasicApplication.CommandClasses
{
    public class AssociationSupport : ApiAchOperation
    {
        #region Enums

        public TransmitOptions TxOptions { get; set; }
        public TransmitOptions2 TxOptions2 { get; set; }
        public TransmitSecurityOptions TxSecOptions { get; set; }

        #endregion Enums

        private readonly NetworkViewPoint _network;
        public byte GroupId { get; set; }
        public string GroupName { get; set; }
        public List<byte> AssociatedNodeIds { get; set; }
        public byte MaxNodesSupported { get; set; }

        public AssociationSupport(NetworkViewPoint network, TransmitOptions txOptions)
            : base(0, 0, new ByteIndex(COMMAND_CLASS_ASSOCIATION_V2.ID))
        {
            _network = network;
            GroupId = 0x01;
            GroupName = "Lifeline";
            AssociatedNodeIds = new List<byte>();
            MaxNodesSupported = 0xE8;
            TxOptions = txOptions;
            TxOptions2 = TransmitOptions2.TRANSMIT_OPTION_2_TRANSPORT_SERVICE;
            TxSecOptions = TransmitSecurityOptions.S2_TXOPTION_VERIFY_DELIVERY;
        }

        protected override void OnHandledInternal(DataReceivedUnit ou)
        {
            ou.SetNextActionItems();
            byte nodeId = ReceivedAchData.SrcNodeId;
            byte[] command = ReceivedAchData.Command;
            var scheme = (SecuritySchemes)ReceivedAchData.SecurityScheme;
            bool isSuportedScheme = IsSupportedScheme(_network, command, scheme);
            if (command != null && command.Length > 1 && isSuportedScheme)
            {
                if (command[1] == COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_GET.ID)
                {
                    var associationGetCmd = (COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_GET)command;
                    var requestedGroupId = associationGetCmd.groupingIdentifier;
                    if (requestedGroupId == GroupId)
                    {
                        ApiOperation sendData = null;
                        var data = new COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_REPORT()
                        {
                            groupingIdentifier = GroupId,
                            nodeid = AssociatedNodeIds,
                            maxNodesSupported = MaxNodesSupported
                        };
                        sendData = new SendDataExOperation(nodeId, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                        ou.SetNextActionItems(sendData);
                    }
                    else if (requestedGroupId != 0x00)
                    {
                        ApiOperation sendData = null;
                        var data = new COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_REPORT()
                        {
                            groupingIdentifier = requestedGroupId,
                            nodeid = AssociatedNodeIds,
                            maxNodesSupported = MaxNodesSupported
                        };
                        sendData = new SendDataExOperation(nodeId, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                        ou.SetNextActionItems(sendData);
                    }
                }
                else if (command[1] == COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_SET.ID)
                {
                    var associationSetCmd = (COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_SET)command;
                    var requestedGroupId = associationSetCmd.groupingIdentifier;
                    if (requestedGroupId == GroupId)
                    {
                        foreach (var associateNodeId in associationSetCmd.nodeId)
                        {
                            if (!AssociatedNodeIds.Contains(associateNodeId))
                            {
                                AssociatedNodeIds.Add(associateNodeId);
                            }
                        }
                    }
                    else if (requestedGroupId != 0x00)
                    {
                        foreach (var associateNodeId in associationSetCmd.nodeId)
                        {
                            if (!AssociatedNodeIds.Contains(associateNodeId))
                            {
                                AssociatedNodeIds.Add(associateNodeId);
                            }
                        }
                    }
                }
                else if (command[1] == COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_REMOVE.ID)
                {
                    var associationRemoveCmd = (COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_REMOVE)command;
                    var requestedGroupId = associationRemoveCmd.groupingIdentifier;
                    if (requestedGroupId == GroupId)
                    {
                        foreach (var associateNodeId in associationRemoveCmd.nodeId)
                        {
                            if (AssociatedNodeIds.Contains(associateNodeId))
                            {
                                AssociatedNodeIds.Remove(associateNodeId);
                            }
                        }
                    }
                    else if (requestedGroupId != 0x00)
                    {
                        foreach (var associateNodeId in associationRemoveCmd.nodeId)
                        {
                            if (AssociatedNodeIds.Contains(associateNodeId))
                            {
                                AssociatedNodeIds.Remove(associateNodeId);
                            }
                        }
                    }
                }
                else if (command[1] == COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_GROUPINGS_GET.ID)
                {
                    ApiOperation sendData = null;
                    var data = new COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_GROUPINGS_REPORT()
                    {
                        supportedGroupings = 0x01
                    };
                    sendData = new SendDataExOperation(nodeId, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                    ou.SetNextActionItems(sendData);
                }
                else if (command[1] == COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_SPECIFIC_GROUP_GET.ID)
                {
                    ApiOperation sendData = null;
                    var data = new COMMAND_CLASS_ASSOCIATION_V2.ASSOCIATION_SPECIFIC_GROUP_REPORT()
                    {
                        group = 0
                    };
                    sendData = new SendDataExOperation(nodeId, data, TxOptions, TxSecOptions, scheme, TxOptions2);
                    ou.SetNextActionItems(sendData);
                }
            }
        }
    }
}