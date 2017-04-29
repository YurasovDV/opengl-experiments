using OpenTK;
using SimpleShooter.Core;

namespace SimpleShooter.Player
{
    class MarkController
    {
        private static Vector3[] markForm = null;

        public static void SetTo(Player player, GameObject mark, Matrix4 rotation)
        {
            if (markForm == null)
            {
                markForm = new Vector3[]
                    {
                       player.DefaultTarget + new Vector3(0, 0.5f, 0),
                       player.DefaultTarget + new Vector3(0, -0.5f, 0),
                       player.DefaultTarget + new Vector3(0, 0f, -0.5f),
                       player.DefaultTarget + new Vector3(0, 0f, 0.5f),
                    };
            }

            for (int i = 0; i < markForm.Length; i++)
            {
                mark.Model.Vertices[i] = player.Position + Vector3.Transform(markForm[i], rotation);
            }
        }
    }
}
