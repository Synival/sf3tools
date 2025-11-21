namespace SF3.MPD {
    /// <summary>
    /// Abstract implementation of any kind of MPD, such as "in-place" editors like MPD_File or a fully deserialized
    /// resource.
    /// </summary>
    public interface IMPD {
        /// <summary>
        /// The flags for the MPD. This contains settings like lighting flags and flags that inform the game about what
        /// chunks are present and how they should be stored in memory.
        /// </summary>
        IMPD_Flags Flags { get; }
    }
}
