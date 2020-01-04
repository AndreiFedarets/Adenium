using System;
using System.Collections.Generic;
using System.Linq;

namespace Layex.Contracts
{
    public sealed class ContractCollection : IDisposable
    {
        private readonly List<IContract> _contracts;
        private readonly List<object> _items;

        public ContractCollection()
        {
            _contracts = new List<IContract>();
            _items = new List<object>();
        }

        public bool ContractRegistered(Type contractType)
        {
            return _contracts.Any(x => x.GetType() == contractType);
        }

        public bool RegisterContract(IContract contract)
        {
            if (ContractRegistered(contract.GetType()))
            {
                return false;
            }
            _contracts.Add(contract);
            foreach (object item in _items)
            {
                contract.Register(item);
            }
            return true;
        }

        public void RegisterItem(object item)
        {
            _items.Add(item);
            foreach (IContract contract in _contracts)
            {
                contract.Register(item);
            }
        }

        public void UnregisterItem(object item)
        {
            foreach (IContract contract in _contracts)
            {
                contract.Unregister(item);
            }
            _items.Remove(item);
        }
        
        public void Dispose()
        {
            foreach (IContract contract in _contracts)
            {
                contract.Dispose();
            }
        }
    }
}
