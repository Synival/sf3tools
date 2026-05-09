using CommonLib.Attributes;

namespace SF3.Types {
    public enum MovementType {
        [EnumDisplayName("Unknown 0x0")] Unknown0x0 = 0x00,
        [EnumDisplayName("Normal")]      Normal     = 0x01,
        [EnumDisplayName("Centaur")]     Centaur    = 0x02,
        [EnumDisplayName("Beast")]       Beast      = 0x03,
        [EnumDisplayName("Tank")]        Tank       = 0x04,
        [EnumDisplayName("Flying")]      Flying     = 0x05,
        [EnumDisplayName("Floating")]    Floating   = 0x06,
        [EnumDisplayName("Aquatic")]     Aquatic    = 0x07,
        [EnumDisplayName("Archer")]      Archer     = 0x08,
        [EnumDisplayName("Bowknight")]   Bowknight  = 0x09,
        [EnumDisplayName("Elf")]         Elf        = 0x0A,
        [EnumDisplayName("Mage")]        Mage       = 0x0B,
        [EnumDisplayName("Healer")]      Healer     = 0x0C,
        [EnumDisplayName("Unknown 0xD")] Unknown0xD = 0x0D,
        [EnumDisplayName("Unknown 0xE")] Unknown0xE = 0x0E,
        [EnumDisplayName("Unknown 0xF")] Unknown0xF = 0x0F,
    }
}
