using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    
    private void Awake(){
        current = this;
    }

    public event Action<int> hasTouchEnded;
    public void TouchEnded(int id){
        if(hasTouchEnded != null){
            hasTouchEnded(id);
        }
    }

    public event Action<int> canCombineIngredients;
    public void CombineIngredients(int id){
        if(canCombineIngredients != null){
            canCombineIngredients(id);
        }
    }
}
