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

    public event Action<int, Vector2> hasTouchEnded;
    public void TouchEnded(int id, Vector2 moveDir){
        if(hasTouchEnded != null){
            hasTouchEnded(id, moveDir);
        }
    }

    public event Action<int, string> hasMovementEnded;
    public void MovementEnded(int ingredientAmount, string strID){
        if(hasMovementEnded != null){
            hasMovementEnded(ingredientAmount, strID);
        }
    }

    public event Action<int, GameObject> canCombineIngredients;
    public void CombineIngredients(int id, GameObject neighbor){
        if(canCombineIngredients != null){
            canCombineIngredients(id, neighbor);
        }
    }

    public event Action<string> hasLevelFinished;
    public void LevelFinished(string result){
        if(hasLevelFinished != null){
            hasLevelFinished(result);
        }
    }

    public event Action toCleanUp;
    public void CleanUp(){
        if(toCleanUp != null){
            toCleanUp();
        }
    }

    public event Action<int> genNewLevel;
    public void NewLevel(int levelComplete){
        if(genNewLevel != null){
            genNewLevel(levelComplete);
        }
    }

}
