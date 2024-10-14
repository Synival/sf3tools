using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Editor.Extensions
{
    public static class ObjectListViewExtensions
    {
        public static void RefreshAllItems(this ObjectListView olv)
        {
            foreach (var item in olv.Items)
            {
                var olvItem = item as OLVListItem;
                olv.RefreshItem(olvItem);
            }
        }
    }
}
