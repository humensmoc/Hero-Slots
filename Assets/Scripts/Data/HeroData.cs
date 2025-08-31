using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Hero")]
public class HeroData : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField][field: TextArea(3,10)] public string Description {get; private set;}
    [field: SerializeField] public Sprite Image {get; private set;}
    [field: SerializeField] public int Attack {get; private set;}
    [field: SerializeField] public HeroType HeroType {get; private set;}
    [field: SerializeField] public int MaxEnergy {get; private set;}
    [field: SerializeField] public ElementType ElementType {get; private set;}
    [field: SerializeField] public HeroEffect HeroEffect ;
}
