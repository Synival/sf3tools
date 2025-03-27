using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace X1_Analyzer {
    public class Program {
        // ,--- Enter the paths for all your X1 files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        private static List<int> s_allSpawnTypes = [];
        private static List<int> s_allUnknown0x0Es = [];
        private static List<int> s_allAiTags = [];
        private static List<int> s_allAiTypes = [];
        private static List<int> s_allAiAggrs = [];
        private static List<string> s_matchReports = [];

        /// <summary>
        /// Check for matching X1 files for certain conditions.
        /// </summary>
        /// <param name="x1File"></param>
        /// <returns>'null' if this file should be skipped, 'true' if it matches our criteria, 'false' if not.</returns>
        private static bool? X1_Match_Func(string filename, IX1_File x1File) {
            // Sample: Skip non-battles, and match non-battles with scripted movements.

            var ramOffset = (x1File.Scenario == ScenarioType.Scenario1) ? 0x0605f000 : 0x0605e000;

            // Gather a list of enemies with their relevant AI-related properties.
            var battles = x1File.Battles ?? [];
            var allEnemies = battles.Values
                .SelectMany(x =>
                    x.SlotTable
                        .Where(y => y.EnemyID != 0x00)
                        .Select(y => new {
                            BattleName = x.Name,

                            SlotID = y.ID,
                            SlotName = y.Name,
                            y.EnemyID,
                            y.CreepUpWhenOutOfRange,
                            y.SpawnType,
                            y.Unknown0x0E,
                            Name = x1File.NameGetterContext.GetName(null, null, y.EnemyID, [NamedValueType.MonsterForSlot]),

                            AITags = new int[] {
                                y.AITag1,
                                y.AITag2,
                                y.AITag3,
                                y.AITag4,
                            },

                            AITypes = new int[] {
                                y.AIType1,
                                y.AIType2,
                                y.AIType3,
                                y.AIType4,
                            },

                            AIAggrs = new int[] {
                                y.AIAggr1,
                                y.AIAggr2,
                                y.AIAggr3,
                                y.AIAggr4,
                            },

                            AI = new int[] {
                                (y.AITag1 << 16) | (y.AIType1 << 8) | (y.AIAggr1),
                                (y.AITag2 << 16) | (y.AIType2 << 8) | (y.AIAggr2),
                                (y.AITag3 << 16) | (y.AIType3 << 8) | (y.AIAggr3),
                                (y.AITag4 << 16) | (y.AIType4 << 8) | (y.AIAggr4),
                            },

                            AIAddrs = new int[] {
                                y.Address + 0x23,
                                y.Address + 0x26,
                                y.Address + 0x29,
                                y.Address + 0x2C,
                            },

                            y.EnemyFlags,
                            y.FlagTieInOrUnknown,
                        }
                    )
                )
                .ToArray() ?? [];

            // Gather some data for a final report.
            // (This shouldn't really be in this function, but w/e)
            s_allAiTags.AddRange(allEnemies.SelectMany(x => x.AITags).ToArray());
            s_allAiTypes.AddRange(allEnemies.SelectMany(x => x.AITypes).ToArray());
            s_allAiAggrs.AddRange(allEnemies.SelectMany(x => x.AIAggrs).ToArray());
            s_allSpawnTypes.AddRange(allEnemies.Select(x => (int) x.SpawnType));
            s_allUnknown0x0Es.AddRange(allEnemies.Select(x => x.Unknown0x0E));

#if false
            // Match enemies with any non-default AI.
            var enemiesWithNonDefaultAI = enemiesWithAI
                .Where(x => x.AITags.Any(y => y != 0xFF) || x.AITypes.Any(y => y != 0xFF) || x.AIAggrs.Any(y => y != 0xFF))
                .ToArray();
            foreach (var enemy in enemiesWithNonDefaultAI) {
                var aiTypesStr = string.Join(", ", enemy.AITypes.Select(x => x.ToString("X2")));
                var enemyStr = $"{enemy.SlotName}, {enemy.Name} (0x{enemy.EnemyID:X2}): SpawnType={enemy.SpawnType:X2}, Unknown0x0E={enemy.Unknown0x0E:X2}";
                for (int i = 0; i < 4; i++)
                    if (enemy.AITags[i] != 0xFF || enemy.AITags[i] != 0xFF || enemy.AIAggrs[i] != 0x00)
                        s_matchReports.Add($"Tags={enemy.AITags[i]:X2}, Type={enemy.AITypes[i]:X2}, Aggr={enemy.AIAggrs[i]:X2} | AI[{i}] | {enemyStr}");
            }
#elif false
            return (x1File.ArrowTable != null && x1File.ArrowTable.Any(x => x.IfFlagOff != 0xFFFF)) ? true : null;
#elif false
            // Match enemies with a team other than 0 or 1.
            foreach (var battle in x1File.Battles ?? []) {
                var enemies = battle.Value.SlotTable.Where(x => x.EnemyID != 0).ToArray();
                foreach (var enemy in enemies.Where(x => x.FlagTieInOrUnknown != 0x00)) {
                    var realName = x1File.NameGetterContext.GetName(null, null, enemy.EnemyID, [NamedValueType.MonsterForSlot]);
                    var flagName = x1File.NameGetterContext.GetName(null, null, enemy.FlagTieInOrUnknown, [NamedValueType.GameFlag]);
                    var itemName = x1File.NameGetterContext.GetName(null, null, enemy.ItemOverride, [NamedValueType.Item]);
                    if (flagName != "")
                        continue;

                    var enemyStr = $"{battle.Key.ToString().PadLeft(7)} | {enemy.Name} (0x{enemy.ID:X2}) | {realName} (0x{enemy.EnemyID:X2})";
                    var flagStr  = $"{flagName.PadLeft(60)} (0x{enemy.FlagTieInOrUnknown:X3})";
                    var itemStr  = $"{itemName} (0x{enemy.ItemOverride:X2})";

                    s_matchReports.Add(
                        enemy.EnemyFlags.ToString("X4") + " | " +
                        BitString(enemy.EnemyFlags) + " | " +
                        flagStr + " | " +
                        enemyStr.PadRight(40) + " | " +
                        itemStr
                    );
                }
            }
#elif false
            if (x1File.NpcTable != null) {
                foreach (var i in x1File.InteractableTable.Where(x => (x.TriggerFlags & 0x07) != 0)) {
                    var actionFlagChecked = i.ActionFlagChecked;
                    var flagName = x1File.NameGetterContext.GetName(null, null, actionFlagChecked, [NamedValueType.GameFlag]) ?? "";
                    var itemName = (i.ActionType == 0x100 || i.ActionType == 0x101) ? x1File.NameGetterContext.GetName(null, null, i.ActionParam, [NamedValueType.Item]) : "";

                    var interactableStr = i.ID.ToString("X2") + " | " + i.Trigger.ToString("X4") + " | " + i.TriggerFlags.ToString("X2") + " | " + i.TriggerTargetID.ToString("X2");
                    var flagStr = $"{flagName.PadLeft(60)} (0x{i.ActionFlagChecked:X3} == {i.ActionFlagStatus})";
                    var itemStr = ((itemName != "") ? (itemName + " ") : "") + $"({(EventActionType) i.ActionType}, 0x{i.ActionParam:X3})";

                    var charName = x1File.NameGetterContext.GetName(null, null, i.TriggerTargetID, [NamedValueType.Character]);

                    s_matchReports.Add(interactableStr + " | " + i.TriggerDescription.PadRight(60) + " | " + flagStr.PadLeft(60) + " | " + itemStr);
                }
            }
#endif

            return s_matchReports.Count > 0 ? true : null;
        }

        public static void Main(string[] args) {
            Console.WriteLine("Press a key to start...");
            _ = Console.ReadKey();

            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "X1*.BIN").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            var matchSet   = new List<string>();
            var nomatchSet = new List<string>();

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        var isBTL99 = filename == "X1BTL99";
                        using (var x1File = X1_File.Create(byteData, nameGetterContexts[scenario], scenario, isBTL99)) {
                            // Condition for match checks here
                            var match = X1_Match_Func(filename, x1File);

                            // If the match is 'null', that means we're just skipping this file completely.
                            if (match == null) {
                                s_matchReports.Clear();
                                continue;
                            }

                            // List the file and any report we may have from X1_Match_Func().
                            var fileStr = GetFileString(scenario, file, x1File);
                            Console.WriteLine(fileStr + " | " + (match == true ? "Match  " : "NoMatch"));
                            foreach (var mr in s_matchReports)
                                Console.WriteLine("    " + mr);
                            Console.WriteLine();
                            s_matchReports.Clear();

                            if (match == true)
                                matchSet.Add(fileStr);
                            else
                                nomatchSet.Add(fileStr);

                            ScanForErrorsAndReport(scenario, x1File);
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }

            var totalCount = matchSet.Count + nomatchSet.Count;

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| MATCH RESULTS                                   |");
            Console.WriteLine("===================================================");

            Console.WriteLine("");
            Console.WriteLine($"Match: {matchSet.Count}/{totalCount}");
            foreach (var str in matchSet)
                Console.WriteLine("  " + str);

            Console.WriteLine($"NoMatch: {nomatchSet.Count}/{totalCount}");
            foreach (var str in nomatchSet)
                Console.WriteLine("  " + str);

            Console.WriteLine("");
            Console.WriteLine("Spawn Types:");
            Console.WriteLine("    " + string.Join(", ", s_allSpawnTypes.Distinct().OrderBy(x => x).Select(x => x.ToString("X2")).ToArray()));

            Console.WriteLine("Unknown 0x0E's:");
            Console.WriteLine("    " + string.Join(", ", s_allUnknown0x0Es.Distinct().OrderBy(x => x).Select(x => x.ToString("X2")).ToArray()));

            Console.WriteLine("AI Tags:");
            Console.WriteLine("    " + string.Join(", ", s_allAiTags.Distinct().OrderBy(x => x).Select(x => x.ToString("X2")).ToArray()));

            Console.WriteLine("AI Flags:");
            Console.WriteLine("    " + string.Join(", ", s_allAiTypes.Distinct().OrderBy(x => x).Select(x => x.ToString("X2")).ToArray()));

            Console.WriteLine("AI Aggrs:");
            Console.WriteLine("    " + string.Join(", ", s_allAiAggrs.Distinct().OrderBy(x => x).Select(x => x.ToString("X2")).ToArray()));
        }

        private static string BitString(uint bits) {
            var str = "";
            for (var i = 0; i < 32; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000_0000 >> i)) != 0 ? "1" : "0";
            }
            return str;
        }

        private static string BitString(ushort bits) {
            var str = "";
            for (var i = 0; i < 16; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000 >> i)) != 0 ? "1" : "0";
            }
            return str;
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, IX1_File x1File) {
            var typeStr = (x1File.IsBattle ? "Battle: " : "Town") + string.Join(", ", x1File.Battles?.Select(x => x.Key.ToString()) ?? [""]);
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12)
                + " | " + typeStr.PadRight(22)
                ;
        }

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, IX1_File mpdFile) {
            var totalErrors = new List<string>();

            // TODO: scan for errors

            foreach (var error in totalErrors)
                Console.WriteLine("    !!! " + error);
        }
    }
}
