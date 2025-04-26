using TMPro;
using UnityEngine;

public class ResourcebarBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _polyCount;

    public Rect rect { get; private set; }

    private void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 min = Vector2.Scale(rt.anchorMin, new Vector2(Screen.width, Screen.height));
        Vector2 max = Vector2.Scale(rt.anchorMax, new Vector2(Screen.width, Screen.height));
        rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    public void setPolyCount(float poly, float income, float expense)
    {
        if (income >= 0)
        {
            _polyCount.SetText(poly.ToString() + " + " + (income - expense).ToString());
        }
        else
        {
            _polyCount.SetText(poly.ToString() + " - " + Mathf.Abs(income).ToString());
        }
    }
}
