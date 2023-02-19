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

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        InputControlVector(); // 변경점
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
        inputVector = clampedDir / leverRange;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {        
        inputVector = Vector2.zero;
        lever.anchoredPosition = Vector2.zero;
    }
}