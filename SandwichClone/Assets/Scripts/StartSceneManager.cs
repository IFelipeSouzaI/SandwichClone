using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public Animator anim;
    public Text inputText;
    public Level[] allLevels;
    public Text errorLabel;
    public Text levelsLabel;
    public bool canTouch = true;

    private void Awake(){
        LevelInfo.levels = allLevels;
    }

    private void Start(){
        levelsLabel.text = "Nº Levels: " + allLevels.Length + " -> " + 0 + " | " + (allLevels.Length-1);
    }

    public void LoadLevel(){
        if(!canTouch){
            return;
        }
        if(Convert.ToInt32(inputText.text) >= allLevels.Length){
            errorLabel.text = "Error, level was not found \n" + "Nº Levels: " + allLevels.Length + " -> " + 0 + " | " + (allLevels.Length-1);
        }else{
            LevelInfo.levelID = Convert.ToInt32(inputText.text);
            anim.Play("out");
            canTouch = false;
        }
    }

    public void RandomLevel(){
        /*if(!canTouch){
            return;
        }
        LevelInfo.levelID = -1;
        anim.Play("out");
        canTouch = false;*/
    }

    public void MainScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
