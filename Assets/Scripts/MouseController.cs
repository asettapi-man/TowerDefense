using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class MouseController : MonoBehaviour
{
    //タグ名（固定）
    private const string enemyTag = "Enemy";

    [Tooltip("攻撃のクールタイム")][SerializeField, Range(0.1f, 3f)] private float attackInterval = 0.5f;
    [Tooltip("攻撃力")][SerializeField] private int damage = 2;

    //最後に攻撃した時の時間
    private float lastAttackTime = 0.0f;

    private void OnValidate()
    {
        //０より大きい値に強制する
        damage = Mathf.Clamp(damage, 0, damage);
    }

    void Update()
    {
        //マウスの座標をワールド座標として取得
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //マウスカーソルに触れたものを取得
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        //触れたものが敵か？
        if (hit.collider != null && hit.collider.CompareTag(enemyTag))
        {
            //ゲーム内時間がインターバルより値が大きくなった？
            if (Time.time >= lastAttackTime + attackInterval)
            {
                //最後に攻撃した時間を記憶する
                lastAttackTime = Time.time;

                //時間経過で攻撃
                hit.collider.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
    }
}
