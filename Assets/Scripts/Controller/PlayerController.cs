using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject _startingPoint;
    private Rigidbody2D _rb;
    public float maxSpeed;

    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _startingPoint = GameObject.FindGameObjectWithTag("Respawn");
        gameObject.transform.position = _startingPoint.transform.position;
    }

    void FixedUpdate()
    {
        if (_rb.velocity.magnitude > maxSpeed)
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);
        }
    }
}
