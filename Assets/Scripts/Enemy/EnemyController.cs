using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyController : MonoBehaviour
{
    const string CASTLETAG = "Castle";

    [Tooltip("敵のステータス情報")]
    [SerializeField] private EnemyStatusData enemyStatusData;
    [Space(10)]
    [SerializeField] private HealthBar healthBar;

    private float hp; //体力
    private EnemySpawnerManager enemySpawner;
    private Transform towerPos; //タワー座標
    private Vector2 selfPos;    //自身の座標
    private bool isMove = true;    //移動停止フラグ
    private bool isDead = false;    //死亡フラグ

    private void Awake()
    {
        hp = enemyStatusData.statusData.hp;
        if (healthBar != null) return;
        healthBar.SetHealth(hp, enemyStatusData.statusData.hp);
    }

    private void Start()
    {
        //参照忘れを防ぐために定義
        towerPos = GameObject.FindWithTag(CASTLETAG).transform;
        enemySpawner = GameObject.FindFirstObjectByType<EnemySpawnerManager>();

        //初期化
        hp = enemyStatusData.statusData.hp;
        isMove = true;
        isDead = false;
    }

    void Update()
    { 
        //移動中かつゲーム進行中か？
        if (isMove && !GameManager.Instance.IsGameStopped) MoveToTower();
    }

    /// <summary>
    /// タワーの位置まで移動
    /// </summary>
    private void MoveToTower()
    {

        //if (towerPos != null)
        //{
        //    //Vector3.MoveTowardsで自身の位置から目標位置までどのように移動するか指定可能
        //    transform.position = Vector3.MoveTowards(transform.position, towerPos.position, enemyStatusData.statusData.moveSpeed * Time.deltaTime);
        //}

        //直進させる
        if (enemyStatusData?.statusData?.moveSpeed <= 0.0f) enemyStatusData.statusData.moveSpeed = 0.1f;
        transform.position += Vector3.left * enemyStatusData.statusData.moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(CASTLETAG))
        {
            //移動不可にする
            isMove = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(CASTLETAG))
        {
            //ダメージを与える
            collision.gameObject.GetComponent<CastleController>().TakeDamage(enemyStatusData.statusData.damage);

            //敵を倒す
            TakeDamage(hp);
        }
    }

    /// <summary>
    /// 敵にダメージを与える処理
    /// </summary>
    /// <param name="dealDamage">ダメージ量</param>
    /// <returns></returns>
    public void TakeDamage(float dealDamage)
    {
        if (isDead) return; //すでに死亡している場合は処理しない

        hp -= dealDamage;

        healthBar.SetHealth(hp, enemyStatusData.statusData.hp);

        //体力が０以下なら
        if (hp <= 0)
        {
            isDead = true; //死亡フラグを立てる
            hp = 0;
            GameManager.Instance.AddMoney(enemyStatusData.statusData.rewardMoney); //敵を倒した報酬金を追加
            Debug.Log("死亡");
            enemySpawner.ReturnToRelease(this.gameObject);
        }
        else
        {
            Debug.Log($"敵の残りの体力: {hp}");
        }
    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    public void Init()
    {
        hp = enemyStatusData.statusData.hp;
        healthBar.SetHealth(hp, enemyStatusData.statusData.hp);
        isDead = false;
        isMove = true;
    }
}
