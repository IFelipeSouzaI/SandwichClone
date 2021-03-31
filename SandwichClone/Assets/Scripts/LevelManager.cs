using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{

    private bool canMove = true;
    private int currentLevelIndex = 0;
    public List<Level> levels;
    
    private int pieceIndex = 0;
    public GameObject piecePreFab;
    public GameObject[,] board;
    private const int BOARD_HEIGHT = 4;
    private const int BOARD_WIDTH = 4;

    void Start()
    {
        board = new GameObject[BOARD_HEIGHT,BOARD_WIDTH];
        FillTheBoard();
        GameEvents.current.hasTouchEnded += HasNeighbor;
    }

    private void FillTheBoard(){
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].bottomRow[i]){
                board[0,i] = NewPiece(levels[currentLevelIndex].bottomRow[i], i+0.5f, 0.5f);
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].bottomMiddleRow[i]){
                board[1,i] = NewPiece(levels[currentLevelIndex].bottomMiddleRow[i], i+0.5f, 1.5f);
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].topMiddleRow[i]){
                board[2,i] = NewPiece(levels[currentLevelIndex].topMiddleRow[i], i+0.5f, 2.5f);
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].topRow[i]){
                board[3,i] = NewPiece(levels[currentLevelIndex].topRow[i], i+0.5f, 3.5f);
            }
            pieceIndex += 1;
        }
    }

    private GameObject NewPiece(Ingredient ingredient, float x, float z){
        Vector3 pos = new Vector3(x, 0f, z);
        GameObject obj = (GameObject)Instantiate(piecePreFab, pos, Quaternion.identity);
        obj.transform.parent = transform;
        obj.GetComponent<PieceManager>().ingredientID = pieceIndex;
        obj.GetComponent<MeshFilter>().mesh = ingredient.mesh;
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
        }
    }
}
