using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName ="Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;}
    [field: SerializeField][field: TextArea(3,10)] public string Description {get; private set;}
    [field: SerializeField] public Sprite Image {get; private set;}
    [field: SerializeField] public int Attack {get; private set;}
    [field: SerializeField] public ElementType ElementType {get; private set;}
    [field: SerializeField] public CardName CardNameEnum {get; private set;}
}
