using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TowerInformationDisplay : MonoBehaviour
{
    /*
     * ここでのやること
     * 1.TowerStatusData.csのデータを使用してTowerButton.prefabに表示するものを当てはめる
     * →TowerNameTextにはTowerNameを、Iconにはiconを当てはめる
    */
    [Header("UI")]
    [Tooltip("アイコン"), SerializeField] private Image icon;
    [Tooltip("タワー名"), SerializeField] private Text towerNameText;
    [Tooltip("設置コスト"), SerializeField] private Text towerCostText;
    [Space(10)]
    [SerializeField] private TowerStatusData towerData;

    void Start()
    {
        if (towerData != null)
        {
            if (icon != null)
            {
                icon.sprite = towerData.statusData.icon;
            }

            if (towerNameText != null)
            {
                towerNameText.text = towerData.statusData.towerName;
            }

            if (towerCostText != null)
            {
                towerCostText.text = $"設置金額:{towerData.statusData.cost.ToString()}";
            }
        }
    }

    /// <summary>
    /// タワーオブジェクトの取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetTowerObject()
    {
        if (towerData == null) return null;
        return towerData.statusData.prefab;
    }

    /// <summary>
    /// タワー設置コストの取得
    /// </summary>
    /// <returns></returns>
    public int GetTowerCost()
    {
        if (towerData == null) return 0;
        return towerData.statusData.cost;
    }
}
