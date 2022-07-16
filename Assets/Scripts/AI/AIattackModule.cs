using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AIattackModule : MonoBehaviour, IaiModule {
    [SerializeField] private AnimalAISystem sys;
    [SerializeField] private DefenceArea reportObj;


    public void MainHandler () { }
 
    private void OnTriggerEnter (Collider other) {
        if (((1 << other.gameObject.layer) & sys.attackMask.value) != 0) {
            IAliveEntity target = null;
            if (other.gameObject.GetComponent<IAliveEntity> () != null) {
                target = other.gameObject.GetComponent<IAliveEntity> ();
            }
            else if (other.transform.parent.GetComponent<IAliveEntity> () != null) {
                target = other.transform.parent.GetComponent<IAliveEntity> ();
            }
            if (target != null) {
                target.Die ();
                if (reportObj != null)
                    reportObj.eatenSheeps++;
            }
        }
    }
}
