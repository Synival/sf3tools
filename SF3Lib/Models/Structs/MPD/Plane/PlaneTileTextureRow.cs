using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Plane {
    public class PlaneTileTextureRow : Struct {
        private readonly int[] xAddress = new int[256];

        public PlaneTileTextureRow(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x1000) {
            for (var i = 0; i < xAddress.Length; i++) {
                var block = i / 0x40;
                var x = i % 0x40;
                xAddress[i] = Address + (block * 0x1000 + x) * 2;
            }
        }

        public ushort this[int index] {
            get => (ushort) Data.GetWord(xAddress[index]);
            set => Data.SetWord(xAddress[index], value);
        }

        private class TileMetadataAttribute : TableViewModelColumnAttribute {
            // TODO: null
            public TileMetadataAttribute(int x) : base(addressField: null, displayName: "X" + x.ToString("D2"), displayOrder: x, displayFormat: "X4", minWidth: 50) { }
        }

        // This is NUTs, but the ObjectListView is excrutiatingly slow with array indexing, so we're stuck
        // with 256 individual properties.
        [TileMetadata(  0)] public ushort X000Tile { get => this[  0]; set => this[  0] = value; }
        [TileMetadata(  1)] public ushort X001Tile { get => this[  1]; set => this[  1] = value; }
        [TileMetadata(  2)] public ushort X002Tile { get => this[  2]; set => this[  2] = value; }
        [TileMetadata(  3)] public ushort X003Tile { get => this[  3]; set => this[  3] = value; }
        [TileMetadata(  4)] public ushort X004Tile { get => this[  4]; set => this[  4] = value; }
        [TileMetadata(  5)] public ushort X005Tile { get => this[  5]; set => this[  5] = value; }
        [TileMetadata(  6)] public ushort X006Tile { get => this[  6]; set => this[  6] = value; }
        [TileMetadata(  7)] public ushort X007Tile { get => this[  7]; set => this[  7] = value; }
        [TileMetadata(  8)] public ushort X008Tile { get => this[  8]; set => this[  8] = value; }
        [TileMetadata(  9)] public ushort X009Tile { get => this[  9]; set => this[  9] = value; }
        [TileMetadata( 10)] public ushort X010Tile { get => this[ 10]; set => this[ 10] = value; }
        [TileMetadata( 11)] public ushort X011Tile { get => this[ 11]; set => this[ 11] = value; }
        [TileMetadata( 12)] public ushort X012Tile { get => this[ 12]; set => this[ 12] = value; }
        [TileMetadata( 13)] public ushort X013Tile { get => this[ 13]; set => this[ 13] = value; }
        [TileMetadata( 14)] public ushort X014Tile { get => this[ 14]; set => this[ 14] = value; }
        [TileMetadata( 15)] public ushort X015Tile { get => this[ 15]; set => this[ 15] = value; }
        [TileMetadata( 16)] public ushort X016Tile { get => this[ 16]; set => this[ 16] = value; }
        [TileMetadata( 17)] public ushort X017Tile { get => this[ 17]; set => this[ 17] = value; }
        [TileMetadata( 18)] public ushort X018Tile { get => this[ 18]; set => this[ 18] = value; }
        [TileMetadata( 19)] public ushort X019Tile { get => this[ 19]; set => this[ 19] = value; }
        [TileMetadata( 20)] public ushort X020Tile { get => this[ 20]; set => this[ 20] = value; }
        [TileMetadata( 21)] public ushort X021Tile { get => this[ 21]; set => this[ 21] = value; }
        [TileMetadata( 22)] public ushort X022Tile { get => this[ 22]; set => this[ 22] = value; }
        [TileMetadata( 23)] public ushort X023Tile { get => this[ 23]; set => this[ 23] = value; }
        [TileMetadata( 24)] public ushort X024Tile { get => this[ 24]; set => this[ 24] = value; }
        [TileMetadata( 25)] public ushort X025Tile { get => this[ 25]; set => this[ 25] = value; }
        [TileMetadata( 26)] public ushort X026Tile { get => this[ 26]; set => this[ 26] = value; }
        [TileMetadata( 27)] public ushort X027Tile { get => this[ 27]; set => this[ 27] = value; }
        [TileMetadata( 28)] public ushort X028Tile { get => this[ 28]; set => this[ 28] = value; }
        [TileMetadata( 29)] public ushort X029Tile { get => this[ 29]; set => this[ 29] = value; }
        [TileMetadata( 30)] public ushort X030Tile { get => this[ 30]; set => this[ 30] = value; }
        [TileMetadata( 31)] public ushort X031Tile { get => this[ 31]; set => this[ 31] = value; }
        [TileMetadata( 32)] public ushort X032Tile { get => this[ 32]; set => this[ 32] = value; }
        [TileMetadata( 33)] public ushort X033Tile { get => this[ 33]; set => this[ 33] = value; }
        [TileMetadata( 34)] public ushort X034Tile { get => this[ 34]; set => this[ 34] = value; }
        [TileMetadata( 35)] public ushort X035Tile { get => this[ 35]; set => this[ 35] = value; }
        [TileMetadata( 36)] public ushort X036Tile { get => this[ 36]; set => this[ 36] = value; }
        [TileMetadata( 37)] public ushort X037Tile { get => this[ 37]; set => this[ 37] = value; }
        [TileMetadata( 38)] public ushort X038Tile { get => this[ 38]; set => this[ 38] = value; }
        [TileMetadata( 39)] public ushort X039Tile { get => this[ 39]; set => this[ 39] = value; }
        [TileMetadata( 40)] public ushort X040Tile { get => this[ 40]; set => this[ 40] = value; }
        [TileMetadata( 41)] public ushort X041Tile { get => this[ 41]; set => this[ 41] = value; }
        [TileMetadata( 42)] public ushort X042Tile { get => this[ 42]; set => this[ 42] = value; }
        [TileMetadata( 43)] public ushort X043Tile { get => this[ 43]; set => this[ 43] = value; }
        [TileMetadata( 44)] public ushort X044Tile { get => this[ 44]; set => this[ 44] = value; }
        [TileMetadata( 45)] public ushort X045Tile { get => this[ 45]; set => this[ 45] = value; }
        [TileMetadata( 46)] public ushort X046Tile { get => this[ 46]; set => this[ 46] = value; }
        [TileMetadata( 47)] public ushort X047Tile { get => this[ 47]; set => this[ 47] = value; }
        [TileMetadata( 48)] public ushort X048Tile { get => this[ 48]; set => this[ 48] = value; }
        [TileMetadata( 49)] public ushort X049Tile { get => this[ 49]; set => this[ 49] = value; }
        [TileMetadata( 50)] public ushort X050Tile { get => this[ 50]; set => this[ 50] = value; }
        [TileMetadata( 51)] public ushort X051Tile { get => this[ 51]; set => this[ 51] = value; }
        [TileMetadata( 52)] public ushort X052Tile { get => this[ 52]; set => this[ 52] = value; }
        [TileMetadata( 53)] public ushort X053Tile { get => this[ 53]; set => this[ 53] = value; }
        [TileMetadata( 54)] public ushort X054Tile { get => this[ 54]; set => this[ 54] = value; }
        [TileMetadata( 55)] public ushort X055Tile { get => this[ 55]; set => this[ 55] = value; }
        [TileMetadata( 56)] public ushort X056Tile { get => this[ 56]; set => this[ 56] = value; }
        [TileMetadata( 57)] public ushort X057Tile { get => this[ 57]; set => this[ 57] = value; }
        [TileMetadata( 58)] public ushort X058Tile { get => this[ 58]; set => this[ 58] = value; }
        [TileMetadata( 59)] public ushort X059Tile { get => this[ 59]; set => this[ 59] = value; }
        [TileMetadata( 60)] public ushort X060Tile { get => this[ 60]; set => this[ 60] = value; }
        [TileMetadata( 61)] public ushort X061Tile { get => this[ 61]; set => this[ 61] = value; }
        [TileMetadata( 62)] public ushort X062Tile { get => this[ 62]; set => this[ 62] = value; }
        [TileMetadata( 63)] public ushort X063Tile { get => this[ 63]; set => this[ 63] = value; }
        [TileMetadata( 64)] public ushort X064Tile { get => this[ 64]; set => this[ 64] = value; }
        [TileMetadata( 65)] public ushort X065Tile { get => this[ 65]; set => this[ 65] = value; }
        [TileMetadata( 66)] public ushort X066Tile { get => this[ 66]; set => this[ 66] = value; }
        [TileMetadata( 67)] public ushort X067Tile { get => this[ 67]; set => this[ 67] = value; }
        [TileMetadata( 68)] public ushort X068Tile { get => this[ 68]; set => this[ 68] = value; }
        [TileMetadata( 69)] public ushort X069Tile { get => this[ 69]; set => this[ 69] = value; }
        [TileMetadata( 70)] public ushort X070Tile { get => this[ 70]; set => this[ 70] = value; }
        [TileMetadata( 71)] public ushort X071Tile { get => this[ 71]; set => this[ 71] = value; }
        [TileMetadata( 72)] public ushort X072Tile { get => this[ 72]; set => this[ 72] = value; }
        [TileMetadata( 73)] public ushort X073Tile { get => this[ 73]; set => this[ 73] = value; }
        [TileMetadata( 74)] public ushort X074Tile { get => this[ 74]; set => this[ 74] = value; }
        [TileMetadata( 75)] public ushort X075Tile { get => this[ 75]; set => this[ 75] = value; }
        [TileMetadata( 76)] public ushort X076Tile { get => this[ 76]; set => this[ 76] = value; }
        [TileMetadata( 77)] public ushort X077Tile { get => this[ 77]; set => this[ 77] = value; }
        [TileMetadata( 78)] public ushort X078Tile { get => this[ 78]; set => this[ 78] = value; }
        [TileMetadata( 79)] public ushort X079Tile { get => this[ 79]; set => this[ 79] = value; }
        [TileMetadata( 80)] public ushort X080Tile { get => this[ 80]; set => this[ 80] = value; }
        [TileMetadata( 81)] public ushort X081Tile { get => this[ 81]; set => this[ 81] = value; }
        [TileMetadata( 82)] public ushort X082Tile { get => this[ 82]; set => this[ 82] = value; }
        [TileMetadata( 83)] public ushort X083Tile { get => this[ 83]; set => this[ 83] = value; }
        [TileMetadata( 84)] public ushort X084Tile { get => this[ 84]; set => this[ 84] = value; }
        [TileMetadata( 85)] public ushort X085Tile { get => this[ 85]; set => this[ 85] = value; }
        [TileMetadata( 86)] public ushort X086Tile { get => this[ 86]; set => this[ 86] = value; }
        [TileMetadata( 87)] public ushort X087Tile { get => this[ 87]; set => this[ 87] = value; }
        [TileMetadata( 88)] public ushort X088Tile { get => this[ 88]; set => this[ 88] = value; }
        [TileMetadata( 89)] public ushort X089Tile { get => this[ 89]; set => this[ 89] = value; }
        [TileMetadata( 90)] public ushort X090Tile { get => this[ 90]; set => this[ 90] = value; }
        [TileMetadata( 91)] public ushort X091Tile { get => this[ 91]; set => this[ 91] = value; }
        [TileMetadata( 92)] public ushort X092Tile { get => this[ 92]; set => this[ 92] = value; }
        [TileMetadata( 93)] public ushort X093Tile { get => this[ 93]; set => this[ 93] = value; }
        [TileMetadata( 94)] public ushort X094Tile { get => this[ 94]; set => this[ 94] = value; }
        [TileMetadata( 95)] public ushort X095Tile { get => this[ 95]; set => this[ 95] = value; }
        [TileMetadata( 96)] public ushort X096Tile { get => this[ 96]; set => this[ 96] = value; }
        [TileMetadata( 97)] public ushort X097Tile { get => this[ 97]; set => this[ 97] = value; }
        [TileMetadata( 98)] public ushort X098Tile { get => this[ 98]; set => this[ 98] = value; }
        [TileMetadata( 99)] public ushort X099Tile { get => this[ 99]; set => this[ 99] = value; }
        [TileMetadata(100)] public ushort X100Tile { get => this[100]; set => this[100] = value; }
        [TileMetadata(101)] public ushort X101Tile { get => this[101]; set => this[101] = value; }
        [TileMetadata(102)] public ushort X102Tile { get => this[102]; set => this[102] = value; }
        [TileMetadata(103)] public ushort X103Tile { get => this[103]; set => this[103] = value; }
        [TileMetadata(104)] public ushort X104Tile { get => this[104]; set => this[104] = value; }
        [TileMetadata(105)] public ushort X105Tile { get => this[105]; set => this[105] = value; }
        [TileMetadata(106)] public ushort X106Tile { get => this[106]; set => this[106] = value; }
        [TileMetadata(107)] public ushort X107Tile { get => this[107]; set => this[107] = value; }
        [TileMetadata(108)] public ushort X108Tile { get => this[108]; set => this[108] = value; }
        [TileMetadata(109)] public ushort X109Tile { get => this[109]; set => this[109] = value; }
        [TileMetadata(110)] public ushort X110Tile { get => this[110]; set => this[110] = value; }
        [TileMetadata(111)] public ushort X111Tile { get => this[111]; set => this[111] = value; }
        [TileMetadata(112)] public ushort X112Tile { get => this[112]; set => this[112] = value; }
        [TileMetadata(113)] public ushort X113Tile { get => this[113]; set => this[113] = value; }
        [TileMetadata(114)] public ushort X114Tile { get => this[114]; set => this[114] = value; }
        [TileMetadata(115)] public ushort X115Tile { get => this[115]; set => this[115] = value; }
        [TileMetadata(116)] public ushort X116Tile { get => this[116]; set => this[116] = value; }
        [TileMetadata(117)] public ushort X117Tile { get => this[117]; set => this[117] = value; }
        [TileMetadata(118)] public ushort X118Tile { get => this[118]; set => this[118] = value; }
        [TileMetadata(119)] public ushort X119Tile { get => this[119]; set => this[119] = value; }
        [TileMetadata(120)] public ushort X120Tile { get => this[120]; set => this[120] = value; }
        [TileMetadata(121)] public ushort X121Tile { get => this[121]; set => this[121] = value; }
        [TileMetadata(122)] public ushort X122Tile { get => this[122]; set => this[122] = value; }
        [TileMetadata(123)] public ushort X123Tile { get => this[123]; set => this[123] = value; }
        [TileMetadata(124)] public ushort X124Tile { get => this[124]; set => this[124] = value; }
        [TileMetadata(125)] public ushort X125Tile { get => this[125]; set => this[125] = value; }
        [TileMetadata(126)] public ushort X126Tile { get => this[126]; set => this[126] = value; }
        [TileMetadata(127)] public ushort X127Tile { get => this[127]; set => this[127] = value; }
        [TileMetadata(128)] public ushort X128Tile { get => this[128]; set => this[128] = value; }
        [TileMetadata(129)] public ushort X129Tile { get => this[129]; set => this[129] = value; }
        [TileMetadata(130)] public ushort X130Tile { get => this[130]; set => this[130] = value; }
        [TileMetadata(131)] public ushort X131Tile { get => this[131]; set => this[131] = value; }
        [TileMetadata(132)] public ushort X132Tile { get => this[132]; set => this[132] = value; }
        [TileMetadata(133)] public ushort X133Tile { get => this[133]; set => this[133] = value; }
        [TileMetadata(134)] public ushort X134Tile { get => this[134]; set => this[134] = value; }
        [TileMetadata(135)] public ushort X135Tile { get => this[135]; set => this[135] = value; }
        [TileMetadata(136)] public ushort X136Tile { get => this[136]; set => this[136] = value; }
        [TileMetadata(137)] public ushort X137Tile { get => this[137]; set => this[137] = value; }
        [TileMetadata(138)] public ushort X138Tile { get => this[138]; set => this[138] = value; }
        [TileMetadata(139)] public ushort X139Tile { get => this[139]; set => this[139] = value; }
        [TileMetadata(140)] public ushort X140Tile { get => this[140]; set => this[140] = value; }
        [TileMetadata(141)] public ushort X141Tile { get => this[141]; set => this[141] = value; }
        [TileMetadata(142)] public ushort X142Tile { get => this[142]; set => this[142] = value; }
        [TileMetadata(143)] public ushort X143Tile { get => this[143]; set => this[143] = value; }
        [TileMetadata(144)] public ushort X144Tile { get => this[144]; set => this[144] = value; }
        [TileMetadata(145)] public ushort X145Tile { get => this[145]; set => this[145] = value; }
        [TileMetadata(146)] public ushort X146Tile { get => this[146]; set => this[146] = value; }
        [TileMetadata(147)] public ushort X147Tile { get => this[147]; set => this[147] = value; }
        [TileMetadata(148)] public ushort X148Tile { get => this[148]; set => this[148] = value; }
        [TileMetadata(149)] public ushort X149Tile { get => this[149]; set => this[149] = value; }
        [TileMetadata(150)] public ushort X150Tile { get => this[150]; set => this[150] = value; }
        [TileMetadata(151)] public ushort X151Tile { get => this[151]; set => this[151] = value; }
        [TileMetadata(152)] public ushort X152Tile { get => this[152]; set => this[152] = value; }
        [TileMetadata(153)] public ushort X153Tile { get => this[153]; set => this[153] = value; }
        [TileMetadata(154)] public ushort X154Tile { get => this[154]; set => this[154] = value; }
        [TileMetadata(155)] public ushort X155Tile { get => this[155]; set => this[155] = value; }
        [TileMetadata(156)] public ushort X156Tile { get => this[156]; set => this[156] = value; }
        [TileMetadata(157)] public ushort X157Tile { get => this[157]; set => this[157] = value; }
        [TileMetadata(158)] public ushort X158Tile { get => this[158]; set => this[158] = value; }
        [TileMetadata(159)] public ushort X159Tile { get => this[159]; set => this[159] = value; }
        [TileMetadata(160)] public ushort X160Tile { get => this[160]; set => this[160] = value; }
        [TileMetadata(161)] public ushort X161Tile { get => this[161]; set => this[161] = value; }
        [TileMetadata(162)] public ushort X162Tile { get => this[162]; set => this[162] = value; }
        [TileMetadata(163)] public ushort X163Tile { get => this[163]; set => this[163] = value; }
        [TileMetadata(164)] public ushort X164Tile { get => this[164]; set => this[164] = value; }
        [TileMetadata(165)] public ushort X165Tile { get => this[165]; set => this[165] = value; }
        [TileMetadata(166)] public ushort X166Tile { get => this[166]; set => this[166] = value; }
        [TileMetadata(167)] public ushort X167Tile { get => this[167]; set => this[167] = value; }
        [TileMetadata(168)] public ushort X168Tile { get => this[168]; set => this[168] = value; }
        [TileMetadata(169)] public ushort X169Tile { get => this[169]; set => this[169] = value; }
        [TileMetadata(170)] public ushort X170Tile { get => this[170]; set => this[170] = value; }
        [TileMetadata(171)] public ushort X171Tile { get => this[171]; set => this[171] = value; }
        [TileMetadata(172)] public ushort X172Tile { get => this[172]; set => this[172] = value; }
        [TileMetadata(173)] public ushort X173Tile { get => this[173]; set => this[173] = value; }
        [TileMetadata(174)] public ushort X174Tile { get => this[174]; set => this[174] = value; }
        [TileMetadata(175)] public ushort X175Tile { get => this[175]; set => this[175] = value; }
        [TileMetadata(176)] public ushort X176Tile { get => this[176]; set => this[176] = value; }
        [TileMetadata(177)] public ushort X177Tile { get => this[177]; set => this[177] = value; }
        [TileMetadata(178)] public ushort X178Tile { get => this[178]; set => this[178] = value; }
        [TileMetadata(179)] public ushort X179Tile { get => this[179]; set => this[179] = value; }
        [TileMetadata(180)] public ushort X180Tile { get => this[180]; set => this[180] = value; }
        [TileMetadata(181)] public ushort X181Tile { get => this[181]; set => this[181] = value; }
        [TileMetadata(182)] public ushort X182Tile { get => this[182]; set => this[182] = value; }
        [TileMetadata(183)] public ushort X183Tile { get => this[183]; set => this[183] = value; }
        [TileMetadata(184)] public ushort X184Tile { get => this[184]; set => this[184] = value; }
        [TileMetadata(185)] public ushort X185Tile { get => this[185]; set => this[185] = value; }
        [TileMetadata(186)] public ushort X186Tile { get => this[186]; set => this[186] = value; }
        [TileMetadata(187)] public ushort X187Tile { get => this[187]; set => this[187] = value; }
        [TileMetadata(188)] public ushort X188Tile { get => this[188]; set => this[188] = value; }
        [TileMetadata(189)] public ushort X189Tile { get => this[189]; set => this[189] = value; }
        [TileMetadata(190)] public ushort X190Tile { get => this[190]; set => this[190] = value; }
        [TileMetadata(191)] public ushort X191Tile { get => this[191]; set => this[191] = value; }
        [TileMetadata(192)] public ushort X192Tile { get => this[192]; set => this[192] = value; }
        [TileMetadata(193)] public ushort X193Tile { get => this[193]; set => this[193] = value; }
        [TileMetadata(194)] public ushort X194Tile { get => this[194]; set => this[194] = value; }
        [TileMetadata(195)] public ushort X195Tile { get => this[195]; set => this[195] = value; }
        [TileMetadata(196)] public ushort X196Tile { get => this[196]; set => this[196] = value; }
        [TileMetadata(197)] public ushort X197Tile { get => this[197]; set => this[197] = value; }
        [TileMetadata(198)] public ushort X198Tile { get => this[198]; set => this[198] = value; }
        [TileMetadata(199)] public ushort X199Tile { get => this[199]; set => this[199] = value; }
        [TileMetadata(200)] public ushort X200Tile { get => this[200]; set => this[200] = value; }
        [TileMetadata(201)] public ushort X201Tile { get => this[201]; set => this[201] = value; }
        [TileMetadata(202)] public ushort X202Tile { get => this[202]; set => this[202] = value; }
        [TileMetadata(203)] public ushort X203Tile { get => this[203]; set => this[203] = value; }
        [TileMetadata(204)] public ushort X204Tile { get => this[204]; set => this[204] = value; }
        [TileMetadata(205)] public ushort X205Tile { get => this[205]; set => this[205] = value; }
        [TileMetadata(206)] public ushort X206Tile { get => this[206]; set => this[206] = value; }
        [TileMetadata(207)] public ushort X207Tile { get => this[207]; set => this[207] = value; }
        [TileMetadata(208)] public ushort X208Tile { get => this[208]; set => this[208] = value; }
        [TileMetadata(209)] public ushort X209Tile { get => this[209]; set => this[209] = value; }
        [TileMetadata(210)] public ushort X210Tile { get => this[210]; set => this[210] = value; }
        [TileMetadata(211)] public ushort X211Tile { get => this[211]; set => this[211] = value; }
        [TileMetadata(212)] public ushort X212Tile { get => this[212]; set => this[212] = value; }
        [TileMetadata(213)] public ushort X213Tile { get => this[213]; set => this[213] = value; }
        [TileMetadata(214)] public ushort X214Tile { get => this[214]; set => this[214] = value; }
        [TileMetadata(215)] public ushort X215Tile { get => this[215]; set => this[215] = value; }
        [TileMetadata(216)] public ushort X216Tile { get => this[216]; set => this[216] = value; }
        [TileMetadata(217)] public ushort X217Tile { get => this[217]; set => this[217] = value; }
        [TileMetadata(218)] public ushort X218Tile { get => this[218]; set => this[218] = value; }
        [TileMetadata(219)] public ushort X219Tile { get => this[219]; set => this[219] = value; }
        [TileMetadata(220)] public ushort X220Tile { get => this[220]; set => this[220] = value; }
        [TileMetadata(221)] public ushort X221Tile { get => this[221]; set => this[221] = value; }
        [TileMetadata(222)] public ushort X222Tile { get => this[222]; set => this[222] = value; }
        [TileMetadata(223)] public ushort X223Tile { get => this[223]; set => this[223] = value; }
        [TileMetadata(224)] public ushort X224Tile { get => this[224]; set => this[224] = value; }
        [TileMetadata(225)] public ushort X225Tile { get => this[225]; set => this[225] = value; }
        [TileMetadata(226)] public ushort X226Tile { get => this[226]; set => this[226] = value; }
        [TileMetadata(227)] public ushort X227Tile { get => this[227]; set => this[227] = value; }
        [TileMetadata(228)] public ushort X228Tile { get => this[228]; set => this[228] = value; }
        [TileMetadata(229)] public ushort X229Tile { get => this[229]; set => this[229] = value; }
        [TileMetadata(230)] public ushort X230Tile { get => this[230]; set => this[230] = value; }
        [TileMetadata(231)] public ushort X231Tile { get => this[231]; set => this[231] = value; }
        [TileMetadata(232)] public ushort X232Tile { get => this[232]; set => this[232] = value; }
        [TileMetadata(233)] public ushort X233Tile { get => this[233]; set => this[233] = value; }
        [TileMetadata(234)] public ushort X234Tile { get => this[234]; set => this[234] = value; }
        [TileMetadata(235)] public ushort X235Tile { get => this[235]; set => this[235] = value; }
        [TileMetadata(236)] public ushort X236Tile { get => this[236]; set => this[236] = value; }
        [TileMetadata(237)] public ushort X237Tile { get => this[237]; set => this[237] = value; }
        [TileMetadata(238)] public ushort X238Tile { get => this[238]; set => this[238] = value; }
        [TileMetadata(239)] public ushort X239Tile { get => this[239]; set => this[239] = value; }
        [TileMetadata(240)] public ushort X240Tile { get => this[240]; set => this[240] = value; }
        [TileMetadata(241)] public ushort X241Tile { get => this[241]; set => this[241] = value; }
        [TileMetadata(242)] public ushort X242Tile { get => this[242]; set => this[242] = value; }
        [TileMetadata(243)] public ushort X243Tile { get => this[243]; set => this[243] = value; }
        [TileMetadata(244)] public ushort X244Tile { get => this[244]; set => this[244] = value; }
        [TileMetadata(245)] public ushort X245Tile { get => this[245]; set => this[245] = value; }
        [TileMetadata(246)] public ushort X246Tile { get => this[246]; set => this[246] = value; }
        [TileMetadata(247)] public ushort X247Tile { get => this[247]; set => this[247] = value; }
        [TileMetadata(248)] public ushort X248Tile { get => this[248]; set => this[248] = value; }
        [TileMetadata(249)] public ushort X249Tile { get => this[249]; set => this[249] = value; }
        [TileMetadata(250)] public ushort X250Tile { get => this[250]; set => this[250] = value; }
        [TileMetadata(251)] public ushort X251Tile { get => this[251]; set => this[251] = value; }
        [TileMetadata(252)] public ushort X252Tile { get => this[252]; set => this[252] = value; }
        [TileMetadata(253)] public ushort X253Tile { get => this[253]; set => this[253] = value; }
        [TileMetadata(254)] public ushort X254Tile { get => this[254]; set => this[254] = value; }
        [TileMetadata(255)] public ushort X255Tile { get => this[255]; set => this[255] = value; }
    }
}
