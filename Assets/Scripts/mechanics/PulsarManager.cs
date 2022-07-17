using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsarManager : MonoBehaviour {
    private void OnTriggerEnter (Collider other) {
        if (other.transform.parent != null && other.transform.parent.GetComponent<AnimalAISystem> () != null) {
            AnimalAISystem s = other.transform.parent.gameObject.GetComponent<AnimalAISystem> ();
            s.RoarEffect (5);
        }
    }
    public void Delete () {
        Destroy (gameObject);
    }
}
