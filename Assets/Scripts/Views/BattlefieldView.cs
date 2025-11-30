using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattlefieldView : MonoBehaviour
{
    public CardView[,] cardViewsInBattlefield;
    public List<HeroView> heroViewsInBattlefield=new(5);
    public List<HeroSlotView> heroSlotViews=new();
    public HeroSlotView currentHeroSlotView;
    private void Awake()
    {
        cardViewsInBattlefield = new CardView[5,5];
    }
    
    public IEnumerator RemoveCard(int x,int y){
        if(cardViewsInBattlefield[x,y]==null)
            yield break;
        cardViewsInBattlefield[x,y].transform.DOScale(Vector3.zero, 0.05f);
        yield return new WaitForSeconds(0.05f);
        Destroy(cardViewsInBattlefield[x,y].gameObject);
        cardViewsInBattlefield[x,y] = null;
    }
}
