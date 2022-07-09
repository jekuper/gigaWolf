using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public float roarReloadTimer = 10f;
    [SerializeField] private KeyCode roarCode;
    [SerializeField] private GameObject pulsar, bloodParticle;
    [SerializeField] private RectTransform uiForeground;

    private float timer = 0;
    private float maxForegroundWidth;

    private void Start () {
        maxForegroundWidth = uiForeground.sizeDelta.x;
    }

    #region roar mechnanic
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
    #endregion

    #region eating mechanic
    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.tag == "sheep") {
            Instantiate (bloodParticle, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
    #endregion
}
