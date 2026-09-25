using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallShooterController : MonoBehaviour
{

    private Rigidbody2D rgbody;
    private int activeTouches;

    public GameObject door;

    void OnEnable()
    {
        // Enable Enhanced Touch support for multi-touch
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        // Disable Enhanced Touch support when the script is disabled
        EnhancedTouchSupport.Disable();
    }

    void Awake()
    {
        rgbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        activeTouches = Touch.activeTouches.Count;
    }

    void FixedUpdate()
    {
        if (activeTouches == 2)
        {
            rgbody.AddForce(Vector2.down * 10000f);
        }
    }

}
