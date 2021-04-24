namespace Assets.Scripts.Classes
{
    internal class Logarithm : FunctionComponent
    {
        private int _cofactor = 1;
        private int _logBase = 10;
        private string _latexRepresentation = "{log}_{10}{t}";
        private bool _targetT = true;
        private bool _targetP = false;

        public override FunctionComponentType GetFunctionComponentType()
        {
            return FunctionComponentType.Logarithmic;
        }

        public Logarithm()
        {
            GenerateRandomComponent();
        }
        public override void AddToComponent(float cofactor)
        {
            throw new System.NotImplementedException();
        }

        public override void DeriveComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void GenerateRandomComponent()
        {

        }

        public override void IntegrateComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void MultiplyComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void SetLatexRepresentation()
        {
            throw new System.NotImplementedException();
        }

        public override string GETLatexRepresentation()
        {
            return _latexRepresentation;
        }

        public override double GETCurrentValue(double t, int p)
        {
            return System.Math.Log(t, _logBase) * _cofactor;
        }
    }
}