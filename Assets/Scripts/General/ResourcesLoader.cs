using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesLoader
{
    public static bool Load<T>(string path, out T asset) where T : Object
    {
        asset = Resources.Load<T>(path);
        return asset != null;
    }

    public static Sprite LoadCardSprite(string path)
    {
        if(Load<Sprite>("Sprite/Card/"+path, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }

    public static Sprite LoadBulletSprite(string path)
    {
        if(Load<Sprite>("Sprite/Bullet/"+path, out Sprite sprite))
        {   
            return sprite;
        }
        return null;
    }

    public static Sprite LoadHeroSprite(string path)
    {
        if(Load<Sprite>("Sprite/Hero/"+path, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }

    public static Sprite LoadEnemySprite(string path)
    {
        if(Load<Sprite>("Sprite/Enemy/"+path, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }

    public static Sprite LoadEffectSprite(string path)
    {
        if(Load<Sprite>("Sprite/Effect/"+path, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }

    public static Sprite LoadRelicSprite(string path)
    {
        if(Load<Sprite>("Sprite/Relic/"+path, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }

    public static GameObject LoadPrefab(string path)
    {
        if(Load<GameObject>("Prefabs/"+path, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }

    public static GameObject LoadUIPrefab(string path)
    {
        if(Load<GameObject>("Prefabs/UI/"+path, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }
}
