using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{

    private bool canMove = false;
    private int currentLevelIndex = 0;
    public Level[] testLevels;
    public List<Ingredient> ingredients;

    private int pieceIndex = 0;
    private int ingredientTotal = 0;
    public GameObject piecePreFab;
    public GameObject[,] board;

    private const int BOARD_HEIGHT = 4;
    private const int BOARD_WIDTH = 4;

    private void Start(){
        board = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];
        if(LevelInfo.levelID >= 0){
            currentLevelIndex = LevelInfo.levelID;
            FillTheBoard();
        }else if(LevelInfo.levelID == -2){
            LevelInfo.levels = testLevels;
            FillTheBoard();
        }else{
            FillTheBoardRandomly();
        }
        GameEvents.current.hasTouchEnded += HasNeighbor;
        GameEvents.current.hasMovementEnded += CheckAfterMove;
        GameEvents.current.genNewLevel += LoadNewLevel;
    }

    private void FillTheBoardRandomly(){

    }

    private void FillTheBoard(){
        for(int i = 0; i < 4; i++){
            if(LevelInfo.levels[currentLevelIndex].bottomRow[i]){
                board[0,i] = NewPiece(LevelInfo.levels[currentLevelIndex].bottomRow[i], i+0.5f, 0.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(LevelInfo.levels[currentLevelIndex].bottomMiddleRow[i]){
                board[1,i] = NewPiece(LevelInfo.levels[currentLevelIndex].bottomMiddleRow[i], i+0.5f, 1.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(LevelInfo.levels[currentLevelIndex].topMiddleRow[i]){
                board[2,i] = NewPiece(LevelInfo.levels[currentLevelIndex].topMiddleRow[i], i+0.5f, 2.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        for(int i = 0; i < 4; i++){
            if(LevelInfo.levels[currentLevelIndex].topRow[i]){
                board[3,i] = NewPiece(LevelInfo.levels[currentLevelIndex].topRow[i], i+0.5f, 3.5f);
                ingredientTotal += 1;
            }
            pieceIndex += 1;
        }
        canMove = true;
    }

    private void LoadNewLevel(int levelComplete){
        BoardClear();
        if(LevelInfo.levelID == -1){
            FillTheBoardRandomly();
            return;
        }
        if(levelComplete == 1){
            currentLevelIndex += 1;
            if(currentLevelIndex >= LevelInfo.levels.Length){
                currentLevelIndex = 0;
            }
        }
        canMove = true;
        pieceIndex = 0;
        ingredientTotal = 0;
        FillTheBoard();
    }

    private void BoardClear(){
        GameEvents.current.CleanUp();
        for(int i = 0; i < BOARD_HEIGHT; i++){
            for(int j = 0; j < BOARD_WIDTH; j++){
                if(board[i,j]){
                    Destroy(board[i,j]);
                }
                board[i,j] = null;
            }
        }
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
            Debug.Log("Size Match");
            if(strID[0] == 'B' && strID[0] == strID[ingredientTotal-1]){
                Debug.Log("WIN");
                GameEvents.current.LevelFinished("Winner");
            }else{
                GameEvents.current.LevelFinished("Loser");
            }
            return;
        }
        canMove = true;
    }

}