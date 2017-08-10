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
