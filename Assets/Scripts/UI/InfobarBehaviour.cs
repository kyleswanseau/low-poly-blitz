using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfobarBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _unitName;
    [SerializeField] private Button _cubeButton;
    [SerializeField] private Button _sphereButton;
    [SerializeField] private Button _tetraButton;
    [SerializeField] private Button _factoryButton;
    [SerializeField] private Button _pylonButton;
    [SerializeField] private Button _mineButton;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private Slider _progressSlider;
    private Factory? _factory = null;

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
            _progressSlider.value = _factory.progress;
        }
    }

    public void SwitchGUI(int mode)
    {
        switch (mode)
        {
            case 1:
                _cubeButton.gameObject.SetActive(true);
                _sphereButton.gameObject.SetActive(true);
                _tetraButton.gameObject.SetActive(true);
                _factoryButton.gameObject.SetActive(false);
                _pylonButton.gameObject.SetActive(false);
                _mineButton.gameObject.SetActive(false);
                _progressText.gameObject.SetActive(true);
                _progressSlider.gameObject.SetActive(true);
                break;
            case 2:
                _cubeButton.gameObject.SetActive(false);
                _sphereButton.gameObject.SetActive(false);
                _tetraButton.gameObject.SetActive(false);
                _factoryButton.gameObject.SetActive(true);
                _pylonButton.gameObject.SetActive(true);
                _mineButton.gameObject.SetActive(true);
                _progressText.gameObject.SetActive(false);
                _progressSlider.gameObject.SetActive(false);
                break;
            case 3:
            default:
                _cubeButton.gameObject.SetActive(false);
                _sphereButton.gameObject.SetActive(false);
                _tetraButton.gameObject.SetActive(false);
                _factoryButton.gameObject.SetActive(false);
                _pylonButton.gameObject.SetActive(false);
                _mineButton.gameObject.SetActive(false);
                _progressText.gameObject.SetActive(false);
                _progressSlider.gameObject.SetActive(false);
                break;
        }
    }

    public void setUnitName(string name)
    {
        _unitName.SetText(name);
    }

    public void setFactory(Factory factory)
    {
        _factory = factory;
        _progressSlider.maxValue = _factory.maxProgress;
    }

    public void BuildCubes()
    {
        _factory.BuildCubes();
    }

    public void BuildSpheres()
    {
        _factory.BuildSpheres();
    }

    public void BuildTetras()
    {
        _factory.BuildTetras();
    }
}
