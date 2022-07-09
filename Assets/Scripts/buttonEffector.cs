using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonEffector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    [SerializeField]private Vector2 size1, size2;
    private Vector2 targetSize;
    private Vector2 refVelocity;
    private RectTransform rt;

    private void Start () {
        rt = GetComponent<RectTransform> ();
        targetSize = size1;
        rt.sizeDelta = size1;
    }

    private void Update () {
        rt.sizeDelta = Vector2.SmoothDamp (rt.sizeDelta, targetSize, ref refVelocity, 0.05f);
    }

    public void OnPointerEnter (PointerEventData eventData) {
        targetSize = size2;
    }

    public void OnPointerExit (PointerEventData eventData) {
        targetSize = size1;
    }
}
