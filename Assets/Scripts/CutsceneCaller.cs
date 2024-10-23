using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCaller : MonoBehaviour
{
    public void StartCutscene () {
        CutsceneManager.instance.StartCutscene();
    }
    public void EndCutscene(){
        CutsceneManager.instance.EndCutscene();
    }
}
