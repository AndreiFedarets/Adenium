using System;
using System.Collections.Generic;
using System.Linq;

namespace Layex.Contracts
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class EnableContractAttribute : Attribute, IEquatable<EnableContractAttribute>
    {
        public EnableContractAttribute(Type contractType)
        {
            if (contractType == null)
            {
                throw new ArgumentNullException(nameof(contractType));
            }
            ContractType = contractType;
        }

        public Type ContractType { get; private set; }

        internal static IEnumerable<EnableContractAttribute> GetContractAttributes(Type type)
        {
            List<EnableContractAttribute> contractAttributes = new List<EnableContractAttribute>();
            GetContractAttributes(type, contractAttributes);
            return contractAttributes;
        }

        public override bool Equals(object other)
        {
            return Equals(other as EnableContractAttribute);
        }

        public bool Equals(EnableContractAttribute other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return ContractType.Equals(other.ContractType);
        }

        private static void GetContractAttributes(Type type, List<EnableContractAttribute> contractAttributes)
        {
            if (type == null)
            {
                return;
            }
            object[] attributeObjects = type.GetCustomAttributes(typeof(EnableContractAttribute), true);
            foreach (EnableContractAttribute contractAttribute in attributeObjects.Select(x => (EnableContractAttribute)x))
            {
                if (!contractAttributes.Contains(contractAttribute))
                {
                    contractAttributes.Add(contractAttribute);
                }
            }
            GetContractAttributes(type.BaseType, contractAttributes);
            foreach (Type interfaceType in type.GetInterfaces())
            {
                GetContractAttributes(interfaceType, contractAttributes);
            }
        }
    }
}
