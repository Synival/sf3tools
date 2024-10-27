using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.IconPointerEditor;
using static SF3.Utils.Resources;

namespace SF3.Tables.IconPointerEditor {
    public class SpellIconTable : Table<SpellIcon> {
        public override int? MaxSize => 256;

        public SpellIconTable(IIconPointerFileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
            _resourceFile = ResourceFileForScenario(_fileEditor.Scenario, "SpellIcons.xml");
        }

        private readonly string _resourceFile;
        private readonly IIconPointerFileEditor _fileEditor;

        public override string ResourceFile => _resourceFile;

        public override bool Load()
            => LoadFromResourceFile((id, name) => new SpellIcon(_fileEditor, id, name));
    }
}
