using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatMechanic : MonoBehaviour {
    public int maxSheepCount = 21;
    public int coughtSheeps = 0;
    [SerializeField] private Animator winMenuAnimator;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject UIHolder;
    [SerializeField] private RectTransform uiIndicator;
    [SerializeField] private Animator uiIndicatorAnimator;
    [SerializeField] private PlayerController player;

    private float maxIndicatorHeight;


    private void Start () {
        maxIndicatorHeight = uiIndicator.sizeDelta.y; 
        uiIndicator.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, coughtSheeps * maxIndicatorHeight / maxSheepCount);
    }

    private void Update () {
        if (coughtSheeps == maxSheepCount) {
            coughtSheeps++;
            winMenuAnimator.SetTrigger ("show");
            player.enabled = false;
            animator.SetFloat ("speed", 0);
            UIHolder.SetActive (false);
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "sheep") {
            animator.SetTrigger ("attack");
            other.transform.parent.GetComponent<IAliveEntity> ().Die ();
            coughtSheeps++;
            uiIndicator.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, coughtSheeps * maxIndicatorHeight / maxSheepCount);
            uiIndicatorAnimator.SetTrigger ("pulse");
        }
    }
}
