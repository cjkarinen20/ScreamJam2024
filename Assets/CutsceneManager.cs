using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public NewFPSController newFPSController;
    private void Awake() {
        if(instance == null){
            instance = this;
        }
    }

    public void StartCutscene(){
        newFPSController.canMove = false;
    }

    public void EndCutscene(){
        newFPSController.canMove = true;
    }
}
