﻿
using SF3.Editor;
using SF3.Types;
using SF3.Values;
using System;
using System.Collections.Generic;
using static SF3.X033_X031_Editor.Forms.frmMain;

namespace SF3.X033_X031_Editor.Models.Stats
{
    public class Stats
    {
        public enum PromotionLevelType
        {
            Unpromoted = 0,
            Promotion1 = 1,
            Promotion2 = 2,
        }

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
        private int hpCurve20_99;
        private int mpPromote;
        private int mpCurve1;
        private int mpCurve5;
        private int mpCurve10;
        private int mpCurve12_15;
        private int mpCurve14_20;
        private int mpCurve17_30;
        private int mpCurve20_99;
        private int atkPromote;
        private int atkCurve1;
        private int atkCurve5;
        private int atkCurve10;
        private int atkCurve12_15;
        private int atkCurve14_20;
        private int atkCurve17_30;
        private int atkCurve20_99;
        private int defPromote;
        private int defCurve1;
        private int defCurve5;
        private int defCurve10;
        private int defCurve12_15;
        private int defCurve14_20;
        private int defCurve17_30;
        private int defCurve20_99;
        private int agiPromote;
        private int agiCurve1;
        private int agiCurve5;
        private int agiCurve10;
        private int agiCurve12_15;
        private int agiCurve14_20;
        private int agiCurve17_30;
        private int agiCurve20_99;

        private int s1LearnedAt;
        private int s1LearnedLevel;
        private int s1LearnedID;
        private int s2LearnedAt;
        private int s2LearnedLevel;
        private int s2LearnedID;
        private int s3LearnedAt;
        private int s3LearnedLevel;
        private int s3LearnedID;
        private int s4LearnedAt;
        private int s4LearnedLevel;
        private int s4LearnedID;
        private int s5LearnedAt;
        private int s5LearnedLevel;
        private int s5LearnedID;
        private int s6LearnedAt;
        private int s6LearnedLevel;
        private int s6LearnedID;
        private int s7LearnedAt;
        private int s7LearnedLevel;
        private int s7LearnedID;
        private int s8LearnedAt;
        private int s8LearnedLevel;
        private int s8LearnedID;
        private int s9LearnedAt;
        private int s9LearnedLevel;
        private int s9LearnedID;
        private int s10LearnedAt;
        private int s10LearnedLevel;
        private int s10LearnedID;
        private int s11LearnedAt;
        private int s11LearnedLevel;
        private int s11LearnedID;
        private int s12LearnedAt;
        private int s12LearnedLevel;
        private int s12LearnedID;

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

        public Stats(ScenarioType scenario, int id, string text)
        {
            Scenario = scenario;
            checkType = FileEditor.getByte(0x00000009); //if it's 0x07 we're in a x033.bin
            checkVersion2 = FileEditor.getByte(0x000000017); //to determine which version of scn2 we are using 
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
            hpCurve20_99 = start + 0x09;
            mpPromote = start + 0x0A;
            mpCurve1 = start + 0x0B;
            mpCurve5 = start + 0x0C;
            mpCurve10 = start + 0x0D;
            mpCurve12_15 = start + 0x0E;
            mpCurve14_20 = start + 0x0F;
            mpCurve17_30 = start + 0x10;
            mpCurve20_99 = start + 0x11;
            atkPromote = start + 0x12;
            atkCurve1 = start + 0x13;
            atkCurve5 = start + 0x14;
            atkCurve10 = start + 0x15;
            atkCurve12_15 = start + 0x16;
            atkCurve14_20 = start + 0x17;
            atkCurve17_30 = start + 0x18;
            atkCurve20_99 = start + 0x19;
            defPromote = start + 0x1a;
            defCurve1 = start + 0x1b;
            defCurve5 = start + 0x1c;
            defCurve10 = start + 0x1d;
            defCurve12_15 = start + 0x1e;
            defCurve14_20 = start + 0x1f;
            defCurve17_30 = start + 0x20;
            defCurve20_99 = start + 0x21;
            agiPromote = start + 0x22;
            agiCurve1 = start + 0x23;
            agiCurve5 = start + 0x24;
            agiCurve10 = start + 0x25;
            agiCurve12_15 = start + 0x26;
            agiCurve14_20 = start + 0x27;
            agiCurve17_30 = start + 0x28;
            agiCurve20_99 = start + 0x29;

            s1LearnedAt = start + 0x2a;
            s1LearnedLevel = start + 0x2b; //actually the ID
            s1LearnedID = start + 0x2c; //actaully the level. true for all the spells. i messed up
            s2LearnedAt = start + 0x2d;
            s2LearnedLevel = start + 0x2e;
            s2LearnedID = start + 0x2f;
            s3LearnedAt = start + 0x30;
            s3LearnedLevel = start + 0x31;
            s3LearnedID = start + 0x32;
            s4LearnedAt = start + 0x33;
            s4LearnedLevel = start + 0x34;
            s4LearnedID = start + 0x35;
            s5LearnedAt = start + 0x36;
            s5LearnedLevel = start + 0x37;
            s5LearnedID = start + 0x38;
            s6LearnedAt = start + 0x39;
            s6LearnedLevel = start + 0x3a;
            s6LearnedID = start + 0x3b;
            s7LearnedAt = start + 0x3c;
            s7LearnedLevel = start + 0x3d;
            s7LearnedID = start + 0x3e;
            s8LearnedAt = start + 0x3f;
            s8LearnedLevel = start + 0x40;
            s8LearnedID = start + 0x41;
            s9LearnedAt = start + 0x42;
            s9LearnedLevel = start + 0x43;
            s9LearnedID = start + 0x44;
            s10LearnedAt = start + 0x45;
            s10LearnedLevel = start + 0x46;
            s10LearnedID = start + 0x47;
            s11LearnedAt = start + 0x48;
            s11LearnedLevel = start + 0x49;
            s11LearnedID = start + 0x4a;
            s12LearnedAt = start + 0x4b;
            s12LearnedLevel = start + 0x4c;
            s12LearnedID = start + 0x4d;

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

        public ScenarioType Scenario { get; }
        public bool IsPromoted => FileEditor.getByte((int)characterClass) >= 0x20;

        public PromotionLevelType PromotionLevel
        {
            get
            {
                int chClass = FileEditor.getByte((int)characterClass);
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
                        case 5: return new ValueRange<int>(HPCurve17_30, HPCurve20_99);
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
                        case 5: return new ValueRange<int>(MPCurve17_30, MPCurve20_99);
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
                        case 5: return new ValueRange<int>(AtkCurve17_30, AtkCurve20_99);
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
                        case 5: return new ValueRange<int>(DefCurve17_30, DefCurve20_99);
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
                        case 5: return new ValueRange<int>(AgiCurve17_30, AgiCurve20_99);
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
            return (Debugs.debugs ? string.Format("{0:x}", growthValue) + " || " : "") +
                    SF3.Stats.GetAverageStatGrowthPerLevelAsPercent(growthValue);
        }

        public int ID => index;
        public string Name => name;

        public int CharacterID
        {
            get => FileEditor.getByte(character);
            set => FileEditor.setByte(character, (byte)value);
        }

        public CharacterClassValue CharacterClass
        {
            get => new CharacterClassValue(FileEditor.getByte(characterClass));
            set => FileEditor.setByte(characterClass, (byte)value.Value);
        }

        public int HPPromote
        {
            get => FileEditor.getByte(hpPromote);
            set => FileEditor.setByte(hpPromote, (byte)value);
        }

        public int HPCurve1
        {
            get => FileEditor.getByte(hpCurve1);
            set => FileEditor.setByte(hpCurve1, (byte)value);
        }

        public int HPCurve5
        {
            get => FileEditor.getByte(hpCurve5);
            set => FileEditor.setByte(hpCurve5, (byte)value);
        }

        public int HPCurve10
        {
            get => FileEditor.getByte(hpCurve10);
            set => FileEditor.setByte(hpCurve10, (byte)value);
        }

        public int HPCurve12_15
        {
            get => FileEditor.getByte(hpCurve12_15);
            set => FileEditor.setByte(hpCurve12_15, (byte)value);
        }

        public int HPCurve14_20
        {
            get => FileEditor.getByte(hpCurve14_20);
            set => FileEditor.setByte(hpCurve14_20, (byte)value);
        }

        public int HPCurve17_30
        {
            get => FileEditor.getByte(hpCurve17_30);
            set => FileEditor.setByte(hpCurve17_30, (byte)value);
        }

        public int HPCurve20_99
        {
            get => FileEditor.getByte(hpCurve20_99);
            set => FileEditor.setByte(hpCurve20_99, (byte)value);
        }

        public string HPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        public string HPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        public string HPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        public string HPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        public string HPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        public string HPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

        public int MPPromote
        {
            get => FileEditor.getByte(mpPromote);
            set => FileEditor.setByte(mpPromote, (byte)value);
        }

        public int MPCurve1
        {
            get => FileEditor.getByte(mpCurve1);
            set => FileEditor.setByte(mpCurve1, (byte)value);
        }

        public int MPCurve5
        {
            get => FileEditor.getByte(mpCurve5);
            set => FileEditor.setByte(mpCurve5, (byte)value);
        }

        public int MPCurve10
        {
            get => FileEditor.getByte(mpCurve10);
            set => FileEditor.setByte(mpCurve10, (byte)value);
        }

        public int MPCurve12_15
        {
            get => FileEditor.getByte(mpCurve12_15);
            set => FileEditor.setByte(mpCurve12_15, (byte)value);
        }

        public int MPCurve14_20
        {
            get => FileEditor.getByte(mpCurve14_20);
            set => FileEditor.setByte(mpCurve14_20, (byte)value);
        }

        public int MPCurve17_30
        {
            get => FileEditor.getByte(mpCurve17_30);
            set => FileEditor.setByte(mpCurve17_30, (byte)value);
        }

        public int MPCurve20_99
        {
            get => FileEditor.getByte(mpCurve20_99);
            set => FileEditor.setByte(mpCurve20_99, (byte)value);
        }

        public string MPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 0);
        public string MPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 1);
        public string MPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 2);
        public string MPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 3);
        public string MPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 4);
        public string MPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 5);

        public int AtkPromote
        {
            get => FileEditor.getByte(atkPromote);
            set => FileEditor.setByte(atkPromote, (byte)value);
        }

        public int AtkCurve1
        {
            get => FileEditor.getByte(atkCurve1);
            set => FileEditor.setByte(atkCurve1, (byte)value);
        }

        public int AtkCurve5
        {
            get => FileEditor.getByte(atkCurve5);
            set => FileEditor.setByte(atkCurve5, (byte)value);
        }

        public int AtkCurve10
        {
            get => FileEditor.getByte(atkCurve10);
            set => FileEditor.setByte(atkCurve10, (byte)value);
        }

        public int AtkCurve12_15
        {
            get => FileEditor.getByte(atkCurve12_15);
            set => FileEditor.setByte(atkCurve12_15, (byte)value);
        }

        public int AtkCurve14_20
        {
            get => FileEditor.getByte(atkCurve14_20);
            set => FileEditor.setByte(atkCurve14_20, (byte)value);
        }

        public int AtkCurve17_30
        {
            get => FileEditor.getByte(atkCurve17_30);
            set => FileEditor.setByte(atkCurve17_30, (byte)value);
        }

        public int AtkCurve20_99
        {
            get => FileEditor.getByte(atkCurve20_99);
            set => FileEditor.setByte(atkCurve20_99, (byte)value);
        }

        public string Atkgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 0);
        public string Atkgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 1);
        public string Atkgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 2);
        public string Atkgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 3);
        public string Atkgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 4);
        public string Atkgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 5);

        public int DefPromote
        {
            get => FileEditor.getByte(defPromote);
            set => FileEditor.setByte(defPromote, (byte)value);
        }

        public int DefCurve1
        {
            get => FileEditor.getByte(defCurve1);
            set => FileEditor.setByte(defCurve1, (byte)value);
        }

        public int DefCurve5
        {
            get => FileEditor.getByte(defCurve5);
            set => FileEditor.setByte(defCurve5, (byte)value);
        }

        public int DefCurve10
        {
            get => FileEditor.getByte(defCurve10);
            set => FileEditor.setByte(defCurve10, (byte)value);
        }

        public int DefCurve12_15
        {
            get => FileEditor.getByte(defCurve12_15);
            set => FileEditor.setByte(defCurve12_15, (byte)value);
        }

        public int DefCurve14_20
        {
            get => FileEditor.getByte(defCurve14_20);
            set => FileEditor.setByte(defCurve14_20, (byte)value);
        }

        public int DefCurve17_30
        {
            get => FileEditor.getByte(defCurve17_30);
            set => FileEditor.setByte(defCurve17_30, (byte)value);
        }

        public int DefCurve20_99
        {
            get => FileEditor.getByte(defCurve20_99);
            set => FileEditor.setByte(defCurve20_99, (byte)value);
        }

        public string Defgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 0);
        public string Defgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 1);
        public string Defgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 2);
        public string Defgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 3);
        public string Defgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 4);
        public string Defgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 5);

        public int AgiPromote
        {
            get => FileEditor.getByte(agiPromote);
            set => FileEditor.setByte(agiPromote, (byte)value);
        }

        public int AgiCurve1
        {
            get => FileEditor.getByte(agiCurve1);
            set => FileEditor.setByte(agiCurve1, (byte)value);
        }

        public int AgiCurve5
        {
            get => FileEditor.getByte(agiCurve5);
            set => FileEditor.setByte(agiCurve5, (byte)value);
        }

        public int AgiCurve10
        {
            get => FileEditor.getByte(agiCurve10);
            set => FileEditor.setByte(agiCurve10, (byte)value);
        }

        public int AgiCurve12_15
        {
            get => FileEditor.getByte(agiCurve12_15);
            set => FileEditor.setByte(agiCurve12_15, (byte)value);
        }

        public int AgiCurve14_20
        {
            get => FileEditor.getByte(agiCurve14_20);
            set => FileEditor.setByte(agiCurve14_20, (byte)value);
        }

        public int AgiCurve17_30
        {
            get => FileEditor.getByte(agiCurve17_30);
            set => FileEditor.setByte(agiCurve17_30, (byte)value);
        }

        public int AgiCurve20_99
        {
            get => FileEditor.getByte(agiCurve20_99);
            set => FileEditor.setByte(agiCurve20_99, (byte)value);
        }

        public string Agigroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 0);
        public string Agigroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 1);
        public string Agigroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 2);
        public string Agigroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 3);
        public string Agigroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 4);
        public string Agigroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 5);

        public int S1LearnedAt
        {
            get => FileEditor.getByte(s1LearnedAt);
            set => FileEditor.setByte(s1LearnedAt, (byte)value);
        }

        public int S1LearnedID
        {
            get => FileEditor.getByte(s1LearnedID);
            set => FileEditor.setByte(s1LearnedID, (byte)value);
        }

        public int S1LearnedLevel
        {
            get => FileEditor.getByte(s1LearnedLevel);
            set => FileEditor.setByte(s1LearnedLevel, (byte)value);
        }

        public int S2LearnedAt
        {
            get => FileEditor.getByte(s2LearnedAt);
            set => FileEditor.setByte(s2LearnedAt, (byte)value);
        }

        public int S2LearnedID
        {
            get => FileEditor.getByte(s2LearnedID);
            set => FileEditor.setByte(s2LearnedID, (byte)value);
        }

        public int S2LearnedLevel
        {
            get => FileEditor.getByte(s2LearnedLevel);
            set => FileEditor.setByte(s2LearnedLevel, (byte)value);
        }

        public int S3LearnedAt
        {
            get => FileEditor.getByte(s3LearnedAt);
            set => FileEditor.setByte(s3LearnedAt, (byte)value);
        }

        public int S3LearnedID
        {
            get => FileEditor.getByte(s3LearnedID);
            set => FileEditor.setByte(s3LearnedID, (byte)value);
        }

        public int S3LearnedLevel
        {
            get => FileEditor.getByte(s3LearnedLevel);
            set => FileEditor.setByte(s3LearnedLevel, (byte)value);
        }

        public int S4LearnedAt
        {
            get => FileEditor.getByte(s4LearnedAt);
            set => FileEditor.setByte(s4LearnedAt, (byte)value);
        }

        public int S4LearnedID
        {
            get => FileEditor.getByte(s4LearnedID);
            set => FileEditor.setByte(s4LearnedID, (byte)value);
        }

        public int S4LearnedLevel
        {
            get => FileEditor.getByte(s4LearnedLevel);
            set => FileEditor.setByte(s4LearnedLevel, (byte)value);
        }

        public int S5LearnedAt
        {
            get => FileEditor.getByte(s5LearnedAt);
            set => FileEditor.setByte(s5LearnedAt, (byte)value);
        }

        public int S5LearnedID
        {
            get => FileEditor.getByte(s5LearnedID);
            set => FileEditor.setByte(s5LearnedID, (byte)value);
        }

        public int S5LearnedLevel
        {
            get => FileEditor.getByte(s5LearnedLevel);
            set => FileEditor.setByte(s5LearnedLevel, (byte)value);
        }

        public int S6LearnedAt
        {
            get => FileEditor.getByte(s6LearnedAt);
            set => FileEditor.setByte(s6LearnedAt, (byte)value);
        }

        public int S6LearnedID
        {
            get => FileEditor.getByte(s6LearnedID);
            set => FileEditor.setByte(s6LearnedID, (byte)value);
        }

        public int S6LearnedLevel
        {
            get => FileEditor.getByte(s6LearnedLevel);
            set => FileEditor.setByte(s6LearnedLevel, (byte)value);
        }

        public int S7LearnedAt
        {
            get => FileEditor.getByte(s7LearnedAt);
            set => FileEditor.setByte(s7LearnedAt, (byte)value);
        }

        public int S7LearnedID
        {
            get => FileEditor.getByte(s7LearnedID);
            set => FileEditor.setByte(s7LearnedID, (byte)value);
        }

        public int S7LearnedLevel
        {
            get => FileEditor.getByte(s7LearnedLevel);
            set => FileEditor.setByte(s7LearnedLevel, (byte)value);
        }

        public int S8LearnedAt
        {
            get => FileEditor.getByte(s8LearnedAt);
            set => FileEditor.setByte(s8LearnedAt, (byte)value);
        }

        public int S8LearnedID
        {
            get => FileEditor.getByte(s8LearnedID);
            set => FileEditor.setByte(s8LearnedID, (byte)value);
        }

        public int S8LearnedLevel
        {
            get => FileEditor.getByte(s8LearnedLevel);
            set => FileEditor.setByte(s8LearnedLevel, (byte)value);
        }

        public int S9LearnedAt
        {
            get => FileEditor.getByte(s9LearnedAt);
            set => FileEditor.setByte(s9LearnedAt, (byte)value);
        }

        public int S9LearnedID
        {
            get => FileEditor.getByte(s9LearnedID);
            set => FileEditor.setByte(s9LearnedID, (byte)value);
        }

        public int S9LearnedLevel
        {
            get => FileEditor.getByte(s9LearnedLevel);
            set => FileEditor.setByte(s9LearnedLevel, (byte)value);
        }

        public int S10LearnedAt
        {
            get => FileEditor.getByte(s10LearnedAt);
            set => FileEditor.setByte(s10LearnedAt, (byte)value);
        }

        public int S10LearnedID
        {
            get => FileEditor.getByte(s10LearnedID);
            set => FileEditor.setByte(s10LearnedID, (byte)value);
        }

        public int S10LearnedLevel
        {
            get => FileEditor.getByte(s10LearnedLevel);
            set => FileEditor.setByte(s10LearnedLevel, (byte)value);
        }

        public int S11LearnedAt
        {
            get => FileEditor.getByte(s11LearnedAt);
            set => FileEditor.setByte(s11LearnedAt, (byte)value);
        }

        public int S11LearnedID
        {
            get => FileEditor.getByte(s11LearnedID);
            set => FileEditor.setByte(s11LearnedID, (byte)value);
        }

        public int S11LearnedLevel
        {
            get => FileEditor.getByte(s11LearnedLevel);
            set => FileEditor.setByte(s11LearnedLevel, (byte)value);
        }

        public int S12LearnedAt
        {
            get => FileEditor.getByte(s12LearnedAt);
            set => FileEditor.setByte(s12LearnedAt, (byte)value);
        }

        public int S12LearnedID
        {
            get => FileEditor.getByte(s12LearnedID);
            set => FileEditor.setByte(s12LearnedID, (byte)value);
        }

        public int S12LearnedLevel
        {
            get => FileEditor.getByte(s12LearnedLevel);
            set => FileEditor.setByte(s12LearnedLevel, (byte)value);
        }

        public int Weapon1Special1
        {
            get => FileEditor.getByte(weapon1Special1);
            set => FileEditor.setByte(weapon1Special1, (byte)value);
        }

        public int Weapon1Special2
        {
            get => FileEditor.getByte(weapon1Special2);
            set => FileEditor.setByte(weapon1Special2, (byte)value);
        }

        public int Weapon1Special3
        {
            get => FileEditor.getByte(weapon1Special3);
            set => FileEditor.setByte(weapon1Special3, (byte)value);
        }

        public int Weapon2Special1
        {
            get => FileEditor.getByte(weapon2Special1);
            set => FileEditor.setByte(weapon2Special1, (byte)value);
        }

        public int Weapon2Special2
        {
            get => FileEditor.getByte(weapon2Special2);
            set => FileEditor.setByte(weapon2Special2, (byte)value);
        }

        public int Weapon2Special3
        {
            get => FileEditor.getByte(weapon2Special3);
            set => FileEditor.setByte(weapon2Special3, (byte)value);
        }

        public int Weapon3Special1
        {
            get => FileEditor.getByte(weapon3Special1);
            set => FileEditor.setByte(weapon3Special1, (byte)value);
        }

        public int Weapon3Special2
        {
            get => FileEditor.getByte(weapon3Special2);
            set => FileEditor.setByte(weapon3Special2, (byte)value);
        }

        public int Weapon3Special3
        {
            get => FileEditor.getByte(weapon3Special3);
            set => FileEditor.setByte(weapon3Special3, (byte)value);
        }

        public int Weapon4Special1
        {
            get => FileEditor.getByte(weapon4Special1);
            set => FileEditor.setByte(weapon4Special1, (byte)value);
        }

        public int Weapon4Special2
        {
            get => FileEditor.getByte(weapon4Special2);
            set => FileEditor.setByte(weapon4Special2, (byte)value);
        }

        public int Weapon4Special3
        {
            get => FileEditor.getByte(weapon4Special3);
            set => FileEditor.setByte(weapon4Special3, (byte)value);
        }

        public int BaseLuck
        {
            get => FileEditor.getByte(baseLuck);
            set => FileEditor.setByte(baseLuck, (byte)value);
        }

        public int BaseMov
        {
            get => FileEditor.getByte(baseMov);
            set => FileEditor.setByte(baseMov, (byte)value);
        }

        public int BaseTurns
        {
            get => FileEditor.getByte(baseTurns);
            set => FileEditor.setByte(baseTurns, (byte)value);
        }

        public int BaseHPRegen
        {
            get => FileEditor.getByte(baseHPRegen);
            set => FileEditor.setByte(baseHPRegen, (byte)value);
        }

        public int BaseMPRegen
        {
            get => FileEditor.getByte(baseMPRegen);
            set => FileEditor.setByte(baseMPRegen, (byte)value);
        }

        public int EarthRes
        {
            get => FileEditor.getByte(earthRes);
            set => FileEditor.setByte(earthRes, (byte)value);
        }

        public int FireRes
        {
            get => FileEditor.getByte(fireRes);
            set => FileEditor.setByte(fireRes, (byte)value);
        }

        public int IceRes
        {
            get => FileEditor.getByte(iceRes);
            set => FileEditor.setByte(iceRes, (byte)value);
        }

        public int SparkRes
        {
            get => FileEditor.getByte(sparkRes);
            set => FileEditor.setByte(sparkRes, (byte)value);
        }

        public int WindRes
        {
            get => FileEditor.getByte(windRes);
            set => FileEditor.setByte(windRes, (byte)value);
        }

        public int LightRes
        {
            get => FileEditor.getByte(lightRes);
            set => FileEditor.setByte(lightRes, (byte)value);
        }

        public int DarkRes
        {
            get => FileEditor.getByte(darkRes);
            set => FileEditor.setByte(darkRes, (byte)value);
        }

        public int UnknownRes
        {
            get => FileEditor.getByte(unknownRes);
            set => FileEditor.setByte(unknownRes, (byte)value);
        }

        public int Slow
        {
            get => FileEditor.getByte(slow);
            set => FileEditor.setByte(slow, (byte)value);
        }

        public int Support
        {
            get => FileEditor.getByte(support);
            set => FileEditor.setByte(support, (byte)value);
        }

        public int MagicBonus
        {
            get => FileEditor.getByte(magicBonus);
            set => FileEditor.setByte(magicBonus, (byte)value);
        }

        public int MovementType
        {
            get => FileEditor.getByte(movementType);
            set => FileEditor.setByte(movementType, (byte)value);
        }

        public int WeaponEquipable1
        {
            get => FileEditor.getByte(weaponEquipable1);
            set => FileEditor.setByte(weaponEquipable1, (byte)value);
        }

        public int WeaponEquipable2
        {
            get => FileEditor.getByte(weaponEquipable2);
            set => FileEditor.setByte(weaponEquipable2, (byte)value);
        }

        public int WeaponEquipable3
        {
            get => FileEditor.getByte(weaponEquipable3);
            set => FileEditor.setByte(weaponEquipable3, (byte)value);
        }

        public int WeaponEquipable4
        {
            get => FileEditor.getByte(weaponEquipable4);
            set => FileEditor.setByte(weaponEquipable4, (byte)value);
        }

        public int AccessoryEquipable1
        {
            get => FileEditor.getByte(accessoryEquipable1);
            set => FileEditor.setByte(accessoryEquipable1, (byte)value);
        }

        public int AccessoryEquipable2
        {
            get => FileEditor.getByte(accessoryEquipable2);
            set => FileEditor.setByte(accessoryEquipable2, (byte)value);
        }

        public int AccessoryEquipable3
        {
            get => FileEditor.getByte(accessoryEquipable3);
            set => FileEditor.setByte(accessoryEquipable3, (byte)value);
        }

        public int AccessoryEquipable4
        {
            get => FileEditor.getByte(accessoryEquipable4);
            set => FileEditor.setByte(accessoryEquipable4, (byte)value);
        }

        public int Address => (address);
    }
}
