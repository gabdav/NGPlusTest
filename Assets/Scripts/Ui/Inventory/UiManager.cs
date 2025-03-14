using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private UiInventory uiInventory;

    public void Init()
    {

    }
    public void TriggerInventory(bool trigger)
    {
        uiInventory.gameObject.SetActive(trigger);
    }
}
