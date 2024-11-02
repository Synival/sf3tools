using System;
using SF3.FileEditors;
using SF3.Models;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class LoadingTable : Table<Loading> {
        public LoadingTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            ResourceFile = ResourceFileForScenario(FileEditor.Scenario, "LoadList.xml");
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Loading(FileEditor, id, name, address));

        public override string ResourceFile { get; }
        public override int Address { get; }
        public override int? MaxSize => 300;
    }
}
