using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField]
    private int _lives = 1;

    [SerializeField]
    private bool _static = false;
    public int reward = 100;
    private Vector2 _waypoint1;
    private Vector2 _waypoint2;
    private Rigidbody2D _rgbody;
    private Vector2 _targetPosition;
    private GameManager _gameManager;
    private bool _movingToWaypoint1 = true;

    public float speed = 2f;

    void Awake()
    {
        _waypoint1 = transform.GetChild(0).transform.position;
        _waypoint2 = transform.GetChild(1).transform.position;
        _rgbody = gameObject.GetComponent<Rigidbody2D>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _targetPosition = _waypoint1; // Start moving to waypoint 1
    }

    private void Start()
    {
        Flip();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_static) MoveBetweenWaypoints();
    }

    void MoveBetweenWaypoints()
    {
        // Move the Rigidbody2D towards the target position
        Vector2 newPosition = Vector2.MoveTowards(_rgbody.position, _targetPosition, speed * Time.fixedDeltaTime);
        _rgbody.MovePosition(newPosition);

        if (Vector2.Distance(_rgbody.position, _targetPosition) < 0.1f)
        {
            // Switch target to the other waypoint
            _movingToWaypoint1 = !_movingToWaypoint1;
            _targetPosition = _movingToWaypoint1 ? _waypoint1 : _waypoint2;

            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(RewardAndDestroy());
        }
    }

    IEnumerator RewardAndDestroy()
    {
        // Add points and print reward
        _gameManager.AddPoints(reward, transform.position);

        yield return new WaitForSeconds(0.2f);

        // Decrease lives counter and destroy if no lives
        if (--_lives <= 0) gameObject.SetActive(false);
    }
}
