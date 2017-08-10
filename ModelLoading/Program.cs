namespace ModelLoading
{
    class Program
    {
        public static void Main()
        {
            using (var f = new ModelForm())
            {
                f.Run(60);
            }

        }
    }
}
