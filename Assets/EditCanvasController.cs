using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditCanvasController : MonoBehaviour
{
    [SerializeField] GameObject encyclopedia;
    [SerializeField] GameObject[] pediaCloses;

    public void OpenEncyclopedia()
    {
        encyclopedia.SetActive(true);
        CardManager.GetInstance.isEncyclopedia = true;
        for (int i = 0; i < pediaCloses.Length; i++)
        {
            pediaCloses[i].SetActive(false);
        }
    }

    public void CloseEncyclopedia()
    {
        CardManager.GetInstance.isEncyclopedia = false;
        encyclopedia.SetActive(false);
        for (int i = 0; i < pediaCloses.Length; i++)
        {
            pediaCloses[i].SetActive(true);
        }
    }
}