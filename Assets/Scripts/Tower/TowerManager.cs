using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private ResultManager resultManager;

    //タワーリスト
    private readonly List<TowerController> towers = new List<TowerController>();

    /// <summary>
    /// タワーリストに要素を追加
    /// </summary>
    /// <param name="tower">追加するタワー</param>
    public void Regista(TowerController tower)
    {
        //towerControllerがリスト内にある？
        if (!towers.Contains(tower))
        {
            //無ければ追加
            towers.Add(tower);
        }
    }

    /// <summary>
    /// タワーリストから要素を削除
    /// </summary>
    /// <param name="tower">削除するタワー</param>
    public void Unregista(TowerController tower)
    {
        //リスト内から外す
        towers.Remove(tower);

        //リストが空？
        if (towers.Count <= 0)
        {
            //リザルト画面を表示する
            resultManager.ResultCanvas.gameObject.SetActive(true);
            
        }
    }
}
