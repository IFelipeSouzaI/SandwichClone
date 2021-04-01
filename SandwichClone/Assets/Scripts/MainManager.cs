using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{   
    public Animator anim;

    public void Start(){
        GameEvents.current.hasLevelFinished += LevelFinished;
    }

    public void StartScenePressed(){ // Canvas Button
        anim.Play("out");
    }

    public void ChangeScene(){ // Called at the end of the animation out
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void RestartLevelPressed(){ // Canvas Button
        anim.Play("levelFail", 0, 0.0f);
    }

    public void NewLevelPressed(){ // Canvas Button
        LevelInfo.levelID = -1;
        anim.Play("levelComplete", 0, 0.0f);
    }

    // Animation "levelFail" is used to restart a level -> (Rong ingredients combination - Button "restart" pressed) 
    public void RestartLevel(){ // Called in animation "levelFail"
        GameEvents.current.LevelReset(false);
    }

    public void NewLevel(){ // Called in animation "levelComplete"
        GameEvents.current.LevelReset(true);
    }

    public void LevelFinished(string result){ // Event listener "hasLevelFinished"
        if(result == "Winner"){
            anim.Play("levelComplete", 0, 0.0f);
        }else{
            anim.Play("levelFail", 0, 0.0f);
        }
    }
}
