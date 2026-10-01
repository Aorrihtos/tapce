using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallShooterControllerTEST : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] frames;

    [Header("Plunger Collider")]
    [SerializeField] private BoxCollider2D plungerCollider;
    [SerializeField] private Vector2[] colliderOffsets;

    [Header("Charge")]
    [SerializeField] private float chargeSpeed = 2f;

    [Header("Launch")]
    [SerializeField] private float maxLaunchForce = 20f;

    [Header("Stretch Animation")]
    [SerializeField] private float stretchFrameTime = 0.05f;

    private Rigidbody2D ballRb;

    private bool wasPressed;
    private bool isPlayingStretch;

    private float charge;
    private float stretchTimer;

    private int currentFrame;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.isPressed;

        bool touchPressed =
            Touch.activeTouches.Count == 2;

        bool isPressed = keyboardPressed || touchPressed;

        // Started pressing
        if (isPressed && !wasPressed)
        {
            charge = 0f;
            isPlayingStretch = false;
            stretchTimer = 0f;

            SetFrame(0);
        }

        // Holding
        if (isPressed && !isPlayingStretch)
        {
            UpdateCharge();
        }

        // Released
        if (!isPressed && wasPressed)
        {
            Release();
        }

        // Stretch animation
        if (isPlayingStretch)
        {
            UpdateStretchAnimation();
        }

        wasPressed = isPressed;
    }

    private void UpdateCharge()
    {
        charge += Time.deltaTime * chargeSpeed;
        charge = Mathf.Clamp01(charge);

        // Frames 0-4 = compression
        int frame = Mathf.RoundToInt(charge * 4f);
        frame = Mathf.Clamp(frame, 0, 4);

        SetFrame(frame);
    }

    private void Release()
    {
        if (ballRb != null)
        {
            float launchForce = maxLaunchForce * charge;

            ballRb.AddForce(
                Vector2.up * launchForce,
                ForceMode2D.Impulse
            );

            Debug.Log("Force added");

            ballRb = null;
        }

        StartStretch();
    }

    private void StartStretch()
    {
        isPlayingStretch = true;
        stretchTimer = 0f;

        SetFrame(5);
    }

    private void UpdateStretchAnimation()
    {
        stretchTimer += Time.deltaTime;

        if (stretchTimer < stretchFrameTime)
            return;

        stretchTimer = 0f;

        if (currentFrame == 5)
        {
            SetFrame(6);
        }
        else
        {
            isPlayingStretch = false;
            SetFrame(0);
        }
    }

    private void SetFrame(int frame)
    {
        frame = Mathf.Clamp(frame, 0, frames.Length - 1);

        currentFrame = frame;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = frames[frame];
        }

        if (plungerCollider != null &&
            colliderOffsets != null &&
            frame < colliderOffsets.Length)
        {
            plungerCollider.offset = colliderOffsets[frame];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D rb = collision.rigidbody;

        Debug.Log("RB added");

        if (rb != null)
        {
            ballRb = rb;
        }
    }
}