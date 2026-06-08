using UnityEngine;
using UnityEngine.Pool;

public class BulletPoolManager : MonoBehaviour
{
    [Header("オブジェクト設定")]
    [SerializeField] private GameObject bulletPrefab;   //弾のプレハブ
    [Tooltip("弾の上限")]
    [SerializeField] private int maxBulletCount = 20;

    //オブジェクトプールのインスタンス
    private ObjectPool<GameObject> bullets;

    private void OnValidate()
    {
        //０より大きい値に強制する
        maxBulletCount = Mathf.Clamp(maxBulletCount, 0, maxBulletCount);
    }

    private void Awake()
    {

    }


}
