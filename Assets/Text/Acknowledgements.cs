using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Acknowledgements")]
public class Acknowledgements : ScriptableObject
{
    [TextArea(14,200)][SerializeField] string Acknowledgement_string;
}
