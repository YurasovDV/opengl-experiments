using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShaderOnForm
{
    public class KeyHandler
    {
        [DllImport("User32.dll")]
        public static extern int GetKeyState(int key);

        public delegate void ProcessKeyDelegate(InputSignal signal);

        public event ProcessKeyDelegate OnKeyPress;

        public Keys[] KeysToWatch { get; set; }

        private Dictionary<Keys, InputSignal> keysToSignal;
        public KeyHandler()
        {
            keysToSignal = new Dictionary<Keys, InputSignal>();
            KeysToWatch = new Keys[]
            {
            Keys.A,
            Keys.W,
            Keys.S,
            Keys.D,
            Keys.Up,//move up
            Keys.Down,
            Keys.Q, 
            Keys.E,//rotate  counterclockwise
            Keys.Left,
            Keys.Right,
            Keys.H,
            Keys.G,
            };

            keysToSignal[Keys.A] = InputSignal.LEFT;
            keysToSignal[Keys.W] = InputSignal.FORWARD;
            keysToSignal[Keys.S] = InputSignal.BACK;
            keysToSignal[Keys.D] = InputSignal.RIGHT;
            keysToSignal[Keys.Up] = InputSignal.UP;
            keysToSignal[Keys.Down] = InputSignal.DOWN;
            keysToSignal[Keys.Q] = InputSignal.ROTATE_CLOCKWISE;
            keysToSignal[Keys.E] = InputSignal.ROTATE_COUNTERCLOCKWISE;
            keysToSignal[Keys.Space] = InputSignal.SHOT;
            keysToSignal[Keys.Left] = InputSignal.FLY_BY_CLOCKWISE;
            keysToSignal[Keys.Right] = InputSignal.FLY_BY_COUNTERCLOCKWISE;
            keysToSignal[Keys.H] = InputSignal.UP_PARALLEL;
            keysToSignal[Keys.G] = InputSignal.DOWN_PARALLEL;
        }

        public void CheckKeys()
        {
            foreach (var key in KeysToWatch)
            {
                var state = GetKeyState((int)key);
                if (state == 127 || state == 128 || state == -127 || state == -128)
                {
                    if (OnKeyPress != null)
                    {
                        InputSignal signal = GetSignalByKey(key);
                        OnKeyPress(signal);
                    }
                }
            }
        }

        private InputSignal GetSignalByKey(Keys key)
        {
            if (keysToSignal.ContainsKey(key))
            {
                return keysToSignal[key];
            }
            return InputSignal.NONE;
        }
    }
}
