using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private PlayerHealth player;
    [SerializeField] private Image healthFillImage;

    void Start()
    {
        player = Player.Instance.GetComponent<PlayerHealth>();
        player.OnHealthChangedCallback += UpdateHealthBar;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float fillAmount = (float)player.Health / player.maxHealth;
        healthFillImage.fillAmount = fillAmount;
    }
}
