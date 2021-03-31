using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;

    public int ingredientID  = 0;
    public int ingredientAmount = 1;

    private Vector3 finalPos;
    private Vector3 heightMatch;
    private Vector3 rotateTo;

    private const float MIN_TOUCH_DISTANCE = 125f;
    private const float PIECE_HEIGHT = 0.2f;

    private void Start(){
        GameEvents.current.canCombineIngredients += Combine;
    }

    private void Update(){
        if(finalPos != Vector3.zero){
            if(Vector3.Distance(transform.position, finalPos) > 0.1f){
                if(transform.position.y < finalPos.y){
                    transform.position = Vector3.Lerp(transform.position, heightMatch, 0.1f);
                }else{
                    transform.position = Vector3.Lerp(transform.position, finalPos, 0.1f);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotateTo), 0.15f);
            }else{
                transform.position = finalPos;
                transform.rotation = Quaternion.Euler(rotateTo);
                finalPos = Vector3.zero;
            }
        }
    }

    private void Combine(int id, GameObject neighbor){
        if(id == ingredientID){
            Vector2 dir = GetDir(AngleBetween(firstTouchPos, finalTouchPos));
            int totalIngredientAmount = (ingredientAmount + neighbor.GetComponent<PieceManager>().ingredientAmount-1);
            finalPos = transform.position + new Vector3(dir.x, totalIngredientAmount*PIECE_HEIGHT, dir.y);
            rotateTo = new Vector3((dir.y*180)*-1, 0, dir.x*180);
            heightMatch = new Vector3(transform.position.x, finalPos.y+1, transform.position.z);
            neighbor.GetComponent<PieceManager>().ingredientAmount += ingredientAmount;
            transform.parent = neighbor.transform;
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnMouseDown(){
        firstTouchPos = Input.mousePosition;
    }

    private void OnMouseUp(){
        finalTouchPos = Input.mousePosition;
        Vector2 dir = GetDir(AngleBetween(firstTouchPos, finalTouchPos));
        if(Vector2.Distance(firstTouchPos, finalTouchPos) > MIN_TOUCH_DISTANCE){
            GameEvents.current.TouchEnded(ingredientID, dir);
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
}

        /*
            //Vector2 target = new Vector2(column + dir.x, row + dir.y);
            //collider.enabled = false;
            //
        }*/
