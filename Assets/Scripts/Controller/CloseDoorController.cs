using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorController : MonoBehaviour
{
    [SerializeField]
    private GameObject door;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            door.SetActive(true);
        }
    }
}
