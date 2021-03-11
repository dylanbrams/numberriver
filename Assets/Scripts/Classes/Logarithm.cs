namespace Assets.Scripts.Classes
{
    internal class Logarithm : FunctionComponent
    {
        private int cofactor = 1;
        private int logBase = 10;
        private string latexRepresentation = "{log}_{10}{t}";
        private bool targetT = true;
        private bool targetP = false;

        public override FunctionComponentType GetFunctionComponentType()
        {
            return FunctionComponentType.Logarithmic;
        }

        public Logarithm()
        {

        }
        public override void AddToComponent(float cofactor)
        {
            throw new System.NotImplementedException();
        }

        public override void DeriveComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void generateRandomComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void IntegrateComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void MultiplyComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void setLatexRepresentation()
        {
            throw new System.NotImplementedException();
        }

        public override string getLatexRepresentation()
        {
            return latexRepresentation;
        }

        public override double getCurrentValue(double t, int p)
        {
            return System.Math.Log(t, logBase) * cofactor;
        }
    }
}