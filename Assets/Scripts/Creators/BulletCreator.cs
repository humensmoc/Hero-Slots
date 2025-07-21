using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletCreator : Singleton<BulletCreator>
{
    [SerializeField] GameObject bulletViewPrefab;
    public BulletView CreateBulletView(Bullet bullet,Vector3 position,Quaternion rotation)
    {
        GameObject bulletInstance = Instantiate(bulletViewPrefab,position,rotation);
        bulletInstance.transform.localScale = Vector3.zero;
        bulletInstance.transform.DOScale(Vector3.one, 0.15f);
        BulletView bulletView = bulletInstance.GetComponent<BulletView>();
        bulletView.Init(bullet);
        return bulletView;
    }
}
