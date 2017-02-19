namespace LSystemsPlants.Core.L_Systems
{
    public enum Symbol
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
        FORWARD_NO_DRAW = 4,

        /// <summary>
        /// [
        /// </summary>
        PUSH_STATE,

        /// <summary>
        /// ]
        /// </summary>
        POP_STATE
    }
}
