using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Relic
{
    public RelicData relicData;
    public string Name;
    public string Description;
    public Relic(RelicData relicData){
        this.relicData = relicData.Clone();
    }
}