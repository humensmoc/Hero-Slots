using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] List<CardData> testCardDatas;
    [SerializeField] HeroData testHeroData;
    void Start()
    {
        CardSystem.Instance.Init(testCardDatas);  
        Hero hero = new Hero(testHeroData);
        HeroView heroView = HeroCreator.Instance.CreateHeroView(hero,Vector3.zero,Quaternion.identity);
        HeroSlotSystem.Instance.MoveToSlot(heroView,HeroSlotSystem.Instance.battlefieldView.heroSlotViews[0]);
    }

    
}
