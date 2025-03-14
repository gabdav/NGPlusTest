using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private SaveManager saveManager;
    [SerializeField]private Inventory playerInventory;

    private void Start()
    {
        playerInventory.Init();
        saveManager.LoadInventory(playerInventory);
        playerInventory.SyncListWithSlots();
    }
    public void OnSave()
    {
        saveManager.SaveInventory(playerInventory);
    }
}
