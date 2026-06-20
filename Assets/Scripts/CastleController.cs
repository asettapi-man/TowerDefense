using UnityEngine;

public class CastleController : MonoBehaviour
{
    [Tooltip("‘Ì—Í"), SerializeField] private int hp;

    private ResultManager resultManager;

    void Awake()
    {
        resultManager = GameObject.FindFirstObjectByType<ResultManager>();
    }

    /// <summary>
    /// é‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚éˆ—
    /// </summary>
    /// <param name="dealDamage"></param>
    public void TakeDamage(int dealDamage)
    {
        hp -= dealDamage;

        if (hp <= 0)
        {
            GameManager.Instance?.StopGame();
            resultManager?.ResultTextDisplay();
            //‘Ì—Í‚ª‚O‚É‚È‚Á‚½‚ç”jŠü
            //Destroy(this.gameObject);
        }

        Debug.Log($"é‚Ìc‚è‚Ì‘Ì—Í: {hp}");
    }
}
