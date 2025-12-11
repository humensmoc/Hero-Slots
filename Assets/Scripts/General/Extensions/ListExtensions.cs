using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T Draw<T>(this List<T> list){

        if(list.Count==0)return default;

        int r=Random.Range(0,list.Count);

        T t=list[r];

        list.Remove(t);
        
        return t;
    }

    public static void Shuffle<T>(this List<T> list){
        if(list == null || list.Count <= 1) return;

        for(int i = list.Count - 1; i > 0; i--){
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
