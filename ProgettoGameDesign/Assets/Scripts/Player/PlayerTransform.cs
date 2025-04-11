using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransform : MonoBehaviour
{
    private float timeSinceLastTransform = 0f;
    private float transformCooldown = 1f; // Cooldown time in seconds
    enum Form
    {
        Human,
        Bat,
        Wolf
    }
    [SerializeField] List<PlayerTransformation> playerTransformations; // List of possible transformations
    Form currentForm;

    private void Update()
    {
        // Update the cooldown timer
        if (timeSinceLastTransform < transformCooldown)
        {
            timeSinceLastTransform += Time.deltaTime;
        }
    }

    public void HandleTransform(bool transform1, bool transform2)
    {
        if (timeSinceLastTransform < transformCooldown)
        {
            return; // Prevent transformation if cooldown is active
        }

        switch (currentForm)
        {
            case Form.Human:
                if (transform1)
                {
                    ChangeState(Form.Bat, 1);
                }
                else if (transform2)
                {
                    ChangeState(Form.Wolf, 2);
                }
                break;
            case Form.Bat:
                if (transform1)
                {
                    ChangeState(Form.Human, 0);
                }
                else if (transform2)
                {
                    ChangeState(Form.Wolf, 2);
                }
                break;
            case Form.Wolf:
                if (transform1)
                {
                    ChangeState(Form.Bat, 0);
                }
                else if (transform2)
                {
                    ChangeState(Form.Human, 1);
                }
                break;
        }
    }

    void ChangeState(Form nextForm, int transformationIndex)
    {
        currentForm = nextForm;
        Player.Instance.playerTransformation = playerTransformations[transformationIndex];
        timeSinceLastTransform = 0f; // Reset cooldown
    }
}
