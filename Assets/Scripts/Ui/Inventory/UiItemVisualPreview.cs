using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemVisualPreview : MonoBehaviour
{
    [SerializeField] private Image itemVisual;
    [SerializeField] private TextMeshProUGUI quant;

    public void UpdateItemVisual(Sprite s, float q)
    {
        itemVisual.sprite = s;
        if (q <= 1)
        {
            quant.gameObject.SetActive(false);
        }
        else
        {
            quant.gameObject.SetActive(true);
            quant.text = q.ToString();
        }

    }
}
