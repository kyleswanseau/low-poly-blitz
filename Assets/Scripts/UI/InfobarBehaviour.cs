using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfobarBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _unitName;

    private void Start()
    {
        
    }

    public void setUnitName(string name)
    {
        _unitName.SetText(name);
    }
}
