using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameShopPanelView : MonoBehaviour
{
    public GameObject panel;
    public GameObject body;
    public Button closeButton;
    public Button hideButton;
    bool isShow =false;
    public void Init(){
        closeButton.onClick.RemoveAllListeners();
        hideButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(ClosePanel);
        hideButton.onClick.AddListener(SwitchVisibility);
    }

    public void OpenPanel(){
        panel.SetActive(true);
        body.SetActive(true);
        isShow = true;

        HeroSelectSystem.Instance.ShowHeroSelectView();
        HeroSelectSystem.Instance.Refresh();

        RelicSelectSystem.Instance.Refresh();
    }

    public void ClosePanel(){
        panel.SetActive(false);
        isShow = false;
    }

    public void SwitchVisibility(){
        if(body.activeSelf){
            Debug.Log("switch visibility");
            body.SetActive(false);
        }else{
            Debug.Log("switch visibility");
            body.SetActive(true);
        }
    }
}