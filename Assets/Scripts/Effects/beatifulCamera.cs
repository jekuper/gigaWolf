using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BeatifulCamera : MonoBehaviour
{
    [SerializeField] private BeatifulCamera nextCam;
    [SerializeField] private RenderTexture targetTexture;
    [SerializeField] private Animator anim;
    [SerializeField] private float timer = -1;

    public void Initiate () {
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
        nextCam.gameObject.SetActive (true);
        anim.SetTrigger ("hide");
        yield return new WaitForSeconds (2);
        GetComponent<Camera> ().targetTexture = null;
        nextCam.Initiate ();
        gameObject.SetActive (false);
    }
}
