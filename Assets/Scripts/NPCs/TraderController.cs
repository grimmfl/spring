using System.Collections;
using Persistence;
using Persistence.Models;
using UnityEngine;

namespace NPCs
{
    public class TraderController : Person
    {
        public int idTrader;
        
        private InventoryUIController _inventoryUI;
        private GameObject _player;

        private Trader _trader;

        private readonly TraderRepository _traderRepository = new TraderRepository();

        protected override void Start()
        {
            _inventoryUI = GameObject.Find("InventoryUI").GetComponent<InventoryUIController>();
            _player = GameObject.FindWithTag("Player");

            _trader = _traderRepository.GetById(idTrader);
            
            base.Start();
        }

        protected override void OnClick()
        {
            StartCoroutine(Trade());
        }

        private IEnumerator Trade()
        {
            yield return new WaitUntil(() => this.IsClose(_player));
            
            Debug.Log($"{_trader.name}: \"Hello. Do you have any goods?\"");
            
            _inventoryUI.StartTrade(_trader);
        }
    }
}