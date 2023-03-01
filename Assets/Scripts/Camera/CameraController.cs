using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    enum PriorityOrder
    {
        BehindAtAll = 8,
        BehingByNormal,
        normal,
        FrontByNormal,
        FrontAtAll
    }

    // values
    bool isTopView = false;
    public bool isFocused = false;
    int normalCamPirority;
    Transform player;
    CinemachineComponentBase normalcamOption;
    CinemachineComponentBase topcamOption;

    // android Values
    float m_touchDis = 0;
    float m_touchOldDis = 0;
    float fDis = 0;
    float zoomDis = 0.5f;
    [SerializeField] Button cameraBtn;
    //[SerializeField] Text testText;

    // 카메라들 프리팹에서 넣어놓기
    [Header("Cams")]
    CinemachineVirtualCamera[] playerTopCams;
    CinemachineVirtualCamera[] playerNormalCams;
    [SerializeField] CinemachineVirtualCamera playerTopViewCam;
    [SerializeField] CinemachineVirtualCamera playerTopViewCamZoomIn;
    [SerializeField] CinemachineVirtualCamera playerTopViewCamZoomOut;
    [SerializeField] CinemachineVirtualCamera playerNormalViewCam;
    [SerializeField] CinemachineVirtualCamera playerNormalViewCamZoomIn;
    [SerializeField] CinemachineVirtualCamera playerNormalViewCamZoomOut;
    [SerializeField] CinemachineVirtualCamera targetCam;
    [SerializeField] CinemachineVirtualCamera playerFocusCam;

    CinemachineVirtualCamera curCam;

    [Header("Zoom settings")]
    [SerializeField][Range(0, 2)] int zoomValue = 1;
    int preZoomValue = 1;
    bool canZoom = true;

    void Awake()
    {
        GameManager.GetInstance.KeyAction += CheckCameraSwitch;
        if (GameManager.GetInstance.cameraController == null)
            GameManager.GetInstance.cameraController = GameObject.Find("Cameras").GetComponent<CameraController>();
#if UNITY_EDITOR
        if (cameraBtn != null)
            cameraBtn.onClick.AddListener(M_SwitchCam);
#elif UNITY_ANDROID
        cameraBtn.onClick.AddListener(M_SwitchCam);
#else
        cameraBtn.gameObject.SetActive(false);
#endif
        Init();
    }

    public void Init()
    {
        // 각 캠의 우선순위 설정 
        playerNormalViewCam.Priority = (int)PriorityOrder.BehingByNormal;
        playerNormalViewCamZoomIn.Priority = (int)PriorityOrder.BehingByNormal;
        playerNormalViewCamZoomOut.Priority = (int)PriorityOrder.BehingByNormal;
        playerTopViewCam.Priority = (int)PriorityOrder.BehingByNormal;
        playerTopViewCamZoomIn.Priority = (int)PriorityOrder.BehingByNormal;
        playerTopViewCamZoomOut.Priority = (int)PriorityOrder.BehingByNormal;
        targetCam.Priority = (int)PriorityOrder.BehindAtAll;
        normalCamPirority = playerNormalViewCam.Priority;

        playerTopCams = new CinemachineVirtualCamera[] { playerTopViewCamZoomOut, playerTopViewCam, playerTopViewCamZoomIn };
        playerNormalCams = new CinemachineVirtualCamera[] { playerNormalViewCamZoomOut, playerNormalViewCam, playerNormalViewCamZoomIn };

        // 모든 팔로우 캠이 플레이어를 따라다니도록 설정
        //player = GameObject.Find("Player").transform;
        //playerNormalViewCam.Follow = player;
        //playerTopViewCam.Follow = player;

        isTopView = false;
        canZoom = true;

        SetPriorityBySize();

        FocusOff();
    }

    public void SetPriorityBySize()
    {
        int mapSize = DetectManager.GetInstance.GetMaxX;
        if (mapSize > 12)
        {
            zoomValue = 2;
            SetPriority();
        }
        else if (mapSize > 8)
        {
            zoomValue = 1;
            SetPriority();
        }
        else
        {
            zoomValue = 0;
            SetPriority();
        }
    }

    public void FocusOn(bool canMove = true)
    {
        playerFocusCam.Priority = (int)PriorityOrder.FrontAtAll;
        targetCam.Priority = (int)PriorityOrder.BehindAtAll;

        // zoom in 상태에서는 카드가 안 보이도록 함 
        CardManager.GetInstance.CardsDown();

        if (!canMove)
        {
            GameManager.GetInstance.isPlayerCanInput = false;
            GameManager.GetInstance.localPlayerEntity.pAnimator.SetFloat("scalar", 0f);
        }

        isFocused = true;
    }

    public void FocusOn(Transform target, bool canMove = true)
    {
        targetCam.LookAt = target;
        targetCam.Follow = target;
        targetCam.Priority = (int)PriorityOrder.FrontAtAll;
        playerFocusCam.Priority = (int)PriorityOrder.BehindAtAll;

        // zoom in 상태에서는 카드가 안 보이도록 함 
        CardManager.GetInstance.CardsDown();

        if (!canMove)
        {
            GameManager.GetInstance.isPlayerCanInput = false;
            GameManager.GetInstance.localPlayerEntity.pAnimator.SetFloat("scalar", 0);
        }

        isFocused = true;
    }

    public void FocusOff()
    {
        playerFocusCam.Priority = (int)PriorityOrder.BehindAtAll;
        targetCam.Priority = (int)PriorityOrder.BehindAtAll;
        targetCam.LookAt = null;
        targetCam.Follow = null;

        // zoom in 상태에서는 카드가 안 보이도록 함
        if (GameManager.GetInstance.CurrentState != GameStates.Encyclopedia)
            CardManager.GetInstance.CardsUp();

        GameManager.GetInstance.isPlayerCanInput = true;

        isFocused = false;
    }

    private void SetPriority()
    {
        curCam = isTopView ? playerTopCams[zoomValue] : playerNormalCams[zoomValue];
        curCam.Priority = (int)PriorityOrder.normal;
    }

    public void SetZoomInStart()
    {
        int mapSize = DetectManager.GetInstance.GetMaxX;
        if (mapSize > 15)
        {
            zoomValue = 0;
        }
        else if (mapSize > 10)
        {
            zoomValue = 1;
        }
        else
        {
            zoomValue = 2;
        }
        SetPriority();
    }

    private void CheckCameraSwitch()
    {
        if (Input.GetKeyDown(GameManager.GetInstance.cameraKey) && GameManager.GetInstance.CurrentState != GameStates.Encyclopedia)
        //if (Input.GetKeyDown((KeyCode.Q)))
        {
            curCam.Priority = (int)PriorityOrder.BehingByNormal;
            isTopView = !isTopView;
            SetPriority();
        }
    }

    private void M_SwitchCam()
    {
        if (GameManager.GetInstance.CurrentState == GameStates.Encyclopedia ||
            CardManager.GetInstance.isAligning) return;
        curCam.Priority = (int)PriorityOrder.BehingByNormal;
        isTopView = !isTopView;
        SetPriority();
    }

    private IEnumerator ZoomInOut()
    {
        canZoom = false;
        curCam.Priority = (int)PriorityOrder.BehingByNormal;
        SetPriority();
        yield return new WaitForSecondsRealtime(0.3f);
        canZoom = true;
    }

    bool CheckCanZoom()
    {
        if (Input.touchCount > 0) // 터치가 1개 이상
        {
            // UI요소를 터치하고 있는가?
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Debug.Log("Touch ui");
                return false;
            }
            if (Input.touchCount >= 2)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
                {
                    Debug.Log("Touch ui");
                    return false;
                }
                else
                {
                    Debug.Log("not Touch ui");
                    return true;
                }
            }

            Debug.Log("Checking Touch");
        }

        return false;
    }

    public void ZoomOut()
    {
        zoomValue = zoomValue <= 0 ? 0 : zoomValue - 1;
        StartCoroutine(ZoomInOut());
    }

    public void ZoomIn()
    {
        zoomValue = zoomValue >= 2 ? 2 : zoomValue + 1;
        StartCoroutine(ZoomInOut());
    }

    void Update()
    {
        if (GameManager.GetInstance.CurrentState != GameStates.InGame) return;

#if UNITY_EDITOR
        if (preZoomValue != zoomValue)
        {
            preZoomValue = zoomValue;
            SetPriority();
        }
#endif
#if UNITY_ANDROID
        if (!CheckCanZoom()) return;
        int touchCount = Input.touchCount;
        if (touchCount == 2 && (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began))
        {
            m_touchDis = (Input.touches[0].position - Input.touches[1].position).magnitude;
            m_touchOldDis = m_touchDis;
        }
        else if (touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            m_touchDis = (Input.touches[0].position - Input.touches[1].position).magnitude;
            fDis = (m_touchDis - m_touchOldDis) * 0.01f;
            if (canZoom && fDis < -zoomDis)
            {
                ZoomIn();
            }
            else if (canZoom && fDis > zoomDis)
            {
                ZoomOut();
            }

            m_touchOldDis = m_touchDis;
        }
#else
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0 || !canZoom) return;
        if (scroll < 0) // zoom in
        {
            zoomValue = zoomValue >= 2 ? 2 : zoomValue + 1;
            StartCoroutine(ZoomInOut());
        }
        else            // zoom out
        {
            zoomValue = zoomValue <= 0 ? 0 : zoomValue - 1;
            StartCoroutine(ZoomInOut());
        }
#endif
    }
}