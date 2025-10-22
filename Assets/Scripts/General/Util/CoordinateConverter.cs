using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordinateConverter{
    public static Vector3 UIToWorld(Vector3 uiPosition, float zDistance=10f){
        Vector3 uiScreenPosition = uiPosition;
        
        // 2. 将UI的屏幕坐标转换为世界坐标
        Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(uiScreenPosition.x, uiScreenPosition.y, zDistance));
        return targetWorldPosition;
    }
}
