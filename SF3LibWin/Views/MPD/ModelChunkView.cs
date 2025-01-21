﻿using System.Linq;
using System.Windows.Forms;
using SF3.Models.Tables.MPD.Model;
using static SF3.Models.Files.MPD.Models;

namespace SF3.Win.Views.MPD {
    public class ModelChunkView : TabView {
        public ModelChunkView(string name, Models.Files.MPD.Models model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Header", Model.ModelsHeaderTable, ngc));
            CreateChild(new TableView("Models", Model.ModelTable, ngc));
            CreateChild(new TableView("PDATAs", Model.PDataTable, ngc));
            CreateChild(new TableArrayView<VertexTable>("POINT[]s", Model.VertexTables.Values.ToArray(), ngc));
            CreateChild(new TableArrayView<PolygonTable>("POLYGON[]s", Model.PolygonTables.Values.ToArray(), ngc));
            CreateChild(new TableArrayView<AttrTable>("ATTR[]s", Model.AttrTables.Values.ToArray(), ngc));

            return Control;
        }

        public Models.Files.MPD.Models Model { get; }
    }
}
