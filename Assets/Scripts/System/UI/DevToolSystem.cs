using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevToolSystem : Singleton<DevToolSystem>
{
    public TMP_InputField cardInputField;
    public Button addCardButton;
    public TMP_InputField heroInputField;
    public Button addHeroButton;

    public void Init(){
        addCardButton.onClick.AddListener(GenerateTestCard);
        addHeroButton.onClick.AddListener(GenerateTestHero);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            GenerateTestCard();
            GenerateTestHero();
        }
    }

    public void GenerateTestCard(){
        string cardName = cardInputField.text;
        if(cardName == "") return;
        CardData cardData = CardLibrary.cardDatas.Find(cardData => cardData.CardNameEnum.ToString() == cardName);
        if(cardData == null){
            Debug.LogError("Card not found: " + cardName);
            return;
        }
        CardSystem.Instance.cardsInDeck.Add(new Card(cardData));
        Debug.Log("Added card: " + cardName);
    }

    public void GenerateTestHero(){
        string heroName = heroInputField.text;
        if(heroName == "") return;
        HeroData heroData = HeroLibrary.heroDatas.Find(heroData => heroData.HeroType.ToString() == heroName);
        if(heroData == null){
            Debug.LogError("Hero not found: " + heroName);
            return;
        }

        HeroSystem.Instance.heroesInDeck.Add(new Hero(heroData));
        Debug.Log("Added hero: " + heroName);
    }
}
