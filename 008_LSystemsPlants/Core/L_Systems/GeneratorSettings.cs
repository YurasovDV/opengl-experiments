namespace LSystemsPlants.Core.L_Systems
{
    public class GeneratorSettings
    {
        public static GeneratorSettings Default = new GeneratorSettings()
        {
            MaxIteration = 5,
            InitialDelta = 0.5f,
            InitialStep = 30,
            DeltaChangeAtEveryLevel = 0.75f,
            StepChangeAtEveryLevel = 0.75f
        };

        public int MaxIteration { get; set; }

        public float InitialDelta { get; set; }

        public float InitialStep { get; set; }

        public float DeltaChangeAtEveryLevel { get; set; }

        public float StepChangeAtEveryLevel { get; set; }

    }
}
