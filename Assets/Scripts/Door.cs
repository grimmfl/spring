using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject indoor;
    public GameObject outdoor;

    private bool _isIndoor;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (_isIndoor)
        {
            indoor.SetActive(false);
            outdoor.SetActive(true);
            _isIndoor = false;
        }
        else
        {
            indoor.SetActive(true);
            outdoor.SetActive(false);
            _isIndoor = true;
        }
    }
}
