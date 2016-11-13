using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze3D.Classes
{
    class Player
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        public Player()
        {
            Position = new Vector3(0f, 20f, 0f);
            Target = new Vector3(50, 0f, 20);

            /*
                         Vector3 target = new Vector3(5, 5f, 0);
            Vector3 eye = new Vector3(0.0f, 0f, 50f);
             
             */

        }
    }
}
