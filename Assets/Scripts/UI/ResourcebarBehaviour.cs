using TMPro;
using UnityEngine;

public class ResourcebarBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _polyCount;

    private void Start()
    {
        
    }

    public void setPolyCount(float count)
    {
        _polyCount.SetText(count.ToString());
    }
}
