using System;

namespace Assets.Scripts.Classes
{
    internal class Polynomial : FunctionComponent
    {
        private int _cofactor;
        private int _power;
        private string _latexRepresentation;
        private bool _targetT = true;
        private bool _targetP = false;
        public Polynomial()
        {
            _cofactor = 50;
            _power = -1;
            _latexRepresentation = "\\frac{50}{t}";
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
            throw new System.NotImplementedException();
        }

        public override double GETCurrentValue(double t, int p)
        {
            return _cofactor * Math.Pow(t, _power);
        }

        public override FunctionComponentType GetFunctionComponentType()
        {
            return FunctionComponentType.Polynomial;
        }

        public override string GETLatexRepresentation()
        {
            return _latexRepresentation;
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
    }
}