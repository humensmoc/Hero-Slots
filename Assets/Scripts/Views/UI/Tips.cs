using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Tips : MonoBehaviour
{
    public TMP_Text text;

    public void Init(string tips)
    {
        text.text = tips;

        transform.DOMoveY(transform.position.y + 100, 1f).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
