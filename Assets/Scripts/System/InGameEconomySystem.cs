using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEconomySystem : Singleton<InGameEconomySystem>
{
    
    public void AddCoin(Vector3 startPosition,int coin)
    {
        StartCoroutine(AddCoinCoroutine(startPosition,coin));
    }

    public void SpendCoin(Vector3 startPosition,int coin)
    {
        StartCoroutine(SpendCoinCoroutine(startPosition,coin));
    }

    public IEnumerator AddCoinCoroutine(Vector3 startPosition,int coin)
    {
        for(int i=0;i<coin;i++){
            ObjectPool.Instance.CreateFlyingTextToTarget(
                coin.ToString(),
                FlyingTextType.AddCoin,
                startPosition, CoordinateConverter.UIToWorld(UISystem.Instance.runtimeEffectDataView.coinText.transform.position), 
                ()=>{Model.Coin+=1;}, true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator SpendCoinCoroutine(Vector3 startPosition,int coin)
    {
        for(int i=0;i<coin;i++){
            Model.Coin-=1;
            
            ObjectPool.Instance.CreateFlyingTextToTarget(coin.ToString(), FlyingTextType.AddCoin, 
            CoordinateConverter.UIToWorld(UISystem.Instance.runtimeEffectDataView.coinText.transform.position),
            startPosition, 
             ()=>{}, true);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
