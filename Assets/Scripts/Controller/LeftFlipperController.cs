using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class LeftFlipperController : MonoBehaviour
{

    private HingeJoint2D _hingeJoint;
    private JointMotor2D _motor;
    private bool applyForce;
    public float motorSpeed = 1000f; // Speed to move up
    public float motorMaxTorque = 10000f; // Torque applied

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
        _hingeJoint = gameObject.GetComponent<HingeJoint2D>();
        _motor = _hingeJoint.motor;
    }

    void Update()
    {
        // Get active touches
        var activeTouches = Touch.activeTouches;
        var detectedTouch = activeTouches.Any(t => t.screenPosition.x < Screen.width / 2);

        bool leftArrowPressed = Keyboard.current != null && Keyboard.current.leftArrowKey.isPressed;
        
        applyForce = detectedTouch || leftArrowPressed;
    }

    void FixedUpdate()
    {
        if (applyForce)
        {
            _motor.motorSpeed = -motorSpeed;
            _motor.maxMotorTorque = motorMaxTorque;
            _hingeJoint.motor = _motor;
            _hingeJoint.useMotor = true;
        }
        else
        {
            // Return flipper to its resting position
            _motor.motorSpeed = motorSpeed;
            _motor.maxMotorTorque = motorMaxTorque;
            _hingeJoint.motor = _motor;
            _hingeJoint.useMotor = true;
        }
    }

}
