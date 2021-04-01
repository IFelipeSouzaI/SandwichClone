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

    public event Action<int, Vector2> hasTouchEnded; // Last touch on piece
    public void TouchEnded(int id, Vector2 moveDir){
        if(hasTouchEnded != null){
            hasTouchEnded(id, moveDir);
        }
    }

    public event Action<int, GameObject> canCombineIngredients; // If the target to move has a ingredient (!= null)
    public void CombineIngredients(int id, GameObject neighbor){
        if(canCombineIngredients != null){
            canCombineIngredients(id, neighbor);
        }
    }

    public event Action<int, string> hasMovementEnded; // Piece movement finished
    public void MovementEnded(int ingredientAmount, string strID){
        if(hasMovementEnded != null){
            hasMovementEnded(ingredientAmount, strID);
        }
    }

    public event Action<string> hasLevelFinished; // The result of a combination
    public void LevelFinished(string result){
        if(hasLevelFinished != null){
            hasLevelFinished(result);
        }
    }

    public event Action<bool> onLevelReset; // Tell if we need a new level or just restart the current one
    public void LevelReset(bool levelComplete){
        if(onLevelReset != null){
            onLevelReset(levelComplete);
        }
    }

    public event Action toCleanUp; // When creating a new board or simply resetting the level (to remove all piece listeners)
    public void CleanUp(){
        if(toCleanUp != null){
            toCleanUp();
        }
    }

}
