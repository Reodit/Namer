using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform lever;
    private RectTransform rectTransform;
    [SerializeField, Range(10f, 150f)] private float leverRange;
    [FormerlySerializedAs("inputVector")] public Vector3 vInputVector;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir 
            : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        vInputVector = new Vector3 (clampedDir.x / leverRange, 0f, clampedDir.y / leverRange);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {        
        vInputVector = Vector3.zero;
        lever.anchoredPosition = Vector2.zero;
    }
}