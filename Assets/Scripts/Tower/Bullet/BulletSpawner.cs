using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxBulletCount = 20;

    //オブジェクトプールのインスタンス
    private ObjectPool<GameObject> bullets;

    void Start()
    {
        bullets = new ObjectPool<GameObject>(
            //bulletsが空の時に初めてInstantiateされる
            createFunc: () => Instantiate(bulletPrefab),

            //Get()した時に呼び出される
            actionOnGet: (obj) => bulletPrefab.SetActive(true),

            //Release()した時に呼び出される
            actionOnRelease: (obj) => bulletPrefab.SetActive(false),

            actionOnDestroy: (obj) => Destroy(obj),

            maxSize: maxBulletCount
            );

        //次のやること：Fire関数を一定間隔で呼ぶようにする
    }

    void Update()
    {
        
    }

    public void Fire(Vector2 spawnPos)
    {
        //オブジェクトプールから弾を取得
        GameObject bullet = bullets.Get();
        bullet.transform.position = spawnPos;

        //取り出した後に初期化
        bullet.GetComponent<BulletController>().Init(this);
    }

    public void ReturnToPool(GameObject bullet)
    {
        bullets.Release(bullet);
    }
}
