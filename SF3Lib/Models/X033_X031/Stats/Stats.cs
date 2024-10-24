using SF3.Attributes;
using SF3.Types;
using SF3.Values;
using SF3.FileEditors;
using System;

namespace SF3.Models.X033_X031.Stats
{
    public class Stats
    {
        /// <summary>
        /// When enabled, GetAverageStatGrowthPerLevelAsPercent() will show the "growthValue" in its output
        /// </summary>
        public static bool DebugGrowthValues { get; set; } = false;

        public enum PromotionLevelType
        {
            Unpromoted = 0,
            Promotion1 = 1,
            Promotion2 = 2,
        }

        private IX033_X031_FileEditor _fileEditor;

        //starting stat table
        private int character;
        private int characterClass;
        private int hpPromote;
        private int hpCurve1;
        private int hpCurve5;
        private int hpCurve10;
        private int hpCurve12_15;
        private int hpCurve14_20;
        private int hpCurve17_30;
        private int hpCurve30_99;
        private int mpPromote;
        private int mpCurve1;
        private int mpCurve5;
        private int mpCurve10;
        private int mpCurve12_15;
        private int mpCurve14_20;
        private int mpCurve17_30;
        private int mpCurve30_99;
        private int atkPromote;
        private int atkCurve1;
        private int atkCurve5;
        private int atkCurve10;
        private int atkCurve12_15;
        private int atkCurve14_20;
        private int atkCurve17_30;
        private int atkCurve30_99;
        private int defPromote;
        private int defCurve1;
        private int defCurve5;
        private int defCurve10;
        private int defCurve12_15;
        private int defCurve14_20;
        private int defCurve17_30;
        private int defCurve30_99;
        private int agiPromote;
        private int agiCurve1;
        private int agiCurve5;
        private int agiCurve10;
        private int agiCurve12_15;
        private int agiCurve14_20;
        private int agiCurve17_30;
        private int agiCurve30_99;

        private int s1LearnedAt;
        private int s1LearnedID;
        private int s1LearnedLevel;
        private int s2LearnedAt;
        private int s2LearnedID;
        private int s2LearnedLevel;
        private int s3LearnedAt;
        private int s3LearnedID;
        private int s3LearnedLevel;
        private int s4LearnedAt;
        private int s4LearnedID;
        private int s4LearnedLevel;
        private int s5LearnedAt;
        private int s5LearnedID;
        private int s5LearnedLevel;
        private int s6LearnedAt;
        private int s6LearnedID;
        private int s6LearnedLevel;
        private int s7LearnedAt;
        private int s7LearnedID;
        private int s7LearnedLevel;
        private int s8LearnedAt;
        private int s8LearnedID;
        private int s8LearnedLevel;
        private int s9LearnedAt;
        private int s9LearnedID;
        private int s9LearnedLevel;
        private int s10LearnedAt;
        private int s10LearnedID;
        private int s10LearnedLevel;
        private int s11LearnedAt;
        private int s11LearnedID;
        private int s11LearnedLevel;
        private int s12LearnedAt;
        private int s12LearnedID;
        private int s12LearnedLevel;

        private int weapon1Special1;
        private int weapon1Special2;
        private int weapon1Special3;
        private int weapon2Special1;
        private int weapon2Special2;
        private int weapon2Special3;
        private int weapon3Special1;
        private int weapon3Special2;
        private int weapon3Special3;
        private int weapon4Special1;
        private int weapon4Special2;
        private int weapon4Special3;
        private int baseLuck;
        private int baseMov;

        private int baseTurns;
        private int baseHPRegen;
        private int baseMPRegen;

        private int earthRes;
        private int fireRes;
        private int iceRes;
        private int sparkRes;
        private int windRes;
        private int lightRes;
        private int darkRes;
        private int unknownRes;
        private int slow;
        private int support;

        private int magicBonus;
        private int movementType;

        private int weaponEquipable1;
        private int weaponEquipable2;
        private int weaponEquipable3;
        private int weaponEquipable4;
        private int accessoryEquipable1;
        private int accessoryEquipable2;
        private int accessoryEquipable3;
        private int accessoryEquipable4;

        private int address;
        private int offset;
        private int checkType;
        private int checkVersion2;

        private int index;
        private string name;

        public Stats(IX033_X031_FileEditor fileEditor, int id, string text)
        {
            _fileEditor = fileEditor;
            checkType = _fileEditor.GetByte(0x00000009); //if it's 0x07 we're in a x033.bin
            checkVersion2 = _fileEditor.GetByte(0x000000017); //to determine which version of scn2 we are using 
            //X031.BIN is during combat
            //X033.BIN is out of combat

            if (Scenario == ScenarioType.Scenario1)
            {
                if (checkType == 0x07)
                {
                    offset = 0x00000da4; //scn1. x033
                }
                else
                {
                    offset = 0x00000d74; //x031
                }
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                if (checkType == 0x07) //x033
                {
                    if (checkVersion2 == 0x8c)
                    {
                        offset = 0x00000ee0; //scn2 ver 1.003
                    }
                    else
                    {
                        offset = 0x00000f08; //scn2
                    }
                }
                else //x031
                {
                    if (checkVersion2 == 0x4c)
                    {
                        offset = 0x00000ea4; //scn2 ver 1.003
                    }
                    else
                    {
                        offset = 0x00000eb4;
                    }
                }
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                if (checkType == 0x07)
                {
                    offset = 0x00001030; //scn3
                }
                else
                {
                    offset = 0x00000ff4;
                }
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                if (checkType == 0x07)
                {
                    offset = 0x00001204; //pd
                }
                else
                {
                    offset = 0x000011bc;
                }
            }

            //offset = 0x00002b28; scn1
            //offset = 0x00002e9c; scn2
            //offset = 0x0000354c; scn3
            //offset = 0x000035fc; pd

            index = id;
            name = text;

            //int start = 0x354c + (id * 24);

            int start = offset + (id * 0x7B);

            character = start + 0x00;
            characterClass = start + 0x01;
            hpPromote = start + 0x02;
            hpCurve1 = start + 0x03;
            hpCurve5 = start + 0x04;
            hpCurve10 = start + 0x05;
            hpCurve12_15 = start + 0x06;
            hpCurve14_20 = start + 0x07;
            hpCurve17_30 = start + 0x08;
            hpCurve30_99 = start + 0x09;
            mpPromote = start + 0x0A;
            mpCurve1 = start + 0x0B;
            mpCurve5 = start + 0x0C;
            mpCurve10 = start + 0x0D;
            mpCurve12_15 = start + 0x0E;
            mpCurve14_20 = start + 0x0F;
            mpCurve17_30 = start + 0x10;
            mpCurve30_99 = start + 0x11;
            atkPromote = start + 0x12;
            atkCurve1 = start + 0x13;
            atkCurve5 = start + 0x14;
            atkCurve10 = start + 0x15;
            atkCurve12_15 = start + 0x16;
            atkCurve14_20 = start + 0x17;
            atkCurve17_30 = start + 0x18;
            atkCurve30_99 = start + 0x19;
            defPromote = start + 0x1a;
            defCurve1 = start + 0x1b;
            defCurve5 = start + 0x1c;
            defCurve10 = start + 0x1d;
            defCurve12_15 = start + 0x1e;
            defCurve14_20 = start + 0x1f;
            defCurve17_30 = start + 0x20;
            defCurve30_99 = start + 0x21;
            agiPromote = start + 0x22;
            agiCurve1 = start + 0x23;
            agiCurve5 = start + 0x24;
            agiCurve10 = start + 0x25;
            agiCurve12_15 = start + 0x26;
            agiCurve14_20 = start + 0x27;
            agiCurve17_30 = start + 0x28;
            agiCurve30_99 = start + 0x29;

            s1LearnedAt = start + 0x2a;
            s1LearnedID = start + 0x2b;
            s1LearnedLevel = start + 0x2c;
            s2LearnedAt = start + 0x2d;
            s2LearnedID = start + 0x2e;
            s2LearnedLevel = start + 0x2f;
            s3LearnedAt = start + 0x30;
            s3LearnedID = start + 0x31;
            s3LearnedLevel = start + 0x32;
            s4LearnedAt = start + 0x33;
            s4LearnedID = start + 0x34;
            s4LearnedLevel = start + 0x35;
            s5LearnedAt = start + 0x36;
            s5LearnedID = start + 0x37;
            s5LearnedLevel = start + 0x38;
            s6LearnedAt = start + 0x39;
            s6LearnedID = start + 0x3a;
            s6LearnedLevel = start + 0x3b;
            s7LearnedAt = start + 0x3c;
            s7LearnedID = start + 0x3d;
            s7LearnedLevel = start + 0x3e;
            s8LearnedAt = start + 0x3f;
            s8LearnedID = start + 0x40;
            s8LearnedLevel = start + 0x41;
            s9LearnedAt = start + 0x42;
            s9LearnedID = start + 0x43;
            s9LearnedLevel = start + 0x44;
            s10LearnedAt = start + 0x45;
            s10LearnedID = start + 0x46;
            s10LearnedLevel = start + 0x47;
            s11LearnedAt = start + 0x48;
            s11LearnedID = start + 0x49;
            s11LearnedLevel = start + 0x4a;
            s12LearnedAt = start + 0x4b;
            s12LearnedID = start + 0x4c;
            s12LearnedLevel = start + 0x4d;

            weapon1Special1 = start + 0x4e;
            weapon1Special2 = start + 0x4f;
            weapon1Special3 = start + 0x50;
            weapon2Special1 = start + 0x51;
            weapon2Special2 = start + 0x52;
            weapon2Special3 = start + 0x53;
            weapon3Special1 = start + 0x54;
            weapon3Special2 = start + 0x55;
            weapon3Special3 = start + 0x56;
            weapon4Special1 = start + 0x57;
            weapon4Special2 = start + 0x58;
            weapon4Special3 = start + 0x59;
            baseLuck = start + 0x5a;
            baseMov = start + 0x5b;
            baseTurns = start + 0x5c;
            baseHPRegen = start + 0x5d;
            baseMPRegen = start + 0x5e;

            earthRes = start + 0x5f;
            fireRes = start + 0x60;
            iceRes = start + 0x61;
            sparkRes = start + 0x62;
            windRes = start + 0x63;
            lightRes = start + 0x64;
            darkRes = start + 0x65;
            unknownRes = start + 0x66;
            slow = start + 0x67;
            support = start + 0x68;
            magicBonus = start + 0x69;
            movementType = start + 0x6a;

            weaponEquipable1 = start + 0x73;
            weaponEquipable2 = start + 0x74;
            weaponEquipable3 = start + 0x75;
            weaponEquipable4 = start + 0x76;
            accessoryEquipable1 = start + 0x77;
            accessoryEquipable2 = start + 0x78;
            accessoryEquipable3 = start + 0x79;
            accessoryEquipable4 = start + 0x7a;

            address = offset + (id * 0x7B);
            //address = 0x0354c + (id * 0x18);
        }

        public ScenarioType Scenario => _fileEditor.Scenario;
        public bool IsPromoted => _fileEditor.GetByte((int)characterClass) >= 0x20;

        public PromotionLevelType PromotionLevel
        {
            get
            {
                int chClass = _fileEditor.GetByte((int)characterClass);
                return chClass < 0x20 ? PromotionLevelType.Unpromoted :
                       chClass < 0x48 ? PromotionLevelType.Promotion1 :
                                        PromotionLevelType.Promotion2;
            }
        }

        public ValueRange<int> GetStatGrowthRange(StatType stat, int groupIndex)
        {
            switch (stat)
            {
                case StatType.HP:
                    switch (groupIndex)
                    {
                        case 0: return new ValueRange<int>(HPCurve1, HPCurve5);
                        case 1: return new ValueRange<int>(HPCurve5, HPCurve10);
                        case 2: return new ValueRange<int>(HPCurve10, HPCurve12_15);
                        case 3: return new ValueRange<int>(HPCurve12_15, HPCurve14_20);
                        case 4: return new ValueRange<int>(HPCurve14_20, HPCurve17_30);
                        case 5: return new ValueRange<int>(HPCurve17_30, HPCurve30_99);
                        default: throw new ArgumentOutOfRangeException();
                    }

                case StatType.MP:
                    switch (groupIndex)
                    {
                        case 0: return new ValueRange<int>(MPCurve1, MPCurve5);
                        case 1: return new ValueRange<int>(MPCurve5, MPCurve10);
                        case 2: return new ValueRange<int>(MPCurve10, MPCurve12_15);
                        case 3: return new ValueRange<int>(MPCurve12_15, MPCurve14_20);
                        case 4: return new ValueRange<int>(MPCurve14_20, MPCurve17_30);
                        case 5: return new ValueRange<int>(MPCurve17_30, MPCurve30_99);
                        default: throw new ArgumentOutOfRangeException();
                    }

                case StatType.Atk:
                    switch (groupIndex)
                    {
                        case 0: return new ValueRange<int>(AtkCurve1, AtkCurve5);
                        case 1: return new ValueRange<int>(AtkCurve5, AtkCurve10);
                        case 2: return new ValueRange<int>(AtkCurve10, AtkCurve12_15);
                        case 3: return new ValueRange<int>(AtkCurve12_15, AtkCurve14_20);
                        case 4: return new ValueRange<int>(AtkCurve14_20, AtkCurve17_30);
                        case 5: return new ValueRange<int>(AtkCurve17_30, AtkCurve30_99);
                        default: throw new ArgumentOutOfRangeException();
                    }

                case StatType.Def:
                    switch (groupIndex)
                    {
                        case 0: return new ValueRange<int>(DefCurve1, DefCurve5);
                        case 1: return new ValueRange<int>(DefCurve5, DefCurve10);
                        case 2: return new ValueRange<int>(DefCurve10, DefCurve12_15);
                        case 3: return new ValueRange<int>(DefCurve12_15, DefCurve14_20);
                        case 4: return new ValueRange<int>(DefCurve14_20, DefCurve17_30);
                        case 5: return new ValueRange<int>(DefCurve17_30, DefCurve30_99);
                        default: throw new ArgumentOutOfRangeException();
                    }

                case StatType.Agi:
                    switch (groupIndex)
                    {
                        case 0: return new ValueRange<int>(AgiCurve1, AgiCurve5);
                        case 1: return new ValueRange<int>(AgiCurve5, AgiCurve10);
                        case 2: return new ValueRange<int>(AgiCurve10, AgiCurve12_15);
                        case 3: return new ValueRange<int>(AgiCurve12_15, AgiCurve14_20);
                        case 4: return new ValueRange<int>(AgiCurve14_20, AgiCurve17_30);
                        case 5: return new ValueRange<int>(AgiCurve17_30, AgiCurve30_99);
                        default: throw new ArgumentOutOfRangeException();
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public double GetAverageStatGrowthPerLevel(StatType stat, int groupIndex)
        {
            var growthValue = SF3.Stats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, SF3.Stats.StatGrowthGroups[IsPromoted][groupIndex].Range.Range);
            return SF3.Stats.GetAverageStatGrowthPerLevel(growthValue);
        }

        public string GetAverageStatGrowthPerLevelAsPercent(StatType stat, int groupIndex)
        {
            var growthValue = SF3.Stats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, SF3.Stats.StatGrowthGroups[IsPromoted][groupIndex].Range.Range);
            return (DebugGrowthValues ? string.Format("{0:x}", growthValue) + " || " : "") +
                    SF3.Stats.GetAverageStatGrowthPerLevelAsPercent(growthValue);
        }

        public int ID => index;

        [BulkCopyRowName]
        public string Name => name;

        [BulkCopy]
        public int CharacterID
        {
            get => _fileEditor.GetByte(character);
            set => _fileEditor.SetByte(character, (byte)value);
        }

        [BulkCopy]
        public CharacterClassValue CharacterClass
        {
            get => new CharacterClassValue(_fileEditor.GetByte(characterClass));
            set => _fileEditor.SetByte(characterClass, (byte)value.Value);
        }

        [BulkCopy]
        public int HPPromote
        {
            get => _fileEditor.GetByte(hpPromote);
            set => _fileEditor.SetByte(hpPromote, (byte)value);
        }

        [BulkCopy]
        public int HPCurve1
        {
            get => _fileEditor.GetByte(hpCurve1);
            set => _fileEditor.SetByte(hpCurve1, (byte)value);
        }

        [BulkCopy]
        public int HPCurve5
        {
            get => _fileEditor.GetByte(hpCurve5);
            set => _fileEditor.SetByte(hpCurve5, (byte)value);
        }

        [BulkCopy]
        public int HPCurve10
        {
            get => _fileEditor.GetByte(hpCurve10);
            set => _fileEditor.SetByte(hpCurve10, (byte)value);
        }

        [BulkCopy]
        public int HPCurve12_15
        {
            get => _fileEditor.GetByte(hpCurve12_15);
            set => _fileEditor.SetByte(hpCurve12_15, (byte)value);
        }

        [BulkCopy]
        public int HPCurve14_20
        {
            get => _fileEditor.GetByte(hpCurve14_20);
            set => _fileEditor.SetByte(hpCurve14_20, (byte)value);
        }

        [BulkCopy]
        public int HPCurve17_30
        {
            get => _fileEditor.GetByte(hpCurve17_30);
            set => _fileEditor.SetByte(hpCurve17_30, (byte)value);
        }

        [BulkCopy]
        public int HPCurve30_99
        {
            get => _fileEditor.GetByte(hpCurve30_99);
            set => _fileEditor.SetByte(hpCurve30_99, (byte)value);
        }

        public string HPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        public string HPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        public string HPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        public string HPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        public string HPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        public string HPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

        [BulkCopy]
        public int MPPromote
        {
            get => _fileEditor.GetByte(mpPromote);
            set => _fileEditor.SetByte(mpPromote, (byte)value);
        }

        [BulkCopy]
        public int MPCurve1
        {
            get => _fileEditor.GetByte(mpCurve1);
            set => _fileEditor.SetByte(mpCurve1, (byte)value);
        }

        [BulkCopy]
        public int MPCurve5
        {
            get => _fileEditor.GetByte(mpCurve5);
            set => _fileEditor.SetByte(mpCurve5, (byte)value);
        }

        [BulkCopy]
        public int MPCurve10
        {
            get => _fileEditor.GetByte(mpCurve10);
            set => _fileEditor.SetByte(mpCurve10, (byte)value);
        }

        [BulkCopy]
        public int MPCurve12_15
        {
            get => _fileEditor.GetByte(mpCurve12_15);
            set => _fileEditor.SetByte(mpCurve12_15, (byte)value);
        }

        [BulkCopy]
        public int MPCurve14_20
        {
            get => _fileEditor.GetByte(mpCurve14_20);
            set => _fileEditor.SetByte(mpCurve14_20, (byte)value);
        }

        [BulkCopy]
        public int MPCurve17_30
        {
            get => _fileEditor.GetByte(mpCurve17_30);
            set => _fileEditor.SetByte(mpCurve17_30, (byte)value);
        }

        [BulkCopy]
        public int MPCurve30_99
        {
            get => _fileEditor.GetByte(mpCurve30_99);
            set => _fileEditor.SetByte(mpCurve30_99, (byte)value);
        }

        public string MPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 0);
        public string MPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 1);
        public string MPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 2);
        public string MPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 3);
        public string MPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 4);
        public string MPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 5);

        [BulkCopy]
        public int AtkPromote
        {
            get => _fileEditor.GetByte(atkPromote);
            set => _fileEditor.SetByte(atkPromote, (byte)value);
        }

        [BulkCopy]
        public int AtkCurve1
        {
            get => _fileEditor.GetByte(atkCurve1);
            set => _fileEditor.SetByte(atkCurve1, (byte)value);
        }

        [BulkCopy]
        public int AtkCurve5
        {
            get => _fileEditor.GetByte(atkCurve5);
            set => _fileEditor.SetByte(atkCurve5, (byte)value);
        }

        [BulkCopy]
        public int AtkCurve10
        {
            get => _fileEditor.GetByte(atkCurve10);
            set => _fileEditor.SetByte(atkCurve10, (byte)value);
        }

        [BulkCopy]
        public int AtkCurve12_15
        {
            get => _fileEditor.GetByte(atkCurve12_15);
            set => _fileEditor.SetByte(atkCurve12_15, (byte)value);
        }

        [BulkCopy]
        public int AtkCurve14_20
        {
            get => _fileEditor.GetByte(atkCurve14_20);
            set => _fileEditor.SetByte(atkCurve14_20, (byte)value);
        }

        [BulkCopy]
        public int AtkCurve17_30
        {
            get => _fileEditor.GetByte(atkCurve17_30);
            set => _fileEditor.SetByte(atkCurve17_30, (byte)value);
        }

        [BulkCopy]
        public int AtkCurve30_99
        {
            get => _fileEditor.GetByte(atkCurve30_99);
            set => _fileEditor.SetByte(atkCurve30_99, (byte)value);
        }

        public string Atkgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 0);
        public string Atkgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 1);
        public string Atkgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 2);
        public string Atkgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 3);
        public string Atkgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 4);
        public string Atkgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 5);

        [BulkCopy]
        public int DefPromote
        {
            get => _fileEditor.GetByte(defPromote);
            set => _fileEditor.SetByte(defPromote, (byte)value);
        }

        [BulkCopy]
        public int DefCurve1
        {
            get => _fileEditor.GetByte(defCurve1);
            set => _fileEditor.SetByte(defCurve1, (byte)value);
        }

        [BulkCopy]
        public int DefCurve5
        {
            get => _fileEditor.GetByte(defCurve5);
            set => _fileEditor.SetByte(defCurve5, (byte)value);
        }

        [BulkCopy]
        public int DefCurve10
        {
            get => _fileEditor.GetByte(defCurve10);
            set => _fileEditor.SetByte(defCurve10, (byte)value);
        }

        [BulkCopy]
        public int DefCurve12_15
        {
            get => _fileEditor.GetByte(defCurve12_15);
            set => _fileEditor.SetByte(defCurve12_15, (byte)value);
        }

        [BulkCopy]
        public int DefCurve14_20
        {
            get => _fileEditor.GetByte(defCurve14_20);
            set => _fileEditor.SetByte(defCurve14_20, (byte)value);
        }

        [BulkCopy]
        public int DefCurve17_30
        {
            get => _fileEditor.GetByte(defCurve17_30);
            set => _fileEditor.SetByte(defCurve17_30, (byte)value);
        }

        [BulkCopy]
        public int DefCurve30_99
        {
            get => _fileEditor.GetByte(defCurve30_99);
            set => _fileEditor.SetByte(defCurve30_99, (byte)value);
        }

        public string Defgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 0);
        public string Defgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 1);
        public string Defgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 2);
        public string Defgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 3);
        public string Defgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 4);
        public string Defgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 5);

        [BulkCopy]
        public int AgiPromote
        {
            get => _fileEditor.GetByte(agiPromote);
            set => _fileEditor.SetByte(agiPromote, (byte)value);
        }

        [BulkCopy]
        public int AgiCurve1
        {
            get => _fileEditor.GetByte(agiCurve1);
            set => _fileEditor.SetByte(agiCurve1, (byte)value);
        }

        [BulkCopy]
        public int AgiCurve5
        {
            get => _fileEditor.GetByte(agiCurve5);
            set => _fileEditor.SetByte(agiCurve5, (byte)value);
        }

        [BulkCopy]
        public int AgiCurve10
        {
            get => _fileEditor.GetByte(agiCurve10);
            set => _fileEditor.SetByte(agiCurve10, (byte)value);
        }

        [BulkCopy]
        public int AgiCurve12_15
        {
            get => _fileEditor.GetByte(agiCurve12_15);
            set => _fileEditor.SetByte(agiCurve12_15, (byte)value);
        }

        [BulkCopy]
        public int AgiCurve14_20
        {
            get => _fileEditor.GetByte(agiCurve14_20);
            set => _fileEditor.SetByte(agiCurve14_20, (byte)value);
        }

        [BulkCopy]
        public int AgiCurve17_30
        {
            get => _fileEditor.GetByte(agiCurve17_30);
            set => _fileEditor.SetByte(agiCurve17_30, (byte)value);
        }

        [BulkCopy]
        public int AgiCurve30_99
        {
            get => _fileEditor.GetByte(agiCurve30_99);
            set => _fileEditor.SetByte(agiCurve30_99, (byte)value);
        }

        public string Agigroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 0);
        public string Agigroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 1);
        public string Agigroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 2);
        public string Agigroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 3);
        public string Agigroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 4);
        public string Agigroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 5);

        [BulkCopy]
        public int S1LearnedAt
        {
            get => _fileEditor.GetByte(s1LearnedAt);
            set => _fileEditor.SetByte(s1LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S1LearnedLevel
        {
            get => _fileEditor.GetByte(s1LearnedLevel);
            set => _fileEditor.SetByte(s1LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S1LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s1LearnedID));
            set => _fileEditor.SetByte(s1LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S2LearnedAt
        {
            get => _fileEditor.GetByte(s2LearnedAt);
            set => _fileEditor.SetByte(s2LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S2LearnedLevel
        {
            get => _fileEditor.GetByte(s2LearnedLevel);
            set => _fileEditor.SetByte(s2LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S2LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s2LearnedID));
            set => _fileEditor.SetByte(s2LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S3LearnedAt
        {
            get => _fileEditor.GetByte(s3LearnedAt);
            set => _fileEditor.SetByte(s3LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S3LearnedLevel
        {
            get => _fileEditor.GetByte(s3LearnedLevel);
            set => _fileEditor.SetByte(s3LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S3LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s3LearnedID));
            set => _fileEditor.SetByte(s3LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S4LearnedAt
        {
            get => _fileEditor.GetByte(s4LearnedAt);
            set => _fileEditor.SetByte(s4LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S4LearnedLevel
        {
            get => _fileEditor.GetByte(s4LearnedLevel);
            set => _fileEditor.SetByte(s4LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S4LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s4LearnedID));
            set => _fileEditor.SetByte(s4LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S5LearnedAt
        {
            get => _fileEditor.GetByte(s5LearnedAt);
            set => _fileEditor.SetByte(s5LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S5LearnedLevel
        {
            get => _fileEditor.GetByte(s5LearnedLevel);
            set => _fileEditor.SetByte(s5LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S5LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s5LearnedID));
            set => _fileEditor.SetByte(s5LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S6LearnedAt
        {
            get => _fileEditor.GetByte(s6LearnedAt);
            set => _fileEditor.SetByte(s6LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S6LearnedLevel
        {
            get => _fileEditor.GetByte(s6LearnedLevel);
            set => _fileEditor.SetByte(s6LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S6LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s6LearnedID));
            set => _fileEditor.SetByte(s6LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S7LearnedAt
        {
            get => _fileEditor.GetByte(s7LearnedAt);
            set => _fileEditor.SetByte(s7LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S7LearnedLevel
        {
            get => _fileEditor.GetByte(s7LearnedLevel);
            set => _fileEditor.SetByte(s7LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S7LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s7LearnedID));
            set => _fileEditor.SetByte(s7LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S8LearnedAt
        {
            get => _fileEditor.GetByte(s8LearnedAt);
            set => _fileEditor.SetByte(s8LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S8LearnedLevel
        {
            get => _fileEditor.GetByte(s8LearnedLevel);
            set => _fileEditor.SetByte(s8LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S8LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s8LearnedID));
            set => _fileEditor.SetByte(s8LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S9LearnedAt
        {
            get => _fileEditor.GetByte(s9LearnedAt);
            set => _fileEditor.SetByte(s9LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S9LearnedLevel
        {
            get => _fileEditor.GetByte(s9LearnedLevel);
            set => _fileEditor.SetByte(s9LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S9LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s9LearnedID));
            set => _fileEditor.SetByte(s9LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S10LearnedAt
        {
            get => _fileEditor.GetByte(s10LearnedAt);
            set => _fileEditor.SetByte(s10LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S10LearnedLevel
        {
            get => _fileEditor.GetByte(s10LearnedLevel);
            set => _fileEditor.SetByte(s10LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S10LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s10LearnedID));
            set => _fileEditor.SetByte(s10LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S11LearnedAt
        {
            get => _fileEditor.GetByte(s11LearnedAt);
            set => _fileEditor.SetByte(s11LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S11LearnedLevel
        {
            get => _fileEditor.GetByte(s11LearnedLevel);
            set => _fileEditor.SetByte(s11LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S11LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s11LearnedID));
            set => _fileEditor.SetByte(s11LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public int S12LearnedAt
        {
            get => _fileEditor.GetByte(s12LearnedAt);
            set => _fileEditor.SetByte(s12LearnedAt, (byte)value);
        }

        [BulkCopy]
        public int S12LearnedLevel
        {
            get => _fileEditor.GetByte(s12LearnedLevel);
            set => _fileEditor.SetByte(s12LearnedLevel, (byte)value);
        }

        [BulkCopy]
        public SpellValue S12LearnedID
        {
            get => new SpellValue(Scenario, _fileEditor.GetByte(s12LearnedID));
            set => _fileEditor.SetByte(s12LearnedID, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon1Special1
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon1Special1));
            set => _fileEditor.SetByte(weapon1Special1, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon1Special2
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon1Special2));
            set => _fileEditor.SetByte(weapon1Special2, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon1Special3
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon1Special3));
            set => _fileEditor.SetByte(weapon1Special3, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon2Special1
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon2Special1));
            set => _fileEditor.SetByte(weapon2Special1, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon2Special2
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon2Special2));
            set => _fileEditor.SetByte(weapon2Special2, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon2Special3
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon2Special3));
            set => _fileEditor.SetByte(weapon2Special3, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon3Special1
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon3Special1));
            set => _fileEditor.SetByte(weapon3Special1, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon3Special2
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon3Special2));
            set => _fileEditor.SetByte(weapon3Special2, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon3Special3
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon3Special3));
            set => _fileEditor.SetByte(weapon3Special3, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon4Special1
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon4Special1));
            set => _fileEditor.SetByte(weapon4Special1, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon4Special2
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon4Special2));
            set => _fileEditor.SetByte(weapon4Special2, (byte)value.Value);
        }

        [BulkCopy]
        public SpecialValue Weapon4Special3
        {
            get => new SpecialValue(Scenario, _fileEditor.GetByte(weapon4Special3));
            set => _fileEditor.SetByte(weapon4Special3, (byte)value.Value);
        }

        [BulkCopy]
        public int BaseLuck
        {
            get => _fileEditor.GetByte(baseLuck);
            set => _fileEditor.SetByte(baseLuck, (byte)value);
        }

        [BulkCopy]
        public int BaseMov
        {
            get => _fileEditor.GetByte(baseMov);
            set => _fileEditor.SetByte(baseMov, (byte)value);
        }

        [BulkCopy]
        public int BaseTurns
        {
            get => _fileEditor.GetByte(baseTurns);
            set => _fileEditor.SetByte(baseTurns, (byte)value);
        }

        [BulkCopy]
        public int BaseHPRegen
        {
            get => _fileEditor.GetByte(baseHPRegen);
            set => _fileEditor.SetByte(baseHPRegen, (byte)value);
        }

        [BulkCopy]
        public int BaseMPRegen
        {
            get => _fileEditor.GetByte(baseMPRegen);
            set => _fileEditor.SetByte(baseMPRegen, (byte)value);
        }

        [BulkCopy]
        public int EarthRes
        {
            get => _fileEditor.GetByte(earthRes);
            set => _fileEditor.SetByte(earthRes, (byte)value);
        }

        [BulkCopy]
        public int FireRes
        {
            get => _fileEditor.GetByte(fireRes);
            set => _fileEditor.SetByte(fireRes, (byte)value);
        }

        [BulkCopy]
        public int IceRes
        {
            get => _fileEditor.GetByte(iceRes);
            set => _fileEditor.SetByte(iceRes, (byte)value);
        }

        [BulkCopy]
        public int SparkRes
        {
            get => _fileEditor.GetByte(sparkRes);
            set => _fileEditor.SetByte(sparkRes, (byte)value);
        }

        [BulkCopy]
        public int WindRes
        {
            get => _fileEditor.GetByte(windRes);
            set => _fileEditor.SetByte(windRes, (byte)value);
        }

        [BulkCopy]
        public int LightRes
        {
            get => _fileEditor.GetByte(lightRes);
            set => _fileEditor.SetByte(lightRes, (byte)value);
        }

        [BulkCopy]
        public int DarkRes
        {
            get => _fileEditor.GetByte(darkRes);
            set => _fileEditor.SetByte(darkRes, (byte)value);
        }

        [BulkCopy]
        public int UnknownRes
        {
            get => _fileEditor.GetByte(unknownRes);
            set => _fileEditor.SetByte(unknownRes, (byte)value);
        }

        [BulkCopy]
        public int Slow
        {
            get => _fileEditor.GetByte(slow);
            set => _fileEditor.SetByte(slow, (byte)value);
        }

        [BulkCopy]
        public int Support
        {
            get => _fileEditor.GetByte(support);
            set => _fileEditor.SetByte(support, (byte)value);
        }

        [BulkCopy]
        public int MagicBonus
        {
            get => _fileEditor.GetByte(magicBonus);
            set => _fileEditor.SetByte(magicBonus, (byte)value);
        }

        [BulkCopy]
        public MovementTypeValue MovementType
        {
            get => new MovementTypeValue(_fileEditor.GetByte(movementType));
            set => _fileEditor.SetByte(movementType, (byte)value.Value);
        }

        [BulkCopy]
        public int WeaponEquipable1
        {
            get => _fileEditor.GetByte(weaponEquipable1);
            set => _fileEditor.SetByte(weaponEquipable1, (byte)value);
        }

        [BulkCopy]
        public int WeaponEquipable2
        {
            get => _fileEditor.GetByte(weaponEquipable2);
            set => _fileEditor.SetByte(weaponEquipable2, (byte)value);
        }

        [BulkCopy]
        public int WeaponEquipable3
        {
            get => _fileEditor.GetByte(weaponEquipable3);
            set => _fileEditor.SetByte(weaponEquipable3, (byte)value);
        }

        [BulkCopy]
        public int WeaponEquipable4
        {
            get => _fileEditor.GetByte(weaponEquipable4);
            set => _fileEditor.SetByte(weaponEquipable4, (byte)value);
        }

        [BulkCopy]
        public int AccessoryEquipable1
        {
            get => _fileEditor.GetByte(accessoryEquipable1);
            set => _fileEditor.SetByte(accessoryEquipable1, (byte)value);
        }

        [BulkCopy]
        public int AccessoryEquipable2
        {
            get => _fileEditor.GetByte(accessoryEquipable2);
            set => _fileEditor.SetByte(accessoryEquipable2, (byte)value);
        }

        [BulkCopy]
        public int AccessoryEquipable3
        {
            get => _fileEditor.GetByte(accessoryEquipable3);
            set => _fileEditor.SetByte(accessoryEquipable3, (byte)value);
        }

        [BulkCopy]
        public int AccessoryEquipable4
        {
            get => _fileEditor.GetByte(accessoryEquipable4);
            set => _fileEditor.SetByte(accessoryEquipable4, (byte)value);
        }

        public int Address => (address);
    }
}
