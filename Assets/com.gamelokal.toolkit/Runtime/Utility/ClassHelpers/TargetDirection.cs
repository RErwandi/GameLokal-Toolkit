namespace GameLokal.Toolkit
{
    using UnityEngine;

    [System.Serializable]
    public class TargetDirection
    {
        public enum Target
        {
            Player,
            Camera,
            CurrentDirection,
            Transform,
            Point
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        public Target target = Target.CurrentDirection;
        public Vector3 offset = Vector3.zero;

        public Transform targetTransform;
        public Vector3 targetPoint = Vector3.zero;

        // INITIALIZERS: --------------------------------------------------------------------------

        public TargetDirection()
        { }

        public TargetDirection(Target target)
        {
            this.target = target;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Vector3 GetDirection(GameObject invoker, Space offsetSpace = Space.World)
        {
            Vector3 direction = Vector3.zero;

            switch (this.target)
            {
                case Target.Player:
                    if (HookPlayer.Instance != null)
                    {
                        Vector3 playerPosition = HookPlayer.Instance.transform.position;
                        switch (offsetSpace)
                        {
                            case Space.World:
                                playerPosition += this.offset;
                                break;

                            case Space.Self:
                                playerPosition += HookPlayer.Instance.transform.TransformDirection(this.offset);
                                break;
                        }

                        direction = playerPosition - invoker.transform.position;
                    }
                    break;

                case Target.Camera:
                    if (HookCamera.Instance != null)
                    {
                        Vector3 cameraPosition = HookCamera.Instance.transform.position;
                        switch (offsetSpace)
                        {
                            case Space.World:
                                cameraPosition += this.offset;
                                break;

                            case Space.Self:
                                cameraPosition += HookCamera.Instance.transform.TransformDirection(this.offset);
                                break;
                        }
                        direction = cameraPosition - invoker.transform.position;
                    }
                    break;

                case Target.CurrentDirection:
                    direction = invoker.transform.forward;
                    break;

                case Target.Transform:
                    if (this.targetTransform != null)
                    {
                        Vector3 transformPosition = this.targetTransform.position;
                        switch (offsetSpace)
                        {
                            case Space.World:
                                transformPosition += this.offset;
                                break;

                            case Space.Self:
                                transformPosition += this.targetTransform.TransformDirection(this.offset);
                                break;
                        }
                        direction = transformPosition - invoker.transform.position;
                    }
                    break;

                case Target.Point:
                    direction = this.targetPoint - invoker.transform.position;
                    break;
            }

            return direction.normalized;
        }

        public override string ToString()
        {
            string result = "(unknown)";
            switch (this.target)
            {
                case Target.Player:
                    result = "Player";
                    break;

                case Target.Camera:
                    result = "Camera";
                    break;

                case Target.CurrentDirection:
                    result = "Current Direction";
                    break;

                case Target.Transform:
                    result = (this.targetTransform == null
                        ? "(none)"
                        : this.targetTransform.gameObject.name
                    );
                    break;

                case Target.Point:
                    result = this.targetPoint.ToString();
                    break;
            }

            return result;
        }
    }
}