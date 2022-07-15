using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GotToScene (string name) {
        SceneManager.LoadScene (name);
    }
    public void ExitGame () {
        Application.Quit ();
    }
    private void Update () {
        if (Input.GetKeyDown (KeyCode.X)) {
            GotToScene ("menu");
        }
    }
}
