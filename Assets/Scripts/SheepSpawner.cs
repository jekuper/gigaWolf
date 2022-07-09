using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sheepPrefab;

    private void Start () {
        for (int i = 0; i < 21; i++) {
            Instantiate (sheepPrefab, new Vector3(Random.Range(-24, 24), 0.25f, Random.Range(-24, 24)), Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
