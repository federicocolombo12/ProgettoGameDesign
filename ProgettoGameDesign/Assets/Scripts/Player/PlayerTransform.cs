using NUnit.Framework;
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
        currentForm = Form.Human;
        timeSinceLastTransform = transformCooldown; // Start with cooldown ready
        collider = GetComponent<CapsuleCollider2D>();
        
    
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
        ChangeSprite(transformationIndex);

        timeSinceLastTransform = 0f;
        OnTransform?.Invoke(); // Notify subscribers about the transformation
    }
    void ChangeSprite(int transformationIndex)
    {
        var transformation = playerTransformations[transformationIndex];
        
        Player.Instance.playerTransformation = transformation;
        baseGO.SetActive(transformationIndex == 0);
        agileGO.SetActive(transformationIndex == 1);
        strongGO.SetActive(transformationIndex == 2);
        

        collider.size = transformation.colliderSize;
        collider.offset = transformation.colliderOffset;
        transform.localScale = transformation.transformationScale;
        
        
    }
    
}
