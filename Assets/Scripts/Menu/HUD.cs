using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    const float InitialHealthBarValue = 1f;
    const int InitialEchoesValue = 0;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI ashEchoesText;

    private void Start()
    {
        UpdateHealthBar(InitialHealthBarValue);
        UpdateAshEchoes(InitialEchoesValue);

        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged += UpdateHealthBar;
            Player.Instance.OnEchoesChanged += UpdateAshEchoes;
        }
    }

    public void UpdateHealthBar(float currentHealth)
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void UpdateAshEchoes(int amount)
    {
        if (ashEchoesText != null)
            ashEchoesText.text = amount.ToString();
    }

    void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnHealthChanged -= UpdateHealthBar;
            Player.Instance.OnEchoesChanged -= UpdateAshEchoes;
        }
    }
}
