using UnityEngine;
using UnityEngine.UI;
public class HeartController : MonoBehaviour
{
    PlayerController player;
    private GameObject[] heartContainers;
    private Image[] heartFills;
    public Transform heartsParent;
    public GameObject heartContainerPrefab;
    void Start()
    {
        player = PlayerController.Instance;
        heartContainers = new GameObject[PlayerController.Instance.maxHealth];
        heartFills = new Image[PlayerController.Instance.maxHealth];
        PlayerController.Instance.OnHealthChangedCallback += UpdateHeartsHUD;
        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetHearthContainers()
    {

        for (int i = 0; i < player.maxHealth; i++)
        {
            if (i < player.maxHealth)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }
    void SetFilledHearts()
    {

        for (int i = 0; i < player.maxHealth; i++)
        {
            if (i < player.Health)
            {
                heartFills[i].fillAmount=1;
            }
            else
            {
                heartFills[i].fillAmount=0;
            }
        }
    }
    void InstantiateHeartContainers()
    {
        for (int i=0; i<player.maxHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
    void UpdateHeartsHUD()
    {
        SetHearthContainers();
        SetFilledHearts();
    }
}
