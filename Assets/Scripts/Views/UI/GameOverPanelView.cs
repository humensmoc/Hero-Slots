using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverPanelView : MonoBehaviour
{
    public GameObject body;
    public Button hideButton;
    public Button returnToMainMenuButton;
    public void Init(){

        gameObject.SetActive(false);

        hideButton.onClick.AddListener(SwitchVisibility);

        returnToMainMenuButton.onClick.AddListener(()=>{
            UISystem.Instance.setupPanelView.Show();
        });
    }

    public void SwitchVisibility(){
        if(body.activeSelf){
            body.SetActive(false);
        }else{
            body.SetActive(true);
        }
    }
}