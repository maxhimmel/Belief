using UnityEngine;
using Zenject;

namespace MaulGrab.Extensions
{
    public static class ZenjectExtensions
    {
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentChildedTo<TContract>( 
            this ConcreteIdBinderGeneric<TContract> self,
            GameObject root,
            bool includeInactive = true )
		{
            return self.FromMethod( context =>
            {
                var childComponent = root.GetComponentInChildren<TContract>( includeInactive );
                return childComponent;
            } );
		}

        public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromComponentsChildedTo<TContract>(
            this ConcreteIdBinderGeneric<TContract> self,
            GameObject root,
            bool includeInactive = true )
        {
            return self.FromMethodMultiple( context =>
            {
                return root.GetComponentsInChildren<TContract>( includeInactive );
            } );
        }
    }
}
