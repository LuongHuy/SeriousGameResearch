using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverMoveImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    private Vector3 originalPos;
    public float moveAmount = 10f; // khoảng cách di chuyển
    public float speed = 5f;       // tốc độ di chuyển

    private bool isHovering = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
    }

    void Update()
    {
        Vector3 targetPos = originalPos + (isHovering ? new Vector3(0, moveAmount, 0) : Vector3.zero);
        rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, targetPos, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
