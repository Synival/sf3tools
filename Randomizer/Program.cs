using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Files.X033_X031;
using SF3.Models.Structs.X033_X031;
using SF3.NamedValues;
using SF3.Types;

namespace Randomizer {
    public class Program {
        private static readonly string c_filePath = "F:/";
        private static readonly ScenarioType c_scenario = ScenarioType.Scenario3;
        private static readonly Random s_random = new Random();

        /// <summary>
        /// Representation of a single character, with all possible information.
        /// </summary>
        public class Character {
            public Character(int id, Dictionary<Stats.PromotionLevelType, Stats> stats, InitialInfo initialInfo) {
                ID          = id;
                Stats       = stats;
                InitialInfo = initialInfo;
            }

            public int ID { get; }
            public Dictionary<Stats.PromotionLevelType, Stats> Stats { get; }
            public InitialInfo InitialInfo { get; }

            // TODO: needs classes
        }

        /// <summary>
        /// A single snapshot of stats that increase per level.
        /// </summary>
        public class StatSnapshot {
            public StatSnapshot(double hp, double mp, double atk, double def, double agi) {
                HP  = hp;
                MP  = mp;
                Atk = atk;
                Def = def;
                Agi = agi;
            }

            public static StatSnapshot operator *(StatSnapshot lhs, StatSnapshot rhs) {
                return new StatSnapshot(
                    lhs.HP * rhs.HP,
                    lhs.MP * rhs.MP,
                    lhs.Atk * rhs.Atk,
                    lhs.Def * rhs.Def,
                    lhs.Agi * rhs.Agi
                );
            }

            public static StatSnapshot operator /(StatSnapshot lhs, StatSnapshot rhs) {
                return new StatSnapshot(
                    lhs.HP / rhs.HP,
                    (lhs.MP == 0.00 && rhs.MP == 0.00) ? 0.50f : lhs.MP / rhs.MP,
                    lhs.Atk / rhs.Atk,
                    lhs.Def / rhs.Def,
                    lhs.Agi / rhs.Agi
                );
            }

            public static StatSnapshot operator +(StatSnapshot lhs, StatSnapshot rhs) {
                return new StatSnapshot(
                    lhs.HP + rhs.HP,
                    lhs.MP + rhs.MP,
                    lhs.Atk + rhs.Atk,
                    lhs.Def + rhs.Def,
                    lhs.Agi + rhs.Agi
                );
            }

            public static StatSnapshot operator -(StatSnapshot lhs, StatSnapshot rhs) {
                return new StatSnapshot(
                    lhs.HP - rhs.HP,
                    lhs.MP - rhs.MP,
                    lhs.Atk - rhs.Atk,
                    lhs.Def - rhs.Def,
                    lhs.Agi - rhs.Agi
                );
            }

            public static StatSnapshot operator *(StatSnapshot lhs, double rhs) {
                return new StatSnapshot(
                    lhs.HP * rhs,
                    lhs.MP * rhs,
                    lhs.Atk * rhs,
                    lhs.Def * rhs,
                    lhs.Agi * rhs
                );
            }

            public static StatSnapshot operator /(StatSnapshot lhs, double rhs) {
                return new StatSnapshot(
                    lhs.HP / rhs,
                    lhs.MP / rhs,
                    lhs.Atk / rhs,
                    lhs.Def / rhs,
                    lhs.Agi / rhs
                );
            }

            public override string ToString() => $"{HP}, {MP}, {Atk}, {Def}, {Agi}";

            public double HP { get; set; }
            public double MP { get; set; }
            public double Atk { get; set; }
            public double Def { get; set; }
            public double Agi { get; set; }
        };

        /// <summary>
        /// All stats that increase per level or promotion for a class, with curves.
        /// </summary>
        public class StatGainData {
            public StatGainData(Stats[] stats) {
                Curves = [
                    new StatSnapshot(stats.Average(x => x.HPCurve1),     stats.Average(x => x.MPCurve1),     stats.Average(x => x.AtkCurve1),     stats.Average(x => x.DefCurve1),     stats.Average(x => x.AgiCurve1)),
                    new StatSnapshot(stats.Average(x => x.HPCurve5),     stats.Average(x => x.MPCurve5),     stats.Average(x => x.AtkCurve5),     stats.Average(x => x.DefCurve5),     stats.Average(x => x.AgiCurve5)),
                    new StatSnapshot(stats.Average(x => x.HPCurve10),    stats.Average(x => x.MPCurve10),    stats.Average(x => x.AtkCurve10),    stats.Average(x => x.DefCurve10),    stats.Average(x => x.AgiCurve10)),
                    new StatSnapshot(stats.Average(x => x.HPCurve12_15), stats.Average(x => x.MPCurve12_15), stats.Average(x => x.AtkCurve12_15), stats.Average(x => x.DefCurve12_15), stats.Average(x => x.AgiCurve12_15)),
                    new StatSnapshot(stats.Average(x => x.HPCurve14_20), stats.Average(x => x.MPCurve14_20), stats.Average(x => x.AtkCurve14_20), stats.Average(x => x.DefCurve14_20), stats.Average(x => x.AgiCurve14_20)),
                    new StatSnapshot(stats.Average(x => x.HPCurve17_30), stats.Average(x => x.MPCurve17_30), stats.Average(x => x.AtkCurve17_30), stats.Average(x => x.DefCurve17_30), stats.Average(x => x.AgiCurve17_30)),
                    new StatSnapshot(stats.Average(x => x.HPCurve30_99), stats.Average(x => x.MPCurve30_99), stats.Average(x => x.AtkCurve30_99), stats.Average(x => x.DefCurve30_99), stats.Average(x => x.AgiCurve30_99)),
                ];

                StatGainOnPromotion = new StatSnapshot(stats.Average(x => x.HPPromote), stats.Average(x => x.MPPromote), stats.Average(x => x.AtkPromote), stats.Average(x => x.DefPromote), stats.Average(x => x.AgiPromote));

                MagicBonus = stats.Average(x => x.MagicBonus);
                Slow       = stats.Average(x => x.Slow);
                Support    = stats.Average(x => x.Support);

                ElementRes = [
                    stats.Average(x => x.EarthRes),
                    stats.Average(x => x.FireRes),
                    stats.Average(x => x.IceRes),
                    stats.Average(x => x.SparkRes),
                    stats.Average(x => x.WindRes),
                    stats.Average(x => x.LightRes),
                    stats.Average(x => x.DarkRes),
                    stats.Average(x => x.UnknownRes),
                ];
            }

            public StatGainData(StatSnapshot[] curves, StatSnapshot statGainOnPromotion, double magicBonus, double slow, double support, double[] elementRes) {
                Curves              = curves;
                StatGainOnPromotion = statGainOnPromotion;
                MagicBonus          = magicBonus;
                Slow                = slow;
                Support             = support;
                ElementRes          = elementRes;
            }

            public static StatGainData operator *(StatGainData lhs, StatGainData rhs) {
                if (lhs.Curves.Length != rhs.Curves.Length)
                    throw new ArgumentException("Lengths of property '" + nameof(lhs.Curves) + "' don't match: " + nameof(lhs) + ", " + nameof(rhs));
                if (lhs.ElementRes.Length != rhs.ElementRes.Length)
                    throw new ArgumentException("Lengths of property '" + nameof(lhs.ElementRes) + "' don't match: " + nameof(lhs) + ", " + nameof(rhs));

                return new StatGainData(
                    lhs.Curves.Select((x, i) => x * rhs.Curves[i]).ToArray(),
                    lhs.StatGainOnPromotion * rhs.StatGainOnPromotion,
                    lhs.MagicBonus * rhs.MagicBonus,
                    lhs.Slow       * rhs.Slow,
                    lhs.Support    * rhs.Support,
                    lhs.ElementRes.Select((x, i) => x * rhs.ElementRes[i]).ToArray()
                );
            }

            public static StatGainData operator /(StatGainData lhs, StatGainData rhs) {
                if (lhs.Curves.Length != rhs.Curves.Length)
                    throw new ArgumentException("Lengths of property '" + nameof(lhs.Curves) + "' don't match: " + nameof(lhs) + ", " + nameof(rhs));
                if (lhs.ElementRes.Length != rhs.ElementRes.Length)
                    throw new ArgumentException("Lengths of property '" + nameof(lhs.ElementRes) + "' don't match: " + nameof(lhs) + ", " + nameof(rhs));

                return new StatGainData(
                    lhs.Curves.Select((x, i) => x / rhs.Curves[i]).ToArray(),
                    lhs.StatGainOnPromotion / rhs.StatGainOnPromotion,
                    (lhs.MagicBonus == 0 & rhs.MagicBonus == 0) ? 0.5 : lhs.MagicBonus / rhs.MagicBonus,
                    (lhs.Slow       == 0 & rhs.Slow       == 0) ? 0.5 : lhs.Slow / rhs.Slow,
                    (lhs.Support    == 0 & rhs.Support    == 0) ? 0.5 : lhs.Support / rhs.Support,
                    lhs.ElementRes.Select((x, i) => (rhs.ElementRes[i] == 0) ? 0 : x / rhs.ElementRes[i]).ToArray()
                );
            }

            public static StatGainData operator +(StatGainData lhs, StatGainData rhs) {
                if (lhs.Curves.Length != rhs.Curves.Length)
                    throw new ArgumentException("Lengths of property 'Curves' don't match: " + nameof(lhs) + ", " + nameof(rhs));

                return new StatGainData(
                    lhs.Curves.Select((x, i) => x + rhs.Curves[i]).ToArray(),
                    lhs.StatGainOnPromotion + rhs.StatGainOnPromotion,
                    lhs.MagicBonus + rhs.MagicBonus,
                    lhs.Slow + rhs.Slow,
                    lhs.Support + rhs.Support,
                    lhs.ElementRes.Select((x, i) => x + rhs.ElementRes[i]).ToArray()
                );
            }

            public static StatGainData operator *(StatGainData lhs, double rhs) {
                return new StatGainData(
                    lhs.Curves.Select(x => x * rhs).ToArray(),
                    lhs.StatGainOnPromotion * rhs,
                    lhs.MagicBonus * rhs,
                    lhs.Slow * rhs,
                    lhs.Support * rhs,
                    lhs.ElementRes.Select((x, i) => x * rhs).ToArray()
                );
            }

            public static StatGainData operator /(StatGainData lhs, double rhs) {
                return new StatGainData(
                    lhs.Curves.Select(x => x / rhs).ToArray(),
                    lhs.StatGainOnPromotion / rhs,
                    lhs.MagicBonus / rhs,
                    lhs.Slow / rhs,
                    lhs.Support / rhs,
                    lhs.ElementRes.Select((x, i) => x / rhs).ToArray()
                );
            }

            public StatSnapshot[] Curves { get; set; }
            public StatSnapshot StatGainOnPromotion { get; set; }

            public double MagicBonus { get; set; }
            public double Slow { get; set; }
            public double Support { get; set; }

            public double[] ElementRes { get; set; }
        }

        /// <summary>
        /// A weapon class as used by a class, with all the specials that come along with it.
        /// </summary>
        public class WeaponClassWithSpecials {
            public WeaponClassWithSpecials(int weapon, int[] specials) {
                Weapon   = weapon;
                Specials = specials;
            }

            public int Weapon { get; }
            public int[] Specials { get; }

            public override bool Equals(object? obj)
                => obj is WeaponClassWithSpecials specials && Weapon == specials.Weapon && EqualityComparer<int[]>.Default.Equals(Specials, specials.Specials);

            public override int GetHashCode() => HashCode.Combine(Weapon, Specials);
        }

        /// <summary>
        /// A single class at a single level of promotion. Has averages of stats and possible equipment,
        /// possible weapons, movement types, and spell pools.
        /// TODO: more than one movement type, but perhaps this should be based on race?
        /// TODO: spells
        /// </summary>
        public class Class {
            public Class(int id, string className, Stats[] stats) {
                ID   = id;
                Name = className;

                StatGainData = new StatGainData(stats);
                Luck = stats.Average(x => x.BaseLuck);
                Move = stats.Average(x => x.BaseMov);

                if (!stats.All(x => x.AccessoryEquipable1 == stats[0].AccessoryEquipable1))
                    throw new ArgumentException("AccessoryEquipable1 doesn't match");
                if (!stats.All(x => x.AccessoryEquipable2 == stats[0].AccessoryEquipable2))
                    throw new ArgumentException("AccessoryEquipable2 doesn't match");
                if (!stats.All(x => x.AccessoryEquipable3 == stats[0].AccessoryEquipable3))
                    throw new ArgumentException("AccessoryEquipable3 doesn't match");
                if (!stats.All(x => x.AccessoryEquipable4 == stats[0].AccessoryEquipable4))
                    throw new ArgumentException("AccessoryEquipable4 doesn't match");

                Accessories = [
                    stats[0].AccessoryEquipable1,
                    stats[0].AccessoryEquipable2,
                    stats[0].AccessoryEquipable3,
                    stats[0].AccessoryEquipable4,
                ];

                var weaponsPerStat = stats
                    .ToDictionary(
                        x => x, x => new WeaponClassWithSpecials[] {
                            new WeaponClassWithSpecials(x.WeaponEquipable1, new int[] { x.Weapon1Special1, x.Weapon1Special2, x.Weapon1Special3 }),
                            new WeaponClassWithSpecials(x.WeaponEquipable2, new int[] { x.Weapon2Special1, x.Weapon2Special2, x.Weapon2Special3 }),
                            new WeaponClassWithSpecials(x.WeaponEquipable3, new int[] { x.Weapon3Special1, x.Weapon3Special2, x.Weapon3Special3 }),
                            new WeaponClassWithSpecials(x.WeaponEquipable4, new int[] { x.Weapon4Special1, x.Weapon4Special2, x.Weapon4Special3 }),
                        }.Where(x => x.Weapon != 0).ToArray()
                    );

                Weapons = weaponsPerStat
                    .SelectMany(x => x.Value)
                    .Distinct()
                    .ToArray();
                WeaponCount = weaponsPerStat.Average(x => x.Value.Length);

                MovementType = stats.GroupBy(x => x.MovementType).OrderByDescending(x => x.Count()).First().Key;
            }

            public Class(int id, string name, StatGainData statGainData, double luck, double move, int[] accessories, WeaponClassWithSpecials[] weapons, double weaponCount, int movementType) {
                ID             = id;
                Name           = name;
                StatGainData   = statGainData;
                Luck           = luck;
                Move           = move;
                Accessories    = accessories;
                Weapons        = weapons;
                WeaponCount    = weaponCount;
                MovementType   = movementType;
            }

            public override string ToString() => $"{Name} (0x{ID:X2})";

            public int ID { get; }
            public string Name { get; }

            public StatGainData StatGainData { get; set; }
            public double Luck { get; set; }
            public double Move { get; set; }

            public int[] Accessories { get; set; }
            public WeaponClassWithSpecials[] Weapons { get; set; }
            public double WeaponCount { get; set; }

            public int MovementType { get; set; }
        }

        public static void Main(string[] args) {
            // Gather are initial stats for each character. This is something we'll ultimately want to modify.
            var initialInfosByID = s_x033.InitialInfoTable
                .Where(x => x.CharacterE != 0xFF)
                .ToDictionary(x => x.CharacterE, x => x);

            // Gather a list of all characters, with all their relevant info.
            // Classes don't exist yet, so we'll add those later.
            // TODO: actually add those later
            var characters = s_x033.StatsTable
                .Where(x => x.CharacterID != 0xFF)
                .GroupBy(x => x.CharacterID)
                .Select(x => new Character(x.Key, x.ToDictionary(y => y.PromotionLevel), initialInfosByID[x.Key]))
                .ToList();

            // Dictionary of characters so we can reference their data by their ID
            var charactersByID = characters
                .ToDictionary(x => x.ID);

            // A list of class IDs that are actually used in-game. We'll need this to build classes.
            var usedCharacterClassIDs = charactersByID
                .SelectMany(x => x.Value.Stats.Values)
                .Select(x => x.CharacterClass)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            // A list of all possible class IDs, including non-existant ones like "Child" and "Mecha Bot".
            var characterClassIDs = Enumerable.Range(0, usedCharacterClassIDs.Max() + 1).ToList();

            // Fetch the names of these classes. Not strictly necessary, but this sure is helpful to have!
            var characterClassNames = characterClassIDs
                .ToDictionary(x => x, x => s_ngc.GetName(null, null, x, [NamedValueType.CharacterClass]));

            // Get the list of class names that aren't used. These are the classes we'll have to build.
            var unusedCharacterClassNames = characterClassNames
                .Where(x => !usedCharacterClassIDs.Any(y => x.Key == y))
                .ToList();

            // Collect all character stats used for each class. This will be used to figure out the average/possible stats for the class.
            var statsByClass = characters
                .SelectMany(x => x.Stats.Values)
                .GroupBy(x => x.CharacterClass)
                .ToDictionary(x => new KeyValuePair<int, string>(x.Key, characterClassNames[x.Key]));

            // Create concrete classes (not just IDs with names) based on the stats wethat have been collected.
            // (This won't have any unused classes; we'll still have to add those.)
            var classes = statsByClass
                .Select(x => new Class(x.Key.Key, x.Key.Value, x.Value.ToArray()))
                .OrderBy(x => x.ID)
                .ToList();

            // Dictionary of classes so we can reference them by ID
            // (Again, no unused classes)
            var classesByID = classes
                .ToDictionary(x => x.ID);

            // Promoted classes exist for all characters. Let's use this as the basis for building new unpromoted classes.
            // Organize all characters by their promoted class. We're going to use this to see which class groups (all promotions)
            // don't have any characters with unpromoted classes.
            var charactersByPromotedClass = charactersByID
                .Select(x => new { PromotedStats = x.Value.Stats[Stats.PromotionLevelType.Promotion1], Character = x.Value })
                .GroupBy(x => x.PromotedStats.CharacterClass)
                .OrderBy(x => x.Key)
                .ToDictionary(x => classesByID[x.Key], x => x.Select(y => y.Character).ToList());

            // Get the classes without unpromoted versions along with their characters.
            var classesWithoutFirstPromotion = charactersByPromotedClass
                .Where(x => !x.Value.Any(y => y.Stats.ContainsKey(Stats.PromotionLevelType.Unpromoted)))
                .ToDictionary();

            // Create a map of 1st promotion to 2nd promotion classes so we can use that for comparisons.
            var promoted1ToPromoted2Map = charactersByPromotedClass
                .SelectMany(x => x.Value.Select(y => new KeyValuePair<Class, Class>(x.Key, classesByID[y.Stats[Stats.PromotionLevelType.Promotion2].CharacterClass])))
                .GroupBy(x => x.Key.ID)
                .OrderBy(x => x.Key)
                .Select(x => new KeyValuePair<Class, List<Class>>(
                    x.First().Key,
                    x.Select(y => y.Value).Distinct().ToList()
                ))
                .ToDictionary();

            // Create a map of all existing unpromoted classes to their promoted classes so we can use that for comparisons.
            var unpromotedToPromotedMap = classes
                .Where(x => x.ID < 0x20)
                .ToDictionary(x => x, x => classesByID[x.ID + 0x20]);

            // Let's generate some statistics! Let's gather ratios of stats for unpromoted classes to promoted classes and all
            // points of their level curve.
            var unpromotedFromPromotedPercentages = unpromotedToPromotedMap
                .ToDictionary(x => x, x => x.Key.StatGainData / x.Value.StatGainData);

            // Calculate the average of the ratios above. We're going to use this to generate curves for new unpromoted classes.
            var avgUnpromotedFromPromotedPercentages = unpromotedFromPromotedPercentages.Values
                .Aggregate((a, b) => a + b) / unpromotedFromPromotedPercentages.Count;

            // Finally create a list of all the unpromoted classes we'll need to make and their promoted counterparts.
            // Fortunately for us, the names of these non-existant classes are in the same order as their promoted counterparts,
            // so we can simply go down the list in order.
            int index = 0;
            var classesWeWillNeedToMake = classesWithoutFirstPromotion
                .ToDictionary(x => x.Key, x => new KeyValuePair<int, string>(unusedCharacterClassNames[index].Key, unusedCharacterClassNames[index++].Value));

            // Make all the classes!
            foreach (var classKv in classesWeWillNeedToMake) {
                var unpromotedClassInfo = classKv.Value;
                var promotedClass       = classKv.Key;
                var promoted2Classes    = promoted1ToPromoted2Map[promotedClass];
                if (promoted2Classes.Count != 1)
                    throw new InvalidOperationException("Not sure how to make a new class out of this when there are multiple second promotion options");
                var promoted2Class = promoted2Classes[0];

                var characterClassID = unpromotedClassInfo.Key;

                var sgd     = promotedClass.StatGainData;
                var sgd2    = promoted2Class.StatGainData;
                var sgdMult = avgUnpromotedFromPromotedPercentages;

                // Most of the info per class is the same as the promoted counterpart except for stat growths.
                // Simply multiply the promoted stat growths by the average ratio of unpromoted : promoted stats to
                // get a reasonable-looking new class.
                // (This is something that should be done by hand at some point. But this is a good start.)
                var unpromotedClass = new Class(
                    unpromotedClassInfo.Key,
                    unpromotedClassInfo.Value,
                    sgd * sgdMult,
                    promotedClass.Luck,
                    promotedClass.Move,
                    promotedClass.Accessories,
                    promotedClass.Weapons,
                    promotedClass.WeaponCount,
                    promotedClass.MovementType
                );

                // Elemental resistance isn't done so well using the method above... It's usually the same as the promoted class.
                // Use the same elemental resistances as the promoted class, but apply the *opposite* of the gains/losses the class
                // sees when promoting to their 2nd promotion.
                for (var i = 0; i < 8; i++)
                    unpromotedClass.StatGainData.ElementRes[i] = sgd.ElementRes[i] * 2 - sgd2.ElementRes[i];

                // (Old code that assigns actual stats. This won't be used as-is, but I don't want to type all this again, so I'm keeping it around)
#if false
                stats.HPCurve1  = (int) Math.Round(sgd.Curves[0].HP  * sgdMult.Curves[0].HP);
                stats.MPCurve1  = (int) Math.Round(sgd.Curves[0].MP  * sgdMult.Curves[0].MP);
                stats.AtkCurve1 = (int) Math.Round(sgd.Curves[0].Atk * sgdMult.Curves[0].Atk);
                stats.DefCurve1 = (int) Math.Round(sgd.Curves[0].Def * sgdMult.Curves[0].Def);
                stats.AgiCurve1 = (int) Math.Round(sgd.Curves[0].Agi * sgdMult.Curves[0].Agi);

                stats.HPCurve5  = (int) Math.Round(sgd.Curves[1].HP  * sgdMult.Curves[1].HP);
                stats.MPCurve5  = (int) Math.Round(sgd.Curves[1].MP  * sgdMult.Curves[1].MP);
                stats.AtkCurve5 = (int) Math.Round(sgd.Curves[1].Atk * sgdMult.Curves[1].Atk);
                stats.DefCurve5 = (int) Math.Round(sgd.Curves[1].Def * sgdMult.Curves[1].Def);
                stats.AgiCurve5 = (int) Math.Round(sgd.Curves[1].Agi * sgdMult.Curves[1].Agi);

                stats.HPCurve10  = (int) Math.Round(sgd.Curves[2].HP  * sgdMult.Curves[2].HP);
                stats.MPCurve10  = (int) Math.Round(sgd.Curves[2].MP  * sgdMult.Curves[2].MP);
                stats.AtkCurve10 = (int) Math.Round(sgd.Curves[2].Atk * sgdMult.Curves[2].Atk);
                stats.DefCurve10 = (int) Math.Round(sgd.Curves[2].Def * sgdMult.Curves[2].Def);
                stats.AgiCurve10 = (int) Math.Round(sgd.Curves[2].Agi * sgdMult.Curves[2].Agi);

                stats.HPCurve12_15  = (int) Math.Round(sgd.Curves[3].HP  * sgdMult.Curves[3].HP);
                stats.MPCurve12_15  = (int) Math.Round(sgd.Curves[3].MP  * sgdMult.Curves[3].MP);
                stats.AtkCurve12_15 = (int) Math.Round(sgd.Curves[3].Atk * sgdMult.Curves[3].Atk);
                stats.DefCurve12_15 = (int) Math.Round(sgd.Curves[3].Def * sgdMult.Curves[3].Def);
                stats.AgiCurve12_15 = (int) Math.Round(sgd.Curves[3].Agi * sgdMult.Curves[3].Agi);

                stats.HPCurve14_20  = (int) Math.Round(sgd.Curves[4].HP  * sgdMult.Curves[4].HP);
                stats.MPCurve14_20  = (int) Math.Round(sgd.Curves[4].MP  * sgdMult.Curves[4].MP);
                stats.AtkCurve14_20 = (int) Math.Round(sgd.Curves[4].Atk * sgdMult.Curves[4].Atk);
                stats.DefCurve14_20 = (int) Math.Round(sgd.Curves[4].Def * sgdMult.Curves[4].Def);
                stats.AgiCurve14_20 = (int) Math.Round(sgd.Curves[4].Agi * sgdMult.Curves[4].Agi);

                stats.HPCurve17_30  = (int) Math.Round(sgd.Curves[5].HP  * sgdMult.Curves[5].HP);
                stats.MPCurve17_30  = (int) Math.Round(sgd.Curves[5].MP  * sgdMult.Curves[5].MP);
                stats.AtkCurve17_30 = (int) Math.Round(sgd.Curves[5].Atk * sgdMult.Curves[5].Atk);
                stats.DefCurve17_30 = (int) Math.Round(sgd.Curves[5].Def * sgdMult.Curves[5].Def);
                stats.AgiCurve17_30 = (int) Math.Round(sgd.Curves[5].Agi * sgdMult.Curves[5].Agi);

                stats.HPCurve30_99  = (int) Math.Round(sgd.Curves[6].HP  * sgdMult.Curves[6].HP);
                stats.MPCurve30_99  = (int) Math.Round(sgd.Curves[6].MP  * sgdMult.Curves[6].MP);
                stats.AtkCurve30_99 = (int) Math.Round(sgd.Curves[6].Atk * sgdMult.Curves[6].Atk);
                stats.DefCurve30_99 = (int) Math.Round(sgd.Curves[6].Def * sgdMult.Curves[6].Def);
                stats.AgiCurve30_99 = (int) Math.Round(sgd.Curves[6].Agi * sgdMult.Curves[6].Agi);

                stats.HPPromote  = (int) Math.Round(sgd.Curves[0].HP  - stats.HPCurve10);
                stats.MPPromote  = (int) Math.Round(sgd.Curves[0].MP  - stats.MPCurve10);
                stats.AtkPromote = (int) Math.Round(sgd.Curves[0].Atk - stats.AtkCurve10);
                stats.DefPromote = (int) Math.Round(sgd.Curves[0].Def - stats.DefCurve10);
                stats.AgiPromote = (int) Math.Round(sgd.Curves[0].Agi - stats.AgiCurve10);

                stats.BaseLuck = (int) Math.Round(promotedClass.Luck);
                stats.BaseMov  = (int) Math.Round(promotedClass.Move);

                stats.AccessoryEquipable1 = promotedClass.Accessories[0];
                stats.AccessoryEquipable2 = promotedClass.Accessories[1];
                stats.AccessoryEquipable3 = promotedClass.Accessories[2];
                stats.AccessoryEquipable4 = promotedClass.Accessories[3];

                var randomWeapons = promotedClass.Weapons.OrderBy(x => s_random.Next(256)).ToArray();
                var weaponCount = Math.Min(randomWeapons.Length, (int) Math.Round(promotedClass.WeaponCount));

                if (0 < weaponCount) {
                    stats.WeaponEquipable1 = randomWeapons[0].Weapon;
                    stats.Weapon1Special1  = randomWeapons[0].Specials[0];
                    stats.Weapon1Special2  = randomWeapons[0].Specials[1];
                    stats.Weapon1Special3  = randomWeapons[0].Specials[2];
                }

                if (1 < weaponCount) {
                    stats.WeaponEquipable2 = randomWeapons[1].Weapon;
                    stats.Weapon2Special1  = randomWeapons[1].Specials[0];
                    stats.Weapon2Special2  = randomWeapons[1].Specials[1];
                    stats.Weapon2Special3  = randomWeapons[1].Specials[2];
                }

                if (2 < weaponCount) {
                    stats.WeaponEquipable3 = randomWeapons[2].Weapon;
                    stats.Weapon3Special1  = randomWeapons[2].Specials[0];
                    stats.Weapon3Special2  = randomWeapons[2].Specials[1];
                    stats.Weapon3Special3  = randomWeapons[2].Specials[2];
                }

                if (3 < weaponCount) {
                    stats.WeaponEquipable4 = randomWeapons[3].Weapon;
                    stats.Weapon4Special1  = randomWeapons[3].Specials[0];
                    stats.Weapon4Special2  = randomWeapons[3].Specials[1];
                    stats.Weapon4Special3  = randomWeapons[3].Specials[2];
                }

                stats.BaseTurns = 1;

                stats.MovementType = promotedClass.MovementType;
                stats.MagicBonus   = (int) Math.Round(sgd.MagicBonus * sgdMult.MagicBonus);
                stats.Slow         = (int) Math.Round(sgd.Slow       * sgdMult.Slow);
                stats.Support      = (int) Math.Round(sgd.Support    * sgdMult.Support);

                stats.EarthRes     = (int) Math.Round(sgd.ElementRes[0] * 2 - sgd2.ElementRes[0]);
                stats.FireRes      = (int) Math.Round(sgd.ElementRes[1] * 2 - sgd2.ElementRes[1]);
                stats.IceRes       = (int) Math.Round(sgd.ElementRes[2] * 2 - sgd2.ElementRes[2]);
                stats.SparkRes     = (int) Math.Round(sgd.ElementRes[3] * 2 - sgd2.ElementRes[3]);
                stats.WindRes      = (int) Math.Round(sgd.ElementRes[4] * 2 - sgd2.ElementRes[4]);
                stats.LightRes     = (int) Math.Round(sgd.ElementRes[5] * 2 - sgd2.ElementRes[5]);
                stats.DarkRes      = (int) Math.Round(sgd.ElementRes[6] * 2 - sgd2.ElementRes[6]);
                stats.UnknownRes   = (int) Math.Round(sgd.ElementRes[7] * 2 - sgd2.ElementRes[7]);

                // TODO: spells, but that's a whole different beast
#endif
            }

            // At this point, we should have all the classes we need, although they're incomplete and likely need a lot of tweaking.
            // They need to be linked together as a 'ClassGroup' with all 3 promotions.
            //
            // The big thing left to do is figure out how to handle spells. Some calculations need to be done:
            //    - At what level to characters learn spells based on their MP pool?
            //    - How many spells do characters have based on their MP pool?
            //    - What ranking of spells do characters have bsaed on their MP pool?
            //    - How do elemental strengths effect the levels/rankings for spells?
            //    - What classes get an artificial boost/penalty in spell rankings? (Heroes, for example)
            //    - What spells are actually available for use in the game? (I'm so sorry, Scenario 1+2)
            //
            // After that, each 'ClassGroup' can get its spell pool, based on rankings.
            //
            // The next step is to actually generate characters using the ClassGroup base stats, possible weapons, and spell pool.
            //
            // Once that's working, we can actually make changes to the X033/X031 files.
        }

        /// <summary>
        /// Applies an action on both X033 and X031 files to avoid redundant code.
        /// </summary>
        private static void OnX033_X031(Action<IX033_X031_File> action) {
            action(s_x031);
            action(s_x033);
        }

        private static NameGetterContext s_ngc = new NameGetterContext(c_scenario);
        private static IX033_X031_File s_x033 = X033_X031_File.Create(new ByteData(new ByteArray(File.ReadAllBytes(c_filePath + "X033.BIN"))), s_ngc, c_scenario);
        private static IX033_X031_File s_x031 = X033_X031_File.Create(new ByteData(new ByteArray(File.ReadAllBytes(c_filePath + "X031.BIN"))), s_ngc, c_scenario);
    }
}
