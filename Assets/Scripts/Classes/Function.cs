using System.Collections;
using System.Collections.Generic;
using CoreGame;
using UnityEngine;

namespace Assets.Scripts.Classes
{

    public class Function
    {
        private CofactorBasesAndMods _myBasesAndMods = new CofactorBasesAndMods();
        int _fontSize = 8;
        string _latexRepresentation = "123";
        public float RandomIn;
        public int CharCount = 0;

        public string LatexRepresentation { get => _latexRepresentation; set => _latexRepresentation = value; }
        public int FontSize { get => _fontSize; set => _fontSize = value; }

        private List<FunctionComponent> _functionComponents;

        // Use this for initialization
        void Start()
        {
            if (_functionComponents == null)
            {
                _functionComponents = new List<FunctionComponent>();
            }
            GetFunctionParameters();
            GenerateFunctionParts(); // Note: this adds function components to the function
            GenerateFunctionLatex();
        }

        public int CalculateOnce(double t, int p)
        {
            int myValue = (int)_functionComponents[0].GETCurrentValue(t, p);
            Debug.Log(myValue.ToString());
            return myValue;
        }

        public void SetNewFunction(string newLatexInput)
        {
            _latexRepresentation = newLatexInput;
            int number;
            if (int.TryParse(newLatexInput, out number))
            {
                // Figure out how to directly set a function with a LaTeX input here.
            }
            SetNewFunction();
            CountMyCharacters();

        }

        public void SetNewFunction()
        {
            // Build a function based on random input.
            GenerateRandomFunctionComponent();
            GenerateFunctionLatex();
        }

        public void ProcessCombine(Function functionIn)
        {
            if (functionIn._functionComponents[0].GetFunctionComponentType() == FunctionComponentType.MetaFunction)
            {
                /* process metafunctions {add, subtract, } here.
                foreach (FunctionComponent currentComponent in functionComponents)
                {
                    currentComponent.
                }
                */
            }
            else
            {
                _latexRepresentation = functionIn.LatexRepresentation;
                _functionComponents = functionIn._functionComponents;
            }

        }


        private void GenerateFunctionLatex()
        {
            _latexRepresentation = _functionComponents[0].GETLatexRepresentation();
        }

        private void GenerateFunctionParts()
        {
            GenerateRandomFunctionComponent();
        }
        
        private void GenerateRandomFunctionComponent()
        {
            if (_functionComponents == null)
            {
                _functionComponents = new List<FunctionComponent>();
            }
            int i = Random.Range(1, System.Enum.GetNames(typeof(FunctionComponentType)).Length);
            switch (i)
            {
                case (int)FunctionComponentType.Polynomial:
                    _functionComponents.Add(new Polynomial());
                    break;
                case (int)FunctionComponentType.Logarithmic:
                    _functionComponents.Add(new Logarithm());
                    break;
                case (int)FunctionComponentType.Trigonometric:
                    _functionComponents.Add(new Trigonometric(
                        _myBasesAndMods.TrigCofactorMin * _myBasesAndMods.TrigCofactorScale,
                        _myBasesAndMods.TrigCofactorMax * _myBasesAndMods.TrigCofactorMax));
                    break;
                case (int)FunctionComponentType.MetaFunction:
                    //functionComponents.Add(new MetaFunction());
                    _functionComponents.Add(new Trigonometric(
                        _myBasesAndMods.TrigCofactorMin * _myBasesAndMods.TrigCofactorScale,
                        _myBasesAndMods.TrigCofactorMax * _myBasesAndMods.TrigCofactorMax));
                    break;

                default:
                    throw new KeyNotFoundException();
                    break;
            }
            CountMyCharacters();
        }

        private void CountMyCharacters() 
        {

        }
        private void GetFunctionParameters()
        {
        }

    }
}