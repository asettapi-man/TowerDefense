using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TowerController : MonoBehaviour
{
    [Tooltip("タワーのステータス情報"), SerializeField] private TowerStatusData towerStatusData;

    private TowerManager towerManager;

    private void Start()
    {
        towerManager = GameObject.FindFirstObjectByType<TowerManager>();

        if (towerManager == null) return;   //nullチェック

        towerManager.Regista(this); //自分自身をリストに追加
    }

    private void Die()
    {
        if (towerManager == null) return;
        towerManager.Unregista(this);   //リストから削除
    }
}
