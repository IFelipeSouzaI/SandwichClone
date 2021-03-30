using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject{
    //public List<Ingredient> board;
    public Ingredient[] topRow = new Ingredient[4];
    public Ingredient[] topMiddleRow = new Ingredient[4];
    public Ingredient[] bottomMiddleRow = new Ingredient[4];
    public Ingredient[] bottomRow = new Ingredient[4];
}
