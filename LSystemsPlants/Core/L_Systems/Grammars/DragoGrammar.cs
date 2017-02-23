using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSystemsPlants.Core.L_Systems
{
    public class DragoGrammar : BaseGrammar
    {
        public override GeneratorSettings DefaultSettings
        {
            get
            {
                return new GeneratorSettings()
                {
                    DeltaChangeAtEveryLevel = 1,
                    InitialDelta = 1.570796f,
                    InitialStep = 250,
                    MaxIteration = 18,
                    StepChangeAtEveryLevel = 0.73f
                };
            }
        }

        public DragoGrammar() : base()
        {
            Axiom = new[]
            {
                Symbol.L,
            };

            var ruleSet = new List<Rule>()
            {
                new Rule(Symbol.L, new[]
                {
                    Symbol.L,// Fl
                    Symbol.TURN_LEFT, // +
                    Symbol.R, // Fr
                    Symbol.TURN_LEFT, // +
                  
                }),

                new Rule(Symbol.R, new[]
                {
                    Symbol.TURN_RIGHT, // -
                   Symbol.L,// Fl
                    Symbol.TURN_RIGHT, // -
                    Symbol.R, // Fr
                })
            };

            RuleSet = ruleSet;
        }

    }
}
