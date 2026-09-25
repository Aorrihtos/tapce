using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BumperController : MonoBehaviour
{

    private GameManager _gameManager;

    public int reward = 100;
    public float timeout = 0.5f;

    private bool _canReward = true;


    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player") && _canReward)
        {
            // Apply the collision
            var playerRbody = collision.rigidbody;
            var _forceDirection = (playerRbody.position - (Vector2)transform.position).normalized * Vector2.Max(playerRbody.velocity, Vector2.one);
            playerRbody.AddForce(_forceDirection, ForceMode2D.Impulse);

            // Add points to counter and display reward
            _gameManager.AddPoints(reward, transform.position);

            // Timeout management
            StartCoroutine(ManageTimeout());
        }
    }

    private IEnumerator ManageTimeout() { 
        _canReward = false;
        yield return new WaitForSeconds(timeout);
        _canReward = true;
    }
}
