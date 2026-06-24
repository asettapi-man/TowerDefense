using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TowerSpawnController : MonoBehaviour
{
    [Tooltip("設置位置"), SerializeField] private Transform targetPos;
    [Tooltip("情報ウィンドウ"), SerializeField] private RectTransform informationWindowPos;

    // == 共通化 ==
    private static bool toggle; //情報ウィンドウ開閉フラグ
    private static Transform selectedPos;   //設置位置の更新用
    private static TowerSpawnController selectedSpawner;    //タワースポナーの更新用

    private TowerInformationDisplay towerInformation;
    private RectTransform myRectPos;    //自身のRectTransform

    private void Awake()
    {
        myRectPos = GetComponent<RectTransform>();
        toggle = false;
    }

    public void ToggleInformationWindow()
    {
        selectedPos = targetPos;
        selectedSpawner = this;

        toggle = !toggle;
        if (toggle)
        {
            //情報ウィンドウのy座標変更
            Vector2 pos = informationWindowPos.anchoredPosition;
            pos.y = myRectPos.anchoredPosition.y;
            informationWindowPos.anchoredPosition = pos;
        }

        informationWindowPos.gameObject.SetActive(toggle);
    }

    public void BuildTower(Button clickButton)
    {
        towerInformation = clickButton.GetComponent<TowerInformationDisplay>();
        if (towerInformation == null)
        {
            Debug.Log("TowerInformationDisplayがありません");
            return;
        }
        int cost = towerInformation.GetTowerCost();
        if (GameManager.Instance.Money >= cost)
        {
            GameManager.Instance.AddMoney(-cost);
            GameObject tower = towerInformation.GetTowerObject();
            selectedSpawner.TowerSpawner(tower, selectedPos);
        }
        else
        {
            Debug.LogWarning("所持金が足りないのでタワーを設置できません");
        }
    }

    /// <summary>
    /// タワーの設置処理
    /// </summary>
    /// <param name="prefab">設置するタワーオブジェクト</param>
    /// <param name="targetPos">設置位置</param>
    private void TowerSpawner(GameObject prefab, Transform targetPos)
    {
        Instantiate(prefab, targetPos.transform.position, Quaternion.identity);
        Debug.Log("タワー設置完了");
        toggle = false;
        informationWindowPos.gameObject.SetActive(toggle);
        this.gameObject.SetActive(toggle);
    }
}
