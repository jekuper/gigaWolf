using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarManager : MonoBehaviour
{
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "sheep") {
            AnimalAI s = other.transform.parent.gameObject.GetComponent<AnimalAI> ();
            s.RoarEffect (5);
        }
    }
    public void Delete () {
        Destroy (gameObject);
    }
}
