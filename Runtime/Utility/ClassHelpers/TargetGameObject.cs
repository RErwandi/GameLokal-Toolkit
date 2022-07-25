namespace GameLokal.Toolkit
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
	public class TargetGameObject
	{
		public enum Target
		{
			Player,
            Camera,
			Invoker,
			GameObject
		}

        [Serializable]
        public class ChangeEvent : UnityEvent { }

		// PROPERTIES: ----------------------------------------------------------------------------

        public Target target = Target.GameObject;

        public GameObject gameObject;

        public ChangeEvent eventChangeVariable = new ChangeEvent();

        // INITIALIZERS: --------------------------------------------------------------------------

        public TargetGameObject() { }

        public TargetGameObject(Target target) 
        {
            this.target = target;
        }

		// PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject GetGameObject(GameObject invoker)
		{
            GameObject result = null;

			switch (this.target)
			{
    			case Target.Player :
                    if (HookPlayer.Instance != null) result = HookPlayer.Instance.gameObject;
    				break;

                case Target.Camera:
                    if (HookCamera.Instance != null) result = HookCamera.Instance.gameObject;
                    break;

                    case Target.Invoker:
    				result = invoker;
    				break;
                    
                case Target.GameObject:
                    result = this.gameObject;
    				break;
            }

			return result;
		}

        public Transform GetTransform(GameObject invoker)
        {
            GameObject targetGo = this.GetGameObject(invoker);
            if (targetGo == null) return null;
            return targetGo.transform;
        }

        public T GetComponent<T>(GameObject invoker) where T : UnityEngine.Object
        {
            GameObject targetGo = this.GetGameObject(invoker);
            if (targetGo == null) return null;
            return targetGo.GetComponent<T>();
        }

        public object GetComponent(GameObject invoker, string type)
        {
            GameObject targetGo = this.GetGameObject(invoker);
            if (targetGo == null) return null;
            return targetGo.GetComponent(type);
        }

        public T GetComponentInChildren<T>(GameObject invoker) where T : UnityEngine.Object
        {
            GameObject targetGo = this.GetGameObject(invoker);
            if (targetGo == null) return null;
            return targetGo.GetComponentInChildren<T>();
        }

        public T[] GetComponentsInChildren<T>(GameObject invoker) where T : UnityEngine.Object
        {
            GameObject targetGo = this.GetGameObject(invoker);
            if (targetGo == null) return new T[0];
            return targetGo.GetComponentsInChildren<T>();
        }

        // EVENTS: --------------------------------------------------------------------------------

        private void OnChangeVariable()
        {
            eventChangeVariable.Invoke();
        }

        private void OnChangeVariable(int index, object prev, object next)
        {
            eventChangeVariable.Invoke();
        }

        // UTILITIES: -----------------------------------------------------------------------------

        public override string ToString ()
		{
			string result = "(unknown)";
			switch (target)
			{
    			case Target.Player : result = "Player"; break;
    			case Target.Invoker: result = "Invoker"; break;
                case Target.Camera: result = "Camera"; break;
                case Target.GameObject: 
                    result = gameObject != null ? gameObject.name : "(null)"; 
                    break;
			}

			return result;
		}
	}
}