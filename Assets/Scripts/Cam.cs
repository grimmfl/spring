using UnityEngine;

public class Cam : MonoBehaviour
{
    public GameObject player;
    public float smoothTime;
    
    private Vector3 _offset;
    private Vector3 _currentVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var target = player.transform.position + _offset;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref _currentVelocity, smoothTime);
    }
}
