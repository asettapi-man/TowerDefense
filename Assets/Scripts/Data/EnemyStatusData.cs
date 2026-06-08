using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatusData", menuName = "Enemy/StatusData")]
public class EnemyStatusData : ScriptableObject
{
    [System.Serializable]
    public class StatusData
    {
        public float moveSpeed = 0.1f;  //移動速度
        public int hp = 10;             //体力
        public int damage = 1;          //攻撃力
        public int rewardMoney = 10;    //報奨金
    }

    public StatusData statusData = new StatusData();    //敵のステータス情報
}
