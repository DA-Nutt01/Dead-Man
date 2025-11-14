using UnityEngine;
using System.Collections;
using System;

public class Deadman : PacmanBase
{
    [SerializeField] private GameObject NemesisSkin;
    [SerializeField] private AnimatedSprite NemesisMovement;
    private bool isTransformed = false;
    private Coroutine currentTransformationCoroutine; // Stores the active coroutine

    protected override void Awake()
    {
        base.Awake();
        
        NemesisSkin.SetActive(false);
        isTransformed = false;
    }

    public override void ResetState()
    {
        enabled = true;
        m_SpriteRenderer.enabled = true;
        m_CircleCollider.enabled = true;
        m_DeathSequence.enabled = false;
        NemesisMovement.enabled = false;
        m_Movement.ResetState();
        gameObject.SetActive(true);
        NemesisSkin.SetActive(false);
    }

    protected override void UpdateRotation()
    {
        if (GameManager.gameState != GameState.Gameplay) return;

        // Do not rotate Deadman if he is transformed as Nemesis
        if (isTransformed) return;

        // Keep Pac-Man upright and just flip or rotate 90° increments
        Vector2 dir = m_Movement.direction;
        if (dir == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (dir == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (dir == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // flip horizontally
        }
        else if (dir == Vector2.right)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public void Transform(float buffer, float duration)
    {
        isTransformed = true;

        // Stop the current transformation coroutine if it's running
        if (currentTransformationCoroutine != null)
        {
            StopCoroutine(currentTransformationCoroutine);
        }

        // Reset the rotation of NemesisSkin to ensure it's upright
        NemesisSkin.transform.rotation = Quaternion.identity;

        // Start a new transformation coroutine with the new duration
        currentTransformationCoroutine = StartCoroutine(NemesisTransformation(buffer, duration));
    }
    
    private IEnumerator NemesisTransformation(float audioBufferTime, float duration)
    {
        // Ensure NemesisSkin is enabled and spriteRenderer is disabled
        m_SpriteRenderer.enabled = false;
        NemesisSkin.SetActive(true);

        // Wait for the audio buffer
        yield return new WaitForSeconds(audioBufferTime);

        // Ensure NemesisSkin is still active after the audio buffer
        if (!NemesisSkin.activeSelf)
        {
            NemesisSkin.SetActive(true);
        }

        // Enable the Nemesis moving animation set
        NemesisMovement.enabled = true;
        NemesisMovement.Restart();

        // Wait for the transformation duration
        yield return new WaitForSeconds(duration);

        // Transform back
        isTransformed = false;
        NemesisMovement.enabled = false;
        NemesisMovement.Restart();
        NemesisSkin.SetActive(false);
        m_SpriteRenderer.enabled = true;

        // Clear the current coroutine reference
        currentTransformationCoroutine = null;
    }
}
    