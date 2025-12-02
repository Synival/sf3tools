namespace SF3.MPD {
    public interface IMPD_PremiumDiskFlags : IMPD_AllScenarioFlags, IMPD_Scenario2PlusFlags, IMPD_Scenario3PlusFlags, IMPD_DerivedFlags {
        /// <summary>
        /// Builds the flags for an MPD file belonging to the Premium Disk (same as Scenario 3).
        /// </summary>
        /// <returns></returns>
        ushort GetPremiumDiskHeaderFlags();
    }
}
