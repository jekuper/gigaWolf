using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class textGlowEffector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
    public TextMeshProUGUI text;

    [SerializeField] private Color glowColor = Color.white;
    [SerializeField] private float glowOffset = 0.16f;
    [SerializeField] private float glowInner = 0;
    [SerializeField] private float glowOuter = 0.16f;
    [SerializeField] private float glowPower = 1f;

    private Material m_TextBaseMaterial;
    private Material m_TextHighlightMaterial;

    //creating glow material according to settings
    private void Awake () {
        m_TextBaseMaterial = text.fontSharedMaterial;

        m_TextHighlightMaterial = new Material (m_TextBaseMaterial);

        m_TextHighlightMaterial.SetColor (ShaderUtilities.ID_GlowColor, glowColor);
        m_TextHighlightMaterial.SetFloat (ShaderUtilities.ID_GlowPower, glowPower);
        m_TextHighlightMaterial.SetFloat (ShaderUtilities.ID_GlowOffset, glowOffset);
        m_TextHighlightMaterial.SetFloat (ShaderUtilities.ID_GlowOuter, glowOuter);
        m_TextHighlightMaterial.SetFloat (ShaderUtilities.ID_GlowInner, glowInner);
    }

    public void OnPointerDown (PointerEventData eventData) {
        text.fontSharedMaterial = m_TextHighlightMaterial;
        text.UpdateMeshPadding ();
    }

    public void OnPointerUp (PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject == gameObject) {
            text.fontSharedMaterial = m_TextBaseMaterial;
            text.UpdateMeshPadding ();
        }
    }

    public void OnPointerEnter (PointerEventData eventData) {
        text.fontSharedMaterial = m_TextHighlightMaterial;
        text.UpdateMeshPadding ();
    }

    public void OnPointerExit (PointerEventData eventData) {
        text.fontSharedMaterial = m_TextBaseMaterial;
        text.UpdateMeshPadding ();
    }
}