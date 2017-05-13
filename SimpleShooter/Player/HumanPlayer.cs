using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Input;
using OpenTK;
using SimpleShooter.Player.Events;

namespace SimpleShooter.Player
{
    class HumanPlayer : Player
    {
        protected Vector3 JumpSpeed { get; set; }
        protected Vector3 JumpSpeedDefault { get; set; }

        public HumanPlayer(Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;
            JumpSpeedDefault = new Vector3(1, 0, 0);
        }


        public override void Handle(InputSignal signal)
        {
            switch (signal)
            {
                case InputSignal.FORWARD:
                    StepXZ(StepForward);
                    break;
                case InputSignal.BACK:
                    StepXZ(StepBack);
                    break;
                case InputSignal.RIGHT:
                    StepXZ(StepRight);
                    break;
                case InputSignal.LEFT:
                    StepXZ(StepLeft);
                    break;
                case InputSignal.SPACE:

                    TryJump();
                    break;
                case InputSignal.MOUSE_CLICK:
                    TryShot();
                    break;
                default:
                    break;
            }
        }

        private void TryJump()
        {
            OnJump(new JumpEventArgs());
        }

        private void TryShot()
        {
            OnShot(new ShotEventArgs());
        }
    }
}
