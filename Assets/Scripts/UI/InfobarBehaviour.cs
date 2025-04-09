using TMPro;
using UnityEngine;

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
