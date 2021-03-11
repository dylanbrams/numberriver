using System;

namespace Assets.Scripts.Classes
{
    internal class Polynomial : FunctionComponent
    {
        private int cofactor;
        private int power;
        private string latexRepresentation;
        private bool targetT = true;
        private bool targetP = false;
        public Polynomial()
        {
            cofactor = 50;
            power = -1;
            latexRepresentation = "\\frac{50}{t}";
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

        public override double getCurrentValue(double t, int p)
        {
            return cofactor * Math.Pow(t, power);
        }

        public override FunctionComponentType GetFunctionComponentType()
        {
            return FunctionComponentType.Polynomial;
        }

        public override string getLatexRepresentation()
        {
            return latexRepresentation;
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
    }
}