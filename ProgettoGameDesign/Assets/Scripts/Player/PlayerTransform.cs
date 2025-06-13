using UnityEngine;
using System.Collections.Generic;

public class PlayerTransform : MonoBehaviour
{
    
    

    [Header("Prefab delle trasformazioni")]
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject agilePrefab;
    [SerializeField] private GameObject strongPrefab;

    [Header("Dati delle trasformazioni")]
    [SerializeField] private List<PlayerTransformation> playerTransformations;

    [Header("Materiali")]
    private List<Material> baseMaterials = new List<Material>();
    private List<Material> agileMaterials = new List<Material>();
    private List<Material> strongMaterials = new List<Material>();

    [Header("Particelle ed effetti")]
    [SerializeField] private ParticleSystem transformationParticle;
    [SerializeField] private ParticleSystem effectParticle;
    [SerializeField] public List<Color> transformationColors;

    [Header("Collider")]
    [SerializeField] private CapsuleCollider2D collider;

    private GameObject currentInstance;
    private Animator currentAnimator;
    private int currentIndex = 0;
    private int totalForms => playerTransformations.Count;

    public static event System.Action OnTransform;

    private void Start()
    {
        // Istanzia forma di default (es. base)
        ChangeSprite(0);
    }
    public void HandleTransform(bool left, bool right)
    {
        if (left)
        {
            switch (currentIndex)
            {
                case 0:
                    currentIndex = 1;
                    break;
                case 1:
                    currentIndex = 0;
                    break;
                case 2:
                    currentIndex = 1;
                    break;
            }
            ChangeSprite(currentIndex);
        }
        else if (right)
        {
            switch (currentIndex)
            {
                case 0:
                    currentIndex = 2;
                    break;
                case 2:
                    currentIndex = 0;
                    break;
                case 1:
                    currentIndex = 2;
                    break;
            }
            ChangeSprite(currentIndex);
        }
    }




    public void ChangeSprite(int transformationIndex)
    {
        // Distruggi la forma corrente
        if (currentInstance != null)
        {
            Destroy(currentInstance);
        }

        // Istanzia il prefab corretto
        GameObject prefab = GetPrefabByIndex(transformationIndex);
        if (prefab == null) return;

        currentInstance = Instantiate(prefab, transform);
        currentAnimator = currentInstance.GetComponentInChildren<Animator>();

        if (currentAnimator != null)
        {
            currentAnimator.Rebind(); // Reset animazioni
            currentAnimator.enabled = true;
        }

        // Aggiorna riferimenti globali se serve
        Player.Instance.playerTransformation = playerTransformations[transformationIndex];
        Player.Instance.animator = currentAnimator;

        // Emetti particelle
        transformationParticle.Stop();
        var main = transformationParticle.main;
        main.startColor = transformationColors[transformationIndex];
        transformationParticle.Play();

        if (effectParticle != null)
        {
            EffectManager.Instance.PlayOneShot(effectParticle, transform.position);
        }

        // Aggiorna Collider e scala
        var transformation = playerTransformations[transformationIndex];
        collider.size = transformation.colliderSize;
        collider.offset = transformation.colliderOffset;
        transform.localScale = transformation.transformationScale;

        // Salva i materiali per effetti successivi
        IterateMaterials(transformationIndex);

        // Trigger evento
        OnTransform?.Invoke();
    }

    private GameObject GetPrefabByIndex(int index)
    {
        return index switch
        {
            0 => basePrefab,
            1 => agilePrefab,
            2 => strongPrefab,
            _ => null
        };
    }

    private void IterateMaterials(int transformationIndex)
    {
        switch (transformationIndex)
        {
            case 0: baseMaterials.Clear(); break;
            case 1: agileMaterials.Clear(); break;
            case 2: strongMaterials.Clear(); break;
        }

        if (currentInstance == null) return;

        foreach (Transform child in currentInstance.transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Material material = spriteRenderer.material;

                switch (transformationIndex)
                {
                    case 0: baseMaterials.Add(material); break;
                    case 1: agileMaterials.Add(material); break;
                    case 2: strongMaterials.Add(material); break;
                }
            }
        }
    }
}
