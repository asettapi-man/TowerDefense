using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TowerController : MonoBehaviour
{
    [Tooltip("タワーのステータス情報"), SerializeField] private TowerStatusData towerStatusData;

    private TowerManager towerManager;
    private int hp; //体力

    private void Start()
    {
        hp = towerStatusData.statusData.hp;
        towerManager = GameObject.FindFirstObjectByType<TowerManager>();    //ゲーム内にあるTowerManagerを探す

        if (towerManager == null) return;   //nullチェック

        towerManager.Regista(this); //自分自身をリストに追加
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

    private void Die()
    {
        if (towerManager == null) return;
        towerManager.Unregista(this);   //リストから削除
    }
}
