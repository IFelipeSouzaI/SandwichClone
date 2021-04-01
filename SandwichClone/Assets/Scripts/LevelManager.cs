using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{

    private bool canMove = false; // Block the piece movement during some actions
    private int currentLevelIndex = 0; // Incare the level what need to be loaded and controls the next

    [Header("Random Level Pieces")]
    public int extraIngredients = 0; // Controls the number of extra pieces when a random level is created
    [Header("Levels to be Tested")]
    public Level[] testLevels; // List of levels to be played directly on editor
    [Header("All the Ingredients")]
    public Ingredient[] ingredients; // List of levels to be used to create random levels
    private Ingredient[,] randomLevel; // Random Level Holder
    [Header("Piece Prefab")]
    public GameObject piecePreFab; 
    private int ingredientTotal = 0; // How much ingredient has the level

    private GameObject[,] board; // Keep all the pieces reference during the level
    private const int BOARD_HEIGHT = 4;
    private const int BOARD_WIDTH = 4;

    private void Start(){
        board = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];
        if(LevelInfo.levelID >= 0){ // If the player is loading an existing level
            currentLevelIndex = LevelInfo.levelID; // Get the levelID to know what level need to be loaded
            FillTheBoard(); 
        }else if(LevelInfo.levelID == -2){ // -2 is a default value, if "Main" scene was played directly the levels to be loaded will come from "LevelManager" inspector
            LevelInfo.levels = testLevels; // Load those levels
            FillTheBoard();
        }else if(LevelInfo.levelID == -1){ // -1 is a value seted on "StartScene", create new random levels
            CreateRandomLevel();
            FillTheBoardRandomLevel();
        }
        GameEvents.current.hasTouchEnded += HasNeighbor;
        GameEvents.current.hasMovementEnded += CheckAfterMove;
        GameEvents.current.onLevelReset += ResetLevel;
    }

    private void FillTheBoardRandomLevel(){ // Fill the board based on "Ingredient[,] randomLevel"
        for(int i = 0; i < BOARD_HEIGHT; i++){
            for(int j = 0; j < BOARD_WIDTH; j++){
                if(randomLevel[i,j]){
                    board[i, j] = NewPiece(randomLevel[i,j], i, j);                
                }
            }
        } 
        canMove = true;
    }

    private void CreateRandomLevel(){ // Create a random level
        randomLevel = new Ingredient[BOARD_HEIGHT, BOARD_WIDTH];
        int randomColumn = Random.Range(0,BOARD_WIDTH-1);
        int randomRow = Random.Range(0,BOARD_HEIGHT-1);
        randomLevel[randomRow, randomColumn] = ingredients[0];
        int dirIndex = Random.Range(0,2);
        randomLevel[randomRow+(1-dirIndex), randomColumn+(dirIndex)] = ingredients[0];
        extraIngredients = Random.Range(2, 10);
        BoardWalker(randomRow+(1-dirIndex), randomColumn+(dirIndex), extraIngredients);
    }

    private void BoardWalker(int currentRow, int currentColumn, int ingredientsLeft){ // Recursive Function
        if(ingredientsLeft <= 0){
            return;
        }else{
            for(int i = 0; i<  2; i++){
                int row = currentRow-1+(i*2);
                if(row >= 0 && row < BOARD_HEIGHT){
                    if(randomLevel[row, currentColumn] == null){
                        randomLevel[row, currentColumn] = ingredients[Random.Range(1, ingredients.Length)];
                        BoardWalker(row, currentColumn, ingredientsLeft-1);
                        return;
                    }
                }
            }
            for(int j = 0; j < 2; j++){
                int column = currentColumn+1-(j*2);
                if(column >= 0 && column < BOARD_WIDTH){
                    if(randomLevel[currentRow, column] == null){
                        randomLevel[currentRow, column] = ingredients[Random.Range(1, ingredients.Length)];
                        BoardWalker(currentRow, column, ingredientsLeft-1);
                        return;
                    }
                }
            }
        }
    }

    private void FillTheBoard(){ // Fill the board based on "LevelInfo.levels" or "testLevels"
        for(int j = 0; j < 4; j++){
            if(LevelInfo.levels[currentLevelIndex].bottomRow[j]){
                board[0,j] = NewPiece(LevelInfo.levels[currentLevelIndex].bottomRow[j], 0, j);
            }
        }
        for(int j = 0; j < 4; j++){
            if(LevelInfo.levels[currentLevelIndex].bottomMiddleRow[j]){
                board[1,j] = NewPiece(LevelInfo.levels[currentLevelIndex].bottomMiddleRow[j], 1, j);
            }
        }
        for(int j = 0; j < 4; j++){
            if(LevelInfo.levels[currentLevelIndex].topMiddleRow[j]){
                board[2,j] = NewPiece(LevelInfo.levels[currentLevelIndex].topMiddleRow[j], 2, j);
            }
        }
        for(int j = 0; j < 4; j++){
            if(LevelInfo.levels[currentLevelIndex].topRow[j]){
                board[3,j] = NewPiece(LevelInfo.levels[currentLevelIndex].topRow[j], 3, j);
            }
        }
        canMove = true;
    }

    // Event listener "onLevelReset"
    private void ResetLevel(bool levelComplete){ // Create or restart a level
        GameEvents.current.CleanUp();
        BoardClear();
        ingredientTotal = 0;
        if(levelComplete){
            if(LevelInfo.levelID == -1){
                CreateRandomLevel();
                FillTheBoardRandomLevel();
            }else{
                currentLevelIndex += 1;
                if(currentLevelIndex >= LevelInfo.levels.Length){
                    currentLevelIndex = 0;
                }
                FillTheBoard();
            }
        }else{
            if(LevelInfo.levelID == -1){
                FillTheBoardRandomLevel();
            }else{
                FillTheBoard();
            }
        }
    }

    private void BoardClear(){ // Reset the board
        for(int i = 0; i < BOARD_HEIGHT; i++){
            for(int j = 0; j < BOARD_WIDTH; j++){
                if(board[i,j]){
                    Destroy(board[i,j]);
                }
                board[i,j] = null;
            }
        }
    }

    private GameObject NewPiece(Ingredient scriptableIngredient, int row, int column){ // Create a new piece
        ingredientTotal += 1;
        Vector3 pos = new Vector3(column+0.5f, 0f, row+0.5f);
        GameObject obj = (GameObject)Instantiate(piecePreFab, pos, Quaternion.identity);
        obj.transform.parent = transform;
        obj.GetComponent<PieceManager>().SetData(row*BOARD_WIDTH+column, scriptableIngredient);
        return obj;
    }

    // Event Listener "hasTouchEnded"
    private void HasNeighbor(int id, Vector2 moveDir){ // Check if has a piece in the target direction
        if(!canMove){
            return;
        }
        int targetRow = (int)id/4 + (int)moveDir.y;
        int targetColumn = id%4 + (int)moveDir.x;
        if(targetRow >= BOARD_HEIGHT || targetRow < 0 || targetColumn >= BOARD_WIDTH || targetColumn < 0){
            return;
        }
        if(board[targetRow, targetColumn]){
            GameEvents.current.CombineIngredients(id, board[targetRow, targetColumn]);
            board[(int)id/4, id%4] = null;
            canMove = false;
        }
    }

    // Event Listener "hasMovementEnded"
    public void CheckAfterMove(int ingredientAmount, string strID){ // Every time a move is finished, check if you have a right combination
        if(ingredientAmount == ingredientTotal){
            if(strID[0] == 'B' && strID[0] == strID[ingredientTotal-1]){
                GameEvents.current.LevelFinished("Winner");
            }else{
                GameEvents.current.LevelFinished("Loser");
            }
            return;
        }
        canMove = true;
    }

}