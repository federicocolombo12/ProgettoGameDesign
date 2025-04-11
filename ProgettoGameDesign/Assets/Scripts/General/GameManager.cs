using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string transitionedFromScene;    
    public static GameManager Instance { get; private set; }
    public Vector2 platformRespawnPoint;
    public Vector2 respawnPoint;
    [SerializeField] Bench bench;
    private void Awake()
    {
        if (Instance!=null && Instance != this)
        {
            Destroy(gameObject);
           
        }
        else
            {
            Instance = this;
           
        }
        DontDestroyOnLoad(gameObject);
        bench = FindObjectOfType<Bench>();
    }

    public void RespawnPlayer()
    {
        respawnPoint = bench.transform.position;
        PlayerController.Instance.transform.position = respawnPoint;
        PlayerController.Instance.Respawned();
    }

}
