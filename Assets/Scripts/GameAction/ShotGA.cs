using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGA : GameAction
{
    public int Damage{get;private set;}
    public ShotGA(int damage)
    {
        Damage = damage;
    }

}
