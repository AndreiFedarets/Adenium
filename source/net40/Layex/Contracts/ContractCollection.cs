using System;
using System.Collections.Generic;

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

        public void RegisterContract(IContract contract)
        {
            _contracts.Add(contract);
            foreach (object item in _items)
            {
                contract.Register(item);
            }
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

        public void Initialize(object contractOwner)
        {
            if (contractOwner == null)
            {
                return;
            }
            _contracts.Clear();
            _items.Clear();
            Type contractOwnerType = contractOwner.GetType();
            IEnumerable<EnableContractAttribute> attributes = EnableContractAttribute.GetContractAttributes(contractOwnerType);
            foreach (EnableContractAttribute attribute in attributes)
            {
                Type contractType = attribute.ContractType;
                object contractObject = Activator.CreateInstance(contractType);
                IContract contract = (IContract)contractObject;
                RegisterContract(contract);
            }
            RegisterItem(contractOwner);
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
