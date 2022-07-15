using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceArea : MonoBehaviour {
    public int maxSheepCount = 21;
    public int coughtSheeps = 0;
    [SerializeField] private Animator winMenuAnimator;
    [SerializeField] private GameObject UIHolder;
    [SerializeField] private RectTransform uiIndicator;
    [SerializeField] private Animator uiIndicatorAnimator;

    private float maxIndicatorHeight;


    private void Start () {
        maxIndicatorHeight = uiIndicator.sizeDelta.y;
        uiIndicator.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, coughtSheeps * maxIndicatorHeight / maxSheepCount);
    }

    private void Update () {
        if (coughtSheeps == maxSheepCount) {
            coughtSheeps++;
            winMenuAnimator.SetTrigger ("show");
            UIHolder.SetActive (false);
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "sheep") {
            other.transform.parent.GetComponent<AnimalAISystem> ().TurnOff ();
            other.transform.parent.GetComponent<Animator> ().SetTrigger ("placed");
            StartCoroutine (Dissapear(other.transform.parent, Random.Range(5, 8)));

            coughtSheeps++;
            uiIndicator.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, coughtSheeps * maxIndicatorHeight / maxSheepCount);
            uiIndicatorAnimator.SetTrigger ("pulse");
        }
    }
    private IEnumerator Dissapear (Transform target, float timer) {
        float speed = Vector3.Distance(target.localScale, Vector3.zero) / timer;
        while(timer > 0) {
            timer -= Time.deltaTime;
            target.localScale = Vector3.MoveTowards (target.localScale, Vector3.zero, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame ();
        }
        Destroy (target.gameObject);
    }
}
