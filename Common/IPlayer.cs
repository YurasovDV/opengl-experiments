using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Common
{
    public interface IPlayer
    {
        Vector3 FlashlightPosition { get; set; }
        Vector3 Position { get; set; }
        Vector3 Target { get; set; }
    }
}
