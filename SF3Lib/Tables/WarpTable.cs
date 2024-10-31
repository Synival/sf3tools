using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Utils;
using SF3.FileEditors;
using SF3.Models;
using SF3.Types;
using static SF3.Utils.ResourceUtils;

namespace SF3.Tables {
    public class WarpTable : Table<Warp> {
        public override int? MaxSize => 1000;

        public WarpTable(ISF3FileEditor fileEditor) : base(fileEditor) {
            ResourceFile = fileEditor.Scenario == ScenarioType.Scenario1
                ? ResourceFileForScenario(ScenarioType.Scenario1, "Warps.xml")
                : null;
        }

        public override int Address => throw new NotImplementedException();

        public override string ResourceFile { get; }

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            var values = ResourceFile != null
                ? ResourceUtils.GetValueNameDictionaryFromXML(ResourceFile)
                : new Dictionary<int, string>();

            _rows = new Warp[0];
            for (var i = 0; _rows.Length == 0 || (_rows[_rows.Length - 1].WarpType != 0x01 && _rows[_rows.Length - 1].WarpType != 0xff); i++) {
                if (i == MaxSize)
                    throw new IndexOutOfRangeException();
                var newRow = new Warp(FileEditor, i, values.ContainsKey(i) ? values[i] : "WarpIndex" + i);
                _rows = _rows.ExpandedWith(newRow);
            }
            return true;
        }
    }
}
