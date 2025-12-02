namespace SF3.MPD {
    public interface IMPD_Scenario3Flags : IMPD_AllScenarioFlags, IMPD_Scenario2PlusFlags, IMPD_Scenario3PlusFlags, IMPD_DerivedFlags {
        /// <summary>
        /// Builds the flags for an MPD file belonging to Scenario 3.
        /// </summary>
        /// <returns></returns>
        ushort GetScenario3HeaderFlags();
    }
}
