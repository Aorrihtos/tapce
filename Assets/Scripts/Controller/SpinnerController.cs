using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class SpinnerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private float _force = 10f;

    private FixedJoint2D joint;
    private Rigidbody2D stuckBody;

    private bool enabled = true;

    void Awake()
    {
        joint = GetComponent<FixedJoint2D>();
        joint.enabled = false;

        // Enable Enhanced Touch system
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnTouch;
    }


    void FixedUpdate()
    {
        float frameRotation = _speed * Time.deltaTime;
        transform.Rotate(0, 0, frameRotation);  // Apply the rotation
    }

    void OnDestroy()
    {
        // Unsubscribe when object is destroyed
        Touch.onFingerDown -= OnTouch;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!joint.enabled && collision.gameObject.tag == "Player" && enabled)
        {

            // Attach the player to this object
            Rigidbody2D playerRb = collision.rigidbody;
            joint.connectedBody = playerRb;
            joint.enabled = true;
            stuckBody = playerRb;

            // Stop player's movement to make sticking smooth
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }
    }

    void OnTouch(Finger finger)
    {
        if (joint.enabled) Unstick();
    }

    void Unstick()
    {
        enabled = false;
        // Get PLayer's current movement direction
        Vector2 forceDirection = (stuckBody.position - (Vector2)transform.position).normalized;

        // Release the player
        joint.enabled = false;
        joint.connectedBody = null;

        // Apply force to player
        stuckBody.velocity = Vector2.zero; // Reset any existing velocity
        stuckBody.AddForce(forceDirection * _force, ForceMode2D.Impulse);
        stuckBody = null;

        StartCoroutine(TimeoutEnabled());
    }

    IEnumerator TimeoutEnabled()
    {
        yield return new WaitForSeconds(0.3f);
        enabled = true;
    }
}
