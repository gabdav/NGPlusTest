using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private SaveManager saveManager;
    [SerializeField]private Inventory playerInventory;
    [SerializeField]private Player player;
    [SerializeField]private UiManager uiManager;
    [SerializeField]private CinemachineFreeLook cameraFreeLook;

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
            cameraFreeLook.enabled = !cameraFreeLook.isActiveAndEnabled;
            player.enabled = !player.isActiveAndEnabled;
        }
    }
    public Inventory GetPlayerInventory()
    {
        return playerInventory;
    }
    public void OnSave()
    {
        saveManager.SaveInventory(playerInventory);
    }
}
