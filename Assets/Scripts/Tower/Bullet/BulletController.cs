using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class BulletController : MonoBehaviour
{
    [Header("パラメーター")]
    [Tooltip("移動速度"), SerializeField] private float speed = 1f;
    [Tooltip("攻撃力")][SerializeField] private int damage = 1;
    [Tooltip("ビューポート座標外の余白")]
    [SerializeField] private float viewportMargin = 0.1f;

    //敵の座標
    private Transform targetPos;

    private EnemySpawnerManager enemySpawner;

    private BulletSpawner bulletSpawner;

    //物理演算用
    private Rigidbody2D rb;

    private Camera cam;

    //現在の座標
    private Vector2 currentPos;

    private void OnValidate()
    {
        //０より大きい値に強制する
        speed = Mathf.Clamp(speed, 0, speed);
        damage = Mathf.Clamp(damage, 0, damage);
        viewportMargin = Mathf.Clamp(viewportMargin, 0, viewportMargin);
    }

    private void Awake()
    {
        //参照忘れを防ぐために定義
        enemySpawner = FindFirstObjectByType<EnemySpawnerManager>();
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //ゲーム停止中か？
        if (GameManager.Instance.IsGameStopped) return;

        Vector3 viewPos = cam.WorldToViewportPoint(transform.position); //ビューポート座標を取得
        if( viewPos.x < 0 - viewportMargin || viewPos.x > 1 + viewportMargin || viewPos.y < 0 - viewportMargin || viewPos.y > 1 + viewportMargin)
        {
            //ビューポート座標外に出たら非表示
            bulletSpawner.ReturnToPool(gameObject);
            return;
        }

        //ターゲット先が存在している？
        if (HasTarget())
        {
            //座標をMoveTowardsで敵に近づける
            currentPos = Vector2.MoveTowards(
                gameObject.transform.position,  // 現在位置
                targetPos.position,             // 目標位置（敵）
                speed * Time.fixedDeltaTime     // 1フレームの移動量
            );

            //スプライトの向きを進行方向に合わせる
            Vector2 dir = (Vector2)targetPos.position - rb.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(angle);         //回転を反映
            rb.MovePosition(currentPos);    //座標を反映
        }
        else
        {
            targetPos = null;   //ターゲットを外す
            //そのまま直進
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    public void Init(BulletSpawner bulletSpawner)
    {
        this.bulletSpawner = bulletSpawner;
        targetPos = FindNearestEnemy(); //最も近い敵の座標を取得
    }

    private bool HasTarget()
    {
        return targetPos != null && targetPos.gameObject.activeInHierarchy;
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
        foreach (var enemy in enemySpawner.Enemies)
        {
            if (enemy == null || !enemy.activeInHierarchy) continue;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵に触れた？
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //弾の非表示
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.TakeDamage(damage); //敵に攻撃
            bulletSpawner.ReturnToPool(gameObject);   //弾を非表示
        }
    }
}
