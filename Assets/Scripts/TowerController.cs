using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour
{
    [SerializeField] GameObject bulletObj;

    [Header("タワーのステータス")]
    [Tooltip("タワーの体力")]
    [SerializeField] private int hp = 10;
    [Tooltip("攻撃速度")]
    [SerializeField] private float attackInterval = 0.1f;

    private CreateEnemy createEnemy;

    private Coroutine currentCoroutine;

    private void OnValidate()
    {
        //０より大きい値に強制する
        hp = Mathf.Clamp(hp, 0, hp);
        attackInterval = Mathf.Clamp(attackInterval, 0, attackInterval);
    }

    void Start()
    {
        
    }


    void Update()
    {
        //敵が出現しているか？
        if (createEnemy.enemies.Count > 0)
        {
            //弾生成開始
            currentCoroutine = StartCoroutine(SpawnBullet(attackInterval));
        }
        else
        {
            //弾生成終了
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    /// <summary>
    /// タワーにダメージを与える処理
    /// </summary>
    /// <param name="dealDamage"></param>
    public void TakeDamage(int dealDamage)
    {
        hp -= dealDamage;

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        Debug.Log($"タワーの残りの体力: {hp}");
    }

    /// <summary>
    /// 弾生成関数
    /// </summary>
    /// <param name="delay">インターバル</param>
    /// <returns></returns>
    private IEnumerator SpawnBullet(float delay)
    {
        Instantiate(bulletObj, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(delay);
    }
}
