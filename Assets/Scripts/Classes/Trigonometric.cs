using System;

namespace Assets.Scripts.Classes
{
    class Trigonometric : FunctionComponent
    {
        private float cofactor;
        private float periodCount;
        float imaginaryComponent = 0;
        private string latexRepresentation;
        private  enum TrigFunctionType
        {
            Sin = 1,
            Cos = 2,
            Tan = 3,
            ASin = 4,
            ACos = 5,
            ATan = 6,
        }

        public Trigonometric()
        {
            periodCount = 1;
            cofactor = 20;
            SetLatexRepresentation();
        }
        public override void AddToComponent(float cofactorInput)
        {
            this.cofactor += cofactorInput;
        }

        public override void DeriveComponent()
        {
            throw new System.NotImplementedException();
        }

        public override void GenerateRandomComponent()
        {
            throw new System.NotImplementedException();
        }

        public override double GETCurrentValue(double t, int p)
        {
            return cofactor * Math.Sin( 2 * Math.PI * periodCount * t);
        }

        public override FunctionComponentType GetFunctionComponentType()
        {
            throw new System.NotImplementedException();
        }

        public override string GETLatexRepresentation()
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

        public override void SetLatexRepresentation()
        {
            latexRepresentation = "{20}{sin}({t})";
        }
    }
}