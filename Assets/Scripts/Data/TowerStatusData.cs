using UnityEngine;

[CreateAssetMenu(fileName = "TowerStatusData", menuName = "Tower/StatusData")]
public class TowerStatusData : ScriptableObject
{
    [System.Serializable]
    public class StatusData
    {
        public int hp = 10;
    }

    public StatusData statusData = new StatusData();
}
