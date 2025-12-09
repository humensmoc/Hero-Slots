using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RelicData{
    public string Name;
    public string Description;
    public RelicType RelicType;
    public Action OnInit;
    public RelicData(RelicType relicType){
        this.RelicType = relicType;
    }
    public RelicData SetDescription(string description){
        Description = description;
        return this;
    }
    public RelicData SetOnInit(Action action){
        OnInit = action;
        return this;
    }
    public RelicData Clone(){
        RelicData clone = new RelicData(RelicType);
        clone.Name = Name;
        clone.Description = Description;
        clone.RelicType = RelicType;
        return clone;
    }
}