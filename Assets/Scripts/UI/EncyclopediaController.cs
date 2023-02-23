using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EncyclopediaController : MonoBehaviour
{
    [SerializeField] GameObject layoutGroup;
    [SerializeField] float wheelSpeed = 0.1f;
    [SerializeField] float maxHeight = 1f;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] UnityEngine.UI.Button returnBtn;
    GameObject[] pediaCards;
    IngameCanvasController canvasController;
    GameDataManager gameDataManager;

    Touch touch;
    float wheelInput;
    Vector2 startPos;
    float scrollSpeed = 0.0001f;

    private void Start()
    {
       Init();
    }

    void Update()
    {
        ScrollWheel();
    }

    private void Init()
    {
        gameDataManager = GameDataManager.GetInstance;
        EncyclopediaInit();

        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            canvasController = GameObject.Find("IngameCanvas").gameObject.GetComponent<IngameCanvasController>();
            returnBtn.onClick.AddListener(canvasController.EncyclopediaClose);
        }
    }

    private void EncyclopediaInit()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            pediaCards = GameDataManager.GetInstance.GetMainCardEncyclopedia();
        }
        else
        {
            pediaCards = GameDataManager.GetInstance.GetIngameCardEncyclopedia();
        }

        if (pediaCards == null) return;
        maxHeight = 0.5f + (float) 0.5 * (pediaCards.Length / 4);

        for (int i = 0; i < pediaCards.Length; i++)
        {
            var cardObject = (GameObject)Instantiate(pediaCards[i], new Vector3(0, 0, 0), Quaternion.identity);
            cardObject.transform.GetChild(0).gameObject.SetActive(true);
            cardObject.transform.parent = GameObject.Find("LayoutCards").transform;
            cardObject.transform.localPosition =
                new Vector3(-0.9f + (0.6f * (i % 4)),
                -0.7f * (int) (i / 4), 0);
            cardObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

        }
    }

    public void SyncScrollBar()
    {
        layoutGroup.transform.localPosition =
            new Vector3(0f, scrollbar.value * maxHeight, 0f);
    }

   
    private void ScrollWheel()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            wheelInput = -1 * touch.deltaPosition.y;
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaY = touch.position.y - startPos.y;
                wheelSpeed =  deltaY * scrollSpeed;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                wheelSpeed = 0f;
            }


        }
        if (wheelInput > 0)
        {
            layoutGroup.transform.localPosition -= new Vector3(0, -1 * wheelSpeed, 0);
            scrollbar.value = layoutGroup.transform.localPosition.y / maxHeight;
            if (layoutGroup.transform.localPosition.y <= 0f)
            {
                layoutGroup.transform.localPosition = new Vector3(0, 0, 0);
            }
            if (layoutGroup.transform.localPosition.y >= maxHeight)
            {
                layoutGroup.transform.localPosition = new Vector3(0, maxHeight, 0);
            }
        }
        else if (wheelInput < 0)
        {
            layoutGroup.transform.localPosition += new Vector3(0, wheelSpeed, 0);
            scrollbar.value = layoutGroup.transform.localPosition.y / maxHeight;
            if (layoutGroup.transform.localPosition.y >= maxHeight)
            {
                layoutGroup.transform.localPosition = new Vector3(0, maxHeight, 0);
            }
            if (layoutGroup.transform.localPosition.y <= 0f)
            {
                layoutGroup.transform.localPosition = new Vector3(0, 0, 0);
            }

        }
    }
}
