using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusItemController : MonoBehaviour
{

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private int _bonusReward;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        _gameManager.SetBonusLevel(_bonusReward);

        Destroy(gameObject);
        Debug.Log("hola");
    }
}
