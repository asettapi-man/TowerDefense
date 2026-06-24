using UnityEngine;
using UnityEngine.Pool;


[DisallowMultipleComponent]
public class BulletSpawner : MonoBehaviour
{
    [Header("オブジェクト設定")]
    [SerializeField] private GameObject bulletPrefab;   //弾のプレハブ
    [Header("パラメーター")]
    [Tooltip("クールダウン"), SerializeField] private float fireInterval = 5.0f;
    [Tooltip("弾の上限"), SerializeField] private int maxBulletCount = 20;
    [Tooltip("確保するメモリ容量"), SerializeField] private int capacity = 10;

    //スクリプト参照
    private EnemySpawnerManager enemySpawner;

    private float fireTimer = 0.0f;   //弾発射時間

    //オブジェクトプールのインスタンス
    private ObjectPool<GameObject> bullets;

    private void OnValidate()
    {
        //０より大きい値に強制する
        maxBulletCount = Mathf.Clamp(maxBulletCount, 0, maxBulletCount);
    }

    void Awake()
    {
        bullets = new ObjectPool<GameObject>(
            //bulletsが空の時に初めてInstantiateされる
            createFunc: () => Instantiate(bulletPrefab),

            //Get()した時に呼び出される
            actionOnGet: (bullet) => bullet.SetActive(true),

            //Release()した時に呼び出される
            actionOnRelease: (bullet) => bullet.SetActive(false),

            //Destroy()した時に呼び出される
            actionOnDestroy: (bullet) => Destroy(bullet),

            //二重Releaseを防止
            collectionCheck: true,

            //メモリ確保のための初期容量
            defaultCapacity: capacity,

            //上限数
            maxSize: maxBulletCount
        );

        //参照忘れを防ぐために定義
        enemySpawner = GameObject.FindFirstObjectByType<EnemySpawnerManager>();
    }

    void Update()
    {
        //ゲーム停止中か？
        if (GameManager.Instance.IsGameStopped) return;

        //敵が存在しない場合は処理終了
        if (enemySpawner.Enemies.Count <= 0) return;

        //発射時間の加算
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            //発射開始
            Fire(transform.position, this);
            fireTimer = 0.0f;
        }
    }

    /// <summary>
    /// 弾の表示（発射）
    /// </summary>
    /// <param name="spawnPos">生成位置</param>
    public void Fire(Vector2 spawnPos, BulletSpawner bulletSpawner)
    {
        //オブジェクトプールから弾を取得
        GameObject bullet = bullets.Get();
        bullet.transform.position = spawnPos;

        //取り出した後に初期化
        bullet.GetComponent<BulletController>().Init(bulletSpawner);
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
