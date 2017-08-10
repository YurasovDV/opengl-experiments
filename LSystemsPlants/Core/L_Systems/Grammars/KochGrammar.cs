﻿using System.Collections.Generic;

namespace LSystemsPlants.Core.L_Systems
{
    public class KochGrammar : BaseGrammar
    {
        public override GeneratorSettings DefaultSettings
        {
            get
            {
                return new GeneratorSettings()
                {
                    DeltaChangeAtEveryLevel = 1,
                    InitialDelta = 1.570796f,
                    InitialStep = 120,
                    MaxIteration = 5,
                    StepChangeAtEveryLevel = 0.5f
                };
            }
        }

        public KochGrammar() : base()
        {
            Axiom = new[] 
            {
                Symbol.FORWARD_DRAW,
                Symbol.TURN_RIGHT,
                Symbol.FORWARD_DRAW,
                Symbol.TURN_RIGHT,
                Symbol.FORWARD_DRAW,
                Symbol.TURN_RIGHT,
                Symbol.FORWARD_DRAW,
            };

            var ruleSet = new List<Rule>()
            {
                // F=FF-
                // [-F+F+F]
                // +
                // [+F-F-F]
                new Rule(Symbol.FORWARD_DRAW, new[]
                {
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT, // -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_LEFT, // +
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                    Symbol.TURN_RIGHT,// -
                    Symbol.FORWARD_DRAW,// F
                })
            };

            RuleSet = ruleSet;
        }
    }
}
