using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIfearModule : MonoBehaviour, IaiModule {
    [SerializeField] private AnimalAISystem sys;

    public float calmDistance = 12;

    public List<Transform> threatRefs = new List<Transform> ();

    public void MainHandler () {
        for (int i = 0; i < threatRefs.Count; i++) {
            if (threatRefs[i] == null || Vector3.Distance (sys.mainTransform.position, threatRefs[i].position) > calmDistance) {
                threatRefs.RemoveAt (i);
                i--;
            }
        }
        if (threatRefs.Count != 0) {
            float targetAngle = GetAverageAngle (threatRefs);
            Quaternion targetRotation = Quaternion.Euler (0f, Mathf.MoveTowardsAngle (sys.mainTransform.eulerAngles.y, targetAngle, Time.fixedDeltaTime * sys.rotationSpeed), 0f);
        
            sys.SendRotation (targetRotation, rotationSource.Fear);
        }
    }
    private void OnTriggerEnter (Collider other) {
        if (((1 << other.gameObject.layer) & sys.threatsMask.value) != 0) {
            threatRefs.Add (other.transform);
        }
    }

    private float GetAverageAngle (List<Transform> threads) {
        if (threads.Count == 0) {
            Debug.LogError ("not enough threads");
            return 0;
        }
        Vector3 avr = Globals.GetOppossiteDir (sys.mainTransform, threads[0]);
        for (int i = 1; i < threads.Count; i++) {
            avr += Globals.GetOppossiteDir (sys.mainTransform, threads[i]);
        }
        avr /= threads.Count;
        return Globals.DirectionVectorToAngle (avr);
    }

}
