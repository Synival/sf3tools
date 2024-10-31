using System;
using SF3.FileEditors;
using SF3.Models.X002;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables.X002 {
    public class LoadingTable : Table<Loading> {
        public override int? MaxSize => 300;

        public LoadingTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "LoadList.xml");
            Address = address;
        }

        public override string ResourceFile { get; }
        public override int Address { get; }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Loading(FileEditor, id, name, address));
    }
}
