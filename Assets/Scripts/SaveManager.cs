using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public ItemDefinition[] allItemDefinitions;
    string saveFilePath = Path.Combine(Application.dataPath, "Saves", "inventory.json");
    public void SaveInventory(Inventory inv)
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");
#if UNITY_EDITOR
        saveFilePath = Path.Combine(Application.dataPath, "Saves", "inventory.json");
#endif
        InventorySaveData saveData = new InventorySaveData();
        saveData.slots = new SerializableInventorySlot[inv.inventorySlotsArray.Length];

        for (int i = 0; i < inv.inventorySlotsArray.Length; i++)
        {
            SerializableInventorySlot serialSlot = new SerializableInventorySlot();
            serialSlot.index = inv.inventorySlotsArray[i].GetIndex();

            if (inv.inventorySlotsArray[i].GetItem().item != null)
            {
                serialSlot.item = new SerializableInventoryItem
                {
                    itemID = inv.inventorySlotsArray[i].GetItem().item.name, // Using ScriptableObject's name as unique ID
                    quantity = inv.inventorySlotsArray[i].GetItem().quantity
                };
            }

            saveData.slots[i] = serialSlot;
        }

        string jsonData = JsonUtility.ToJson(saveData, true);

        System.IO.File.WriteAllText(saveFilePath, jsonData);
    }

    // Method to load inventory
    public void LoadInventory(Inventory inv)
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");
#if UNITY_EDITOR
        saveFilePath = Path.Combine(Application.dataPath, "Saves", "inventory.json");
#endif        
        if (System.IO.File.Exists(saveFilePath))
        {
            string jsonData = System.IO.File.ReadAllText(saveFilePath);
            InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(jsonData);

            if (inv.inventorySlotsArray == null || inv.inventorySlotsArray.Length != saveData.slots.Length)
            {
                inv.inventorySlotsArray = new InventorySlot[saveData.slots.Length];
            }
            for (int i = 0; i < saveData.slots.Length; i++)
            {
                inv.inventorySlotsArray[i] = new InventorySlot(saveData.slots[i].index);

                if (saveData.slots[i].item != null && !string.IsNullOrEmpty(saveData.slots[i].item.itemID))
                {
                    ItemDefinition itemDef = FindItemDefinition(saveData.slots[i].item.itemID);

                    if (itemDef != null)
                    {
                        InventoryItem tempItem = new InventoryItem
                        {
                            item = itemDef,
                            quantity = saveData.slots[i].item.quantity
                        };
                        inv.inventorySlotsArray[i].SetItem(tempItem);
                    }
                }
            }
        }
    }
    private ItemDefinition FindItemDefinition(string itemID)
    {
        foreach (ItemDefinition itemDef in allItemDefinitions)
        {
            if (itemDef.name == itemID)
                return itemDef;
        }
        return null;
    }
}
[System.Serializable]
public class SerializableInventoryItem
{
    public string itemID;
    public float quantity;
}

[System.Serializable]
public class SerializableInventorySlot
{
    public int index;
    public SerializableInventoryItem item;
}

[System.Serializable]
public class InventorySaveData
{
    public SerializableInventorySlot[] slots;
}