using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Input
{
    public enum InputSignal
    {
        FORWARD,
        BACK,
        RIGHT,
        LEFT,
        UP,
        DOWN,
        ROTATE_CLOCKWISE,
        ROTATE_COUNTERCLOCKWISE,
        SHOT,
        LOOK_UP,
        LOOK_DOWN,
        FLY_BY_CLOCKWISE,
        FLY_BY_COUNTERCLOCKWISE,
        NONE,
        DOWN_PARALLEL,
        UP_PARALLEL,
        MOUSE_CLICK,
        RENDER_MODE,
        CHANGE_CAMERA
    }
}
