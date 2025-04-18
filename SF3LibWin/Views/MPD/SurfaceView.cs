﻿using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class SurfaceView : TabView {
        public SurfaceView(string name, Surface model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Heightmap",             Model.HeightmapRowTable, ngc));
            CreateChild(new TableView("Height + Terrain Type", Model.HeightTerrainRowTable, ngc));
            CreateChild(new TableView("Event IDs",             Model.EventIDRowTable, ngc));

            return Control;
        }

        public Surface Model { get; }
    }
}
