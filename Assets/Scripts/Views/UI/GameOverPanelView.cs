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

    private bool _buttonListenersBound;

    public void Init(){
        EventSystem.Instance.OnGameLose-=Show;
        EventSystem.Instance.OnGameLose+=Show;

        gameObject.SetActive(false);

        if(_buttonListenersBound){
            return;
        }

        hideButton.onClick.AddListener(SwitchVisibility);

        returnToMainMenuButton.onClick.AddListener(()=>{
            UISystem.Instance.setupPanelView.Show();
        });

        _buttonListenersBound = true;
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void SwitchVisibility(){
        if(body.activeSelf){
            body.SetActive(false);
        }else{
            body.SetActive(true);
        }
    }
}