using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplaySystem : MonoBehaviour
{
    [SerializeField] private Text moneyText; //所持金を表示するUIテキスト

    private void OnDisable()
    {
        GameManager.Instance.OnMoneyChanged -= UpdateMoneyDisplay;
    }
    void Start()
    {
        GameManager.Instance.OnMoneyChanged += UpdateMoneyDisplay;
        UpdateMoneyDisplay(GameManager.Instance.Money); //初期表示を更新
    }

    void Update()
    {
        
    }

    private void UpdateMoneyDisplay(int money)
    {
        moneyText.text = $"所持金: {money}"; //GameManagerから最新の所持金を取得して表示
    }
}
