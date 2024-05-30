using System.Collections;
using System.Linq;
using Persistence;
using Persistence.Models;
using UnityEngine;
using MouseButton = Unity.VisualScripting.MouseButton;

public class Dismantable : MonoBehaviour
{
    public int idGameEntity;
    
    private Camera _cam;
    private GameObject _player;

    private GameEntityRepository _gameEntityRepository;
    private InventoryRepository _inventoryRepository;
    
    private bool _isDismantling;
    private GameEntity _gameEntity;
    private int _progress = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _player = GameObject.FindWithTag("Player");

        _gameEntityRepository = new GameEntityRepository();
        _inventoryRepository = new InventoryRepository();
        
        _gameEntity = _gameEntityRepository.GetById(idGameEntity);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameEntity.isDismantable) return;
        
        if (!Input.GetMouseButtonDown((int)MouseButton.Left)) return;

        if (_isDismantling) _isDismantling = false;
        
        var ray = _cam.ScreenPointToRay(Input.mousePosition);

        // find raycast hit
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return;

        // if this has been clicked
        if (hit.transform.gameObject != gameObject) return;

        StartCoroutine(Dismantle());
    }

    IEnumerator Dismantle()
    {
        yield return new WaitUntil(() => (gameObject.transform.position - _player.transform.position).magnitude < 1);
            
        _isDismantling = true;

        var remainingSeconds = _gameEntity.dismantleSeconds - _progress;

        for (var i = 0; i < remainingSeconds; i++)
        {
            if (!_isDismantling) break;
            
            Debug.Log($"Dismantling - Progress: {_progress + 1}/{_gameEntity.dismantleSeconds}");
            yield return new WaitForSecondsRealtime(1);
            
            _progress++;
        }
        
        Drop();
        
        Destroy(gameObject);
    }

    private void Drop()
    {
        var dropCounts = _gameEntity.dropItems.ToDictionary(x => x.idItem, x => Random.Range(x.min, x.max));
        
        var toDrop = _inventoryRepository.AddItems(dropCounts);

        foreach (var pair in toDrop)
        {
            Debug.Log($"Item {pair.Key} dropped {pair.Value} times.");
        }
    }
}
