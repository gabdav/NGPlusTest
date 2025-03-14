using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private ItemDefinition itemDefinition;
    private Inventory _playerInventory;
    private void Start()
    {
        _playerInventory = FindAnyObjectByType<GameManager>().GetPlayerInventory();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            _playerInventory.AddItemViaButton(itemDefinition);
            Destroy(gameObject);
        }
    }
}
