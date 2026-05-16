using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int money { get; private set; }

    private void Awake()
    {
        //ƒVƒ“ƒOƒ‹ƒgƒ“‚ÌÀ‘•
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// “G‚ğ“|‚µ‚½‚É‚¨‹à‚ğ’Ç‰Á‚·‚é
    /// </summary>
    /// <param name="amount">•ñV‹à</param>
    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log($"Š‹àF{money}");
    }
}
