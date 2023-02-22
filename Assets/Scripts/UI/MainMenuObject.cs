using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuObject : MonoBehaviour
{
    [SerializeField] GameObject popUpName;
    public bool isTouched = false;

    //카드를 선택한 상태에서 오브젝트를 호버링하면 카드의 타겟으로 설정
    //오브젝트의 이름을 화면에 띄움

    private void Update()
    {
        AllPopUpNameCtr();
    }

    private void OnMouseDown()
    {
        if (UIManager.GetInstance.isShowNameKeyPressed && CardManager.GetInstance.pickCard != null)
        {
            CardManager.GetInstance.target = gameObject;
            CardManager.GetInstance.pickCard.GetComponent<MainMenuCardController>().TouchInteractObj();
        }
        else if (!isTouched)
        {
            if (GameManager.GetInstance.CurrentState == GameStates.Pause) return;
            if (GameManager.GetInstance.CurrentState == GameStates.Victory &&
                this.name != "PlanetObj") return;

            if (CardManager.GetInstance.target != null && !CardManager.GetInstance.isPickCard)
            {
                CardManager.GetInstance.target.GetComponent<MainMenuObject>().isTouched = false;
                CardManager.GetInstance.target = this.gameObject;
            }
            isTouched = true;
            if (this.gameObject.CompareTag("InteractObj"))
            {
                CardManager.GetInstance.target = this.gameObject;
                PopUpNameOn();
            }
            if (CardManager.GetInstance.isPickCard)
            {
                CardManager.GetInstance.target = this.gameObject;
                CardManager.GetInstance.pickCard.GetComponent<MainMenuCardController>().TouchInteractObj();
            }
        }
        else
        {
            if (GameManager.GetInstance.CurrentState == GameStates.Pause) return;
            isTouched = false;
            CardManager.GetInstance.target = null;
            popUpName.SetActive(false);
            if (this.gameObject.CompareTag("InteractObj"))
            {
                PopUpNameOff();
                CardManager.GetInstance.ableAddCard = true;
            }
        }
    }


    //오브젝트 현재 이름 팝업을 띄움 
    void PopUpNameOn()
    {
        popUpName.SetActive(true);
    }

    //오브젝트 현재 이름 팝업을 지움 
    void PopUpNameOff()
    {
        popUpName.SetActive(false);
    }

    //탭키에 따라 모든 네임 팝업을 띄움
    private void AllPopUpNameCtr()
    {
        if (UIManager.GetInstance.isShowNameKeyPressed && !popUpName.activeSelf)
        {
            PopUpNameOn();
        }
        if (!UIManager.GetInstance.isShowNameKeyPressed && popUpName.activeSelf && !isTouched)
        {
            PopUpNameOff();
        }
    }
    
}
