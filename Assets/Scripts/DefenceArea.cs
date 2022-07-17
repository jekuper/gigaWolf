using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenceArea : MonoBehaviour {
    public int maxSheepCount = 21;
    public int coughtSheeps = 0;
    public int eatenSheeps = 0;
    [SerializeField] private Animator winMenuAnimator;
    [SerializeField] private GameObject UIHolder;
    [SerializeField] private TextMeshProUGUI uiIndicator;
    [SerializeField] private TextMeshProUGUI resultText;


    private void Start () {
        UpdateText ();
    }
    private void Update () {
        if (coughtSheeps + eatenSheeps == maxSheepCount) {
            winMenuAnimator.SetTrigger ("show");
            UIHolder.SetActive (false);
            enabled = false;
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "sheep") {
            StartCoroutine (Dissapear (other.transform.parent));
            coughtSheeps++;
            UpdateText ();
            uiIndicator.text = coughtSheeps.ToString ();
        }
    }
    private IEnumerator Dissapear (Transform target) {
        float moveTime = Random.Range (3, 5);
        target.GetComponent<AnimalAISystem> ().movementModule.MoveForward (moveTime);

        yield return new WaitForSeconds (moveTime);

        target.GetComponent<AnimalAISystem> ().TurnOff ();
        target.GetComponent<Animator> ().SetTrigger ("placed");
    }
    private void UpdateText () {
        resultText.text = coughtSheeps.ToString () + "/" + maxSheepCount.ToString ();
    }
}
