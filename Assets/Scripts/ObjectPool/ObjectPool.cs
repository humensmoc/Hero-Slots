using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum ObjectPoolType{
    JumpingText,
    FlyingText,
    FlyingImage,
    Line
}

public class ObjectPool : Singleton<ObjectPool>
{

    public Dictionary<ObjectPoolType, List<GameObject>> activeObjects=new Dictionary<ObjectPoolType, List<GameObject>>();
    public Dictionary<ObjectPoolType, List<GameObject>> inactiveObjects=new Dictionary<ObjectPoolType, List<GameObject>>();

    protected override void Awake(){
        base.Awake();
        // 初始化所有对象池类型的字典
        foreach(ObjectPoolType type in System.Enum.GetValues(typeof(ObjectPoolType))){
            activeObjects[type] = new List<GameObject>();
            inactiveObjects[type] = new List<GameObject>();
        }
    }


    public void Update()
    {
        // if(Input.GetMouseButtonDown(0)){
        //     // 将屏幕坐标转换为世界坐标
        //     Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //     Debug.Log("worldPos: "+worldPos);
        //     CreateFlyingTextToTarget("100", FlyingTextType.Coin, new Vector3(0,0,0), worldPos);
        // }
    }

    public GameObject GetObject(ObjectPoolType objectPoolType){
        // 由于在Awake中已经初始化，这里不再需要检查和初始化字典
        if(inactiveObjects[objectPoolType].Count>0){
            GameObject numberObject=inactiveObjects[objectPoolType][0];
            inactiveObjects[objectPoolType].RemoveAt(0);
            activeObjects[objectPoolType].Add(numberObject);
            numberObject.SetActive(true);

            ObjectPoolItem objectPoolItem=numberObject.GetComponent<ObjectPoolItem>();
            objectPoolItem.objectPoolType=objectPoolType;

            return numberObject;
        }
        else{
            GameObject numberObject=Instantiate(Resources.Load<GameObject>("ObjectPoolItem/"+objectPoolType.ToString()));
            numberObject.transform.SetParent(transform);
            activeObjects[objectPoolType].Add(numberObject);
            
            // 确保新创建的对象有ObjectPoolItem组件并设置类型
            ObjectPoolItem objectPoolItem = numberObject.GetComponent<ObjectPoolItem>();
            if(objectPoolItem != null){
                objectPoolItem.objectPoolType = objectPoolType;
            }
            
            return numberObject;
        }
    }

    public void ReturnObject(ObjectPoolItem objectItem,Action onComplete=null){
        onComplete?.Invoke();
        
        // 清理所有DOTween动画
        objectItem.transform.DOKill();
        
        objectItem.gameObject.SetActive(false);
        activeObjects[objectItem.objectPoolType].Remove(objectItem.gameObject);
        inactiveObjects[objectItem.objectPoolType].Add(objectItem.gameObject);

        
    }

    public void CreateObjectAtPosition(ObjectPoolType objectPoolType, Vector3 position){
        GameObject objectItem=GetObject(objectPoolType);
        objectItem.transform.position=position;
    }

    public void CreateJumpingText(string text, Vector3 position, JumpingTextType type){
        GameObject objectItem=GetObject(ObjectPoolType.JumpingText);
        objectItem.transform.position=position;
        objectItem.GetComponent<JumpingText>().Init(text,type);
    }
    public void CreateFlyingTextToTarget(string text, FlyingTextType type, Vector3 startPos, Vector3 targetPos, Action onComplete=null,bool isCurve=false){
        GameObject objectItem=GetObject(ObjectPoolType.FlyingText);
        
        // 确保清理之前的DOTween动画
        objectItem.transform.DOKill();
        
        FlyingText flyingText = objectItem.GetComponent<FlyingText>();
        if(flyingText != null){
            flyingText.Init(text, type, startPos, targetPos, isCurve, onComplete);
        }else{
            Debug.LogError("FlyingText component not found on FlyingText prefab!");
        }
    }

    public void CreateFlyingImageToTarget(Vector3 startPos, Vector3 targetPos, Action onComplete=null){
        GameObject objectItem=GetObject(ObjectPoolType.FlyingImage);
        FlyingImage flyingImage = objectItem.GetComponent<FlyingImage>();
        if(flyingImage != null){
            flyingImage.Init(startPos, targetPos, onComplete);
        }else{
            Debug.LogError("FlyingImage component not found on FlyingImage prefab!");
        }
    }
    public void CreateLine(Vector3 startPos, Vector3 endPos){
        GameObject objectItem=GetObject(ObjectPoolType.Line);
        Line line = objectItem.GetComponent<Line>();
        if(line != null){
            line.Init(startPos, endPos);
        }else{
            Debug.LogError("Line component not found on Line prefab!");
        }
    }

    public void RemoveAllObject(){
        foreach(ObjectPoolType type in System.Enum.GetValues(typeof(ObjectPoolType))){
            for(int i=0;i<activeObjects[type].Count;i++){
                // 需要先重置字体大小再清理
                if(type == ObjectPoolType.FlyingText){
                    FlyingText flyingText = activeObjects[type][i].GetComponent<FlyingText>();
                    if(flyingText != null){
                        flyingText.ResetFontSize();
                    }
                }
                if(type == ObjectPoolType.JumpingText){
                    JumpingText jumpingText = activeObjects[type][i].GetComponent<JumpingText>();
                    if(jumpingText != null){
                        jumpingText.ResetFontSize();
                    }
                }
                ReturnObject(activeObjects[type][i].GetComponent<ObjectPoolItem>());
            }
        }
    }
}
