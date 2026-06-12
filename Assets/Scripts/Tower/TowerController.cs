using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TowerController : MonoBehaviour
{
    [Tooltip("タワーのステータス情報"), SerializeField] private TowerStatusData towerStatusData;

    private int hp; //体力

    private void Start()
    {
        hp = towerStatusData.statusData.hp;
    }

    /// <summary>
    /// タワーにダメージを与える処理
    /// </summary>
    /// <param name="dealDamage"></param>
    public void TakeDamage(int dealDamage)
    {
        hp -= dealDamage;

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        Debug.Log($"タワーの残りの体力: {hp}");
    }
}
