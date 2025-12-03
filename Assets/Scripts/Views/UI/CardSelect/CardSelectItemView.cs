using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class CardRarityColorPair
{
    public CardRarity rarity;
    public Color color;
}

public class CardSelectItemView : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public CardData cardData;
    public Image BackgroundImage;
    public Image cardImage;
    public TMP_Text cardName;
    public TMP_Text cardDescription;
    public TMP_Text cardAttack;
    public Image elementImage;

    public Image veilImage;
    
    [SerializeField]
    private List<CardRarityColorPair> cardRarityColorList = new List<CardRarityColorPair>();
    public void Init(CardData cardData){
        this.cardData = cardData;
        BackgroundImage.color = cardRarityColorList.Find(pair => pair.rarity == cardData.CardRarity).color;
        cardImage.sprite = ResourcesLoader.LoadCardSprite(cardData.CardNameEnum.ToString());
        cardName.text = cardData.CardNameEnum.ToString();
        cardDescription.text = cardData.Description;
        cardAttack.text = cardData.Attack.ToString();
        elementImage.color = cardData.ElementType switch{
            ElementType.Element_Fire => Color.red,
            ElementType.Element_Water => Color.blue,
            ElementType.Element_Earth => Color.green,   
            ElementType.Element_Air => Color.white,
            ElementType.Element_Electricity => Color.yellow,
            ElementType.Element_Dark => Color.black,
            _ => Color.white,
        };

        StartCoroutine(ShowVeil(cardData.CardRarity));
    }

    public void OnPointerClick(PointerEventData eventData){
        CardSelectSystem.Instance.SelectCard(cardData);
    }

    public void OnPointerEnter(PointerEventData eventData){
        transform.DOScale(1.1f, 0.15f);
    }

    public void OnPointerExit(PointerEventData eventData){
        transform.DOScale(1f, 0.15f);
    }

    public IEnumerator ShowVeil(CardRarity cardRarity){
        int upgradeTime=0;
        switch(cardRarity){
            case CardRarity.Common:
                upgradeTime = 0;
                break;
            case CardRarity.Rare:
                upgradeTime = 1;
                break;
            case CardRarity.Epic:
                upgradeTime = 2;
                break;
            case CardRarity.Legendary:
                upgradeTime = 3;
                break;
            default:
                upgradeTime = 0;
                break;
        }

        veilImage.color = cardRarityColorList[0].color;

        // 按顺序播放多次动画
        for(int i=0;i<upgradeTime+1;i++){
            veilImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.15f+i*0.3f);

            Color fromColor = veilImage.color;
            Color toColor = cardRarityColorList[i].color;
            Tween colorTween = veilImage.DOColor(toColor, 0.15f);

            Tween scaleTween = veilImage.transform.DOScale(1.1f, 0.15f).OnComplete(()=>{
                veilImage.transform.DOScale(1f, 0.05f);
                
            });
            // 等待动画完成
            yield return scaleTween.WaitForCompletion();
        }

        yield return new WaitForSeconds(0.15f);

        Tween fadeTween = veilImage.DOFade(0f, 0.15f).OnComplete(()=>{
            veilImage.gameObject.SetActive(false);
        });
        yield return fadeTween.WaitForCompletion();
    }
}