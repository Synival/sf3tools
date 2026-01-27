namespace SF3.MPD.Interfaces.Flags {
    public interface IMPD_Flags_Scenario1And2 : IMPD_Flags_AllScenarios {
        /// <summary>
        /// Always on for Scenario 1 and 2. Not known if this is read anywhere and must be set.
        /// </summary>
        bool Bit_0x0002_Unknown { get; set; }
    }
}
