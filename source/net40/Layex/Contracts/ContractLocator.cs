using System;
using System.Collections.Generic;

namespace Layex.Contracts
{
    public static class ContractLocator
    {
        //public static IEnumerable<IContract> CreateFromInstance(object owner)
        //{
        //    return CreateFromType(owner.GetType());
        //}

        public static IEnumerable<IContract> CreateFromOwnerType(Type contractOwnerType, IDependencyContainer container)
        {
            IEnumerable<EnableContractAttribute> attributes = EnableContractAttribute.GetContractAttributes(contractOwnerType);
            foreach (EnableContractAttribute attribute in attributes)
            {
                Type contractType = attribute.ContractType;
                IContract contract = (IContract)container.Resolve(contractType);
                yield return contract;
            }
        }
    }
}
