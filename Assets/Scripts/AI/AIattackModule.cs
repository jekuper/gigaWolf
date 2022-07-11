using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AIattackModule : MonoBehaviour, IaiModule {
    [SerializeField] private AnimalAISystem sys;


    public void MainHandler () { }
 
    private void OnTriggerEnter (Collider other) {
        if (((1 << other.gameObject.layer) & sys.attackMask.value) != 0) {
            other.gameObject.GetComponent<IAliveEntity> ().Die ();
        }
    }
}
