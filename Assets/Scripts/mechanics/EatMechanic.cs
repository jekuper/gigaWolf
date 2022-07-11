using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatMechanic : MonoBehaviour {

    public int coughtSheeps = 0;
    [SerializeField] private Animator winMenuAnimator;
    [SerializeField] private Animator animator;

    

    private void Update () {
        if (coughtSheeps == 21) {
            coughtSheeps++;
            winMenuAnimator.SetTrigger ("show");
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "sheep") {
            animator.SetTrigger ("attack");
            other.transform.parent.GetComponent<IAliveEntity> ().Die ();
            coughtSheeps++;
        }
    }
}
