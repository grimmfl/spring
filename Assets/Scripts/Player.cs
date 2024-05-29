using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask terrainLayer;
    public float speed = 1;
    public float groundDistance = 0;
    
    private Camera _cam;
    
    private Vector3? _target;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main!;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask: terrainLayer))
            {
                _target = hit.point;
            }
        }
        
        Move();
    }

    private void Move()
    {
        if (_target == null) return;
        
        var direction = _target.Value - gameObject.transform.position;
        
        direction.y = 0;

        if (direction.magnitude > 1)
            direction.Normalize();

        direction *= speed;

        // if target is closer than step => reached at next step
        if ((_target.Value - gameObject.transform.position).magnitude < direction.magnitude)
            _target = null;

        // set height according to terrain
        if (Physics.Raycast(gameObject.transform.position + direction, -transform.up, out var hit, Mathf.Infinity, terrainLayer))
        {
            direction.y = hit.point.y - transform.position.y + groundDistance;
        }

        gameObject.transform.position += direction;
    }
}
