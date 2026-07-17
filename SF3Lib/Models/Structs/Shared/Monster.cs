using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Models.Structs.X002;
using SF3.Models.Tables.X002;
using SF3.Types;

namespace SF3.Models.Structs.X019 {
    public class Monster : Struct {
        private readonly int _maxHPAddr;
        private readonly int _maxMPAddr;
        private readonly int _levelAddr;
        private readonly int _attackAddr;
        private readonly int _defenseAddr;
        private readonly int _agilityAddr;
        private readonly int _movAddr;
        private readonly int _luckAddr;
        private readonly int _turnsAddr;
        private readonly int _hpRegenAddr;
        private readonly int _mpRegenAddr;
        private readonly int _earthResAddr;
        private readonly int _fireResAddr;
        private readonly int _iceResAddr;
        private readonly int _sparkResAddr;
        private readonly int _windResAddr;
        private readonly int _lightResAddr;
        private readonly int _darkResAddr;
        private readonly int _unusedResAddr;
        private readonly int _spell1Addr;
        private readonly int _spell1LevelAddr;
        private readonly int _spell2Addr;
        private readonly int _spell2LevelAddr;
        private readonly int _spell3Addr;
        private readonly int _spell3LevelAddr;
        private readonly int _spell4Addr;
        private readonly int _spell4LevelAddr;
        private readonly int _weaponAddr;
        private readonly int _accessoryAddr;
        private readonly int _itemSlot1Addr;
        private readonly int _itemSlot2Addr;
        private readonly int _itemSlot3Addr;
        private readonly int _itemSlot4Addr;
        private readonly int _special1Addr;
        private readonly int _special2Addr;
        private readonly int _special3Addr;
        private readonly int _special4Addr; //?
        private readonly int _special5Addr; //?
        private readonly int _special6Addr; //?
        private readonly int _special7Addr; //?
        private readonly int _special8Addr; //?
        private readonly int _special9Addr; //?
        private readonly int _special10Addr; //?
        private readonly int _unknown0x32Addr;
        private readonly int _unknown0x33Addr;
        private readonly int _intelligence;
        private readonly int _flagsAddr;
        private readonly int _isObject;
        private readonly int _timesSpawnedAddr;
        private readonly int _goldAddr;
        private readonly int _dropItemAddr;
        private readonly int _dropDisableAddr;
        private readonly int _dropRateAddr;
        private readonly int _slowPlusAddr;
        private readonly int _supportPlusAddr;
        private readonly int _magicBonusIdAddr;
        private readonly int _movementTypeAddr;
        private readonly int _orderChoiceModeAddr;
        private readonly int _attackChoiceModeAddr;
        private readonly int _attackChoice1Addr;
        private readonly int _attackChoice2Addr;
        private readonly int _attackChoice3Addr;
        private readonly int _attackChoice4Addr;
        private readonly int _attackChoice5Addr;
        private readonly int _attackChoice6Addr;
        private readonly int _attackChoiceExtra;
        private readonly int _unknown0x4BAddr;

        public Monster(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x4C) {
            _maxHPAddr             = Address + 0x00; // 2 bytes
            _maxMPAddr             = Address + 0x02;
            _levelAddr             = Address + 0x03;
            _attackAddr            = Address + 0x04;
            _defenseAddr           = Address + 0x05;
            _agilityAddr           = Address + 0x06;
            _movAddr               = Address + 0x07;
            _luckAddr              = Address + 0x08;
            _turnsAddr             = Address + 0x09;
            _hpRegenAddr           = Address + 0x0A;
            _mpRegenAddr           = Address + 0x0B;
            _earthResAddr          = Address + 0x0C;
            _fireResAddr           = Address + 0x0D;
            _iceResAddr            = Address + 0x0E;
            _sparkResAddr          = Address + 0x0F;
            _windResAddr           = Address + 0x10;
            _lightResAddr          = Address + 0x11;
            _darkResAddr           = Address + 0x12;
            _unusedResAddr         = Address + 0x13;
            _spell1Addr            = Address + 0x14;
            _spell1LevelAddr       = Address + 0x15;
            _spell2Addr            = Address + 0x16;
            _spell2LevelAddr       = Address + 0x17;
            _spell3Addr            = Address + 0x18;
            _spell3LevelAddr       = Address + 0x19;
            _spell4Addr            = Address + 0x1A;
            _spell4LevelAddr       = Address + 0x1B;
            _weaponAddr            = Address + 0x1C; // 2 bytes
            _accessoryAddr         = Address + 0x1E; // 2 bytes
            _itemSlot1Addr         = Address + 0x20; // 2 bytes
            _itemSlot2Addr         = Address + 0x22; // 2 bytes
            _itemSlot3Addr         = Address + 0x24; // 2 bytes
            _itemSlot4Addr         = Address + 0x26; // 2 bytes
            _special1Addr          = Address + 0x28;
            _special2Addr          = Address + 0x29;
            _special3Addr          = Address + 0x2A;
            _special4Addr          = Address + 0x2B; // ?
            _special5Addr          = Address + 0x2C; // ?
            _special6Addr          = Address + 0x2D; // ?
            _special7Addr          = Address + 0x2E; // ?
            _special8Addr          = Address + 0x2F; // ?
            _special9Addr          = Address + 0x30; // ?
            _special10Addr         = Address + 0x31; // ?
            _unknown0x32Addr       = Address + 0x32;
            _unknown0x33Addr       = Address + 0x33;
            _intelligence          = Address + 0x34;
            _flagsAddr             = Address + 0x35;
            _isObject              = Address + 0x36;
            _timesSpawnedAddr      = Address + 0x37;
            _goldAddr              = Address + 0x38; // 2 bytes
            _dropItemAddr          = Address + 0x3A; // 2 bytes
            _dropDisableAddr       = Address + 0x3C;
            _dropRateAddr          = Address + 0x3D; // droprate/drops items when attacked. Set E for thief rules
            _slowPlusAddr          = Address + 0x3E;
            _supportPlusAddr       = Address + 0x3F;
            _magicBonusIdAddr      = Address + 0x40;
            _movementTypeAddr      = Address + 0x41;
            _orderChoiceModeAddr   = Address + 0x42; // heal when damaged when set?
            _attackChoiceModeAddr  = Address + 0x43;
            _attackChoice1Addr     = Address + 0x44; // what to do on turn1?. 0 = atk. 1 = spell. 4 = use weapon?
            _attackChoice2Addr     = Address + 0x45; // what to do on turn2?
            _attackChoice3Addr     = Address + 0x46; // what to do on turn3?
            _attackChoice4Addr     = Address + 0x47; // what to do on turn4?
            _attackChoice5Addr     = Address + 0x48; // what to do on turn5?
            _attackChoice6Addr     = Address + 0x49; // what to do on turn6?
            _attackChoiceExtra     = Address + 0x4A;
            _unknown0x4BAddr       = Address + 0x4B;
            SpriteID               = id + 200;
        }

        /// <summary>
        /// Adjusts Monster stats based on their equipment.
        /// </summary>
        /// <param name="itemTable">Table from which to get equipment.</param>
        /// <param name="apply">When true, stat changes are applied. When false, stat changes are unapplied.</param>
        /// <returns>The number of items which had their stats applied.</returns>
        public int ApplyEquipmentStats(ItemTable itemTable, bool apply) {
            return (ApplyItemStats(itemTable, Weapon, apply) ? 1 : 0) +
                   (ApplyItemStats(itemTable, Accessory, apply) ? 1 : 0);
        }

        /// <summary>
        /// Adjusts Monster stats based on an item/piece of equipment.
        /// </summary>
        /// <param name="itemTable">Table from which to get the item.</param>
        /// <param name="itemId">The ID of the item.</param>
        /// <param name="apply">When true, stat changes are applied. When false, stat changes are unapplied.</param>
        /// <returns>Returns 'true' if an item was found and applied, otherwise 'false'.</returns>
        public bool ApplyItemStats(ItemTable itemTable, int itemId, bool apply) {
            if (itemId <= 0 || itemId >= itemTable.Count)
                return false;
            ApplyItemStats(itemTable[itemId], apply);
            return true;
        }

        /// <summary>
        /// Adjusts Monster stats based on an item/piece of equipment.
        /// </summary>
        /// <param name="item">The item to apply/unapply</param>
        /// <param name="apply">When true, stat changes are applied. When false, stat changes are unapplied.</param>
        public void ApplyItemStats(Item item, bool apply) {
            var statMult = apply ? 1 : -1;
            Attack  += (byte) (item.Attack  * statMult);
            Defense += (byte) (item.Defense * statMult);
        }

        [TableViewModelColumn(addressField: null, displayOrder: -0.5f, displayFormat: "X2", displayGroup: "Stats1")]
        public int SpriteID { get; }

        [TableViewModelColumn(addressField: nameof(_maxHPAddr), displayOrder: 0, displayGroup: "Stats1")]
        [BulkCopy]
        public ushort MaxHP {
            get => Data.GetUInt16(_maxHPAddr);
            set => Data.SetUInt16(_maxHPAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_maxMPAddr), displayOrder: 1, displayGroup: "Stats1")]
        [BulkCopy]
        public byte MaxMP {
            get => Data.GetUInt8(_maxMPAddr);
            set => Data.SetUInt8(_maxMPAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_levelAddr), displayOrder: 2, displayGroup: "Stats1")]
        [BulkCopy]
        public byte Level {
            get => Data.GetUInt8(_levelAddr);
            set => Data.SetUInt8(_levelAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackAddr), displayOrder: 3, displayGroup: "Stats1")]
        [BulkCopy]
        public byte Attack {
            get => Data.GetUInt8(_attackAddr);
            set => Data.SetUInt8(_attackAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_defenseAddr), displayOrder: 4, displayGroup: "Stats1")]
        [BulkCopy]
        public byte Defense {
            get => Data.GetUInt8(_defenseAddr);
            set => Data.SetUInt8(_defenseAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_agilityAddr), displayOrder: 5, displayGroup: "Stats1")]
        [BulkCopy]
        public byte Agility {
            get => Data.GetUInt8(_agilityAddr);
            set => Data.SetUInt8(_agilityAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_movAddr), displayOrder: 6, displayGroup: "Stats1")]
        [BulkCopy]
        public byte Mov {
            get => Data.GetUInt8(_movAddr);
            set => Data.SetUInt8(_movAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_luckAddr), displayOrder: 7, displayGroup: "Stats1")]
        [BulkCopy]
        public byte Luck {
            get => Data.GetUInt8(_luckAddr);
            set => Data.SetUInt8(_luckAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_turnsAddr), displayOrder: 8, displayGroup: "Stats1")]
        [BulkCopy]
        public byte Turns {
            get => Data.GetUInt8(_turnsAddr);
            set => Data.SetUInt8(_turnsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_hpRegenAddr), displayOrder: 9, displayGroup: "Stats1")]
        [BulkCopy]
        public byte HPRegen {
            get => Data.GetUInt8(_hpRegenAddr);
            set => Data.SetUInt8(_hpRegenAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_mpRegenAddr), displayOrder: 10, displayGroup: "Stats1")]
        [BulkCopy]
        public byte MPRegen {
            get => Data.GetUInt8(_mpRegenAddr);
            set => Data.SetUInt8(_mpRegenAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_earthResAddr), displayOrder: 11, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte EarthRes {
            get => Data.GetInt8(_earthResAddr);
            set => Data.SetInt8(_earthResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_fireResAddr), displayOrder: 12, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte FireRes {
            get => Data.GetInt8(_fireResAddr);
            set => Data.SetInt8(_fireResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_iceResAddr), displayOrder: 13, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte IceRes {
            get => Data.GetInt8(_iceResAddr);
            set => Data.SetInt8(_iceResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_sparkResAddr), displayOrder: 14, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte SparkRes {
            get => Data.GetInt8(_sparkResAddr);
            set => Data.SetInt8(_sparkResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_windResAddr), displayOrder: 15, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte WindRes {
            get => Data.GetInt8(_windResAddr);
            set => Data.SetInt8(_windResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_lightResAddr), displayOrder: 16, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte LightRes {
            get => Data.GetInt8(_lightResAddr);
            set => Data.SetInt8(_lightResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_darkResAddr), displayOrder: 17, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte DarkRes {
            get => Data.GetInt8(_darkResAddr);
            set => Data.SetInt8(_darkResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unusedResAddr), displayOrder: 18, displayGroup: "MagicRes")]
        [BulkCopy]
        public sbyte UnusedRes {
            get => Data.GetInt8(_unusedResAddr);
            set => Data.SetInt8(_unusedResAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell1Addr), displayOrder: 19, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte Spell1 {
            get => Data.GetUInt8(_spell1Addr);
            set => Data.SetUInt8(_spell1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell1LevelAddr), displayOrder: 20, displayGroup: "Spells")]
        [BulkCopy]
        public byte Spell1Level {
            get => Data.GetUInt8(_spell1LevelAddr);
            set => Data.SetUInt8(_spell1LevelAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell2Addr), displayOrder: 21, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte Spell2 {
            get => Data.GetUInt8(_spell2Addr);
            set => Data.SetUInt8(_spell2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell2LevelAddr), displayOrder: 22, displayGroup: "Spells")]
        [BulkCopy]
        public byte Spell2Level {
            get => Data.GetUInt8(_spell2LevelAddr);
            set => Data.SetUInt8(_spell2LevelAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell3Addr), displayOrder: 23, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte Spell3 {
            get => Data.GetUInt8(_spell3Addr);
            set => Data.SetUInt8(_spell3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell3LevelAddr), displayOrder: 24, displayGroup: "Spells")]
        [BulkCopy]
        public byte Spell3Level {
            get => Data.GetUInt8(_spell3LevelAddr);
            set => Data.SetUInt8(_spell3LevelAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell4Addr), displayOrder: 25, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public byte Spell4 {
            get => Data.GetUInt8(_spell4Addr);
            set => Data.SetUInt8(_spell4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_spell4LevelAddr), displayOrder: 26, displayGroup: "Spells")]
        [BulkCopy]
        public byte Spell4Level {
            get => Data.GetUInt8(_spell4LevelAddr);
            set => Data.SetUInt8(_spell4LevelAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponAddr), displayOrder: 27, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public short Weapon {
            get => Data.GetInt16(_weaponAddr);
            set => Data.SetInt16(_weaponAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryAddr), displayOrder: 27.5F, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public ushort Accessory {
            get => Data.GetUInt16(_accessoryAddr);
            set => Data.SetUInt16(_accessoryAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot1Addr), displayOrder: 28, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public ushort ItemSlot1 {
            get => Data.GetUInt16(_itemSlot1Addr);
            set => Data.SetUInt16(_itemSlot1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot2Addr), displayOrder: 29, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public ushort ItemSlot2 {
            get => Data.GetUInt16(_itemSlot2Addr);
            set => Data.SetUInt16(_itemSlot2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot3Addr), displayOrder: 30, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public ushort ItemSlot3 {
            get => Data.GetUInt16(_itemSlot3Addr);
            set => Data.SetUInt16(_itemSlot3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot4Addr), displayOrder: 31, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public ushort ItemSlot4 {
            get => Data.GetUInt16(_itemSlot4Addr);
            set => Data.SetUInt16(_itemSlot4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special1Addr), displayOrder: 32, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special1 {
            get => Data.GetUInt8(_special1Addr);
            set => Data.SetUInt8(_special1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special2Addr), displayOrder: 33, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special2 {
            get => Data.GetUInt8(_special2Addr);
            set => Data.SetUInt8(_special2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special3Addr), displayOrder: 34, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special3 {
            get => Data.GetUInt8(_special3Addr);
            set => Data.SetUInt8(_special3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special4Addr), displayOrder: 35, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special4 {
            get => Data.GetUInt8(_special4Addr);
            set => Data.SetUInt8(_special4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special5Addr), displayOrder: 36, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special5 {
            get => Data.GetUInt8(_special5Addr);
            set => Data.SetUInt8(_special5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special6Addr), displayOrder: 37, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special6 {
            get => Data.GetUInt8(_special6Addr);
            set => Data.SetUInt8(_special6Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special7Addr), displayOrder: 38, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special7 {
            get => Data.GetUInt8(_special7Addr);
            set => Data.SetUInt8(_special7Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special8Addr), displayOrder: 39, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special8 {
            get => Data.GetUInt8(_special8Addr);
            set => Data.SetUInt8(_special8Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special9Addr), displayOrder: 40, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special9 {
            get => Data.GetUInt8(_special9Addr);
            set => Data.SetUInt8(_special9Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special10Addr), displayOrder: 41, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public byte Special10 {
            get => Data.GetUInt8(_special10Addr);
            set => Data.SetUInt8(_special10Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x32Addr), displayOrder: 42, displayGroup: "Specials", displayFormat: "X2")]
        [BulkCopy]
        public byte Unknown0x32 {
            get => Data.GetUInt8(_unknown0x32Addr);
            set => Data.SetUInt8(_unknown0x32Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x33Addr), displayOrder: 43, displayGroup: "Specials", displayFormat: "X2")]
        [BulkCopy]
        public byte Unknown0x33 {
            get => Data.GetUInt8(_unknown0x33Addr);
            set => Data.SetUInt8(_unknown0x33Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_intelligence), displayOrder: 44, displayGroup: "Flags", displayFormat: "X2", minWidth: 400)]
        [BulkCopy]
        [NameGetter(NamedValueType.TargetScoreFunc)]
        public byte Intelligence {
            get => Data.GetUInt8(_intelligence);
            set => Data.SetUInt8(_intelligence, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.01f, displayGroup: "Flags", displayFormat: "X2")]
        [BulkCopy]
        public byte Flags {
            get => Data.GetUInt8(_flagsAddr);
            set => Data.SetUInt8(_flagsAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.11f, displayGroup: "Flags")]
        public bool IsThief {
            get => Data.GetBit(_flagsAddr, 1);
            set => Data.SetBit(_flagsAddr, 1, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.12f, displayGroup: "Flags")]
        public bool DamageImmune {
            get => Data.GetBit(_flagsAddr, 2);
            set => Data.SetBit(_flagsAddr, 2, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.13f, displayGroup: "Flags")]
        public bool IsBig3x3 {
            get => Data.GetBit(_flagsAddr, 3);
            set => Data.SetBit(_flagsAddr, 3, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.14f, displayGroup: "Flags")]
        public bool CantSeeStatus {
            get => Data.GetBit(_flagsAddr, 4);
            set => Data.SetBit(_flagsAddr, 4, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.15f, displayGroup: "Flags")]
        public bool IsBig5x5Broken {
            get => Data.GetBit(_flagsAddr, 5);
            set => Data.SetBit(_flagsAddr, 5, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.16f, displayGroup: "Flags")]
        public bool IsBig5x5Diamond {
            get => Data.GetBit(_flagsAddr, 6);
            set => Data.SetBit(_flagsAddr, 6, value);
        }

        [TableViewModelColumn(addressField: nameof(_flagsAddr), displayOrder: 44.17f, displayGroup: "Flags")]
        public bool PrioritizeReachingTarget {
            get => Data.GetBit(_flagsAddr, 7);
            set => Data.SetBit(_flagsAddr, 7, value);
        }

        [TableViewModelColumn(addressField: nameof(_isObject), displayOrder: 47, displayGroup: "Flags", displayFormat: "X2")]
        [BulkCopy]
        public byte IsObject {
            get => Data.GetUInt8(_isObject);
            set => Data.SetUInt8(_isObject, value);
        }

        [TableViewModelColumn(addressField: nameof(_timesSpawnedAddr), displayOrder: 48, displayGroup: "Flags", displayFormat: "X2", displayName: nameof(TimesSpawned) + " (Placeholder)")]
        [BulkCopy]
        public byte TimesSpawned {
            get => Data.GetUInt8(_timesSpawnedAddr);
            set => Data.SetUInt8(_timesSpawnedAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_goldAddr), displayOrder: 49, displayGroup: "Stats2")]
        [BulkCopy]
        public ushort Gold {
            get => Data.GetUInt16(_goldAddr);
            set => Data.SetUInt16(_goldAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_dropItemAddr), displayOrder: 50, displayGroup: "Stats2", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public ushort DropItem {
            get => Data.GetUInt16(_dropItemAddr);
            set => Data.SetUInt16(_dropItemAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_dropDisableAddr), displayOrder: 51, displayGroup: "Stats2", displayFormat: "X2")]
        [BulkCopy]
        public byte DropDisable {
            get => Data.GetUInt8(_dropDisableAddr);
            set => Data.SetUInt8(_dropDisableAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_dropRateAddr), displayOrder: 52, displayGroup: "Stats2", minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Droprate)]
        public byte DropRate {
            get => Data.GetUInt8(_dropRateAddr);
            set => Data.SetUInt8(_dropRateAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_slowPlusAddr), displayOrder: 53, displayGroup: "Stats2")]
        [BulkCopy]
        public byte SlowPlus {
            get => Data.GetUInt8(_slowPlusAddr);
            set => Data.SetUInt8(_slowPlusAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_supportPlusAddr), displayOrder: 54, displayGroup: "Stats2")]
        [BulkCopy]
        public byte SupportPlus {
            get => Data.GetUInt8(_supportPlusAddr);
            set => Data.SetUInt8(_supportPlusAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_magicBonusIdAddr), displayOrder: 55, displayGroup: "Stats2", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.MagicBonus)]
        public byte MagicBonusID {
            get => Data.GetUInt8(_magicBonusIdAddr);
            set => Data.SetUInt8(_magicBonusIdAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_movementTypeAddr), displayOrder: 56, displayGroup: "Stats2", displayFormat: "X2", minWidth: 100)]
        [BulkCopy]
        [NameGetter(NamedValueType.MovementType)]
        public byte MovementType {
            get => Data.GetUInt8(_movementTypeAddr);
            set => Data.SetUInt8(_movementTypeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_orderChoiceModeAddr), displayOrder: 56.5f, displayGroup: "AI", minWidth: 230)]
        [BulkCopy]
        [NameGetter(NamedValueType.OrderChoiceMode)]
        public byte OrderChoiceMode {
            get => Data.GetUInt8(_orderChoiceModeAddr);
            set => Data.SetUInt8(_orderChoiceModeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoiceModeAddr), displayOrder: 57, displayFormat: "X2", displayGroup: "AI", minWidth: 200)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoiceMode)]
        public byte AttackChoiceMode {
            get => Data.GetUInt8(_attackChoiceModeAddr);
            set => Data.SetUInt8(_attackChoiceModeAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoice1Addr), displayOrder: 58, displayGroup: "AI", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoice)]
        public byte AttackChoice1 {
            get => Data.GetUInt8(_attackChoice1Addr);
            set => Data.SetUInt8(_attackChoice1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoice2Addr), displayOrder: 59, displayGroup: "AI", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoice)]
        public byte AttackChoice2 {
            get => Data.GetUInt8(_attackChoice2Addr);
            set => Data.SetUInt8(_attackChoice2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoice3Addr), displayOrder: 60, displayGroup: "AI", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoice)]
        public byte AttackChoice3 {
            get => Data.GetUInt8(_attackChoice3Addr);
            set => Data.SetUInt8(_attackChoice3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoice4Addr), displayOrder: 61, displayGroup: "AI", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoice)]
        public byte AttackChoice4 {
            get => Data.GetUInt8(_attackChoice4Addr);
            set => Data.SetUInt8(_attackChoice4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoice5Addr), displayOrder: 62, displayGroup: "AI", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoice)]
        public byte AttackChoice5 {
            get => Data.GetUInt8(_attackChoice5Addr);
            set => Data.SetUInt8(_attackChoice5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoice6Addr), displayOrder: 63, displayGroup: "AI", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoice)]
        public byte AttackChoice6 {
            get => Data.GetUInt8(_attackChoice6Addr);
            set => Data.SetUInt8(_attackChoice6Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_attackChoiceExtra), displayOrder: 64, displayGroup: "AI", displayFormat: "X2", minWidth: 140)]
        [BulkCopy]
        [NameGetter(NamedValueType.AttackChoice)]
        public byte AttackChoiceExtra {
            get => Data.GetUInt8(_attackChoiceExtra);
            set => Data.SetUInt8(_attackChoiceExtra, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x4BAddr), displayOrder: 65, displayGroup: "AI", displayFormat: "X2")]
        [BulkCopy]
        public byte Unknown0x4B {
            get => Data.GetUInt8(_unknown0x4BAddr);
            set => Data.SetUInt8(_unknown0x4BAddr, value);
        }
    }
}
