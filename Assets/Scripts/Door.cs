using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using MouseButton = Unity.VisualScripting.MouseButton;

public class Door : MonoBehaviour
{
    public GameObject indoor;
    public GameObject outdoor;

    private Camera _cam;
    private GameObject _player;

    private bool _isIndoor;
    private Guid? _iconId;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        var ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit))
        {
            RemoveIcon();
            return;
        }

        if (hit.transform.gameObject != gameObject)
        {
            RemoveIcon();
            return;
        }

        ShowIcon();
        
        if (!Input.GetMouseButtonDown((int)MouseButton.Left)) return;
        
        StartCoroutine(ToggleDoor());
    }

    private IEnumerator ToggleDoor()
    {
        yield return new WaitUntil(() => this.IsClose(_player));

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

    private IEnumerator GoOutside()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        indoor.SetActive(false);
        outdoor.SetActive(true);
        _isIndoor = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isIndoor) return;
        
        if (!other.gameObject.CompareTag("Player")) return;

        if (outdoor.GetComponent<Renderer>().bounds.Contains(other.transform.position)) return;

        StartCoroutine(GoOutside());
    }

    private void ShowIcon()
    {
        if (_iconId != null) return;
        _iconId = IconDisplayController.AddIcon(Input.mousePosition);
    }

    private void RemoveIcon()
    {
        if (_iconId == null) return;
        
        IconDisplayController.RemoveIcon(_iconId.Value);
        
        _iconId = null;
    }
}