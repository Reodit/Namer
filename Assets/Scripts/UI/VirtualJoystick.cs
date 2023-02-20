using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform lever;
    private RectTransform rectTransform;
    [SerializeField, Range(10f, 150f)] private float leverRange;
    
    private Vector3 inputVector;

    private PlayerMovement _player; // Move 함수 호출시 사용

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _player = GameManager.GetInstance.localPlayerMovement;
    }

    public void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }


    public void InputControlVector()
    {
        _player.PlayerMove(inputVector);
    }
    
    public void ControlJoystickLever(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir 
            : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        inputVector = new Vector3 (clampedDir.x / leverRange, 0f, clampedDir.y / leverRange);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {        
        inputVector = Vector3.zero;
        lever.anchoredPosition = Vector2.zero;
    }
}