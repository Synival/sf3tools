using System.Linq;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.X1;

namespace SF3.Win.Views.X1 {
    public class TechnicalView : TabView {
        public TechnicalView(string name, IX1_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.Discoveries?.HasDiscoveries == true) {
                var allDiscoveries = Model.Discoveries.GetAllOrdered();
                CreateChild(new TextView("All Discovered Data", Model.Discoveries.CreateReport(allDiscoveries, true)));
 
                var functions = allDiscoveries.Where(x => x.Type == DiscoveredDataType.Function).ToArray();
                if (functions.Length > 0)
                    CreateChild(new TextView("Functions", Model.Discoveries.CreateReport(functions, false)));

                var arrays = allDiscoveries.Where(x => x.Type == DiscoveredDataType.Array).ToArray();
                if (arrays.Length > 0)
                    CreateChild(new TextView("Arrays", Model.Discoveries.CreateReport(arrays, false)));

                var pointers = allDiscoveries.Where(x => x.Type == DiscoveredDataType.Pointer).ToArray();
                if (pointers.Length > 0) {
                    CreateChild(new TextView("Pointers", Model.Discoveries.CreateReport(pointers, false)));

                    var identifiedPointers = pointers.Where(x => !x.IsUnidentifiedPointer).ToArray();
                    if (identifiedPointers.Length > 0)
                        CreateChild(new TextView("Identified Pointers", Model.Discoveries.CreateReport(identifiedPointers, false)));

                    var unidentifiedPointers = pointers.Where(x => x.IsUnidentifiedPointer).ToArray();
                    if (unidentifiedPointers.Length > 0)
                        CreateChild(new TextView("Unidentified Pointers", Model.Discoveries.CreateReport(unidentifiedPointers, false)));
                }

                var unknowns = allDiscoveries.Where(x => x.Type == DiscoveredDataType.Unknown).ToArray();
                if (unknowns.Length > 0)
                    CreateChild(new TextView("Unknowns", Model.Discoveries.CreateReport(unknowns, false)));
            }

            return Control;
        }

        public IX1_File Model { get; }
    }
}
