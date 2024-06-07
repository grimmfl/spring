using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public LayerMask terrainLayer;

    private Camera _cam;
    private NavMeshAgent _agent;

    private Vector3? _target;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main!;
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask: terrainLayer))
            {
                _agent.SetDestination(hit.point);
            }
        }
    }
}