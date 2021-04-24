using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Classes;
using TexDrawLib;
using TMPro;

public class FallingFunction : MonoBehaviour
{
    public Function Function;
    float _xSize = 0;
    float _ySize = 0;
    BoxCollider2D _myCollider2D;
    TEXDraw _texDrawUGUI;
    GameState _gameState;
    Rigidbody2D _myRigidBody2D;
    DragAndDropFallingFunction _myDragAndDropTMP;
    public float randomIn;

    [SerializeField] float randomXForceMax = 1;
    [SerializeField] float randomYForceMax = 1;
    [SerializeField] Vector3 scoreDisplayOffset = new Vector3(0, 0, 0);

    private TextMeshProUGUI _linkedPointsDisplay;

    void Start()
    {
        SetupPrivateComponents();
        SetNewEquation(_texDrawUGUI.text);
        //StartCoroutine(SetNewEquationLater(10));
    }


    void SetupPrivateComponents()
    {
        _texDrawUGUI = GetComponent<TEXDraw>();
        _gameState = FindObjectOfType<GameState>();
        _myRigidBody2D = GetComponent<Rigidbody2D>();
        _myDragAndDropTMP = GetComponent<DragAndDropFallingFunction>();

    }
    void Update()
    {
        if (_myDragAndDropTMP.CanMove())
        {
            AddRandomForce();
        }
    }

    public Function GETFunction()
    {
        return Function;
    }

    private void SetNewEquation(string newEquationLatex = "")
    {
        SetupFunction(newEquationLatex);
        SetText();
        SetSize();
    }
    IEnumerator SetNewEquationLater(float nextEquationWait) 
    {
        yield return new WaitForSeconds(nextEquationWait);
        SetNewEquation("1");
    }

    public void SetupFunction(string newEquationLatex)
    {
        Function = new Function();
        Function.SetNewFunction(newEquationLatex);

    }

    public void SetupFunction()
    {
        Function = new Function();
        Function.RandomIn = randomIn;
        Function.SetNewFunction();
        SetText();
        SetSize();
    }

    void SetText()
    {
        if (Function != null)
        {
            if (_texDrawUGUI == null)
            {
                _texDrawUGUI = GetComponent<TEXDraw>();
            }
            _texDrawUGUI.text = Function.LatexRepresentation;
        }
        else
        {
            throw new MissingReferenceException();
        }
    }

    void AddRandomForce()
    {
        _myRigidBody2D.AddForce(new Vector2(Random.Range(-randomXForceMax, randomXForceMax), 
            Random.Range(-randomYForceMax, randomYForceMax)));
    }

    void SetSize()
    {
        float x = _texDrawUGUI.preferredWidth;
        float y = _texDrawUGUI.preferredHeight;
        _xSize = x;
        _ySize = y;
        SetTexDrawGUIObjectSize();
    }



    void SetTexDrawGUIObjectSize()
    {
        _texDrawUGUI.rectTransform.sizeDelta = new Vector2(_xSize, _ySize);
    }

    public void ProcessCombine(FallingFunction functionIn)
    {
        Function.ProcessCombine(functionIn.GETFunction());
    }

    public void SetPointsDisplay(TextMeshProUGUI tmpUGUIIn)
    {
        _linkedPointsDisplay = tmpUGUIIn;
    }

    public void DisplayPoints(int pointsValue)
    {
        _linkedPointsDisplay.text = pointsValue.ToString();
    }

}

