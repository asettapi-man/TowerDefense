using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public class BulletSpawner : MonoBehaviour
{
    [Header("オブジェクト設定")]
    [SerializeField] private GameObject bulletPrefab;   //弾のプレハブ
    [Space(10)]
    [Header("パラメーター")]
    [Tooltip("弾の上限")][SerializeField] private int maxBulletCount = 20;
    [Tooltip("クールダウン")][SerializeField] private float fireInterval = 5.0f;

    //オブジェクトプールのインスタンス
    private ObjectPool<GameObject> bullets;
    private float fireTimer = 0.0f;   //弾発射時間

    void Awake()
    {
        bullets = new ObjectPool<GameObject>(
            //bulletsが空の時に初めてInstantiateされる
            createFunc: () => Instantiate(bulletPrefab),

            //Get()した時に呼び出される
            actionOnGet: (obj) => obj.SetActive(true),

            //Release()した時に呼び出される
            actionOnRelease: (obj) => obj.SetActive(false),

            actionOnDestroy: (obj) => Destroy(obj),

            maxSize: maxBulletCount
            );
    }

    void Update()
    {
        //発射時間の加算
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            //発射開始
            Fire(transform.position);
            fireTimer -= fireInterval;
        }
    }

    /// <summary>
    /// 弾の表示（発射）
    /// </summary>
    /// <param name="spawnPos">生成位置</param>
    public void Fire(Vector2 spawnPos)
    {
        //オブジェクトプールから弾を取得
        GameObject bullet = bullets.Get();
        bullet.transform.position = spawnPos;

        //取り出した後に初期化
        bullet.GetComponent<BulletController>().Init(this);
    }

    /// <summary>
    /// 弾の非表示（消去）
    /// </summary>
    /// <param name="bullet"></param>
    public void ReturnToPool(GameObject bullet)
    {
        bullets.Release(bullet);
    }
}
