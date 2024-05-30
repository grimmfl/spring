using Unity.VisualScripting;
using UnityEngine;

namespace NPCs
{
    public class Person : MonoBehaviour
    {
        private Camera _cam;
    
        // Start is called before the first frame update
        protected virtual void Start()
        {
            _cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Input.GetMouseButtonDown((int)MouseButton.Left)) return;

            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            // find raycast hit
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity)) return;

            // if this has been clicked
            if (hit.transform.gameObject != gameObject) return;
        
            OnClick();
        }

        protected virtual void OnClick()
        {
            Debug.Log("Person: Hallo");
        }
    }
}
