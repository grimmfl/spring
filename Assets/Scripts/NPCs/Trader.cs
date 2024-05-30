using UnityEngine;

namespace NPCs
{
    public class Trader : Person
    {
        private Inventory _inventory;

        protected override void Start()
        {
            _inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
            base.Start();
        }

        protected override void OnClick()
        {
            Debug.Log("Hello. Do you have any goods?");
            
            _inventory.OpenInventory();
        }
    }
}