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
}