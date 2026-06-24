using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatusData", menuName = "Tower/StatusData")]
public class TowerStatusData : ScriptableObject
{
    [System.Serializable]
    public class StatusData
    {
        public string towerName;    //タワー名
        public Sprite icon;         //タワーアイコン
        public int cost;            //設置コスト
        public GameObject prefab;   //タワーオブジェクト
    }

    public StatusData statusData = new StatusData();
}
