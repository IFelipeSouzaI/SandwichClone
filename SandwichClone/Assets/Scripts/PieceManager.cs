using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    private Vector2 firstTouchPos; // First touch position
    private Vector2 finalTouchPos; // Last touch position

    private Ingredient ingredient;
    public string strID = ""; // What type of ingredient that is

    public int pieceID  = 0; // ID to check informations in board
    public int ingredientAmount = 1; // How many ingredients are attached

    private Vector3 destiny; // Piece during a move
    private Vector3 heightMatch; // Height to be achieved (just to prevent the ingredients models intersection)
    private Vector3 rotateTo; // Fina rotation value

    private const float MIN_TOUCH_DISTANCE = 125f; // Minimun distance between the touchs (pixel)
    private const float PIECE_HEIGHT = 0.2f; // Height of the model

    private void Start(){
        GameEvents.current.canCombineIngredients += MoveTo;
        GameEvents.current.toCleanUp += Disappear;
    }

    public void SetData(int id, Ingredient scriptableIngredient){ // Called when a piece is created on "LevelManager", load the ingredients information
        ingredient = scriptableIngredient;
        GetComponent<MeshFilter>().mesh = ingredient.mesh;
        //GetComponent<MeshRenderer>().material = ingredient.material;
        strID = ingredient.strID;
        pieceID = id;
    }

    private void Update(){
        if(destiny != Vector3.zero){ // If a destination was seted -> move
            if(Vector3.Distance(transform.position, destiny) > 0.1f){
                if(transform.position.y < destiny.y){ // Match the height
                    transform.position = Vector3.Lerp(transform.position, heightMatch, 0.1f);
                }else{
                    transform.position = Vector3.Lerp(transform.position, destiny, 0.1f);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotateTo), 0.15f);
            }else{
                transform.position = destiny;
                transform.rotation = Quaternion.Euler(rotateTo);
                destiny = Vector3.zero;
                GameEvents.current.MovementEnded(ingredientAmount, strID);
            }
        }
    }

    private void MoveTo(int id, GameObject neighbor){ // Set the piece destiny
        if(id == pieceID){
            Vector2 dir = GetDir(AngleBetween(firstTouchPos, finalTouchPos));
            int yPos = (ingredientAmount + neighbor.GetComponent<PieceManager>().ingredientAmount-1);
            destiny = transform.position + new Vector3(dir.x, yPos*PIECE_HEIGHT, dir.y);
            
            rotateTo = new Vector3((dir.y*180)*-1, 0, dir.x*180);
            heightMatch = new Vector3(transform.position.x, destiny.y+1, transform.position.z);
            
            strID = neighbor.GetComponent<PieceManager>().strID + str2rts(strID); // Join the id (invert the self id to "rotate")
            neighbor.GetComponent<PieceManager>().strID = strID;
            ingredientAmount = (ingredientAmount + neighbor.GetComponent<PieceManager>().ingredientAmount); // Increse both pieces amount
            neighbor.GetComponent<PieceManager>().ingredientAmount = ingredientAmount;

            transform.parent = neighbor.transform; // Attach to the neighbor piece
            GetComponent<Collider>().enabled = false;
        }
    }

    // Event Listener "toCleanUp"
    private void Disappear(){
        GameEvents.current.canCombineIngredients -= MoveTo;
        GameEvents.current.toCleanUp -= Disappear;
    }
    
    private string str2rts(string str){ // Invert the id (rotate)
        string rts = "";
        for(int i = 0; i < ingredientAmount; i++){
            rts += str[ingredientAmount-1-i];
        }
        return rts;
    }

    private void OnMouseDown(){
        firstTouchPos = Input.mousePosition;
    }

    private void OnMouseUp(){
        finalTouchPos = Input.mousePosition;
        Vector2 dir = GetDir(AngleBetween(firstTouchPos, finalTouchPos));
        if(Vector2.Distance(firstTouchPos, finalTouchPos) > MIN_TOUCH_DISTANCE){
            GameEvents.current.TouchEnded(pieceID, dir);
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
