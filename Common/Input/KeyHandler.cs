using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Input
{
    public class KeyHandler
    {
        [DllImport("User32.dll")]
        public static extern int GetKeyState(int key);

        public delegate void ProcessKeyDelegate(InputSignal signal);

        public event ProcessKeyDelegate KeyPress;

        public Keys[] KeysToWatch { get; set; }

        private Dictionary<Keys, InputSignal> keysToSignal;
        public KeyHandler()
        {
            keysToSignal = new Dictionary<Keys, InputSignal>()
            {
                {Keys.A, InputSignal.LEFT},
                    {Keys.W, InputSignal.FORWARD},
                    {Keys.S, InputSignal.BACK},
                    {Keys.D, InputSignal.RIGHT},
                    {Keys.Up, InputSignal.UP},
                    {Keys.Down,InputSignal.DOWN },
                    {Keys.Q, InputSignal.ROTATE_CLOCKWISE},
                    {Keys.E, InputSignal.ROTATE_COUNTERCLOCKWISE},
                    {Keys.Space,InputSignal.SHOT },
                    {Keys.Left, InputSignal.FLY_BY_CLOCKWISE},
                    {Keys.Right, InputSignal.FLY_BY_COUNTERCLOCKWISE},
                    {Keys.H, InputSignal.UP_PARALLEL},
                    {Keys.G, InputSignal.DOWN_PARALLEL},
                    {Keys.R, InputSignal.RENDER_MODE}
            };


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
            Keys.R,//render mode change
            };
        }

        public void CheckKeys()
        {
            foreach (var key in KeysToWatch)
            {
                var state = GetKeyState((int)key);
                if (state == 127 || state == 128 || state == -127 || state == -128)
                {
                    if (KeyPress != null)
                    {
                        InputSignal signal = GetSignalByKey(key);
                        KeyPress(signal);
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
