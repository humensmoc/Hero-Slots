using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Relic
{
    public RelicData relicData;
    public Relic(RelicData relicData){
        this.relicData = relicData.Clone();
    }
}