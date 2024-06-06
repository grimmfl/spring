using Persistence;
using Persistence.Models;
using UnityEngine;

namespace NPCs
{
    public class TraderController : Person
    {
        public int idTrader;
        private InventoryUIController _inventoryUI;

        private Trader _trader;

        private readonly TraderRepository _traderRepository = new TraderRepository();

        protected override void Start()
        {
            _inventoryUI = GameObject.Find("UserInterface").GetComponent<InventoryUIController>();

            _trader = _traderRepository.GetById(idTrader);
            
            base.Start();
        }

        protected override void OnClick()
        {
            Debug.Log($"{_trader.name}: \"Hello. Do you have any goods?\"");
            
            _inventoryUI.StartTrade(_trader);
        }
    }
}