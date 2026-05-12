using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
public class CreateEnemy : MonoBehaviour
{
    [Header("オブジェクト設定")]
    [SerializeField] GameObject enemyPrefab;
    [Space(10)]
    [Header("パラメーター")]
    [Tooltip("生成開始時間")][SerializeField] private float startCreateInterval = 3.0f;
    [Tooltip("生成クールタイム")][SerializeField] private float createInterval = 7.0f;

    //出現している敵の管理
    [HideInInspector]public List<GameObject> enemies = new List<GameObject>();

    //カメラ
    private Camera cam;


    void Start()
    {
        //カメラの取得
        cam = Camera.main;

        //繰り返し敵を生成
        InvokeRepeating("SpawnEnemy", startCreateInterval, createInterval);
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 敵生成関数
    /// </summary>
    private void SpawnEnemy()
    {
        //カメラのZ座標を取得
        var camZ = cam.transform.position.z;

        //スクリーン座標の左下を取得
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, -camZ));

        //スクリーン座標の右上を取得
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -camZ));

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
                spawnPos = new Vector3(left - 1.0f, Random.Range(top, bottom), 0.0f);
                break;

            case 3: //右
                spawnPos = new Vector3(right + 1.0f, Random.Range(top, bottom), 0.0f);
                break;
        }

        var spawnObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemies.Add(spawnObj);
        Debug.Log($"敵の出現数：{enemies.Count}");
    }
}
