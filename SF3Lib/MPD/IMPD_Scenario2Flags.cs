namespace SF3.MPD {
    public interface IMPD_Scenario2Flags : IMPD_AllScenarioFlags, IMPD_Scenario1and2Flags, IMPD_Scenario2PlusFlags, IMPD_DerivedFlags {
        /// <summary>
        /// Builds the flags for an MPD file belonging to Scenario 2.
        /// </summary>
        /// <returns></returns>
        ushort GetScenario2HeaderFlags();
    }
}
