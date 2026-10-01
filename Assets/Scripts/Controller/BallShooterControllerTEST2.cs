using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallShooterControllerTEST2 : MonoBehaviour
{
    [Header("Physics References")]
    [SerializeField] private SliderJoint2D sliderJoint;
    [SerializeField] private Rigidbody2D headRigidbody;

    [Header("Visual References")]
    [SerializeField] private Transform springTransform;
    [SerializeField] private SpriteRenderer springRenderer;

    [Header("Plunger Speeds")]
    [SerializeField] private float pullSpeed = -6f;     // Speed pulling down
    [SerializeField] private float launchSpeed = 80f;   // Speed launching ball up
    [SerializeField] private float restSpeed = 2f;       // Holding speed at top position

    // Baseline spring scaling metrics
    private float restDistanceY;
    private float initialScaleY;
    private Vector2 initialSpriteSize;

    private bool isPressed;
    private bool wasPressed;
    private bool isLaunching;

    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();

    private void Start()
    {
        if (springTransform != null && headRigidbody != null)
        {
            restDistanceY = Mathf.Abs(headRigidbody.position.y - springTransform.position.y);
            initialScaleY = springTransform.localScale.y;

            if (springRenderer != null)
            {
                initialSpriteSize = springRenderer.size;
            }
        }
    }

    private void Update()
    {
        // 1. Inputs
        bool keyboardPressed = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
        bool touchPressed = false;
#if !UNITY_EDITOR
        touchPressed = Touch.activeTouches.Count >= 2;
#endif

        isPressed = keyboardPressed || touchPressed;

        // 2. Press State Transitions
        if (isPressed)
        {
            // Holding Space: Drive plunger back (downward)
            SetMotorSpeed(pullSpeed);
            isLaunching = false;
        }
        else if (wasPressed && !isPressed)
        {
            // Release Space: Fire plunger forward (upward launch)
            SetMotorSpeed(launchSpeed);
            isLaunching = true;
        }
        else if (isLaunching && sliderJoint.jointTranslation >= -0.05f)
        {
            // Reached top position after launch: Reset to gentle holding force
            SetMotorSpeed(restSpeed);
            isLaunching = false;
        }

        wasPressed = isPressed;

        // 3. Update Visual Spring
        UpdateSpringVisual();
    }

    private void SetMotorSpeed(float speed)
    {
        if (sliderJoint == null) return;
        JointMotor2D motor = sliderJoint.motor;
        motor.motorSpeed = speed;
        sliderJoint.motor = motor;
    }

    private void UpdateSpringVisual()
    {
        if (springTransform == null || headRigidbody == null || restDistanceY <= 0.0001f) return;

        // Calculate only vertical distance
        float currentDistanceY = Mathf.Abs(headRigidbody.position.y - springTransform.position.y);

        if (springRenderer != null && springRenderer.drawMode == SpriteDrawMode.Sliced)
        {
            springRenderer.size = new Vector2(initialSpriteSize.x, currentDistanceY);
        }
        else
        {
            float scaleRatio = currentDistanceY / restDistanceY;
            Vector3 currentScale = springTransform.localScale;
            springTransform.localScale = new Vector3(currentScale.x, initialScaleY * scaleRatio, currentScale.z);
        }
    }
}