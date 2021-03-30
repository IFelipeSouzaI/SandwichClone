using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{
    public GameObject piecePreFab;
    private int currentLevelIndex = 0;
    public List<Level> levels;

    void Start()
    {
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].topRow[i]){
                NewPiece(levels[currentLevelIndex].topRow[i], i+0.5f, 3.5f);
            }
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].topMiddleRow[i]){
                NewPiece(levels[currentLevelIndex].topMiddleRow[i], i+0.5f, 2.5f);
            }
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].bottomMiddleRow[i]){
                NewPiece(levels[currentLevelIndex].bottomMiddleRow[i], i+0.5f, 1.5f);
            }
        }
        for(int i = 0; i < 4; i++){
            if(levels[currentLevelIndex].bottomRow[i]){
                NewPiece(levels[currentLevelIndex].bottomRow[i], i+0.5f, 0.5f);
            }
        }
    }

    void NewPiece(Ingredient ingredient, float x, float z){
        Vector3 pos = new Vector3(x, 0, z);
        GameObject obj = (GameObject)Instantiate(piecePreFab, pos, Quaternion.identity);
        obj.transform.parent = transform;
        obj.GetComponent<MeshFilter>().mesh = ingredient.mesh;
    }
}
