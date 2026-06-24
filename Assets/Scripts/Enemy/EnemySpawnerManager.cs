using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public class EnemySpawnerManager : MonoBehaviour
{
    [Header("オブジェクト設定")]
    [SerializeField] GameObject enemyPrefab;
    [Space(10)]
    [Header("パラメーター")]
    [Tooltip("生成クールタイム"), SerializeField] private float createInterval = 7.0f;
    [Tooltip("敵の上限数"), SerializeField] private int maxEnemyCount = 20;
    [Tooltip("確保するメモリ容量"), SerializeField] private int capacity = 10;

    //カメラ
    private Camera cam;

    //敵のリスト
    private List<GameObject> enemies = new List<GameObject>();

    //敵用オブジェクトプール
    private ObjectPool<GameObject> enemyPool;

    //敵生成カウント
    private float fireTimer;

    // == プロパティ ==
    public List<GameObject> Enemies { get => enemies; set => enemies = value; }

    private void Awake()
    {
        enemyPool = new ObjectPool<GameObject>(
            //enemyPoolが空の時に初めてInstantiateされる
            createFunc: () => Instantiate(enemyPrefab),

            //Get()した時に呼び出される
            actionOnGet: (enemy) => enemy.SetActive(true),

            //Release()した時に呼び出される
            actionOnRelease: (enemy) => enemy.SetActive(false),

            //Destroy()した時に呼び出される
            actionOnDestroy: (enemy) => Destroy(enemy),

            //二重Releaseを防止
            collectionCheck: true,

            //メモリ確保のための初期容量
            defaultCapacity: capacity,

            //上限数
            maxSize: maxEnemyCount
        );
    }

    void Start()
    {
        //カメラの取得
        cam = Camera.main;
    }

    private void Update()
    {
        //ゲーム停止中か？
        if (GameManager.Instance.IsGameStopped) return; 

        //敵の数が上限数未満なら生成
        if (enemyPool.CountActive < maxEnemyCount)
        {
            fireTimer += Time.deltaTime;  //クールダウン時間を減らす
            if (fireTimer >= createInterval)
            {
                Fire();  //敵生成
                fireTimer = 0.0f;  //クールダウン時間をリセット
            }
        }
    }

    //
    private GameObject Fire()
    {
        GameObject enemyObj = enemyPool.Get();  //敵のオブジェクトをプールから取得
        Init(enemyObj); //初期化
        enemyObj.transform.position = GetSpawnPosition();  //敵生成位置を設定
        enemies.Add(enemyObj);  //敵のリストに追加
        Debug.Log($"ゲーム内にいる敵の数: {enemyPool.CountActive}");
        return enemyObj;
    }

    /// <summary>
    /// 敵を非表示する
    /// </summary>
    /// <param name="releaseObj">非表示するオブジェクト</param>
    public void ReturnToRelease(GameObject releaseObj)
    {
        enemyPool.Release(releaseObj);  //敵のオブジェクトをプールに返す
        enemies.Remove(releaseObj);  //敵のリストから削除
        GameManager.Instance.EnemyKillCounter++;    //敵を倒した数をインクリメント
    }

    /// <summary>
    /// 敵生成位置を取得
    /// </summary>
    private Vector3 GetSpawnPosition()
    {
        //スクリーン座標の左下を取得
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));

        //スクリーン座標の右上を取得
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        //3方向の座標取得（画面右側から敵を配置するため）
        float right = topRight.x;
        float top = topRight.y - 1.0f;
        float bottom = bottomLeft.y + 1.0f;

        Vector3 spawnPos = Vector3.zero;

        //右の上から下までをランダムに選ぶ
        spawnPos = new Vector3(right + 1.0f, Random.Range(bottom, top), 0.0f);

        return spawnPos;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="enemy">敵オブジェクト</param>
    private void Init(GameObject enemy)
    {
        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController.Init();
    }
}
