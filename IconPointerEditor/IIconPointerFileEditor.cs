using SF3.IconPointerEditor.Models.ItemIcons;
using SF3.IconPointerEditor.Models.SpellIcons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.IconPointerEditor
{
    public interface IIconPointerFileEditor : ISF3FileEditor
    {
        bool X026 { get; }

        SpellIconList SpellIconList { get; }
        ItemIconList ItemIconList { get; }
    }
}
