using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject{
    public string strID = "N";
    public Mesh mesh;
    //public Material material; // -> all the materials are the same, so, this was removed
}
