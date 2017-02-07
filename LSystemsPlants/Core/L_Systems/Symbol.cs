using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystemsPlants.Core.L_Systems
{
    enum Symbol
    {
        /// <summary>
        /// F
        /// </summary>
        FORWARD_DRAW = 1,

        /// <summary>
        /// +
        /// </summary>
        TURN_LEFT = 2,

        /// <summary>
        /// -
        /// </summary>
        TURN_RIGHT = 3,

        /// <summary>
        /// f
        /// </summary>
        FORWARD_NO_DRAW = 4
    }
}
