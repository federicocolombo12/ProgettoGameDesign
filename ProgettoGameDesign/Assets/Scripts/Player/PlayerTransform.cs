using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
    private float timeSinceLastTransform = 0f;
    private float transformCooldown = 1f; // Cooldown time in seconds
    public static event System.Action OnTransform; // Event to notify when transformation occurs
    [SerializeField] GameObject baseGO;
    
    [SerializeField] GameObject agileGO;
    
    [SerializeField] GameObject strongGO;
    [SerializeField] List<Material> baseMaterials;
    [SerializeField] List<Material> agileMaterials;
    [SerializeField] List<Material> strongMaterials;
    ParticleSystem transformationParticle;
    [SerializeField] List<Color> transformationColors;
    private int lastTransformationIndex = 0;


    private CapsuleCollider2D collider;
    
    
   

    enum Form
    {
        Human,
        Bat,
        Wolf
    }
    [SerializeField] List<PlayerTransformation> playerTransformations; // List of possible transformations
    Form currentForm;
    private void Start()
    {
        // Initialize the current form to Human
        
        timeSinceLastTransform = transformCooldown; // Start with cooldown ready
        collider = GetComponent<CapsuleCollider2D>();
        transformationParticle = GetComponentInChildren<ParticleSystem>();
        transformationParticle.Play();
        IterateMaterials(0); // Inizializza i materiali per la forma umana
        IterateMaterials(1); // Inizializza i materiali per la forma agile
        IterateMaterials(2); // Inizializza i materiali per la forma forte
        ChangeState(Form.Human, 0); // Inizializza la forma umana
    }
    private void Update()
    {
        // Update the cooldown timer
        if (timeSinceLastTransform < transformCooldown)
        {
            timeSinceLastTransform += Time.deltaTime;
        }
    }

    public void HandleTransform(bool leftTransform, bool rightTransform)
    {
        if (timeSinceLastTransform < transformCooldown)
        {
            return; // Prevent transformation if cooldown is active
        }

        switch (currentForm)
        {
            case Form.Human:
                if (leftTransform)
                {
                    ChangeState(Form.Bat, 1);
                }
                else if (rightTransform)
                {
                    ChangeState(Form.Wolf, 2);
                }
                break;
            case Form.Bat:
                if (leftTransform)
                {
                    ChangeState(Form.Human, 0);
                }
                else if (rightTransform)
                {
                    ChangeState(Form.Wolf, 2);
                }
                break;
            case Form.Wolf:
                if (leftTransform)
                {
                    ChangeState(Form.Bat, 1);
                }
                else if (rightTransform)
                {
                    ChangeState(Form.Human, 0);
                }
                break;
        }
    }

    void ChangeState(Form nextForm, int transformationIndex)
{
    currentForm = nextForm;
    
    AnimateMaterialValues(lastTransformationIndex, transformationIndex); // prima di cambiare sprite
    ChangeSprite(transformationIndex);

    timeSinceLastTransform = 0f;
    OnTransform?.Invoke();
    
    lastTransformationIndex = transformationIndex; // aggiorna dopo l'animazione
}

    void ChangeSprite(int transformationIndex)
    {
        var transformation = playerTransformations[transformationIndex];
        
        Player.Instance.playerTransformation = transformation;
        baseGO.SetActive(transformationIndex == 0);
        agileGO.SetActive(transformationIndex == 1);
        strongGO.SetActive(transformationIndex == 2);
        transformationParticle.Stop();
        var main = transformationParticle.main;
        
        main.startColor = transformationColors[transformationIndex];
        
        
      
        transformationParticle.Play();
        
        collider.size = transformation.colliderSize;
        collider.offset = transformation.colliderOffset;
        transform.localScale = transformation.transformationScale;
        
        
    }

    

    void IterateMaterials(int transformationIndex)
    {
        // Svuota le liste prima di riempirle
        switch (transformationIndex)
        {
            case 0: baseMaterials.Clear(); break;
            case 1: agileMaterials.Clear(); break;
            case 2: strongMaterials.Clear(); break;
        }

        // Seleziona il GameObject giusto
        GameObject targetGO = null;
        switch (transformationIndex)
        {
            case 0: targetGO = baseGO; break;
            case 1: targetGO = agileGO; break;
            case 2: targetGO = strongGO; break;
        }

        if (targetGO == null) return;

        // Itera sui figli del GameObject selezionato
        foreach (Transform child in targetGO.transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Material material = spriteRenderer.material;

                // Aggiungi alla lista corretta
                switch (transformationIndex)
                {
                    case 0: baseMaterials.Add(material); break;
                    case 1: agileMaterials.Add(material); break;
                    case 2: strongMaterials.Add(material); break;
                }
            }
        }
    }
    void AnimateMaterialValues(int fromIndex, int toIndex)
    {
        List<Material> fromMaterials = GetMaterialsByIndex(fromIndex);
        List<Material> toMaterials = GetMaterialsByIndex(toIndex);

        // Dissolvi la forma precedente: dissolveAmount da 0 → 1
        foreach (var mat in fromMaterials)
        {
            if (mat.HasProperty("_DissolveAmount"))
            {
                mat.DOFloat(0f, "_DissolveAmount", 0.5f); // dissolve out in 0.5s
            }
            else
            {
                Debug.LogWarning($"Material {mat.name} does not have 'Dissolve Amount' property.");
            }
        }

        // Appari con la nuova forma: dissolveAmount da 1 → 0
        foreach (var mat in toMaterials)
        {
            if (mat.HasProperty("_DissolveAmount"))
            {
                mat.SetFloat("_DissolveAmount", 0f); // forza inizio da 1
                mat.DOFloat(1f, "_DissolveAmount", 0.5f); // dissolve in in 0.5s
            }
            else
            {
                Debug.LogWarning($"Material {mat.name} does not have 'Dissolve Amount' property.");
            }
        }
    }

    List<Material> GetMaterialsByIndex(int index)
    {
        switch (index)
        {
            case 0: return baseMaterials;
            case 1: return agileMaterials;
            case 2: return strongMaterials;
            default: return new List<Material>();
        }
    }


}
