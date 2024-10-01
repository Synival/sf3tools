
using SF3.Editor;
using SF3.Values;
using System;
using static SF3.X033_X031_Editor.Forms.frmMain;

namespace SF3.X033_X031_Editor.Models.Items
{
    public class Item
    {
        //starting stat table
        private int character;
        private int characterClass;
        private int hpPromote;
        private int hpStart;
        private int hpCurve6;
        private int hpCurve11;
        private int hpCurve13;
        private int hpCurve15;
        private int hpCurve17;
        private int hpCurve20;
        private int mpPromote;
        private int mpStart;
        private int mpCurve6;
        private int mpCurve11;
        private int mpCurve13;
        private int mpCurve15;
        private int mpCurve17;
        private int mpCurve20;
        private int atkPromote;
        private int atkStart;
        private int atkCurve6;
        private int atkCurve11;
        private int atkCurve13;
        private int atkCurve15;
        private int atkCurve17;
        private int atkCurve20;
        private int defPromote;
        private int defStart;
        private int defCurve6;
        private int defCurve11;
        private int defCurve13;
        private int defCurve15;
        private int defCurve17;
        private int defCurve20;
        private int agiPromote;
        private int agiStart;
        private int agiCurve6;
        private int agiCurve11;
        private int agiCurve13;
        private int agiCurve15;
        private int agiCurve17;
        private int agiCurve20;

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

        int value;
        int otherValue;
        double percent = 0;
        int something = 127;
        String result = "";
        //public static int[] arrayOne = {1 ,2, 3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,17,18,19,120,121,122,123,124,125,126,127,128,127,126,125,124,123,122,121,120,119,118,117,116,115,114,113,112,111,110,109,108,107,106,105,104,103,102,101,100,99, 98,97,96,95,94,93,92,91,90,89,88,87,86,85,84,83,82,81,80,79,78,77,76,75,74,73,72,71,70,69,68,67,66,65,64,63,62,61,60,59,58,57,56,55,54,53,52,51,50,49,48,47,46,45,44,43,42,41,40,39,38,37,36,35,34,33,32,31,30,29,28,27,26,25,24,23,22,21,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1 };
        //public static double[] arrayMath = {0, 0, 1, 2, 3,  6, 10, 15, 21, 28, 36, 45, 55, 66, 78, 91, 105, 120, 136, 153, 171, 190, 210, 231, 253, 276, 300, 325, 351, 378, 406, 435, 465, 496, 528, 561, 595,630, 666, 703, 741, 780, 820, 861, 903, 946, 990, 1035, 1081, 1128, 1176, 1225, 1275, 1326, 1378, 1431, 1485, 1540, 1596, 1653, 1711, 1770, 1830, 1891, 1953, 2016, 2080};
        //public static double[] arrayMath2 = { 0, 0, 1, 2 ,4,7,11,16,22,29,37,46, 56, 67, 79, 92, 106, 121, 137, 154, 172, 191, 211, 232, 254, 277, 301, 326, 352, 379, 407, 436, 466, 497, 529, 562, 596, 631, 667, 704, 742, 781, 821, 862, 904, 947, 991, 1036, 1082, 1129, 1177, 1226, 1276, 1327, 1379, 1432, 1486, 1541, 1597, 1654, 1712, 1771, 1831, 1892, 1954, 2017, 2081, 2146, 2212, 2279, 2347, 2416, 2486, 2557, 2629, 2702, 2776, 2851, 2927, 3004, 3082, 3161, 3241, 3322, 3404, 3487, 3571, 3656, 3742, 3829, 3917, 4006, 4096, 4187, 4279, 4372, 4466, 4561, 4657, 4754, 4852, 4951, 5051, 5152, 5254, 5357, 5461, 5566, 5672, 5779, 5887, 5996, 6106, 6217, 6329, 6442, 6556, 6671, 6787, 6904, 7022, 7141, 7261, 7382, 7504, 7627, 7751, 7876, 8002, 8129, 8257, 8384,8510, 8635,8759, 8882, 9004, 9125, 9245, 9364, 9482, 9599, 9715, 9830, 9944, 10057, 10169, 10280, 10390, 10499, 10607, 10714, 10820, 10925, 11029, 11132, 11234, 11335, 11435, 11534, 11632, 11729, 11825, 11920, 12014, 12107, 12199, 12290, 12380, 12469, 12557, 12644, 12730, 12815, 12899, 12982, 13064, 13145, 13225, 13304, 13382, 13459, 13535, 13610, 13684, 13757, 13829, 13900, 13970, 14039, 14107, 14174, 14240, 14305, 14369, 14432, 14494, 14555, 14615, 14674, 14732, 14789, 14845, 14900, 14954, 15007, 15059, 15110, 15160, 15209, 15257, 15304, 15350, 15395, 15439, 15482, 15524, 15565, 15605, 15644, 15682, 15719, 15757, 15794, 15830, 15865, 15899, 15932, 15964, 15995, 16025, 16054, 16082, 16109, 16135, 16160, 16184, 16207, 16229, 16250, 16270, 16289, 16307, 16324, 16340, 16355, 16369, 16382, 16394, 16405, 16415, 16424, 16432, 16439, 16445, 16450, 16454, 16457, 16459, 16460};
        /*public static double[] arrayMath ={ 0,
0
,3
,6
,10
,15
,21
,28
,36
,45
,55
,66
,78
,91
,105
,120
,136
,153
,171
,190
,210
,231
,253
,276
,300
,325
,351
,378
,406
,435
,465
,496
,528
,561
,595
,630
,666
,703
,741
,780
,820
,861
,903
,946
,990
,1035
,1081
,1128
,1176
,1225
,1275
,1326
,1378
,1431
,1485
,1540
,1596
,1653
,1711
,1770
,1830
,1891
,1953
,2016
,2080
,2145
,2211
,2278
,2346
,2415
,2485
,2556
,2628
,2701
,2775
,2850
,2926
,3003
,3081
,3160
,3240
,3321
,3403
,3486
,3570
,3655
,3741
,3828
,3916
,4005
,4095
,4186
,4278
,4371
,4465
,4560
,4656
,4753
,4851
,4950
,5050
,5151
,5253
,5356
,5460
,5565
,5671
,5778
,5886
,5995
,6105
,6216
,6328
,6441
,6555
,6670
,6786
,6903
,7021
,7140
,7260
,7381
,7503
,7626
,7750
,7875
,8001 //0x7e
,8128 //8065 is 50% hm. 0x7f
,8254 //0x80
,8379
,8503
,8626
,8748
,8869
,8989
,9108
,9226
,9343
,9459
,9574
,9688
,9801
,9913
,10024
,10134
,10243
,10351
,10458
,10564
,10669
,10773
,10876
,10978
,11079
,11179
,11278
,11376
,11473
,11569
,11664
,11758
,11851
,11943
,12034
,12124
,12213
,12301
,12388
,12474
,12559
,12643
,12726
,12808
,12889
,12969
,13048
,13126
,13203
,13279
,13354
,13428
,13501
,13573
,13644
,13714
,13783
,13851
,13918
,13984
,14049
,14113
,14176
,14238
,14299
,14359
,14418
,14476
,14533
,14589
,14644
,14698
,14751
,14803
,14854
,14904
,14953
,15001
,15048
,15094
,15139
,15183
,15226
,15268
,15309
,15349
,15388
,15426
,15463
,15499
,15534
,15568
,15601
,15633
,15664
,15694
,15723
,15751
,15778
,15804
,15829
,15853
,15876
,15898
,15919
,15939
,15958
,15976
,15993
,16009
,16024
,16038
,16051
,16063
,16074
,16084
,16093
,16101
,16108
,16114
,16119
,16123
,16126
,16128
,16129 };*/

        //public static double[] arrayMath3 = { 0, 0, 2, arrayMath3[2]+ 3};
        public static double[] arrayMath ={0, 0
,0
,3
,7
,12
,18
,25
,33
,42
,52
,63
,75
,88
,102
,117
,133
,150
,168
,187
,207
,228
,250
,273
,297
,322
,348
,375
,403
,432
,462
,493
,525
,558
,592
,627
,663
,700
,738
,777
,817
,858
,900
,943
,987
,1032
,1078
,1125
,1173
,1222
,1272
,1323
,1375
,1428
,1482
,1537
,1593
,1650
,1708
,1767
,1827
,1888
,1950
,2013
,2077
,2142
,2208
,2275
,2343
,2412
,2482
,2553
,2625
,2698
,2772
,2847
,2923
,3000
,3078
,3157
,3237
,3318
,3400
,3483
,3567
,3652
,3738
,3825
,3913
,4002
,4092
,4183
,4275
,4368
,4462
,4557
,4653
,4750
,4848
,4947
,5047
,5148
,5250
,5353
,5457
,5562
,5668
,5775
,5883
,5992
,6102
,6213
,6325
,6438
,6552
,6667
,6783
,6900
,7018
,7137
,7257
,7378
,7500
,7623
,7747
,7872
,7998
,8125
,8252
,8378
,8503
,8627
,8750
,8872
,8993
,9113
,9232
,9350
,9467
,9583
,9698
,9812
,9925
,10037
,10148
,10258
,10367
,10475
,10582
,10688
,10793
,10897
,11000
,11102
,11203
,11303
,11402
,11500
,11597
,11693
,11788
,11882
,11975
,12067
,12158
,12248
,12337
,12425
,12512
,12598
,12683
,12767
,12850
,12932
,13013
,13093
,13172
,13250
,13327
,13403
,13478
,13552
,13625
,13697
,13768
,13838
,13907
,13975
,14042
,14108
,14173
,14237
,14300
,14362
,14423
,14483
,14542
,14600
,14657
,14713
,14768
,14822
,14875
,14927
,14978
,15028
,15077
,15125
,15172
,15218
,15263
,15307
,15350
,15392
,15433
,15473
,15512
,15550
,15587
,15623
,15658
,15692
,15725
,15757
,15788
,15818
,15847
,15875
,15902
,15928
,15953
,15977
,16000
,16022
,16043
,16063
,16082
,16100
,16117
,16133
,16148
,16162
,16175
,16187
,16198
,16208
,16217
,16225
,16232
,16238
,16243
,16247
,16250
,16252
,16253 };

        int valueNumber = 16253; //16129; //16384

        //int[] arrayTwo = {arrayOne[0] =  1 };
        //arrayOne[0] = {2} ;

        private int address;
        private int offset;
        private int checkType;
        private int checkVersion2;

        private int index;
        private string name;

        public Item(int id, string text)
        {
            checkType = FileEditor.getByte(0x00000009); //if it's 0x07 we're in a x033.bin
            checkVersion2 = FileEditor.getByte(0x000000017); //to determine which version of scn2 we are using 
            //X031.BIN is during combat
            //X033.BIN is out of combat

            if (Globals.scenario == 1)
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
            else if (Globals.scenario == 2)
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
            else if (Globals.scenario == 3)
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
            else if (Globals.scenario == 4)
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
            hpStart = start + 0x03;
            hpCurve6 = start + 0x04;
            hpCurve11 = start + 0x05;
            hpCurve13 = start + 0x06;
            hpCurve15 = start + 0x07;
            hpCurve17 = start + 0x08;
            hpCurve20 = start + 0x09;
            mpPromote = start + 0x0A;
            mpStart = start + 0x0B;
            mpCurve6 = start + 0x0C;
            mpCurve11 = start + 0x0D;
            mpCurve13 = start + 0x0E;
            mpCurve15 = start + 0x0F;
            mpCurve17 = start + 0x10;
            mpCurve20 = start + 0x11;
            atkPromote = start + 0x12;
            atkStart = start + 0x13;
            atkCurve6 = start + 0x14;
            atkCurve11 = start + 0x15;
            atkCurve13 = start + 0x16;
            atkCurve15 = start + 0x17;
            atkCurve17 = start + 0x18;
            atkCurve20 = start + 0x19;
            defPromote = start + 0x1a;
            defStart = start + 0x1b;
            defCurve6 = start + 0x1c;
            defCurve11 = start + 0x1d;
            defCurve13 = start + 0x1e;
            defCurve15 = start + 0x1f;
            defCurve17 = start + 0x20;
            defCurve20 = start + 0x21;
            agiPromote = start + 0x22;
            agiStart = start + 0x23;
            agiCurve6 = start + 0x24;
            agiCurve11 = start + 0x25;
            agiCurve13 = start + 0x26;
            agiCurve15 = start + 0x27;
            agiCurve17 = start + 0x28;
            agiCurve20 = start + 0x29;

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

        public int ID => index;
        public string Name => name;

        public int Character
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
        public int HPStart
        {
            get => FileEditor.getByte(hpStart);
            set => FileEditor.setByte(hpStart, (byte)value);
        }
        public int HPCurve6
        {
            get => FileEditor.getByte(hpCurve6);
            set => FileEditor.setByte(hpCurve6, (byte)value);
        }
        public int HPCurve11
        {
            get => FileEditor.getByte(hpCurve11);
            set => FileEditor.setByte(hpCurve11, (byte)value);
        }
        public int HPCurve13
        {
            get => FileEditor.getByte(hpCurve13);
            set => FileEditor.setByte(hpCurve13, (byte)value);
        }
        public int HPCurve15
        {
            get => FileEditor.getByte(hpCurve15);
            set => FileEditor.setByte(hpCurve15, (byte)value);
        }
        public int HPCurve17
        {
            get => FileEditor.getByte(hpCurve17);
            set => FileEditor.setByte(hpCurve17, (byte)value);
        }
        public int HPCurve20
        {
            get => FileEditor.getByte(hpCurve20);
            set => FileEditor.setByte(hpCurve20, (byte)value);
        }

        public bool IsPromoted => FileEditor.getByte((int)characterClass) >= 0x20;

        public string HPgroup1
        {
            get
            {
                if (!IsPromoted)
                {
                    //return ((FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6);
                    value = ((FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6);
                    otherValue = value;
                }
                else
                {
                    //return (FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6;
                    value = ((FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6);
                    otherValue = value;
                    //return otherValue / 256 * 100 + "%";
                }

                //value = value & 0xf00;
                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;
                //percent = arrayMath[0x80] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";

                /*
                percent = 0;
                something = 128;
                

                for (int i = 0; i < (otherValue - 1); i++)
                {
                    if (i > 0x80)
                    {
                        percent = percent + something;
                        something = something - 1;
                    }
                    else
                    {
                        percent = percent + i;
                    }
                }*/

                //return string.Format("{0:0.##}", ((arrayMath[otherValue] / 16384) + "%"));
                //return otherValue + "";
                //return arrayMath[otherValue] + "";
                //return percent + "%";
                //return string.Format("{0:0.##}", ((percent = percent / 16384) + (value % 15)) * 100) + "%";
            }
        }

        public string HPgroup2
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1 ;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                    //* 0x10 / 0x28 is meant to similate multiplying by 0x66666667
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue /256 * 100 + "%";

                    value = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string HPgroup3
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                /*value = value & 0xf00;
                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / 16384;
                return string.Format("{0:0.##}", (percent + (value % 15)) * 100) + "%";*/

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string HPgroup4
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";
                    value = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string HPgroup5
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x300;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x300;
                    otherValue = value;
                    //*0x100 / 0x300 is to simulate *0x55555556
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x228 >> 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x280 >> 2;
                    //*0x100 / 0x280 is to simulate *0x66666667
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string HPgroup6
        {
            get
            {
                if (!IsPromoted)
                {
                    value = ((FileEditor.getByte(hpCurve20) - FileEditor.getByte(hpCurve17)) << 8) * 0x100 / 0x340 >> 2;
                    otherValue = value;
                    //*0x100 / 0x340 is to simulate *0x4ec4ec4f
                }
                else
                {
                    value = ((FileEditor.getByte(hpCurve20) - FileEditor.getByte(hpCurve17)) << 8) * 0x100 / 0x228 >> 5;
                    otherValue = value;
                    //*0x100 / 0x228 is to simulate *0x76b981d8
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public int MPPromote
        {
            get => FileEditor.getByte(mpPromote);
            set => FileEditor.setByte(mpPromote, (byte)value);
        }
        public int MPStart
        {
            get => FileEditor.getByte(mpStart);
            set => FileEditor.setByte(mpStart, (byte)value);
        }
        public int MPCurve6
        {
            get => FileEditor.getByte(mpCurve6);
            set => FileEditor.setByte(mpCurve6, (byte)value);
        }
        public int MPCurve11
        {
            get => FileEditor.getByte(mpCurve11);
            set => FileEditor.setByte(mpCurve11, (byte)value);
        }
        public int MPCurve13
        {
            get => FileEditor.getByte(mpCurve13);
            set => FileEditor.setByte(mpCurve13, (byte)value);
        }
        public int MPCurve15
        {
            get => FileEditor.getByte(mpCurve15);
            set => FileEditor.setByte(mpCurve15, (byte)value);
        }
        public int MPCurve17
        {
            get => FileEditor.getByte(mpCurve17);
            set => FileEditor.setByte(mpCurve17, (byte)value);
        }
        public int MPCurve20
        {
            get => FileEditor.getByte(mpCurve20);
            set => FileEditor.setByte(mpCurve20, (byte)value);
        }

        public string MPgroup1
        {
            get
            {
                if (!IsPromoted)
                {
                    //return ((FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6);
                    value = ((FileEditor.getByte(mpCurve6) - FileEditor.getByte(mpStart)) << 6); ;
                    otherValue = value;
                }
                else
                {
                    //return (FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6;
                    value = ((FileEditor.getByte(mpCurve6) - FileEditor.getByte(mpStart)) << 6);
                    otherValue = value;
                    //return otherValue / 256 * 100 + "%";
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string MPgroup2
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1 ;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(mpCurve11) - FileEditor.getByte(mpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                    //* 0x10 / 0x28 is meant to similate multiplying by 0x66666667
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue /256 * 100 + "%";

                    value = ((FileEditor.getByte(mpCurve11) - FileEditor.getByte(mpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string MPgroup3
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(mpCurve13) - FileEditor.getByte(mpCurve11)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(mpCurve13) - FileEditor.getByte(mpCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string MPgroup4
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";
                    value = ((FileEditor.getByte(mpCurve15) - FileEditor.getByte(mpCurve13)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(mpCurve15) - FileEditor.getByte(mpCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string MPgroup5
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x300;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(mpCurve17) - FileEditor.getByte(mpCurve15)) << 8) * 0x100 / 0x300;
                    otherValue = value;
                    //*0x100 / 0x300 is to simulate *0x55555556
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x228 >> 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(mpCurve17) - FileEditor.getByte(mpCurve15)) << 8) * 0x100 / 0x280 >> 2;
                    otherValue = value;
                    //*0x100 / 0x228 is to simulate *0x76b981d8
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string MPgroup6
        {
            get
            {
                if (!IsPromoted)
                {
                    value = ((FileEditor.getByte(mpCurve20) - FileEditor.getByte(mpCurve17)) << 8) * 0x100 / 0x340 >> 2;
                    otherValue = value;
                    //*0x100 / 0x340 is to simulate *0x4ec4ec4f
                }
                else
                {
                    value = ((FileEditor.getByte(mpCurve20) - FileEditor.getByte(mpCurve17)) << 8) * 0x100 / 0x228 >> 5;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public int AtkPromote
        {
            get => FileEditor.getByte(atkPromote);
            set => FileEditor.setByte(atkPromote, (byte)value);
        }
        public int AtkStart
        {
            get => FileEditor.getByte(atkStart);
            set => FileEditor.setByte(atkStart, (byte)value);
        }
        public int AtkCurve6
        {
            get => FileEditor.getByte(atkCurve6);
            set => FileEditor.setByte(atkCurve6, (byte)value);
        }
        public int AtkCurve11
        {
            get => FileEditor.getByte(atkCurve11);
            set => FileEditor.setByte(atkCurve11, (byte)value);
        }
        public int AtkCurve13
        {
            get => FileEditor.getByte(atkCurve13);
            set => FileEditor.setByte(atkCurve13, (byte)value);
        }
        public int AtkCurve15
        {
            get => FileEditor.getByte(atkCurve15);
            set => FileEditor.setByte(atkCurve15, (byte)value);
        }
        public int AtkCurve17
        {
            get => FileEditor.getByte(atkCurve17);
            set => FileEditor.setByte(atkCurve17, (byte)value);
        }
        public int AtkCurve20
        {
            get => FileEditor.getByte(atkCurve20);
            set => FileEditor.setByte(atkCurve20, (byte)value);
        }

        public string Atkgroup1
        {
            get
            {
                if (!IsPromoted)
                {
                    //return ((FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6);
                    value = ((FileEditor.getByte(atkCurve6) - FileEditor.getByte(atkStart)) << 6); ;
                    otherValue = value;
                }
                else
                {
                    //return (FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6;
                    value = ((FileEditor.getByte(atkCurve6) - FileEditor.getByte(atkStart)) << 6);
                    otherValue = value;
                    //return otherValue / 256 * 100 + "%";
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Atkgroup2
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1 ;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(atkCurve11) - FileEditor.getByte(atkCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                    //* 0x10 / 0x28 is meant to similate multiplying by 0x66666667
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue /256 * 100 + "%";

                    value = ((FileEditor.getByte(atkCurve11) - FileEditor.getByte(atkCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Atkgroup3
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(atkCurve13) - FileEditor.getByte(atkCurve11)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(atkCurve13) - FileEditor.getByte(atkCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Atkgroup4
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";
                    value = ((FileEditor.getByte(atkCurve15) - FileEditor.getByte(atkCurve13)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(atkCurve15) - FileEditor.getByte(atkCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Atkgroup5
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x300;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(atkCurve17) - FileEditor.getByte(atkCurve15)) << 8) * 0x100 / 0x300;
                    otherValue = value;
                    //*0x100 / 0x300 is to simulate *0x55555556
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x228 >> 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(atkCurve17) - FileEditor.getByte(atkCurve15)) << 8) * 0x100 / 0x280 >> 2;
                    otherValue = value;
                    //*0x100 / 0x228 is to simulate *0x76b981d8
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Atkgroup6
        {
            get
            {
                if (!IsPromoted)
                {
                    value = ((FileEditor.getByte(atkCurve20) - FileEditor.getByte(atkCurve17)) << 8) * 0x100 / 0x340 >> 2;
                    otherValue = value;
                    //*0x100 / 0x340 is to simulate *0x4ec4ec4f
                }
                else
                {
                    value = ((FileEditor.getByte(atkCurve20) - FileEditor.getByte(atkCurve17)) << 8) * 0x100 / 0x228 >> 5;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public int DefPromote
        {
            get => FileEditor.getByte(defPromote);
            set => FileEditor.setByte(defPromote, (byte)value);
        }
        public int DefStart
        {
            get => FileEditor.getByte(defStart);
            set => FileEditor.setByte(defStart, (byte)value);
        }
        public int DefCurve6
        {
            get => FileEditor.getByte(defCurve6);
            set => FileEditor.setByte(defCurve6, (byte)value);
        }
        public int DefCurve11
        {
            get => FileEditor.getByte(defCurve11);
            set => FileEditor.setByte(defCurve11, (byte)value);
        }
        public int DefCurve13
        {
            get => FileEditor.getByte(defCurve13);
            set => FileEditor.setByte(defCurve13, (byte)value);
        }
        public int DefCurve15
        {
            get => FileEditor.getByte(defCurve15);
            set => FileEditor.setByte(defCurve15, (byte)value);
        }
        public int DefCurve17
        {
            get => FileEditor.getByte(defCurve17);
            set => FileEditor.setByte(defCurve17, (byte)value);
        }
        public int DefCurve20
        {
            get => FileEditor.getByte(defCurve20);
            set => FileEditor.setByte(defCurve20, (byte)value);
        }

        public string Defgroup1
        {
            get
            {
                if (!IsPromoted)
                {
                    //return ((FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6);
                    value = ((FileEditor.getByte(defCurve6) - FileEditor.getByte(defStart)) << 6); ;
                    otherValue = value;
                }
                else
                {
                    //return (FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6;
                    value = ((FileEditor.getByte(defCurve6) - FileEditor.getByte(defStart)) << 6);
                    otherValue = value;
                    //return otherValue / 256 * 100 + "%";
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Defgroup2
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1 ;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(defCurve11) - FileEditor.getByte(defCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                    //* 0x10 / 0x28 is meant to similate multiplying by 0x66666667
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue /256 * 100 + "%";

                    value = ((FileEditor.getByte(defCurve11) - FileEditor.getByte(defCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Defgroup3
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(defCurve13) - FileEditor.getByte(defCurve11)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(defCurve13) - FileEditor.getByte(defCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Defgroup4
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";
                    value = ((FileEditor.getByte(defCurve15) - FileEditor.getByte(defCurve13)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(defCurve15) - FileEditor.getByte(defCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Defgroup5
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x300;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(defCurve17) - FileEditor.getByte(defCurve15)) << 8) * 0x100 / 0x300;
                    otherValue = value;
                    //*0x100 / 0x300 is to simulate *0x55555556
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x228 >> 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(defCurve17) - FileEditor.getByte(defCurve15)) << 8) * 0x100 / 0x280 >> 2;
                    otherValue = value;
                    //*0x100 / 0x228 is to simulate *0x76b981d8
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Defgroup6
        {
            get
            {
                if (!IsPromoted)
                {
                    value = ((FileEditor.getByte(defCurve20) - FileEditor.getByte(defCurve17)) << 8) * 0x100 / 0x340 >> 2;
                    otherValue = value;
                    //*0x100 / 0x340 is to simulate *0x4ec4ec4f
                }
                else
                {
                    value = ((FileEditor.getByte(defCurve20) - FileEditor.getByte(defCurve17)) << 8) * 0x100 / 0x228 >> 5;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public int AgiPromote
        {
            get => FileEditor.getByte(agiPromote);
            set => FileEditor.setByte(agiPromote, (byte)value);
        }
        public int AgiStart
        {
            get => FileEditor.getByte(agiStart);
            set => FileEditor.setByte(agiStart, (byte)value);
        }
        public int AgiCurve6
        {
            get => FileEditor.getByte(agiCurve6);
            set => FileEditor.setByte(agiCurve6, (byte)value);
        }
        public int AgiCurve11
        {
            get => FileEditor.getByte(agiCurve11);
            set => FileEditor.setByte(agiCurve11, (byte)value);
        }
        public int AgiCurve13
        {
            get => FileEditor.getByte(agiCurve13);
            set => FileEditor.setByte(agiCurve13, (byte)value);
        }
        public int AgiCurve15
        {
            get => FileEditor.getByte(agiCurve15);
            set => FileEditor.setByte(agiCurve15, (byte)value);
        }
        public int AgiCurve17
        {
            get => FileEditor.getByte(agiCurve17);
            set => FileEditor.setByte(agiCurve17, (byte)value);
        }
        public int AgiCurve20
        {
            get => FileEditor.getByte(agiCurve20);
            set => FileEditor.setByte(agiCurve20, (byte)value);
        }

        public string Agigroup1
        {
            get
            {
                if (!IsPromoted)
                {
                    //return ((FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6);
                    value = ((FileEditor.getByte(agiCurve6) - FileEditor.getByte(agiStart)) << 6); ;
                    otherValue = value;
                }
                else
                {
                    //return (FileEditor.getByte(hpCurve6) - FileEditor.getByte(hpStart)) << 6;
                    value = ((FileEditor.getByte(agiCurve6) - FileEditor.getByte(agiStart)) << 6);
                    otherValue = value;
                    //return otherValue / 256 * 100 + "%";
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Agigroup2
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1 ;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(agiCurve11) - FileEditor.getByte(agiCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                    //* 0x10 / 0x28 is meant to similate multiplying by 0x66666667
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve11) - FileEditor.getByte(hpCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue /256 * 100 + "%";

                    value = ((FileEditor.getByte(agiCurve11) - FileEditor.getByte(agiCurve6)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Agigroup3
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(agiCurve13) - FileEditor.getByte(agiCurve11)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve13) - FileEditor.getByte(hpCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(agiCurve13) - FileEditor.getByte(agiCurve11)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Agigroup4
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 2) * 2 << 2;
                    //return otherValue / 256 * 100 + "%";
                    value = ((FileEditor.getByte(agiCurve15) - FileEditor.getByte(agiCurve13)) << 4) * 2 << 2;
                    otherValue = value;
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve15) - FileEditor.getByte(hpCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(agiCurve15) - FileEditor.getByte(agiCurve13)) << 8) * 0x100 / 0x280 >> 1;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Agigroup5
        {
            get
            {
                if (!IsPromoted)
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x300;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(agiCurve17) - FileEditor.getByte(agiCurve15)) << 8) * 0x100 / 0x300;
                    otherValue = value;
                    //*0x100 / 0x300 is to simulate *0x55555556
                }
                else
                {
                    //otherValue = ((FileEditor.getByte(hpCurve17) - FileEditor.getByte(hpCurve15)) << 8) * 0x100 / 0x228 >> 2;
                    //return otherValue / 256 * 100 + "%";

                    value = ((FileEditor.getByte(agiCurve17) - FileEditor.getByte(agiCurve15)) << 8) * 0x100 / 0x280 >> 2;
                    otherValue = value;
                    //*0x100 / 0x228 is to simulate *0x76b981d8
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

        public string Agigroup6
        {
            get
            {
                if (!IsPromoted)
                {
                    value = ((FileEditor.getByte(agiCurve20) - FileEditor.getByte(agiCurve17)) << 8) * 0x100 / 0x340 >> 2;
                    otherValue = value;
                    //*0x100 / 0x340 is to simulate *0x4ec4ec4f
                }
                else
                {
                    value = ((FileEditor.getByte(agiCurve20) - FileEditor.getByte(agiCurve17)) << 8) * 0x100 / 0x228 >> 5;
                    otherValue = value;
                }

                otherValue = otherValue % 0x100;
                if (otherValue < 0)
                {
                    otherValue = 0;
                }
                percent = arrayMath[otherValue] / valueNumber;

                result = "";
                if (Debugs.debugs == 1)
                {
                    result = string.Format("{0:x}", value) + " || ";
                }
                return result + string.Format("{0:0.##}", (percent + ((value & 0xf00) % 15)) * 100) + "%";
            }
        }

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
