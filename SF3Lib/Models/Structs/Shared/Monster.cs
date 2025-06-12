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
        private readonly int _unknown0x34Addr;
        private readonly int _protectionsAddr;
        private readonly int _expIs5Addr;
        private readonly int _unknown0x37Addr;
        private readonly int _goldAddr;
        private readonly int _dropAddr;
        private readonly int _unknown0x3CAddr;
        private readonly int _droprateAddr;
        private readonly int _slowPlusAddr;
        private readonly int _supportPlusAddr;
        private readonly int _magicBonusIdAddr;
        private readonly int _movementTypeAddr;
        private readonly int _unknown0x42Addr;
        private readonly int _unknown0x43Addr;
        private readonly int _spellChancePlus1Addr;
        private readonly int _spellChancePlus2Addr;
        private readonly int _spellChancePlus3Addr;
        private readonly int _spellChancePlus4Addr;
        private readonly int _spellChancePlus5Addr;
        private readonly int _spellChancePlus6Addr;
        private readonly int _unknown0x4AAddr;
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
            _unknown0x34Addr       = Address + 0x34;
            _protectionsAddr       = Address + 0x35; // protections? 8 = no crit? 0a = damage immunity?
            _expIs5Addr            = Address + 0x36;
            _unknown0x37Addr       = Address + 0x37;
            _goldAddr              = Address + 0x38; // 2 bytes
            _dropAddr              = Address + 0x3A; // 2 bytes
            _unknown0x3CAddr       = Address + 0x3C;
            _droprateAddr          = Address + 0x3D; // droprate/drops items when attacked. Set E for thief rules
            _slowPlusAddr          = Address + 0x3E;
            _supportPlusAddr       = Address + 0x3F;
            _magicBonusIdAddr      = Address + 0x40;
            _movementTypeAddr      = Address + 0x41;
            _unknown0x42Addr       = Address + 0x42; // heal when damaged when set?
            _unknown0x43Addr       = Address + 0x43;
            _spellChancePlus1Addr  = Address + 0x44; // what to do on turn1?. 0 = atk. 1 = spell. 4 = use weapon?
            _spellChancePlus2Addr  = Address + 0x45; // what to do on turn2?
            _spellChancePlus3Addr  = Address + 0x46; // what to do on turn3?
            _spellChancePlus4Addr  = Address + 0x47; // what to do on turn4?
            _spellChancePlus5Addr  = Address + 0x48; // what to do on turn5?
            _spellChancePlus6Addr  = Address + 0x49; // what to do on turn6?
            _unknown0x4AAddr       = Address + 0x4A;
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
            if (itemId <= 0 || itemId >= itemTable.Length)
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
            Attack  += item.Attack  * statMult;
            Defense += item.Defense * statMult;
        }

        [TableViewModelColumn(addressField: null, displayOrder: -0.5f, displayFormat: "X2", displayGroup: "Stats1")]
        public int SpriteID { get; }

        [TableViewModelColumn(addressField: nameof(_maxHPAddr), displayOrder: 0, displayGroup: "Stats1")]
        [BulkCopy]
        public int MaxHP {
            get => Data.GetWord(_maxHPAddr);
            set => Data.SetWord(_maxHPAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_maxMPAddr), displayOrder: 1, displayGroup: "Stats1")]
        [BulkCopy]
        public int MaxMP {
            get => Data.GetByte(_maxMPAddr);
            set => Data.SetByte(_maxMPAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_levelAddr), displayOrder: 2, displayGroup: "Stats1")]
        [BulkCopy]
        public int Level {
            get => Data.GetByte(_levelAddr);
            set => Data.SetByte(_levelAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_attackAddr), displayOrder: 3, displayGroup: "Stats1")]
        [BulkCopy]
        public int Attack {
            get => Data.GetByte(_attackAddr);
            set => Data.SetByte(_attackAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_defenseAddr), displayOrder: 4, displayGroup: "Stats1")]
        [BulkCopy]
        public int Defense {
            get => Data.GetByte(_defenseAddr);
            set => Data.SetByte(_defenseAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_agilityAddr), displayOrder: 5, displayGroup: "Stats1")]
        [BulkCopy]
        public int Agility {
            get => Data.GetByte(_agilityAddr);
            set => Data.SetByte(_agilityAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_movAddr), displayOrder: 6, displayGroup: "Stats1")]
        [BulkCopy]
        public int Mov {
            get => Data.GetByte(_movAddr);
            set => Data.SetByte(_movAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_luckAddr), displayOrder: 7, displayGroup: "Stats1")]
        [BulkCopy]
        public int Luck {
            get => Data.GetByte(_luckAddr);
            set => Data.SetByte(_luckAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_turnsAddr), displayOrder: 8, displayGroup: "Stats1")]
        [BulkCopy]
        public int Turns {
            get => Data.GetByte(_turnsAddr);
            set => Data.SetByte(_turnsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_hpRegenAddr), displayOrder: 9, displayGroup: "Stats1")]
        [BulkCopy]
        public int HPRegen {
            get => Data.GetByte(_hpRegenAddr);
            set => Data.SetByte(_hpRegenAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_mpRegenAddr), displayOrder: 10, displayGroup: "Stats1")]
        [BulkCopy]
        public int MPRegen {
            get => Data.GetByte(_mpRegenAddr);
            set => Data.SetByte(_mpRegenAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_earthResAddr), displayOrder: 11, displayGroup: "MagicRes")]
        [BulkCopy]
        public int EarthRes {
            get => (sbyte) Data.GetByte(_earthResAddr);
            set => Data.SetByte(_earthResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_fireResAddr), displayOrder: 12, displayGroup: "MagicRes")]
        [BulkCopy]
        public int FireRes {
            get => (sbyte) Data.GetByte(_fireResAddr);
            set => Data.SetByte(_fireResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_iceResAddr), displayOrder: 13, displayGroup: "MagicRes")]
        [BulkCopy]
        public int IceRes {
            get => (sbyte) Data.GetByte(_iceResAddr);
            set => Data.SetByte(_iceResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sparkResAddr), displayOrder: 14, displayGroup: "MagicRes")]
        [BulkCopy]
        public int SparkRes {
            get => (sbyte) Data.GetByte(_sparkResAddr);
            set => Data.SetByte(_sparkResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_windResAddr), displayOrder: 15, displayGroup: "MagicRes")]
        [BulkCopy]
        public int WindRes {
            get => (sbyte) Data.GetByte(_windResAddr);
            set => Data.SetByte(_windResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_lightResAddr), displayOrder: 16, displayGroup: "MagicRes")]
        [BulkCopy]
        public int LightRes {
            get => (sbyte) Data.GetByte(_lightResAddr);
            set => Data.SetByte(_lightResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_darkResAddr), displayOrder: 17, displayGroup: "MagicRes")]
        [BulkCopy]
        public int DarkRes {
            get => (sbyte) Data.GetByte(_darkResAddr);
            set => Data.SetByte(_darkResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unusedResAddr), displayOrder: 18, displayGroup: "MagicRes")]
        [BulkCopy]
        public int UnusedRes {
            get => (sbyte) Data.GetByte(_unusedResAddr);
            set => Data.SetByte(_unusedResAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell1Addr), displayOrder: 19, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell1 {
            get => Data.GetByte(_spell1Addr);
            set => Data.SetByte(_spell1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell1LevelAddr), displayOrder: 20, displayGroup: "Spells")]
        [BulkCopy]
        public int Spell1Level {
            get => Data.GetByte(_spell1LevelAddr);
            set => Data.SetByte(_spell1LevelAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell2Addr), displayOrder: 21, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell2 {
            get => Data.GetByte(_spell2Addr);
            set => Data.SetByte(_spell2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell2LevelAddr), displayOrder: 22, displayGroup: "Spells")]
        [BulkCopy]
        public int Spell2Level {
            get => Data.GetByte(_spell2LevelAddr);
            set => Data.SetByte(_spell2LevelAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell3Addr), displayOrder: 23, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell3 {
            get => Data.GetByte(_spell3Addr);
            set => Data.SetByte(_spell3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell3LevelAddr), displayOrder: 24, displayGroup: "Spells")]
        [BulkCopy]
        public int Spell3Level {
            get => Data.GetByte(_spell3LevelAddr);
            set => Data.SetByte(_spell3LevelAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell4Addr), displayOrder: 25, displayGroup: "Spells", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Spell)]
        public int Spell4 {
            get => Data.GetByte(_spell4Addr);
            set => Data.SetByte(_spell4Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spell4LevelAddr), displayOrder: 26, displayGroup: "Spells")]
        [BulkCopy]
        public int Spell4Level {
            get => Data.GetByte(_spell4LevelAddr);
            set => Data.SetByte(_spell4LevelAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_weaponAddr), displayOrder: 27, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Weapon {
            get => Data.GetWord(_weaponAddr);
            set => Data.SetWord(_weaponAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_accessoryAddr), displayOrder: 27, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Accessory {
            get => Data.GetWord(_accessoryAddr);
            set => Data.SetWord(_accessoryAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot1Addr), displayOrder: 28, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot1 {
            get => Data.GetWord(_itemSlot1Addr);
            set => Data.SetWord(_itemSlot1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot2Addr), displayOrder: 29, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot2 {
            get => Data.GetWord(_itemSlot2Addr);
            set => Data.SetWord(_itemSlot2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot3Addr), displayOrder: 30, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot3 {
            get => Data.GetWord(_itemSlot3Addr);
            set => Data.SetWord(_itemSlot3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_itemSlot4Addr), displayOrder: 31, displayGroup: "Items", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int ItemSlot4 {
            get => Data.GetWord(_itemSlot4Addr);
            set => Data.SetWord(_itemSlot4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_special1Addr), displayOrder: 32, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special1 {
            get => Data.GetByte(_special1Addr);
            set => Data.SetByte(_special1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special2Addr), displayOrder: 33, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special2 {
            get => Data.GetByte(_special2Addr);
            set => Data.SetByte(_special2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special3Addr), displayOrder: 34, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special3 {
            get => Data.GetByte(_special3Addr);
            set => Data.SetByte(_special3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special4Addr), displayOrder: 35, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special4 {
            get => Data.GetByte(_special4Addr);
            set => Data.SetByte(_special4Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special5Addr), displayOrder: 36, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special5 {
            get => Data.GetByte(_special5Addr);
            set => Data.SetByte(_special5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special6Addr), displayOrder: 37, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special6 {
            get => Data.GetByte(_special6Addr);
            set => Data.SetByte(_special6Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special7Addr), displayOrder: 38, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special7 {
            get => Data.GetByte(_special7Addr);
            set => Data.SetByte(_special7Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special8Addr), displayOrder: 39, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special8 {
            get => Data.GetByte(_special8Addr);
            set => Data.SetByte(_special8Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special9Addr), displayOrder: 40, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special9 {
            get => Data.GetByte(_special9Addr);
            set => Data.SetByte(_special9Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_special10Addr), displayOrder: 41, displayGroup: "Specials", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Special)]
        public int Special10 {
            get => Data.GetByte(_special10Addr);
            set => Data.SetByte(_special10Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x32Addr), displayOrder: 42, displayGroup: "Unknown", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x32 {
            get => Data.GetByte(_unknown0x32Addr);
            set => Data.SetByte(_unknown0x32Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x33Addr), displayOrder: 43, displayGroup: "Unknown", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x33 {
            get => Data.GetByte(_unknown0x33Addr);
            set => Data.SetByte(_unknown0x33Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x34Addr), displayOrder: 44, displayGroup: "Unknown", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x34 {
            get => Data.GetByte(_unknown0x34Addr);
            set => Data.SetByte(_unknown0x34Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_protectionsAddr), displayOrder: 45, displayGroup: "Unknown", displayFormat: "X2")]
        [BulkCopy]
        public int Protections {
            get => Data.GetByte(_protectionsAddr);
            set => Data.SetByte(_protectionsAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_protectionsAddr), displayOrder: 46, displayGroup: "Unknown")]
        public bool CantSeeStatus {
            get => Data.GetBit(_protectionsAddr, 4);
            set => Data.SetBit(_protectionsAddr, 4, value);
        }

        [TableViewModelColumn(addressField: nameof(_expIs5Addr), displayOrder: 47, displayGroup: "Unknown", displayFormat: "X2")]
        [BulkCopy]
        public int ExpIs5 {
            get => Data.GetByte(_expIs5Addr);
            set => Data.SetByte(_expIs5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x37Addr), displayOrder: 48, displayGroup: "Unknown", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x37 {
            get => Data.GetByte(_unknown0x37Addr);
            set => Data.SetByte(_unknown0x37Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_goldAddr), displayOrder: 49, displayGroup: "Stats2")]
        [BulkCopy]
        public int Gold {
            get => Data.GetWord(_goldAddr);
            set => Data.SetWord(_goldAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_dropAddr), displayOrder: 50, displayGroup: "Stats2", minWidth: 120, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Item)]
        public int Drop {
            get => Data.GetWord(_dropAddr);
            set => Data.SetWord(_dropAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x3CAddr), displayOrder: 51, displayGroup: "Stats2", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x3C {
            get => Data.GetByte(_unknown0x3CAddr);
            set => Data.SetByte(_unknown0x3CAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_droprateAddr), displayOrder: 52, displayGroup: "Stats2", minWidth: 100, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.Droprate)]
        public int Droprate {
            get => Data.GetByte(_droprateAddr);
            set => Data.SetByte(_droprateAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_slowPlusAddr), displayOrder: 53, displayGroup: "Stats2")]
        [BulkCopy]
        public int SlowPlus {
            get => Data.GetByte(_slowPlusAddr);
            set => Data.SetByte(_slowPlusAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_supportPlusAddr), displayOrder: 54, displayGroup: "Stats2")]
        [BulkCopy]
        public int SupportPlus {
            get => Data.GetByte(_supportPlusAddr);
            set => Data.SetByte(_supportPlusAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_magicBonusIdAddr), displayOrder: 55, displayGroup: "Stats2", displayFormat: "X2", minWidth: 120)]
        [BulkCopy]
        [NameGetter(NamedValueType.MagicBonus)]
        public int MagicBonusID {
            get => Data.GetByte(_magicBonusIdAddr);
            set => Data.SetByte(_magicBonusIdAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_movementTypeAddr), displayOrder: 56, displayGroup: "Stats2", displayFormat: "X2", minWidth: 100)]
        [BulkCopy]
        [NameGetter(NamedValueType.MovementType)]
        public int MovementType {
            get => Data.GetByte(_movementTypeAddr);
            set => Data.SetByte(_movementTypeAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x42Addr), displayOrder: 56.5f, displayName: "CanHeal", displayGroup: "LastPage")]
        [BulkCopy]
        public int Unknown0x42 {
            get => Data.GetByte(_unknown0x42Addr);
            set => Data.SetByte(_unknown0x42Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x43Addr), displayOrder: 57, displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x43 {
            get => Data.GetByte(_unknown0x43Addr);
            set => Data.SetByte(_unknown0x43Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellChancePlus1Addr), displayOrder: 58, displayName: "+SpellChance1", displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int SpellChancePlus1 {
            get => Data.GetByte(_spellChancePlus1Addr);
            set => Data.SetByte(_spellChancePlus1Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellChancePlus2Addr), displayOrder: 59, displayName: "+SpellChance2", displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int SpellChancePlus2 {
            get => Data.GetByte(_spellChancePlus2Addr);
            set => Data.SetByte(_spellChancePlus2Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellChancePlus3Addr), displayOrder: 60, displayName: "+SpellChance3", displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int SpellChancePlus3 {
            get => Data.GetByte(_spellChancePlus3Addr);
            set => Data.SetByte(_spellChancePlus3Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellChancePlus4Addr), displayOrder: 61, displayName: "+SpellChance4", displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int SpellChancePlus4 {
            get => Data.GetByte(_spellChancePlus4Addr);
            set => Data.SetByte(_spellChancePlus4Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellChancePlus5Addr), displayOrder: 62, displayName: "+SpellChance5", displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int SpellChancePlus5 {
            get => Data.GetByte(_spellChancePlus5Addr);
            set => Data.SetByte(_spellChancePlus5Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_spellChancePlus6Addr), displayOrder: 63, displayName: "+SpellChance6", displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int SpellChancePlus6 {
            get => Data.GetByte(_spellChancePlus6Addr);
            set => Data.SetByte(_spellChancePlus6Addr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x4AAddr), displayOrder: 64, displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x4A {
            get => Data.GetByte(_unknown0x4AAddr);
            set => Data.SetByte(_unknown0x4AAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x4BAddr), displayOrder: 65, displayGroup: "LastPage", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x4B {
            get => Data.GetByte(_unknown0x4BAddr);
            set => Data.SetByte(_unknown0x4BAddr, (byte) value);
        }
    }
}
