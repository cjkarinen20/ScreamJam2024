using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class GameObjectTrigger : MonoBehaviour
{
    [System.Serializable]
    public class Objects{
        public float delay = 0;
        public GameObject GO;
        public bool wantedState;// The state we want the objects to be set too
    }

    [System.Serializable]
    public class Events{
        public float delay = 0;
        public UnityEvent _event;
    }
    
    public Objects[] objects;// Objects to toggle
    public Events[] events;

    public bool isTrigger = true, shouldDestroy = true, shouldDestroyObject = false, onEnable = false, canStateChangeMoreThanOnce = false;

    private bool hasTriggered = false;
    
    private void OnEnable() {
        if(hasTriggered && !canStateChangeMoreThanOnce) return;
        if(onEnable){

            ChangeState();
            
            hasTriggered = true;
        }
    }

    private void OnTriggerEnter(Collider other) {// Checks if trigger is player and if so applies the logic
        if(hasTriggered && !canStateChangeMoreThanOnce || other.tag != "Player" || !isTrigger) return;
        
        if(other.tag == "Player" && isTrigger){
            ChangeState();
        }

        hasTriggered = true;
    }

    public void SetStateTrue(){
        float deleteDelay = 0;
        for(int i = 0; i < objects.Length; i++) {
            StartCoroutine(State(objects[i].GO, true, objects[i].delay));
            deleteDelay += objects[i].delay;
        }
        Invoke("Delete", deleteDelay + .2f);
    }

    public void SetStateFalse(){
        float deleteDelay = 0;
        for(int i = 0; i < objects.Length; i++) {
            StartCoroutine(State(objects[i].GO, false, objects[i].delay));
            deleteDelay += objects[i].delay;
        }
        Invoke("Delete", deleteDelay + .2f);
    }

    public void ChangeState () {
        float deleteDelay = 0;
        for(int i = 0; i < events.Length; i++) {
            StartCoroutine(CallEvent(events[i]._event, events[i].delay));

            deleteDelay += events[i].delay;
        }
        for(int i = 0; i < objects.Length; i++) {

            StartCoroutine(State(objects[i].GO, objects[i].wantedState, objects[i].delay));

            deleteDelay += objects[i].delay;
        }
        Invoke("Delete", deleteDelay + .2f);
    }

    IEnumerator CallEvent(UnityEvent _event, float delay){
        yield return new WaitForSeconds(delay);
        if(_event != null){
            _event?.Invoke();
        }
    }

    IEnumerator State(GameObject GO, bool state, float delay){
        yield return new WaitForSeconds(delay);
        if(GO != null){
            GO.SetActive(state);
        }
    }

    private void Delete(){
        if(shouldDestroyObject){
            Destroy(this.gameObject);
        }
        if(shouldDestroy){
            Destroy(this);
        }
    }
}
