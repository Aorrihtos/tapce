using System.Collections;
using UnityEngine;

public class TransportHole : MonoBehaviour
{

    [SerializeField]
    private GameObject _destination;

    [SerializeField]
    private float _delay = 0.9f;

    private FixedJoint2D _joint;

    private void Awake()
    {
        _joint = GetComponent<FixedJoint2D>();
        _joint.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_destination != null && collision.CompareTag("Player"))
        {
            StartCoroutine(AnimateAndTeleport(collision));
        }
    }

    private IEnumerator AnimateAndTeleport(Collider2D collision) {

        _destination.GetComponent<Collider2D>().enabled = false;
        Animator _animator = collision.gameObject.GetComponentInChildren<Animator>();

        Debug.Log(_animator);

        // Stop player's movement to make sticking smooth
        collision.attachedRigidbody.velocity = Vector2.zero;
        collision.attachedRigidbody.angularVelocity = 0f;

        // Enable the fixed Joint
        _joint.connectedBody = collision.attachedRigidbody;
        _joint.anchor = Vector3.zero;
        _joint.enabled = true;

        // Activate animation
        _animator.SetTrigger("Disappear");

        // Wait for the animation to complete
        yield return new WaitForSeconds(1);

        // Disable Joint
        _joint.enabled = false;
        _joint.connectedBody = null;

        // Transport player to the other hole
        collision.gameObject.transform.position = _destination.transform.position;
        gameObject.GetComponent<Collider2D>().enabled = false;

        // Activate destination fixed Joint
        FixedJoint2D _destinationJoint = _destination.GetComponent<FixedJoint2D>();
        _destinationJoint.connectedBody = collision.attachedRigidbody;
        _destinationJoint.anchor = Vector3.zero;
        _destinationJoint.enabled = true;

        // Activate animation
        _animator.SetTrigger("Appear");

        yield return new WaitForSeconds(1);

        // Disable destination Joint
        _destinationJoint.enabled = false;
        _destinationJoint.connectedBody = null;

        Invoke("ResetColliders", _delay);

    }

    private void ResetColliders()
    {
        _destination.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
