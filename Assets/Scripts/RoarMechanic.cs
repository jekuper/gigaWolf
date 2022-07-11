using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarMechanic : MonoBehaviour
{
    public float roarReloadTimer = 10f;
    [SerializeField] private KeyCode roarCode;
    [SerializeField] private RectTransform uiForeground;

    private GameObject pulsar;
    private float timer = 0;
    private float maxForegroundWidth;

    private void Start () {
        maxForegroundWidth = uiForeground.sizeDelta.x;
        pulsar = Resources.Load ("pulsar") as GameObject;
    }

    private void Update () {
        
        if (Input.GetKeyDown (roarCode) && timer <= 0) {
            Roar ();
            timer = roarReloadTimer;
        }
        if (timer > 0) {
            uiForeground.sizeDelta = new Vector2 ((roarReloadTimer - timer) * maxForegroundWidth / roarReloadTimer, uiForeground.sizeDelta.y);
            timer -= Time.deltaTime;
        } else {
            uiForeground.sizeDelta = new Vector2 (maxForegroundWidth, uiForeground.sizeDelta.y);
        }
    }
    public void Roar () {
        Instantiate (pulsar, transform);
    }
}
