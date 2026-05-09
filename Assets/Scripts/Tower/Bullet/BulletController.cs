using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    //敵の座標
    private Transform targetPos;

    //タワー座標
    private Transform towerPos;

    private CreateEnemy createEnemy;

    private BulletSpawner spawner;

    //物理演算用
    private Rigidbody2D rb;

    private void OnValidate()
    {
        //０より大きい値に強制する
        speed = Mathf.Clamp(speed, 0, speed);
    }

    void Start()
    {
        //参照忘れを防ぐために定義
        towerPos = GameObject.FindWithTag("Tower").transform;
        createEnemy = FindFirstObjectByType<CreateEnemy>();
    }

    void Update()
    {
        //敵がいない場合は最も近い敵の座標を再取得
        if (targetPos == null)
        {
            targetPos = FindNearestEnemy();
            if (targetPos == null) return;
        }

        //座標をMoveTowardsで敵に近づける
        Vector2 newPos = Vector2.MoveTowards(
            rb.position,            // 現在位置
            targetPos.position,        // 目標位置（敵）
            speed * Time.fixedDeltaTime  // 1フレームの移動量
        );

        //スプライトの向きを進行方向に合わせる
        Vector2 dir = (Vector2)targetPos.position - rb.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rb.MoveRotation(angle);     //回転を反映
        rb.MovePosition(newPos);    //座標を反映
    }

    public void Init(BulletSpawner spawner)
    {
        this.spawner = spawner;
        rb = GetComponent<Rigidbody2D>();
        targetPos = FindNearestEnemy(); //最も近い敵の座標を取得
    }

    /// <summary>
    /// 最も近い敵の座標を取得
    /// </summary>
    /// <returns></returns>
    private Transform FindNearestEnemy()
    {
        //敵の座標を取得
        Transform nearestEnemy = null;

        //最も近い敵との距離を初期化
        float minDistance = Mathf.Infinity; //float型の最大値を初期値として設定

        //敵のリストをループして最も近い敵を見つける
        foreach (var enemy in createEnemy.enemies)
        {
            //弾と敵との距離を計算
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            //最も近い敵を更新
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        //最も近い敵の座標を返す
        return nearestEnemy;
    }
}
