using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelicType{
    A,
    B,
    C,
    D,
    E,

}

public static class RelicLibrary{
    public static List<RelicData> relicDatas = new List<RelicData>(){
        new RelicData(RelicType.A)
            .SetDescription("Relic_A Description")
            .SetOnInit(() => {
                Debug.Log("Relic_A Init");
            }),
        new RelicData(RelicType.B)
            .SetDescription("Relic_B Description")
            .SetOnInit(() => {
                Debug.Log("Relic_B Init");
            }),
        new RelicData(RelicType.C)
            .SetDescription("Relic_C Description")
            .SetOnInit(() => {
                Debug.Log("Relic_C Init");
            }),
        new RelicData(RelicType.D)
            .SetDescription("Relic_D Description")
            .SetOnInit(() => {
                Debug.Log("Relic_D Init");
            }),
        new RelicData(RelicType.E)
            .SetDescription("Relic_E Description")
            .SetOnInit(() => {
                Debug.Log("Relic_E Init");
            }),
    };

    public static RelicData GetRelicData(RelicType relicType){
        return relicDatas.Find(relicData => relicData.RelicType == relicType);
    }
}