using CommonLib.Attributes;

namespace SF3.Types {
    public enum AIOrderType {
        [EnumDisplayName("Invalid")]
        Invalid  = -1,

        [EnumDisplayName("No Order")]
        NoOrder  =  0,

        [EnumDisplayName("Leader")]
        Leader   =  1,

        [EnumDisplayName("Closest")]
        Closest  =  2,

        [EnumDisplayName("Unit")]
        Unit     =  3,

        [EnumDisplayName("Location")]
        Location =  4,

        [EnumDisplayName("Path")]
        Path     =  5
    }
}
