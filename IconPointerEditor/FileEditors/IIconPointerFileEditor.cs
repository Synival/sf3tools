using SF3.FileEditors;
using SF3.IconPointerEditor.Models.ItemIcons;
using SF3.IconPointerEditor.Models.SpellIcons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.IconPointerEditor.FileEditors
{
    public interface IIconPointerFileEditor : ISF3FileEditor
    {
        bool IsX026 { get; }

        SpellIconList SpellIconList { get; }
        ItemIconList ItemIconList { get; }
    }
}
