using OcTreeRevisited.OcTree;

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
