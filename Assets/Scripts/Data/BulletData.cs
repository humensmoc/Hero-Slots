using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Bullet")]
public class BulletData : ScriptableObject
{
    [field: SerializeField] public Sprite Image {get; private set;}
    [field: SerializeField] public int Attack {get; private set;}

}
