using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolItem : MonoBehaviour
{
    public ObjectPoolType objectPoolType;

    public void ReturnToPool(){
        ObjectPool.Instance.ReturnObject(this);   
    }
}
