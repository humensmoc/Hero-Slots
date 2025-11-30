using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public enum FlyingTextType{
    AddCoin,
    AddElectricity,
    AddBloodGem,
    PowerUpBloodGem,
    Dart,

    ChargeRed,ChargeBlue,ChargeYellow,
    Shine,
}

public class FlyingText : ObjectPoolItem
{
    public TMP_Text text;
    public SpriteRenderer spriteRenderer;

    private float currentHorizontalSpeed;
    private float fontSize;
    private bool hasStartedFlying = false; // 标记是否已经开始飞行
    private bool isCurve = false;
    
    // 飞行到目标的参数
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float flyDuration;

    public Action onComplete;

    public void Update(){
        // 只在还没开始飞行时才调用DoEffect
        if(!hasStartedFlying){
            DoEffect();
        }
    }

    public void Init(string text, FlyingTextType type, Vector3 startPos, Vector3 targetPos,bool isCurve=false, Action onComplete=null){
        fontSize=this.text.fontSize;
        this.text.text = text;
        startPosition=startPos;
        targetPosition=targetPos;
        flyDuration=Vector3.Distance(startPosition,targetPosition)/VFXConfig.Instance.flySpeed;
        hasStartedFlying = false; // 重置飞行标记
        this.isCurve = isCurve;
        // 重置文本透明度
        Color color = this.text.color;
        color.a = 1f;
        this.text.color = color;

        this.text.enabled=false;
        this.spriteRenderer.enabled=true;

        this.spriteRenderer.sprite=ResourcesLoader.LoadEffectSprite(type.ToString());

        currentHorizontalSpeed = UnityEngine.Random.Range(VFXConfig.Instance.horizontalSpeedMin, VFXConfig.Instance.horizontalSpeedMax);

        this.onComplete=onComplete;
    }

    public void DoEffect(){
        // 检查是否已经在飞行中
        if(!DOTween.IsTweening(transform) && !hasStartedFlying){
            hasStartedFlying = true; // 标记为已开始飞行
            
            // 设置起始位置
            transform.position = startPosition;

            if(isCurve){
                flyDuration=1f;
                transform.DOPath(new Vector3[]{startPosition,(startPosition+targetPosition)/2+new Vector3(0,1,0), targetPosition},
                    flyDuration, PathType.CatmullRom)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => {
                        // 飞行完成后淡出
                        ObjectPool.Instance.ReturnObject(this,(()=>{
                            this.text.fontSize = fontSize;
                            onComplete?.Invoke();   
                        }));
                    });

            }else{
                // 开始飞行动画
                transform.DOMove(targetPosition, flyDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => {
                        // 飞行完成后淡出
                        
                        ObjectPool.Instance.ReturnObject(this,(()=>{
                            this.text.fontSize = fontSize;
                            onComplete?.Invoke();   
                        }));
                    });
            }
        }
    }
    
    public void ResetFontSize(){
        // 重置字体大小到原始值
        this.text.fontSize = fontSize;
    }
}
