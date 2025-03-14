using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private SaveManager saveManager;
    [SerializeField]private Inventory playerInventory;
    [SerializeField]private UiManager uiManager;

    private void Start()
    {
        Cursor.visible = false;
        playerInventory.Init();
        uiManager.InitInventory(playerInventory);
        saveManager.LoadInventory(playerInventory);
        playerInventory.SyncListWithSlots();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) 
        {
            uiManager.TriggerInventory();
        }
    }
    public void OnSave()
    {
        saveManager.SaveInventory(playerInventory);
    }
}
