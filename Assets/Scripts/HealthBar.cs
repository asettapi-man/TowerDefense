using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image hpImage;

    private void Awake()
    {
        hpImage = GetComponent<Image>();
    }

    /// <summary>
    /// ‘Ì—ÍƒQ[ƒW‚Ìİ’è
    /// </summary>
    /// <param name="current">Œ»İ‚Ì‘Ì—Í</param>
    /// <param name="max">Å‘å‘Ì—Í</param>
    public void SetHealth(float current, float max)
    {
        if (hpImage == null) return;
        hpImage.fillAmount = Mathf.Max(current / max);
    }
}
