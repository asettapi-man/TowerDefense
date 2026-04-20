using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MouseController : MonoBehaviour
{
    //タグ名（固定）
    private const string enemyTag = "Enemy";

    [Tooltip("マウスによる攻撃のダメージ量")]
    [SerializeField] private uint damage = 2;

    //現在のコルーチン
    private Coroutine currentCoroutine;

    void Update()
    {
        //マウスの座標をワールド座標として取得
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //マウスカーソルに触れたものを取得
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        //触れたものが敵か？
        if (hit.collider != null && hit.collider.CompareTag(enemyTag) && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentCoroutine == null)
            {
                //ダメージを与える
                hit.collider.gameObject.GetComponent<EnemyMove>().MouseDamage(damage);
            }
        }
    }
}
