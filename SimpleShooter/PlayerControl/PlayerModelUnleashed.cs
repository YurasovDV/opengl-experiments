using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Input;
using OpenTK;
using SimpleShooter.Core;
using SimpleShooter.PlayerControl.Events;

namespace SimpleShooter.PlayerControl
{
    public class PlayerModelUnleashed : Player
    {

        public PlayerModelUnleashed(SimpleModel model, Vector3 position, Vector3 target) : base(model, 0)
        {
            Position = position;
            target = Target;
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
                case InputSignal.UP:

                    AngleVerticalRadians -= 0.01f;
                    Rotate();

                    break;
                case InputSignal.DOWN:
                    AngleVerticalRadians += 0.01f;
                    Rotate();

                    break;
                case InputSignal.ROTATE_CLOCKWISE:
                    AngleHorizontalRadians += 0.01f;
                    Rotate();
                    break;
                case InputSignal.ROTATE_COUNTERCLOCKWISE:
                    AngleHorizontalRadians -= 0.01f;
                    Rotate();
                    break;
                case InputSignal.SPACE:
                    OnShot(new ShotEventArgs(Position));
                    break;
                case InputSignal.LOOK_UP:
                    break;
                case InputSignal.LOOK_DOWN:
                    break;
                case InputSignal.FLY_BY_CLOCKWISE:
                    break;
                case InputSignal.FLY_BY_COUNTERCLOCKWISE:
                    break;
                case InputSignal.NONE:
                    break;
                case InputSignal.DOWN_PARALLEL:
                    StepYZ(StepDown);
                    break;
                case InputSignal.UP_PARALLEL:
                    StepYZ(StepUp);
                    break;
                case InputSignal.MOUSE_CLICK:
                    OnShot(new ShotEventArgs(Position));
                    break;
                case InputSignal.RENDER_MODE:
                    break;
                case InputSignal.CHANGE_CAMERA:
                    break;
                default:
                    break;
            }
        }


    }
}
