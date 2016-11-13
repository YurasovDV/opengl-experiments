using OcTreeExample.OcTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcTreeExample
{
    public static class Extensions
    {

        public static OcTreeItem FindVolume(this OcTreeItem[] items, BoundingVolume obj)
        {
            OcTreeItem res = null;
            foreach (var vol in items)
            {
                var contains = vol.Volume.Contains(obj);
                if (contains)
                {
                    res = vol;
                    break;
                }
            }

            return res;
        }
    }
}
