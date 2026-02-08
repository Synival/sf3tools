using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Geometry;
using CommonLib.Imaging;
using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.MPD.Interfaces.Flags;
using SF3.Types;

namespace SF3.MPD.Project {
    /// <summary>
    /// Abstracted, editable MPD file.
    /// </summary>
    public class MPD_Project : IMPD, IJsonResource {
        /// <summary>
        /// Makes a copy of an existing IMPD as an IMPD_Project.
        /// </summary>
        /// <param name="original">IMPD to copy.</param>
        public MPD_Project(IMPD original) {
            Flags = new MPD_Flags(this);

            if (original.Settings != null)
                Settings = new MPD_Settings(original.Settings);
            if (original.BinaryReproductionFlags != null)
                BinaryReproductionFlags = new MPD_BinaryReproductionFlags(original.BinaryReproductionFlags);
            if (original.Lighting != null)
                Lighting = new MPD_Lighting(original.Lighting);
            if (original.Surface != null)
                Surface = new MPD_Surface(Settings, original.Surface);
            if (original.Planes != null)
                Planes = new MPD_Planes(original.Planes);

            ModelCollections = new Dictionary<MPD_CollectionType, IMPD_ModelCollection>();
            if (original.ModelCollections != null)
                foreach (var modelCollection in original.ModelCollections)
                    ModelCollections.Add(modelCollection.Key, new MPD_ModelCollection(modelCollection.Value));

            if (original.ModelSwitchGroups != null)
                ModelSwitchGroups = original.ModelSwitchGroups.Select(x => (IMPD_ModelSwitchGroup) new MPD_ModelSwitchGroup(x)).ToArray().ToEnumerableWithLength();

            if (original.Scenario1UnknownTable1 != null)
                Scenario1UnknownTable1 = ((ushort[]) (original.Scenario1UnknownTable1.AsArray().Clone())).ToEnumerableWithLength();
            if (original.Scenario1UnknownTable2 != null)
                Scenario1UnknownTable2 = ((ushort[]) (original.Scenario1UnknownTable2.AsArray().Clone())).ToEnumerableWithLength();
            if (original.GroundAnimationData != null)
                GroundAnimationData = ((byte[]) (original.GroundAnimationData.AsArray().Clone())).ToEnumerableWithLength();

            if (original.CameraBoundaries != null)
                CameraBoundaries = new RectangleShort() { P1 = original.CameraBoundaries.P1, P2 = original.CameraBoundaries.P2 };
            if (original.BattleCursorBoundaries != null)
                BattleCursorBoundaries = new RectangleShort() { P1 = original.BattleCursorBoundaries.P1, P2 = original.BattleCursorBoundaries.P2 };

            if (original.Collisions != null)
                Collisions = new MPD_Collisions(original.Collisions);
            if (original.Gradient != null)
                Gradient = new MPD_Gradient(original.Gradient);

            if (original.TexturePalette != null)
                TexturePalette = new Palette(original.TexturePalette);
        }

        /// <summary>
        /// Creates a new project from a string in JSON format. The JSON expected is the same produced as the
        /// IMPD.ToJSON_String() extension method.
        /// </summary>
        /// <param name="jsonStr">Input string containing the entire MPD_Project in JSON format.</param>
        /// <returns>A newly constructed MPD_Project.</returns>
        public static MPD_Project FromJSON(string jsonStr)
            => new MPD_Project(jsonStr);

        /// <summary>
        /// Creates a new project from JSON object. The JSON expected is the same produced as the IMPD.ToJObject()
        /// extension method.
        /// </summary>
        /// <param name="jobj">JSON object containing the entire MPD_Project.</param>
        /// <returns>A newly constructed MPD_Project.</returns>
        public static MPD_Project FromJSON(JObject jObject)
            => new MPD_Project(jObject);

        private MPD_Project(string jsonStr) : this(JObject.Parse(jsonStr)) { }
        private MPD_Project(JObject jObject) {
            Flags = new MPD_Flags(this);
            AssignFromJObject(jObject);
        }

        public bool AssignFromJSON_String(string json) => AssignFromJObject(JObject.Parse(json));
        public bool AssignFromJToken(JToken jToken) => AssignFromJObject((JObject) jToken);
        public bool AssignFromJObject(JObject jObject) {
            Settings = jObject.GetValueIfExists("Settings", t => MPD_Settings.FromJToken(t));
            BinaryReproductionFlags = jObject.GetValueIfExists("BinaryReproductionFlags", t => MPD_BinaryReproductionFlags.FromJToken(t));
            Lighting = jObject.GetValueIfExists("Lighting", t => MPD_Lighting.FromJToken(t));
            Surface  = jObject.GetValueIfExists("Surface",  t => MPD_Surface.FromJToken(Settings, t));
            Planes   = jObject.GetValueIfExists("Planes",   t => MPD_Planes.FromJToken(t));

            TexturePalette = jObject.GetValueIfExists("TexturePalette", t => Palette.FromJToken(t));

            ModelCollections = new Dictionary<MPD_CollectionType, IMPD_ModelCollection>();
            if (jObject.TryGetValue("ModelCollections", out var modelCollectionsToken)) {
                foreach (var modelCollectionJObj in ((JObject) modelCollectionsToken).Properties()) {
                    var collectionType = (MPD_CollectionType) Enum.Parse(typeof(MPD_CollectionType), modelCollectionJObj.Name);
                    ModelCollections.Add(collectionType, MPD_ModelCollection.FromJToken(modelCollectionJObj.Value, collectionType, TexturePalette));
                }
            }

            ModelSwitchGroups = jObject.GetValueIfExists("ModelSwitchGroups",
                t => ((JArray) t)
                    .Select(x => (IMPD_ModelSwitchGroup) MPD_ModelSwitchGroup.FromJToken(x))
                    .ToArray()
                    .ToEnumerableWithLength()
                );

            Scenario1UnknownTable1 = jObject.GetValueIfExists("Scenario1UnknownTable1", t => ((JArray) t).Select(x => (ushort) (int) x).ToArray().ToEnumerableWithLength());
            Scenario1UnknownTable2 = jObject.GetValueIfExists("Scenario1UnknownTable2", t => ((JArray) t).Select(x => (ushort) (int) x).ToArray().ToEnumerableWithLength());
            GroundAnimationData    = jObject.GetValueIfExists("GroundAnimationData",    t => ((JArray) t).Select(x => (byte) x).ToArray().ToEnumerableWithLength());

            CameraBoundaries       = jObject.GetValueIfExists("CameraBoundaries",       t => RectangleShort.FromJToken(t));
            BattleCursorBoundaries = jObject.GetValueIfExists("BattleCursorBoundaries", t => RectangleShort.FromJToken(t));

            if (jObject.TryGetValue("Collisions", out var collisionsToken))
                Collisions = MPD_Collisions.FromJToken(collisionsToken);
            if (jObject.TryGetValue("Gradient", out var gradientToken))
                Gradient = null; // .FromJToken()

            return true;
        }

        public string ToJSON_String() => IMPD_Extensions.ToJSON_String(this);
        public JToken ToJToken() => IMPD_Extensions.ToJObject(this);

        public IMPD_EditableFlags Flags { get; }
        public IMPD_Settings Settings { get; private set; }
        public IMPD_BinaryReproductionFlags BinaryReproductionFlags { get; private set; }
        public IMPD_Surface Surface { get; private set; }
        public Dictionary<MPD_CollectionType, IMPD_ModelCollection> ModelCollections { get; private set; }
        public Palette TexturePalette { get; set; }
        public IMPD_Lighting Lighting { get; set; }
        public IIndexedEnumerableWithLength<IMPD_ModelSwitchGroup> ModelSwitchGroups { get; set; }
        public IMPD_Planes Planes { get; set; }
        public IMPD_Collisions Collisions { get; set; }
        public IRectangleShort CameraBoundaries { get; set; }
        public IRectangleShort BattleCursorBoundaries { get; set; }
        public IMPD_Gradient Gradient { get; set; }
        public IIndexedEnumerableWithLength<byte> GroundAnimationData { get; set; }
        public IIndexedEnumerableWithLength<ushort> Scenario1UnknownTable1 { get; set; }
        public IIndexedEnumerableWithLength<ushort> Scenario1UnknownTable2 { get; set; }

        public EventHandler ModelsUpdated { get; set; }
    }
}
