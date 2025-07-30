using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeroCreator : Singleton<HeroCreator>
{
    [SerializeField] GameObject heroViewPrefab;
    public HeroView CreateHeroView(Hero hero,Vector3 position,Quaternion rotation,int y)
    {
        GameObject heroInstance = Instantiate(heroViewPrefab,position,rotation);
        heroInstance.transform.localScale = Vector3.zero;
        heroInstance.transform.DOScale(Vector3.one, 0.15f);
        HeroView heroView = heroInstance.GetComponent<HeroView>();
        heroView.Init(hero,y);

        foreach(HeroEffect heroEffect in HeroLibrary.heroEffects){
            if(heroEffect.heroType==heroView.hero.heroData.HeroType){
                heroView.hero.heroData.HeroEffect=heroEffect.Clone();
            }
        }

        heroView.hero.heroData.HeroEffect.OnInit?.Invoke(heroView);

        return heroView;
    }
}
