using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoarMechanic : MonoBehaviour {
    public float roarReloadTimer = 10f;
    [SerializeField] private KeyCode roarCode;
    [SerializeField] private RectTransform uiForeground;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private Animator pulsarButtonAnimator;

    private GameObject pulsar;
    private float timer = 0;
    private float maxForegroundHeight;

    private void Start () {
        maxForegroundHeight = uiForeground.sizeDelta.y;
        pulsar = Resources.Load ("pulsar") as GameObject;
    }

    private void Update () {

        if (Input.GetKeyDown (roarCode)) {
            Roar ();
        }

        if (timer > 0) {
            uiForeground.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, (roarReloadTimer - timer) * maxForegroundHeight / roarReloadTimer);
            timer -= Time.deltaTime;
            if (timer <= 0) {
                pulsarButtonAnimator.SetTrigger ("pulse");
                uiForeground.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, maxForegroundHeight);
                uiText.color = new Color (uiText.color.r, uiText.color.g, uiText.color.b, 1f);
                timer = -1;
            }
        }
    }
    public void Roar () {
        if (timer > 0) {
            return;
        }
        pulsarButtonAnimator.SetTrigger ("pulse");
        Instantiate (pulsar, transform);
        timer = roarReloadTimer;
        uiText.color = new Color (uiText.color.r, uiText.color.g, uiText.color.b, 0.5f);
    }
}
