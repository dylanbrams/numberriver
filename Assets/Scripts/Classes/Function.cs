using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes
{

    public class Function
    {
        int fontSize = 8;
        string latexRepresentation = "123";
        public float randomIn;
        public int charCount = 0;

        public string LatexRepresentation { get => latexRepresentation; set => latexRepresentation = value; }
        public int FontSize { get => fontSize; set => fontSize = value; }

        private List<FunctionComponent> functionComponents;

        // Use this for initialization
        void Start()
        {
            if (functionComponents == null)
            {
                functionComponents = new List<FunctionComponent>();
            }
            GetFunctionParameters();
            GenerateFunctionParts(); // Note: this adds function components to the function
            GenerateFunctionLatex();
        }

        public int CalculateOnce(double t, int p)
        {
            int myValue = (int)functionComponents[0].GETCurrentValue(t, p);
            Debug.Log(myValue.ToString());
            return myValue;
        }

        public void SetNewFunction(string newLatexInput)
        {
            latexRepresentation = newLatexInput;
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
            if (functionIn.functionComponents[0].GetFunctionComponentType() == FunctionComponentType.MetaFunction)
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
                latexRepresentation = functionIn.LatexRepresentation;
                functionComponents = functionIn.functionComponents;
            }

        }


        private void GenerateFunctionLatex()
        {
            latexRepresentation = functionComponents[0].GETLatexRepresentation();
        }

        private void GenerateFunctionParts()
        {
            GenerateRandomFunctionComponent();
        }
        
        private void GenerateRandomFunctionComponent()
        {
            if (functionComponents == null)
            {
                functionComponents = new List<FunctionComponent>();
            }
            int i = Random.Range(1, System.Enum.GetNames(typeof(FunctionComponentType)).Length);
            switch (i)
            {
                case (int)FunctionComponentType.Polynomial:
                    functionComponents.Add(new Polynomial());
                    break;
                case (int)FunctionComponentType.Logarithmic:
                    functionComponents.Add(new Logarithm());
                    break;
                case (int)FunctionComponentType.Trigonometric:
                    functionComponents.Add(new Trigonometric());
                    break;
                case (int)FunctionComponentType.MetaFunction:
                    //functionComponents.Add(new MetaFunction());
                    functionComponents.Add(new Trigonometric());
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