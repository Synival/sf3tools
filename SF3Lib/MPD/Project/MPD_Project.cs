using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Geometry;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using Newtonsoft.Json.Linq;
using SF3.Models.Files;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.MPD.Interfaces.Flags;
using SF3.Types;

namespace SF3.MPD.Project {
    /// <summary>
    /// Abstracted, editable MPD file.
    /// </summary>
    public class MPD_Project : IMPD, IJsonResource, IBaseFile {
        /// <summary>
        /// Creates a brand new MPD_Project.
        /// </summary>
        public MPD_Project() {
            Flags    = new MPD_Flags(this);
            Settings = new MPD_Settings();
            BinaryReproductionFlags = new MPD_BinaryReproductionFlags();
            Lighting = new MPD_Lighting();
            Surface  = new MPD_Surface(this, Settings);
            Planes   = new MPD_Planes();

            ModelCollections = new Dictionary<MPD_CollectionType, IMPD_ModelCollection>() {
                { MPD_CollectionType.Primary, new MPD_ModelCollection(MPD_CollectionType.Primary) }
            };

            CameraBoundaries = new RectangleShort() {
                P1 = new PointShort(64, 64),
                P2 = new PointShort(1984, 1984),
            };

            BattleCursorBoundaries = new RectangleShort() {
                P1 = new PointShort(0, 0),
                P2 = new PointShort(2048, 2048),
            };
        }

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
                Surface = new MPD_Surface(this, Settings, original.Surface);
            if (original.Planes != null)
                Planes = new MPD_Planes(original.Planes);

            ModelCollections = new Dictionary<MPD_CollectionType, IMPD_ModelCollection>();
            if (original.ModelCollections != null)
                foreach (var modelCollection in original.ModelCollections)
                    ModelCollections.Add(modelCollection.Key, new MPD_ModelCollection(modelCollection.Value));

            if (original.ModelSwitchGroups != null)
                ModelSwitchGroups = original.ModelSwitchGroups.Select(x => (IMPD_ModelSwitchGroup) new MPD_ModelSwitchGroup(x)).ToArray();

            if (original.Scenario1UnknownTable1 != null)
                Scenario1UnknownTable1 = original.Scenario1UnknownTable1.ToArray();
            if (original.Scenario1UnknownTable2 != null)
                Scenario1UnknownTable2 = original.Scenario1UnknownTable2.ToArray();
            if (original.GroundAnimationData != null)
                GroundAnimationData = original.GroundAnimationData.ToArray();

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

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    if (ModelCollections != null)
                        foreach (var mc in ModelCollections.Values)
                            mc.Dispose();

                _disposedValue = true;
            }
        }

        public bool AssignFromJSON_String(string json) => AssignFromJObject(JObject.Parse(json));
        public bool AssignFromJToken(JToken jToken) => AssignFromJObject((JObject) jToken);
        public bool AssignFromJObject(JObject jObject) {
            Settings = jObject.GetValueIfExists("Settings", t => MPD_Settings.FromJToken(t));
            BinaryReproductionFlags = jObject.GetValueIfExists("BinaryReproductionFlags", t => MPD_BinaryReproductionFlags.FromJToken(t));
            Lighting = jObject.GetValueIfExists("Lighting", t => MPD_Lighting.FromJToken(t));
            Surface  = jObject.GetValueIfExists("Surface",  t => MPD_Surface.FromJToken(this, Settings, t));
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
                );

            Scenario1UnknownTable1 = jObject.GetValueIfExists("Scenario1UnknownTable1", t => ((JArray) t).Select(x => (ushort) (int) x).ToArray());
            Scenario1UnknownTable2 = jObject.GetValueIfExists("Scenario1UnknownTable2", t => ((JArray) t).Select(x => (ushort) (int) x).ToArray());
            GroundAnimationData    = jObject.GetValueIfExists("GroundAnimationData",    t => ((JArray) t).Select(x => (byte) x).ToArray());
            CameraBoundaries       = jObject.GetValueIfExists("CameraBoundaries",       t => RectangleShort.FromJToken(t));
            BattleCursorBoundaries = jObject.GetValueIfExists("BattleCursorBoundaries", t => RectangleShort.FromJToken(t));
            Collisions             = jObject.GetValueIfExists("Collisions",             t => MPD_Collisions.FromJToken(t));
            Gradient               = jObject.GetValueIfExists("Gradient",               t => MPD_Gradient.FromJToken(t));

            return true;
        }

        public string ToJSON_String() => IMPD_Extensions.ToJSON_String(this);
        public JToken ToJToken() => IMPD_Extensions.ToJObject(this);

        public virtual void Save(string filename) {
            var serializedMPD = ToJSON_String();
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var writer = new StreamWriter(stream)) {
                writer.NewLine = "\n";
                writer.Write(serializedMPD);
            }
        }

        // TODO: Implement these!
        public string[] GetErrors() => new string[0];
        public bool Finish() => true;
        public ScopeGuard IsModifiedChangeBlocker() => new ScopeGuard(() => {}, () => {});

        public IMPD_EditableFlags Flags { get; }
        public IMPD_Settings Settings { get; private set; }
        public IMPD_BinaryReproductionFlags BinaryReproductionFlags { get; private set; }
        public IMPD_Surface Surface { get; private set; }
        public Dictionary<MPD_CollectionType, IMPD_ModelCollection> ModelCollections { get; private set; }
        public Palette TexturePalette { get; set; }
        public IMPD_Lighting Lighting { get; set; }
        public IReadOnlyList<IMPD_ModelSwitchGroup> ModelSwitchGroups { get; set; }
        public IMPD_Planes Planes { get; set; }
        public IMPD_Collisions Collisions { get; set; }
        public IRectangleShort CameraBoundaries { get; set; }
        public IRectangleShort BattleCursorBoundaries { get; set; }
        public IMPD_Gradient Gradient { get; set; }
        public IReadOnlyList<byte> GroundAnimationData { get; set; }
        public IReadOnlyList<ushort> Scenario1UnknownTable1 { get; set; }
        public IReadOnlyList<ushort> Scenario1UnknownTable2 { get; set; }

        public INameGetterContext NameGetterContext => null;
        public string Title => "";
        public bool IsModified { get; set; }

        private bool _disposedValue;

        public event EventHandler ModelsUpdated;
        public event EventHandler Finished;
        public event EventHandler IsModifiedChanged;
    }
}
