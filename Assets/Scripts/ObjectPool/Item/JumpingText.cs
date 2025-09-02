using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum JumpingTextType{
    Normal,
    Critical,
    AutoHand,
    AOEAutoHand,
    GenerateDummy,
    SummonAutoHand,
    SummonAOEAutoHand
}

public class JumpingText : ObjectPoolItem
{
    public TMP_Text text;

    private float currentHorizontalSpeed;
    private Vector3 moveDirection;
    private float fontSize;

    public void Update(){
        DoEffect();
    }

    public void Init(string text, JumpingTextType type){
        fontSize=this.text.fontSize;
        this.text.text = text;
        // 初始化运动参数
        currentHorizontalSpeed = Random.Range(VFXConfig.Instance.horizontalSpeedMin, VFXConfig.Instance.horizontalSpeedMax);
        moveDirection = new Vector3(currentHorizontalSpeed, Random.Range(VFXConfig.Instance.verticalSpeedMin, VFXConfig.Instance.verticalSpeedMax), 0);
        // 重置文本透明度

        this.text.color = VFXConfig.Instance.jumpingTextColor;

        Color color = this.text.color;
        color.a = 1f;
        this.text.color = color;

        transform.DOScale(VFXConfig.Instance.jumpingTextScale,VFXConfig.Instance.jumpingTextScaleTime).OnComplete(()=>
        {
            transform.DOScale(1f,VFXConfig.Instance.jumpingTextScaleTime);
        });
    }

    public void DoEffect(){
        // 移动文本
        transform.position += moveDirection * Time.deltaTime;
        moveDirection.y -= VFXConfig.Instance.gravity * Time.deltaTime;
        
        // 降低透明度
        Color color = text.color;
        color.a -= VFXConfig.Instance.fadeSpeed * Time.deltaTime;
        text.color = color;
        
        // 当完全透明时返回对象池
        if(color.a <= 0){
            this.text.fontSize=fontSize;
            ObjectPool.Instance.ReturnObject(this);
        }
    }

    public void ResetFontSize(){
        // 重置字体大小到原始值
        this.text.fontSize = fontSize;
    }
}
