using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DamageRankItemData
{
    public CardData cardData;
    public int damage;
}
public class DamageRankItemView : MonoBehaviour
{
    public DamageRankItemData damageRankItemData;
    public Image image;
    public TMP_Text damageText;

    public void Init(DamageRankItemData damageRankItemData){
        this.damageRankItemData = damageRankItemData;
        image.sprite = ResourcesLoader.LoadCardSprite(damageRankItemData.cardData.CardNameEnum.ToString());
        damageText.text = damageRankItemData.damage.ToString();
    }
}