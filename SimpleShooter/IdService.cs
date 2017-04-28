namespace SimpleShooter
{
    class IdService
    {
        private static long _counter = 0;

        public static long GetNext()
        {
            _counter++;
            return _counter;
        }
    }
}
