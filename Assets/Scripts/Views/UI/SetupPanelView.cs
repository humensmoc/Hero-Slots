using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupPanelView : MonoBehaviour
{
    public GameObject body;
    public Button startButton;
    public void Init(){
        startButton.onClick.AddListener(()=>{
            EventSystem.Instance.OnGameStart?.Invoke();

            Hide();

            GameInitializer.Instance.ResetGame();
        });
    }

    public void Show(){
        body.SetActive(true);
    }

    public void Hide(){
        body.SetActive(false);
    }
}