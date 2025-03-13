using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiTooltip : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private TextMeshProUGUI tooltip;

    public void DisplayTooltip(bool showTooltip, string description)
    {
        bg.SetActive(showTooltip);
        tooltip.text = description;
    }
}
