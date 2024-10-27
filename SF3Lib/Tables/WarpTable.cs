using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;

namespace SF3.Tables {
    public class WarpTable : Table<Warp> {
        public override int? MaxSize => 1000;

        public WarpTable(ISF3FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private readonly ISF3FileEditor _fileEditor;
        public override int Address => throw new NotImplementedException();

        public override string ResourceFile => _fileEditor.Scenario == ScenarioType.Scenario1 ? "Resources/S1/Warps.xml" : null;

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            var values = ResourceFile != null
                ? ResourceUtils.GetValueNameDictionaryFromXML(ResourceFile)
                : new Dictionary<int, string>();

            _rows = new Warp[0];
            for (var i = 0; _rows.Length == 0 || _rows[_rows.Length - 1].WarpType != 0x01 && _rows[_rows.Length - 1].WarpType != 0xff; i++) {
                if (i == MaxSize)
                    throw new IndexOutOfRangeException();
                var newRow = new Warp(_fileEditor, i, values.ContainsKey(i) ? values[i] : "WarpIndex" + i);
                _rows = _rows.ExpandedWith(newRow);
            }
            return true;
        }
    }
}
