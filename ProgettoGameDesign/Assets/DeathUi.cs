using DG.Tweening;
using UnityEngine;

public class DeathUi : MonoBehaviour
{
    

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += ShowDeathUI;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= ShowDeathUI;
    }
    void ShowDeathUI()
    {
        DOVirtual.DelayedCall(0.5f, () => 
        {
            gameObject.SetActive(true);
            
            
        });
    }
}
