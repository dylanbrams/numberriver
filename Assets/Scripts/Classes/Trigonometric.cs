using System;
using System.Globalization;
using Unity.Mathematics;
using UnityRandom = UnityEngine.Random;

namespace Assets.Scripts.Classes
{
    class Trigonometric : FunctionComponent
    {
        private float _cofactorMin;
        private float _cofactorMax;
        private float _cofactorDefault = 2f;
        private float _cofactor;
        private float _periodCount = 1;
        float _imaginaryComponent = 0;
        private string _latexRepresentation;
        private  enum TrigFunctionType
        {
            Sin = 1,
            Cos = 2,
            Tan = 3,
            ASin = 4,
            ACos = 5,
            ATan = 6,
        }

        public Trigonometric(float cofactorMin, float cofactorMax)
        {
            _cofactorMin = cofactorMin;
            _cofactorMax = cofactorMax;
            
            _cofactor = UnityRandom.Range(_cofactorMin, _cofactorMax);
            if (-1 < math.abs(_cofactor) || math.abs(_cofactor)  < 1)
            {
                _cofactor = _cofactorDefault;
            }
            else
            {
                _cofactor = math.round(_cofactor);
            }
            SetLatexRepresentation();
        }
        public override void AddToComponent(float cofactorInput)
        {
            this._cofactor += cofactorInput;
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
            return _cofactor * Math.Sin( 2 * Math.PI * _periodCount * t);
        }

        public override FunctionComponentType GetFunctionComponentType()
        {
            throw new System.NotImplementedException();
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

        public sealed override void SetLatexRepresentation()
        {
            _latexRepresentation = "{"+_cofactor.ToString(CultureInfo.CurrentCulture)+"}{sin}({t})";
        }
    }
}