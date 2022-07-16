using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseMechanic : MonoBehaviour
{
    public float impulseDuration = 1f;
    [SerializeField] private Transform mainTransform;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator uiAnimator;
    [SerializeField] private KeyCode inputKey = KeyCode.Q;

    private List<AnimalAISystem> targets = new List<AnimalAISystem> ();


    private void Update () {
        if (Input.GetKeyDown (inputKey) && targets.Count > 0) {
            Impulse ();
        }
    }

    public void Impulse () {
        if (targets.Count == 0) {
            return;
        }
        int nearestIndex = 0;
        for (int i = 1; i < targets.Count; i++) {
            if (Vector3.Distance (targets[i].mainTransform.position, transform.position) <
                Vector3.Distance (targets[nearestIndex].mainTransform.position, transform.position))
                nearestIndex = i;
        }
        uiAnimator.SetTrigger ("pulse");
        targets[nearestIndex].ImpulseEffect (impulseDuration, mainTransform.rotation);
        playerAnimator.SetTrigger ("impulse");
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "sheep") {
            targets.Add (other.transform.parent.GetComponent<AnimalAISystem> ());
        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "sheep") {
            for (int i = 0; i < targets.Count; i++) {
                if (targets[i].Equals (other.transform.parent.GetComponent<AnimalAISystem> ())) {
                    targets.RemoveAt (i);
                    break;
                }
            }
        }
    }
}
