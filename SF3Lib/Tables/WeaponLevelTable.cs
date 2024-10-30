using System;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;

namespace SF3.Tables {
    public class WeaponLevelTable : Table<WeaponLevel> {

        public WeaponLevelTable(ISF3FileEditor fileEditor, int address) : base(fileEditor) {
            Address = address;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponLevel(FileEditor, id, name, address));

        public override string ResourceFile => "Resources/WeaponLevel.xml";
        public override int Address { get; }
        public override int? MaxSize => 2;
    }
}
