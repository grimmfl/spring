using System;
using System.Collections;
using SQLite4Unity3d;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject indoor;
    public GameObject outdoor;

    private Camera _cam;
    private GameObject _player;

    private bool _isIndoor;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown((int)MouseButton.Left)) return;

        var ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit)) return;

        if (hit.transform.gameObject != gameObject) return;

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

        if (other.transform.position.IsInObject(outdoor)) return;
        
        StartCoroutine(GoOutside());
    }
}