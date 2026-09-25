using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterLaneDoorController : MonoBehaviour
{
    public GameObject door;
    private Collider2D closeDoorCollider;
    private Collider2D openDoorCollider;

    void Awake()
    {
        closeDoorCollider = gameObject.GetComponent<BoxCollider2D>();
        openDoorCollider = gameObject.GetComponent<CapsuleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(closeDoorCollider);
        Debug.Log(collider.bounds.Intersects(closeDoorCollider.bounds));
        Debug.Log(collider.bounds.Intersects(openDoorCollider.bounds));

        Debug.Log("entro al metodo");
        if (!collider.CompareTag("Player"))
            return;

        if (collider.bounds.Intersects(closeDoorCollider.bounds))
        {
            door.SetActive(true);
        }
        else if (collider.bounds.Intersects(openDoorCollider.bounds))
        {
            door.SetActive(false);
        }
    }
}
