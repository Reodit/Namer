using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuObject : MonoBehaviour
{
    [SerializeField] GameObject popUpName;

    //카드를 선택한 상태에서 오브젝트를 호버링하면 카드의 타겟으로 설정
    //오브젝트의 이름을 화면에 띄움
    bool isHoverling = false;

    private void Update()
    {
        AllPopUpNameCtr();
    }

    bool isTouched = false;
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
            isTouched = true;
            if (this.gameObject.CompareTag("InteractObj") && CardManager.GetInstance.isPickCard)
            {
                CardManager.GetInstance.target = this.gameObject;
            }
            if (this.gameObject.CompareTag("InteractObj"))
            {
                PopUpNameOn();
            }

            if (CardManager.GetInstance.isPickCard)
            {
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
        if (!UIManager.GetInstance.isShowNameKeyPressed && popUpName.activeSelf && !isHoverling)
        {
            PopUpNameOff();
        }
    }
    
}
