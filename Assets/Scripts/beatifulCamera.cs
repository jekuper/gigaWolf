using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class beatifulCamera : MonoBehaviour
{
    [SerializeField] private beatifulCamera nextCam;
    [SerializeField] private Animator anim;
    [SerializeField] private float timer = -1;

    public void Generate () {
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
        anim.SetTrigger ("show");
        yield return new WaitForSeconds (1);
        nextCam.gameObject.SetActive (true);
        nextCam.Generate ();
        gameObject.SetActive (false);
    }
}
