using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class beatifulCamera : MonoBehaviour
{
    [SerializeField] private beatifulCamera nextCam;
    [SerializeField] private RenderTexture targetTexture;
    [SerializeField] private Animator anim;
    [SerializeField] private float timer = -1;

    public void Initiate () {
        nextCam.gameObject.SetActive (true);
        GetComponent<Camera> ().targetTexture = targetTexture;
        anim.SetTrigger ("show");
        timer = Random.Range (6, 10);
    }

    private void Update () {
        if (timer >= 0) {
            timer -= Time.deltaTime;
            if (timer < 0) {
                if (nextCam != null) {
                    StartCoroutine (Animate ());
                }
            }
        }
    }
    private IEnumerator Animate () {
        anim.SetTrigger ("hide");
        yield return new WaitForSeconds (2);
        GetComponent<Camera> ().targetTexture = null;
        nextCam.Initiate ();
        gameObject.SetActive (false);
    }
}
