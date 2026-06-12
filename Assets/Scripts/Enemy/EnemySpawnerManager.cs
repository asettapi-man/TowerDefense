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
        releaseObj.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
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

        //4方向の座標取得
        float left = bottomLeft.x;
        float right = topRight.x;
        float top = topRight.y;
        float bottom = bottomLeft.y;

        // 上下左右のどの辺から出すかをランダムに選ぶ
        Vector3 spawnPos = Vector3.zero;
        int side = Random.Range(0, 4); // 0:上 1:下 2:左 3:右

        switch (side)
        {
            case 0: //上
                spawnPos = new Vector3(Random.Range(left, right), top + 1.0f, 0.0f);
                break;

            case 1: //下
                spawnPos = new Vector3(Random.Range(left, right), bottom - 1.0f, 0.0f);
                break;

            case 2: //左
                spawnPos = new Vector3(left - 1.0f, Random.Range(bottom, top), 0.0f);
                break;

            case 3: //右
                spawnPos = new Vector3(right + 1.0f, Random.Range(bottom, top), 0.0f);
                break;
        }

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
