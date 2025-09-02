using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXConfig : Singleton<VFXConfig>
{
    [Header("Jumping Text")]
    public float jumpingTextScale=2f;
    public float jumpingTextScaleTime=0.15f;
    public float horizontalSpeedMin = -2f;
    public float horizontalSpeedMax = 2f;
    public float verticalSpeedMax = 2f;
    public float verticalSpeedMin = 1f;
    public float gravity = 0.1f;
    public float fadeSpeed = 1f;

    [Header("Flying Text")]
    public float flyingTextScale=2f;
    public float flyingTextScaleTime=0.15f;
    public float flySpeed=10f;
    public Color coinColor;
    public Color gemColor;
    public Color jumpingTextColor;
}
