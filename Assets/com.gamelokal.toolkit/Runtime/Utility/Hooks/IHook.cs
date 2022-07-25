namespace GameLokal.Toolkit
{
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class IHook<T> : MonoBehaviour 
	{
		private const string ERR_NOCOMP = "Component of type {0} could not be found in object {1}";

        public static IHook<T> Instance;

        // PROPERTIES: -------------------------------------------------------------------------------------------------

        private Dictionary<int, Behaviour> components;

		// INITIALIZERS: -----------------------------------------------------------------------------------------------

		private void Awake()
		{
			Instance = this;
			components = new Dictionary<int, Behaviour>();
		}

        // PUBLIC METHODS: ---------------------------------------------------------------------------------------------

        public TComponent Get<TComponent>() where TComponent : Behaviour
		{
			int componentHash = typeof(TComponent).GetHashCode();
			if (!components.ContainsKey(componentHash))
			{
				Behaviour mono = gameObject.GetComponent<TComponent>();
                if (mono == null) return null;

				components.Add(componentHash, mono);
				return (TComponent)mono;
			}

			return (TComponent)this.components[componentHash];
		}
	}
}
