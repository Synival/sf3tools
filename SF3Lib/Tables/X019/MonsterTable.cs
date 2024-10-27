using System;
using System.IO;
using CommonLib.Extensions;
using SF3.FileEditors;
using SF3.Models.X019;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.Tables.X019 {
    public class MonsterTable : Table<Monster> {
        public override int? MaxSize => 256;

        public MonsterTable(IX019_FileEditor fileEditor, bool isX044) : base(fileEditor) {
            _fileEditor = fileEditor;
            _isX044 = isX044;
            _resourceFile = Scenario == ScenarioType.PremiumDisk && isX044
                ? "Resources/PD/Monsters_X044.xml"
                : ResourceFileForScenario(_fileEditor.Scenario, "Monsters.xml");
        }

        private readonly string _resourceFile;
        private readonly IX019_FileEditor _fileEditor;
        private readonly bool _isX044;

        public override string ResourceFile => _resourceFile;
        public override int Address { get; }

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Monster(_fileEditor, id, name));
    }
}
