using UnityEngine.EventSystems;

public class Config{
    public const int MAX_HEALTH=3;

    //敌人移动几次之后对我方造成伤害
    public const int ENEMY_DAMAGE_TURN=8;

    public const int COIN_PER_ENEMY=3;

    public const int DELETE_CARD_COST=2;
    public const int REFRESH_CARD_COST=1;
    public const int REFRESH_HERO_COST=2;

    public const float CARD_POSITION_INTERVAL=1.2f;

    public const float HERO_POSITION_INTERVAL=1.2f;

    public static void Init(){

    }

}