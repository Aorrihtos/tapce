using System;
using UnityEngine;

public class SlignshotController : MonoBehaviour
{

    [SerializeField]
    private bool isRight = false;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            var rigidbody = collider.attachedRigidbody;
            var forceX = Math.Max(Math.Abs(rigidbody.velocity.x), 1f);
            if (isRight)
            {
                forceX *= -1;
            }
            var forceY = Math.Max(Math.Abs(rigidbody.velocity.y), 1f);
            rigidbody.velocity.Set(0f, 0f);
            rigidbody.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
        }
    }
}
