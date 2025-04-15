using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfobarBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _unitName;
    [SerializeField] private Slider _slider;
    private Factory? _factory;

    public Rect rect { get; private set; }

    private void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 min = Vector2.Scale(rt.anchorMin, new Vector2(Screen.width, Screen.height));
        Vector2 max = Vector2.Scale(rt.anchorMax, new Vector2(Screen.width, Screen.height));
        rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    private void OnGUI()
    {
        if (null != _factory)
        {
            _slider.value = _factory.progress;
        }
    }

    public void setUnitName(string name)
    {
        _unitName.SetText(name);
    }

    public void setFactory(Factory factory)
    {
        _factory = factory;
        _slider.maxValue = _factory.maxProgress;
    }
}
