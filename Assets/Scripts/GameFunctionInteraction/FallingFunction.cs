using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Classes;
using TexDrawLib;
public class FallingFunction : MonoBehaviour
{
    public Function function;
    float xSize = 0;
    float ySize = 0;
    BoxCollider2D myCollider2D;
    TEXDraw texDrawUGUI;
    GameState gameState;
    Rigidbody2D myRigidBody2D;
    DragAndDropFallingFunction myDragAndDropTMP;
    public float randomIn;
    [SerializeField] float randomXForceMax = 1;
    [SerializeField] float randomYForceMax = 1;
    [SerializeField] bool rotateNumbers = true;
    [SerializeField] TMPro.TextMeshProUGUI PointsDisplayPrefab;

    void Start()
    {
        SetupPrivateComponents();
        SetNewEquation(texDrawUGUI.text);
        //StartCoroutine(SetNewEquationLater(10));
    }


    void SetupPrivateComponents()
    {
        texDrawUGUI = GetComponent<TEXDraw>();
        gameState = FindObjectOfType<GameState>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myDragAndDropTMP = GetComponent<DragAndDropFallingFunction>();

    }
    void Update()
    {
        if (myDragAndDropTMP.CanMove())
        {
            AddRandomForce();
        }
    }

    public Function getFunction()
    {
        return function;
    }

    private void SetNewEquation(string NewEquationLatex = "")
    {
        SetupFunction(NewEquationLatex);
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
        function = new Function();
        function.SetNewFunction(newEquationLatex);

    }

    public void SetupFunction()
    {
        function = new Function();
        function.randomIn = randomIn;
        function.SetNewFunction();
        SetText();
        SetSize();
    }

    void SetText()
    {
        if (function != null)
        {
            if (texDrawUGUI == null)
            {
                texDrawUGUI = GetComponent<TEXDraw>();
            }
            texDrawUGUI.text = function.LatexRepresentation;
        }
        else
        {
            throw new MissingReferenceException();
        }
    }

    void AddRandomForce()
    {
        myRigidBody2D.AddForce(new Vector2(Random.Range(-randomXForceMax, randomXForceMax), 
            Random.Range(-randomYForceMax, randomYForceMax)));
    }

    void SetSize()
    {
        float x = texDrawUGUI.preferredWidth;
        float y = texDrawUGUI.preferredHeight;
        xSize = x;
        ySize = y;
        SetTexDrawGUIObjectSize();
    }



    void SetTexDrawGUIObjectSize()
    {
        texDrawUGUI.rectTransform.sizeDelta = new Vector2(xSize, ySize);
    }

    public void ProcessCombine(FallingFunction functionIn)
    {
        function.ProcessCombine(functionIn.getFunction());
    }

    public void DisplayPoints(int pointsValue)
    {

    }
}

