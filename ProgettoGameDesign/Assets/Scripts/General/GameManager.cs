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
        bench = FindFirstObjectByType<Bench>();

    }
   
    public void SetRespawnPoint(Vector2 newRespawnPoint)
    {
        platformRespawnPoint = newRespawnPoint;
    }


    public void RespawnPlayer(Player player)
    {
        respawnPoint = bench.transform.position;
        player.RespawnAt(respawnPoint);
    }






}
