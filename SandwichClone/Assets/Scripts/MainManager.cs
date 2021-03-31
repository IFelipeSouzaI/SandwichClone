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

    public void StartScene(){
        anim.Play("out");
    }

    public void ResetLevel(){
        anim.Play("levelFail", 0, 0.0f);
    }

    public void ChangeScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
    
    public void NewLevel(int levelComplete){
        GameEvents.current.NewLevel(levelComplete);
    }

    public void LevelFinished(string result){
        if(result == "Winner"){
            anim.Play("levelComplete", 0, 0.0f);
        }else{
            anim.Play("levelFail", 0, 0.0f);
        }
    }
}
