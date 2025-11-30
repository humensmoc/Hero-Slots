using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageRankPanelView : MonoBehaviour
{
    public List<DamageRankItemData> damageRankItemDatas=new List<DamageRankItemData>();
    public List<DamageRankItemView> damageRankItemViews=new List<DamageRankItemView>();
    public Transform damageRankItemParent;
    public GameObject damageRankItemPrefab;
    public Button hideButton;
    public GameObject panel;
    public GameObject body;
    bool isShow =true;

    public void Init(){
        hideButton.onClick.AddListener(SwitchVisibility);
        // body.SetActive(false);
        HidePanel();
    }

    public void ShowPanel(){
        panel.SetActive(true);
        body.SetActive(true);
        CardSelectSystem.Instance.isShow = true;
    }

    public void HidePanel(){
        panel.SetActive(false);
        CardSelectSystem.Instance.isShow = false;
    }

    public void SwitchVisibility(){
        if(isShow){
            body.SetActive(false);
            CardSelectSystem.Instance.isShow = false;
            isShow = false;
        }else{
            body.SetActive(true);
            CardSelectSystem.Instance.isShow = true;
            isShow = true;
        }
    }

    public void ClearDamageRankData(){
        damageRankItemDatas.Clear();
        RefreshUI();
    }

    public void AddDamageRankData(CardView cardView,int damage){
        if(damageRankItemDatas.Count <=0){
            DamageRankItemData damageRankItemData = new DamageRankItemData(){
                cardData = cardView.card.CardData,
                damage = damage
            };
            damageRankItemDatas.Add(damageRankItemData);
            RefreshUI();
            return;
        }

        bool isExist = false;
        foreach(DamageRankItemData data in damageRankItemDatas){
            if(data.cardData == cardView.card.CardData){
                data.damage += damage;
                isExist = true;
            }
        }

        if(!isExist){
            DamageRankItemData damageRankItemData = new DamageRankItemData(){
                cardData = cardView.card.CardData,
                damage = damage
            };
            damageRankItemDatas.Add(damageRankItemData);
        }

        RefreshUI();
    }

    public void RefreshUI(){
        foreach(DamageRankItemView itemView in damageRankItemViews){
            Destroy(itemView.gameObject);
        }
        damageRankItemViews.Clear();
        
        damageRankItemDatas.Sort((a,b)=>b.damage.CompareTo(a.damage));

        int maxItemCount = 10;
        int currentItemCount = 0;

        foreach(DamageRankItemData data in damageRankItemDatas){
            if(currentItemCount < maxItemCount){
                currentItemCount++; 
            }else{
                break;
            }
            
            DamageRankItemView damageRankItemView = Instantiate(damageRankItemPrefab,damageRankItemParent).GetComponent<DamageRankItemView>();
            damageRankItemView.Init(data);
            damageRankItemViews.Add(damageRankItemView);
            
        }
    }
}