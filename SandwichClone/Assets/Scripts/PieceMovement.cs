using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;

    public int ingredientAmount = 1;

    private void OnMouseDown(){
        firstTouchPos = Input.mousePosition;
    }

    private void OnMouseUp(){
        finalTouchPos = Input.mousePosition;
        Debug.Log("I: " + firstTouchPos + " <-> F: " + finalTouchPos);
        if(Vector2.Distance(firstTouchPos, finalTouchPos) > 200f){
            Vector2 dir = GetDir(AngleBetween(firstTouchPos, finalTouchPos));
            transform.position += new Vector3(dir.x, ingredientAmount*0.2f, dir.y);
        }
    }
    
    private float AngleBetween(Vector2 P1, Vector2 P2){
        return Mathf.Atan2(P2.y - P1.y, P2.x - P1.x)*(180/Mathf.PI);
    }

    private Vector2 GetDir(float angle){ // Return an unit vector to be used as the target direction 
        if(angle >= 45 && angle < 135){
            return new Vector2(0,1);
        }else if(angle >= 135 || angle < -135){
            return new Vector2(-1,0);
        }else if(angle >= -135 && angle < -45){
            return new Vector2(0,-1);
        }else if(angle >= -45 && angle < 45){
            return new Vector2(1,0);
        }
        return new Vector2(0,0);
    }

    public void Disappear(){
        Destroy(gameObject);
    }
}
