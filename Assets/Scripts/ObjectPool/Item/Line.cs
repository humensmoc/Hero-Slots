using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public enum LineType{
    Normal
}

public class Line : ObjectPoolItem
{
    public LineRenderer lineRenderer;

    public void Init(Vector3 startPos, Vector3 endPos){
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        DOVirtual.DelayedCall(1f, () => {
            lineRenderer.enabled = false;
            ReturnToPool();
        });
    }

}
