using System;
using System.Windows.Forms;
using LSystemsPlants.Core.L_Systems;

namespace LSystemsPlants
{
    public partial class MainForm : Form
    {
        private Engine _engine;
        private Timer _timer;

        public MainForm()
        {
            InitializeComponent();


        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _engine.Tick(_timer.Interval);
            RenderFrame();
        }

        private void RenderFrame()
        {
            portraitControl.SwapBuffers();
            portraitControl.Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var grammar = new SquareGrammar(); //new SimplestGrammar();
            var settings = grammar.DefaultSettings;

            SetFormState(grammar, settings);

            _engine = new Engine(portraitControl.Width, portraitControl.Height, grammar, settings);

            _timer = new Timer()
            {
                Interval = 60
            };
            _timer.Tick += _timer_Tick;

            _timer.Start();
        }

        private void SetFormState(IGrammar grammar, GeneratorSettings settings)
        {
            tbAxiom.Text = grammar.GetAxiom();

            var textBoxes = new[] {
                tbRule0 ,
                tbRule1 ,
                tbRule2 ,
                tbRule3 ,
                tbRule4 ,
            };

            foreach (var tb in textBoxes)
            {
                tb.Text = string.Empty;
            }

            for (int i = 0; i < Math.Min(textBoxes.Length, grammar.RulesCount); i++)
            {
                textBoxes[i].Text = grammar.GetRule(i);
            }

            tbDefaultDelta.Text = settings.InitialDelta.ToString();
            tbDefaultStep.Text = settings.InitialStep.ToString();
            tbIterations.Text = settings.MaxIteration.ToString();
            tbDeltaChange.Text = settings.DeltaChangeAtEveryLevel.ToString();
            tbStepChange.Text = settings.StepChangeAtEveryLevel.ToString();
        }

        private void btRegenerate_Click(object sender, EventArgs e)
        {
            var settings = ParseSettings();
            var grammar = ParseGrammar();

            _engine.InitModel(grammar, settings);

            SetFormState(grammar, settings);
        }

        private IGrammar ParseGrammar()
        {
            var rule0 = tbRule0.Text;
            var rule1 = tbRule1.Text;
            var rule2 = tbRule2.Text;
            var rule3 = tbRule3.Text;
            var rule4 = tbRule4.Text;
            var axiom = tbAxiom.Text;

            IGrammar result = ParseHelper.GetGrammar(axiom, new[] {rule0, rule1, rule2, rule3, rule4 });

            return result;
        }

        private GeneratorSettings ParseSettings()
        {
            var res = new GeneratorSettings();

            float delta;
            float.TryParse(tbDefaultDelta.Text, out delta);
            res.InitialDelta = delta;

            float step;
            float.TryParse(tbDefaultStep.Text, out step);
            res.InitialStep = step;

            int iter;
            int.TryParse(tbIterations.Text, out iter);
            res.MaxIteration = iter;

            float deltaChange;
            float.TryParse(tbDeltaChange.Text, out deltaChange);
            res.DeltaChangeAtEveryLevel = deltaChange;

            float stepChange;
            float.TryParse(tbStepChange.Text, out stepChange);
            res.StepChangeAtEveryLevel = stepChange;

            return res;
        }

        private void btSimple_Click(object sender, EventArgs e)
        {
            var grammar = new SimplestGrammar();
            UpdateState(grammar);
        }

        private void btSetKoch_Click(object sender, EventArgs e)
        {
            var grammar = new KochGrammar();
            UpdateState(grammar);
        }

        private void btSquares_Click(object sender, EventArgs e)
        {
            var grammar = new SquareGrammar();
            UpdateState(grammar);
        }

        private void btTrinagles_Click(object sender, EventArgs e)
        {
            var grammar = new TriangleGrammar();
            UpdateState(grammar);
        }

        private void btDrago_Click(object sender, EventArgs e)
        {
            var grammar = new DragoGrammar();
            UpdateState(grammar);
        }

        private void UpdateState(IGrammar grammar)
        {
            var settings = grammar.DefaultSettings;
            _engine.InitModel(grammar, settings);
            SetFormState(grammar, settings);
        }
    }
}
