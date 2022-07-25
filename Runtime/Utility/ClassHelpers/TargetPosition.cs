namespace GameLokal.Toolkit
{
	using UnityEngine;

	[System.Serializable]
	public class TargetPosition
	{
		public enum Target
		{
			Player,
            Camera,
			Invoker,
			Transform,
			Position
		}

		// PROPERTIES: ----------------------------------------------------------------------------

		public Target target = Target.Position;
		public Vector3 offset = Vector3.zero;

        public Transform targetTransform;
        public Vector3 targetPosition = Vector3.zero;

        // INITIALIZERS: --------------------------------------------------------------------------

        public TargetPosition() 
        { }

        public TargetPosition(Target target)
        {
            this.target = target;
        }

		// PUBLIC METHODS: ------------------------------------------------------------------------

        public Vector3 GetPosition(GameObject invoker, Space offsetSpace = Space.World)
		{
			Vector3 resultPosition = Vector3.zero;
            Vector3 resultOffset = Vector3.zero;

			switch (this.target)
			{
    			case Target.Player :
    				if (HookPlayer.Instance != null)
    				{
                        resultPosition = HookPlayer.Instance.transform.position;
                        switch (offsetSpace)
                        {
                            case Space.World: 
                                resultOffset = this.offset; 
                                break;

                            case Space.Self: 
                                resultOffset = HookPlayer.Instance.transform.TransformDirection(this.offset);
                                break;
                        }
                    }
    				break;

                case Target.Camera:
                    if (HookCamera.Instance != null)
                    {
                        resultPosition = HookCamera.Instance.transform.position;
                        switch (offsetSpace)
                        {
                            case Space.World:
                                resultOffset = this.offset;
                                break;

                            case Space.Self:
                                resultOffset = HookCamera.Instance.transform.TransformDirection(this.offset);
                                break;
                        }
                    }
                    break;

                case Target.Invoker:
                    resultPosition = invoker.transform.position;
                    resultOffset = this.offset;
                    break;

    			case Target.Transform:
    				if (this.targetTransform != null)
    				{
                        if (this.targetTransform != null)
                        {
                            resultPosition = this.targetTransform.position;
                            switch (offsetSpace)
                            {
                                case Space.World:
                                    resultOffset = this.offset;
                                    break;

                                case Space.Self:
                                    resultOffset = this.targetTransform.TransformDirection(this.offset);
                                    break;
                            }
                        }
    				}
    				break;

    			case Target.Position:
                    resultPosition = this.targetPosition;
                    resultOffset = Vector3.zero;
    				break;
			}

			return resultPosition + resultOffset;
		}

        public Quaternion GetRotation(GameObject invoker)
		{
			Quaternion rotation = invoker.transform.rotation;
			switch (this.target)
			{
    			case Target.Player :
    				if (HookPlayer.Instance != null) rotation = HookPlayer.Instance.transform.rotation;
    				break;

    			case Target.Transform:
                    if (this.targetTransform != null) rotation = this.targetTransform.rotation;
    				break;
			}

			return rotation;
		}

		public override string ToString()
		{
			string result = "(unknown)";
			switch (this.target)
			{
			    case Target.Player : 
                    result = "Player"; 
                    break;

			    case Target.Invoker: 
                    result = "Invoker"; 
                    break;

                case Target.Transform: 
                    result = (this.targetTransform == null 
                        ? "(none)" 
                        : this.targetTransform.gameObject.name
                    ); 
                    break;

			    case Target.Position:
                    result = this.targetPosition.ToString(); 
                    break;
			}

			return result;
		}
	}
}