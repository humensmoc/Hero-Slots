using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattlefieldView : MonoBehaviour
{
    public CardView[,] cardViews;
    public List<HeroSlotView> heroSlotViews=new();
    public HeroSlotView currentHeroSlotView;
    private void Awake()
    {
        cardViews = new CardView[5,5];
    }
    
    public IEnumerator RemoveCard(int x,int y){
        if(cardViews[x,y]==null)
            yield break;
        cardViews[x,y].transform.DOScale(Vector3.zero, 0.15f);
        yield return new WaitForSeconds(0.15f);
        Destroy(cardViews[x,y].gameObject);
        cardViews[x,y] = null;
    }
}
