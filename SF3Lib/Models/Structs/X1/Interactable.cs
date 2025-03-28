using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables.X1.Town;
using SF3.Types;

namespace SF3.Models.Structs.X1 {
    public class Interactable : Struct {
        private readonly int _triggerAddr;
        private readonly int _triggerFlagsAddr;
        private readonly int _triggerTargetIdAddr;
        private readonly int _flagCheckedAddr;
        private readonly int _padding0x06;
        private readonly int _eventTypeAddr;
        private readonly int _itemIDAddr;

        public Interactable(IByteData data, int id, string name, int address, INameGetterContext nameGetterContext, NpcTable npcTable)
        : base(data, id, name, address, 0x0C) {
            NameGetterContext = nameGetterContext;
            NpcTable = npcTable;

            _triggerAddr         = Address + 0x00; // 2 bytes
            _triggerFlagsAddr    = Address + 0x02; // 1 byte
            _triggerTargetIdAddr = Address + 0x03; // 1 byte
            _flagCheckedAddr     = Address + 0x04; // 2 bytes
            _padding0x06         = Address + 0x06; // 2 bytes
            _eventTypeAddr       = Address + 0x08; // 2 bytes
            _itemIDAddr          = Address + 0x0a; // 2 bytes
        }

        public INameGetterContext NameGetterContext { get; }
        public NpcTable NpcTable { get; }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4")]
        [BulkCopy]
        public ushort Trigger {
            get => (ushort) Data.GetWord(_triggerAddr);
            set => Data.SetWord(_triggerAddr, value);
        }

        [TableViewModelColumn(displayOrder: 0.1f, minWidth: 60)]
        [NameGetter(NamedValueType.EventTriggerType)]
        public int TriggerType {
            get => Trigger & 0x000F;
            set => Trigger = (ushort) ((Trigger & ~0x000Fu) | ((ushort) value & 0x000Fu));
        }

        public NamedValueType? TriggerParam1Type {
            get {
                switch ((EventTriggerType) TriggerType) {
                    case EventTriggerType.Inspect:
                        return NamedValueType.EventTriggerInspectType;
                    case EventTriggerType.MoveOnTile:
                        return NamedValueType.EventTriggerMoveOnTileType;
                    case EventTriggerType.Warp:
                        return NamedValueType.EventTriggerWarpSound;
                    case EventTriggerType.UseItem:
                        return NamedValueType.EventTriggerUseItemType;
                    default:
                        return null;
                }
            }
        }

        public NamedValueType? TriggerParam2Type {
            get {
                switch ((EventTriggerType) TriggerType) {
                    case EventTriggerType.Inspect:
                        return (TriggerParam1 == (int) EventTriggerInspectType.TreasureChest) ? NamedValueType.EventTriggerDirection : (NamedValueType?) null;
                    case EventTriggerType.MoveOnTile:
                        return ((TriggerParam1 & 0x2) == 0x2) ? NamedValueType.EventTriggerDirection : (NamedValueType?) null;
                    case EventTriggerType.UseItem:
                        return NamedValueType.Item;
                    default:
                        return null;
                }
            }
        }

        [TableViewModelColumn(displayOrder: 0.3f, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.ConditionalType, nameof(TriggerParam1Type))]
        public int? TriggerParam1 {
            get {
                var triggerParam1Type = TriggerParam1Type;
                if (!triggerParam1Type.HasValue)
                    return null;

                var trigger = Trigger;
                var triggerType = (EventTriggerType) (trigger & 0x000F);

                switch (triggerType) {
                    case EventTriggerType.Inspect:
                        return (Trigger & 0x0FF0) >> 4;

                    case EventTriggerType.MoveOnTile:
                    case EventTriggerType.Warp:
                        return (Trigger & 0x0F00) >> 8;

                    case EventTriggerType.UseItem:
                        return (Trigger & 0x00F0) >> 4;

                    default:
                        return null;
                }
            }
            set {
                if (!value.HasValue)
                    return;

                var triggerParam1Type = TriggerParam1Type;
                if (!triggerParam1Type.HasValue)
                    return;

                var trigger = Trigger;
                var triggerType = (EventTriggerType) (trigger & 0x000F);

                switch (triggerType) {
                    case EventTriggerType.Inspect:
                        Trigger = (ushort) ((trigger & ~0x0FF0) | (((value) << 4) & 0x0FF0));
                        break;

                    case EventTriggerType.MoveOnTile:
                    case EventTriggerType.Warp:
                        Trigger = (ushort) ((trigger & ~0x0F00) | (((value) << 8) & 0x0F00));
                        break;

                    case EventTriggerType.UseItem:
                        Trigger = (ushort) ((trigger & ~0x00F0) | (((value) << 4) & 0x00F0));
                        break;

                    default:
                        break;
                }
            }
        }

        [TableViewModelColumn(displayOrder: 0.31f, displayFormat: "X2", minWidth: 150)]
        [NameGetter(NamedValueType.ConditionalType, nameof(TriggerParam2Type))]
        public int? TriggerParam2 {
            get {
                var triggerParam2Type = TriggerParam2Type;
                if (!triggerParam2Type.HasValue)
                    return null;

                var trigger = Trigger;
                var triggerType = (EventTriggerType) (trigger & 0x000F);

                switch (triggerType) {
                    case EventTriggerType.Inspect:
                    case EventTriggerType.MoveOnTile:
                        return ((trigger & 0xF000) >> 12);

                    case EventTriggerType.UseItem:
                        return (Trigger & 0xFF00) >> 8;

                    default:
                        return null;
                }
            }
            set {
                if (!value.HasValue)
                    return;

                var triggerParam2Type = TriggerParam2Type;
                if (!triggerParam2Type.HasValue)
                    return;

                var trigger = Trigger;
                var triggerType = (EventTriggerType) (trigger & 0x000F);

                switch (triggerType) {
                    case EventTriggerType.Inspect:
                    case EventTriggerType.MoveOnTile:
                        Trigger = (ushort) ((trigger & ~0xF000) | (((value) << 12) & 0xF000));
                        break;

                    case EventTriggerType.UseItem:
                        Trigger = (ushort) ((trigger & ~0xFF00) | (((value) << 8) & 0xFF00));
                        break;

                    default:
                        break;
                }
            }
        }

        public byte? TriggerMPDEventIDGroup {
            get {
                switch ((EventTriggerType) TriggerType) {
                    case EventTriggerType.Warp:
                        return 0x10;
                    case EventTriggerType.MoveOnTile:
                        return 0x20;
                    case EventTriggerType.Inspect:
                        return 0x30;
                    case EventTriggerType.UseItem:
                        return 0x40;
                    case EventTriggerType.Bookshelf:
                        return 0x80;
                    case EventTriggerType.Cupboard:
                        return 0xB0;
                    default:
                        return null;
                }
            }
        }

        [TableViewModelColumn(displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public byte TriggerFlags {
            get => (byte) Data.GetByte(_triggerFlagsAddr);
            set => Data.SetByte(_triggerFlagsAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1.1f, displayFormat: "X2")]
        [BulkCopy]
        public byte TriggerTargetID {
            get => (byte) Data.GetByte(_triggerTargetIdAddr);
            set => Data.SetByte(_triggerTargetIdAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1.2f, displayName: "Trigger MPD Tie-In", isReadOnly: true, displayFormat: "X2")]
        [BulkCopy]
        public byte? TriggerMPDTieIn {
            get {
                var eventNumber = TriggerTargetID;
                var mpdEventIdGroup = TriggerMPDEventIDGroup;
                return (eventNumber <= 0x0F && mpdEventIdGroup.HasValue) ? (byte) (eventNumber + mpdEventIdGroup) : (byte?) null;
            }
        }

        private string GetNpcName(int tieInId) {
            if (tieInId <= 0x3B)
                return NameGetterContext.GetName(null, null, tieInId, new object[] { NamedValueType.Character });

            var npcName = $"NPC 0x{(tieInId - 61):X2}";
            var npc = NpcTable?.FirstOrDefault(x => x.InteractableTieIn == tieInId);
            if (npc != null) {
                var npcRealName = NameGetterContext.GetName(null, null, npc.SpriteID, new object[] { NamedValueType.Sprite });
                if (npcRealName != null)
                    npcName += $" ({npcRealName})";
            }

            return npcName;
        }

        [TableViewModelColumn(displayOrder: 1.3f, minWidth: 350)]
        public string TriggerDescription {
            get {
                switch ((EventTriggerType) TriggerType) {
                    case EventTriggerType.TalkToNPC: {
                        var targetId = TriggerTargetID;
                        string npcName = GetNpcName(targetId);
                        return "When talking to " + npcName;
                    }

                    case EventTriggerType.Warp: {
                        var withSoundEffect = ((EventTriggerWarpSoundType) TriggerParam1 == EventTriggerWarpSoundType.PlaySound) ? " with sound effect" : "";
                        return "When entering tile 0x" + (TriggerMPDTieIn ?? -1).ToString("X2") + withSoundEffect;
                    }

                    case EventTriggerType.MoveOnTile: {
                        var tileMessage      = " moving on tile 0x" + (TriggerMPDTieIn ?? -1).ToString("X2");
                        var isDoor = (TriggerParam1 & 0x01) != 0;
                        var collisionMessage = isDoor ? " and pushing a door" : "";
                        var directionMessage = (TriggerParam2Type == NamedValueType.EventTriggerDirection) ? " facing " + ((EventTriggerDirectionType) TriggerParam2).ToString() : "";
                        return "When" + tileMessage + collisionMessage + directionMessage;
                    }

                    case EventTriggerType.Inspect: {
                        var actionType = EventType;
                        var inspectType = (EventTriggerInspectType) TriggerParam1;
                        var inspectTypeMessage =
                            (inspectType == EventTriggerInspectType.Nothing)
                            ? ""
                            : (((inspectType == EventTriggerInspectType.TreasureChest && actionType == 0x0101) ? "a locked " : "a ") + inspectType.ToString());
                        var facingMessage = (TriggerParam2Type == NamedValueType.EventTriggerDirection) ? " facing " + ((EventTriggerDirectionType) TriggerParam2).ToString() : "";
                        var tileMessage = ((inspectTypeMessage == "") ? "tile" : " at tile");

                        return $"When inspecting {inspectTypeMessage}{facingMessage}{tileMessage} 0x" + (TriggerMPDTieIn ?? -1).ToString("X2");
                    }

                    case EventTriggerType.UseItem: {
                        var useOnNpc = TriggerParam1 == 1;
                        var itemId = TriggerParam2;

                        var itemName = (itemId == 0) ? null : NameGetterContext.GetName(null, null, itemId, new object[] { NamedValueType.Item });
                        var itemMessage = (itemName == null) ? "using any item" : "using " + itemName;
                        var targetMessage = useOnNpc ? GetNpcName(TriggerTargetID) : "tile 0x"  + (TriggerMPDTieIn ?? -1).ToString("X2");

                        return $"When {itemMessage} on {targetMessage}";
                    }

                    case EventTriggerType.Bookshelf:
                        return "When checking bookshelf at tile " + (TriggerMPDTieIn ?? -1).ToString("X2");

                    case EventTriggerType.Cupboard:
                        return "When checking cupboard at tile " + (TriggerMPDTieIn ?? -1).ToString("X2");

                    default:
                        return "(Unhandled event)";
                }
            }
        }

        [BulkCopy]
        public int FlagCheckedWthExpectedValue {
            get => Data.GetWord(_flagCheckedAddr);
            set => Data.SetWord(_flagCheckedAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2.0f, displayFormat: "X3", minWidth: 200)]
        [NameGetter(NamedValueType.GameFlag)]
        public int FlagChecked {
            get => FlagCheckedWthExpectedValue & 0x0FFF;
            set => FlagCheckedWthExpectedValue = (FlagCheckedWthExpectedValue & ~0xFFF) | (value & 0x0FFF);
        }

        [TableViewModelColumn(displayOrder: 2.1f)]
        public bool FlagExpectedValue {
            get => (FlagCheckedWthExpectedValue & 0x1000) != 0;
            set => FlagCheckedWthExpectedValue = value ? (FlagCheckedWthExpectedValue | ~0x1000) : (FlagCheckedWthExpectedValue & ~0x1000);
        }

        [BulkCopy]
        public int Padding0x06 {
            get => Data.GetWord(_padding0x06);
            set => Data.SetWord(_padding0x06, value);
        }

        [TableViewModelColumn(displayOrder: 4, displayName: "EventType/Code", displayFormat: "X4")]
        [BulkCopy]
        public int EventType {
            get => Data.GetWord(_eventTypeAddr);
            set => Data.SetWord(_eventTypeAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5, displayName: "Item/Text/Code", displayFormat: "X4")]
        [BulkCopy]
        [NameGetter(NamedValueType.EventParameter, nameof(EventType))]
        public int EventParameter {
            get => Data.GetWord(_itemIDAddr);
            set => Data.SetWord(_itemIDAddr, value);
        }
    }
}
