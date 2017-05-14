using System;
using System.Collections.Generic;
using Common.Geometry;
using Common.Input;
using OpenTK;
using SimpleShooter.Player.Events;

namespace SimpleShooter.Player
{
    class HumanPlayer : Player
    {
        public Vector3 Acceleration { get; set; }
        public Vector3 Speed { get; set; }

        public HumanPlayer(Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;


            Speed = new Vector3(0, 0, 0);
            Acceleration = new Vector3(0, 0, 0);

            BoundingBox = BoundingVolume.CreateVolume(position, 1);

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
                    TryShoot();
                    break;
                default:
                    break;
            }
        }

        private void TryJump()
        {
            
        }

        private void TryShoot()
        {
            OnShot(new ShotEventArgs());
        }
    }
}
