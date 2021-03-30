using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    //private Collider collider;

    public int ingredientID  = 0;
    public int ingredientAmount = 1;

    private const float MIN_TOUCH_DISTANCE = 125f;
    private const float PIECE_HEIGHT = 0.2f;

    private void Start(){
        GameEvents.current.canCombineIngredients += Combine;
    }

    private void OnMouseDown(){
        firstTouchPos = Input.mousePosition;
        //collider = GetComponent<Collider>();
    }

    private void OnMouseUp(){
        finalTouchPos = Input.mousePosition;
        Debug.Log("I: " + firstTouchPos + " <-> F: " + finalTouchPos);
        if(Vector2.Distance(firstTouchPos, finalTouchPos) > MIN_TOUCH_DISTANCE){
            GameEvents.current.TouchEnded(ingredientID);
        }
    }
    
    private void Combine(int id){
        if(id == ingredientID){
            Vector2 dir = GetDir(AngleBetween(firstTouchPos, finalTouchPos));
            transform.position += new Vector3(dir.x, ingredientAmount*PIECE_HEIGHT, dir.y);
            transform.Rotate(new Vector3(dir.y*180, 0, dir.x*180));
        }
        /*
            //Vector2 target = new Vector2(column + dir.x, row + dir.y);
            //collider.enabled = false;
            //anotherPiece.GetComponent<PieceMovement>().ingredientAmount += ingredientAmount;
            //transform.parent = anotherPiece.transform;
        }*/
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
}
