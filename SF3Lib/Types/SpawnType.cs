using CommonLib.Attributes;

namespace SF3.Types {
    public enum SpawnType {
        [EnumDisplayName("Start Spawned")]
        StartSpawned = 0,

        [EnumDisplayName("Respawn Limited Times")]
        RespawnLimitedTimes = 1,

        [EnumDisplayName("Zone-Triggered Spawn")]
        ZoneTriggeredSpawn = 2,

        [EnumDisplayName("Zone-Triggered Continuous Respawn")]
        ZoneTriggeredContinuousRespawn = 3,

        [EnumDisplayName("Event-Triggered Spawn")]
        EventTriggeredSpawn = 4,
    }
}
