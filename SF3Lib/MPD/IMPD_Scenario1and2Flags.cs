namespace SF3.MPD {
    public interface IMPD_Scenario1and2Flags : IMPD_AllScenarioFlags {
        /// <summary>
        /// Always on for Scenario 1 and 2. Not known if this is read anywhere and must be set.
        /// </summary>
        bool Bit_0x0002_Unknown { get; set; }
    }
}
