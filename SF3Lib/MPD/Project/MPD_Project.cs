using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.Geometry;
using CommonLib.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using SF3.MPD.Interfaces.Flags;
using SF3.Types;

namespace SF3.MPD.Project {
    /// <summary>
    /// Abstracted, editable MPD file.
    /// </summary>
    public class MPD_Project : IMPD {
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

        public IMPD_EditableFlags Flags { get; }
        public IMPD_Settings Settings { get; }
        public IMPD_BinaryReproductionFlags BinaryReproductionFlags { get; }
        public IMPD_Surface Surface { get; }
        public Dictionary<MPD_CollectionType, IMPD_ModelCollection> ModelCollections { get; }
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
