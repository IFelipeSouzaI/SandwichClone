using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{

    private bool canMove = false;
    private int currentLevelIndex = 0;
    public List<Level> levels;
    
    private int pieceIndex = 0;
    private int ingredientTotal = 0;
    public GameObject piecePreFab;
    public GameObject[,] board;
    private const int BOARD_HEIGHT = 4;
    private const int BOARD_WIDTH = 4;

    private void Start()
    {
        board = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];
        FillTheBoard();
        GameEvents.current.hasTouchEnded += HasNeighbor;
        GameEvents.current.hasMovementEnded += CheckAfterMove;
    }

    private void FillTheBoard(){
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].bottomRow[i]){
                board[0,i] = NewPiece(levels[currentLevelIndex].bottomRow[i], i+0.5f, 0.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].bottomMiddleRow[i]){
                board[1,i] = NewPiece(levels[currentLevelIndex].bottomMiddleRow[i], i+0.5f, 1.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].topMiddleRow[i]){
                board[2,i] = NewPiece(levels[currentLevelIndex].topMiddleRow[i], i+0.5f, 2.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].topRow[i]){
                board[3,i] = NewPiece(levels[currentLevelIndex].topRow[i], i+0.5f, 3.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        canMove = true;
    }

    private GameObject NewPiece(Ingredient scriptableIngredient, float x, float z){
        Vector3 pos = new Vector3(x, 0f, z);
        GameObject obj = (GameObject)Instantiate(piecePreFab, pos, Quaternion.identity);
        obj.transform.parent = transform;
        obj.GetComponent<PieceManager>().SetData(pieceIndex, scriptableIngredient);
        return obj;
    }

    private void HasNeighbor(int id, Vector2 moveDir){
        if(!canMove){
            return;
        }
        int targetRow = (int)id/4 + (int)moveDir.y;
        int targetColumn = id%4 + (int)moveDir.x;
        //Debug.Log("TRow: "targetRow + " -- TColumn" + targetColumn);
        if(targetRow >= BOARD_HEIGHT || targetRow < 0 || targetColumn >= BOARD_WIDTH || targetColumn < 0){
            return;
        }
        if(board[targetRow, targetColumn]){
            GameEvents.current.CombineIngredients(id, board[targetRow, targetColumn]);
            board[(int)id/4, id%4] = null;
            canMove = false;
        }
    }

    public void CheckAfterMove(int ingredientAmount, string strID){
        Debug.Log(strID);
        if(ingredientAmount == ingredientTotal){
            Debug.Log("GameFinished");
            if(strID[0] == 'B' && strID[0] == strID[ingredientTotal-1]){
                Debug.Log("WINNER");
                GameEvents.current.GameFinished("Winner");
            }else{
                Debug.Log("LOSER");
                GameEvents.current.GameFinished("Loser");
            }
            return;
        }
        canMove = true;
    }

}