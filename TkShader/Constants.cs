namespace ShaderOnForm
{
    public class Constants
    {

        public const int MAP_SIZE = 256;

        public const bool DRAW_MARK = false;
        /// <summary>
        /// Растяжение клеток при отрисовке
        /// </summary>
        public const float CELL_SIZE = 0.7f;

        /// <summary>
        /// Определяет, как сильно при генерации карты высота в вычисляемой точке зависит от расстояния между опорными точками
        /// </summary>
        public const float VARIATION = 0.05f;//05
        /*
        /// <summary>
        /// Сжатие высоты при отрисовке
        /// </summary>
        public const double HEIGHT_COEFF = 1;
        */

        public const int PREFERRED_FPS = 52;

        public const float DEFAULT_SPEED = 0.4f;

        public const int DEFAULT_ROTATION = 4;

        /// <summary>
        /// максимаьная разница по высоте между соседними точками
        /// </summary>
        public const float MAX_DIFFERENCE = 0.83f;//08

        /// <summary>
        /// расстояние до точки, в которую смотрим
        /// </summary>
        public const int TARGET_DISTANCE = 10;
    }
}
