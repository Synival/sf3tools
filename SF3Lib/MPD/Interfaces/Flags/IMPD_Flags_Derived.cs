namespace SF3.MPD.Interfaces.Flags {
    public interface IMPD_Flags_Derived {
        /// <summary>
        /// When set, a sky chunk is present for either battles (Scenario 1) or cutscenes (Scenario 2+).
        /// </summary>
        bool HasAnySky { get; }

        /// <summary>
        /// When set, an extra model is present like the Titan (Scenario 1) or the Kraken (Scenario 2+).
        /// </summary>
        bool HasExtraModel { get; }
    }
}
