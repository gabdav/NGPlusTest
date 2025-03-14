using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private UiInventory uiInventory;

    public void InitInventory(Inventory inv)
    {
        uiInventory.SetInventory(inv);
        uiInventory.Init();
    }
    public void TriggerInventory()
    {
        uiInventory.gameObject.SetActive(!uiInventory.gameObject.activeSelf);
        Cursor.visible = uiInventory.gameObject.activeSelf;
    }
}
