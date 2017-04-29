﻿using OcTreeRevisited.OcTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OcTreeRevisited
{
    public static class Extensions
    {
        public static OcTreeItem FindVolume(this OcTreeItem[] items, GameObject obj)
        {
            OcTreeItem res = null;
            foreach (var vol in items)
            {
                if (vol != null)
                {
                    var contains = vol.Volume.Contains(obj.BoundingBox);
                    if (contains)
                    {
                        res = vol;
                        break;
                    }
                }
            }

            return res;
        }



    }
}