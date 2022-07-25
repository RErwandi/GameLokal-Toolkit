using Sirenix.OdinInspector;
using UnityEngine;

namespace GameLokal.Toolkit
{
    public class CharacterFeatureTest : MonoBehaviour
    {
        private Character character;
        private CharacterAnimator animator;
        private CharacterAttachments attachments;
        
        public enum AttachAction
        {
            Attach,
            Detach,
            Remove
        }
        
        public enum TrackState
        {
            TrackTarget,
            Untrack
        }
        
        public enum HandAction
        {
            Reach,
            LetGo
        }
        
        public enum RagdollOperation
        {
            Ragdoll,
            Recover
        }

        [Header("Attachments")]
        public GameObject attachObject;
        public HumanBodyBones bone = HumanBodyBones.RightHand;
        public AttachAction attachAction;
        public Space space = Space.Self;
        public Vector3 attachPosition;
        public Vector3 attachRotation;

        [Header("Head Tracking")]
        public GameObject trackTarget;
        public TrackState trackState;
        public float trackSpeed = 0.5f;

        [Header("Hand IK")]
        public CharacterHandIK.Limb hand = CharacterHandIK.Limb.LeftHand;
        public GameObject reachTarget;
        public HandAction handAction;
        public float handDuration = 0.5f;

        [Header("Ragdoll")]
        public RagdollOperation ragdollOperation;

        [Header("Gesture")]
        public AnimationClip clip;
        public AvatarMask avatarMask;
        public float gestureSpeed = 1f;
        public float fadeIn = 0.1f;
        public float fadeOut = 0.1f;

        [Button(ButtonSizes.Large)]
        public void Attach()
        {
            character = GetComponent<Character>();
            animator = character.GetCharacterAnimator();
            attachments = animator.GetCharacterAttachments();
            
            switch (attachAction)
            {
                case AttachAction.Attach:
                    attachments.Attach(bone, attachObject, attachPosition, Quaternion.Euler(attachRotation), space);
                    break;
                case AttachAction.Detach:
                    attachments.Detach(bone);
                    break;
                case AttachAction.Remove:
                    attachments.Remove(bone);
                    break;
            }
        }

        [Button(ButtonSizes.Large)]
        public void HeadTracking()
        {
            character = GetComponent<Character>();
            if (character != null)
            {
                CharacterHeadTrack headTrack = character.GetHeadTracker();
                if (headTrack != null)
                {
                    switch (this.trackState)
                    {
                        case TrackState.TrackTarget:
                            headTrack.Track(trackTarget.transform, trackSpeed);
                            break;

                        case TrackState.Untrack:
                            headTrack.Untrack();
                            break;
                    }
                }
            }
        }

        [Button(ButtonSizes.Large)]
        public void Ragdoll()
        {
            character = GetComponent<Character>();
            switch (ragdollOperation)
            {
                case RagdollOperation.Ragdoll:
                    character.SetRagdoll(true, false);
                    break;
                case RagdollOperation.Recover:
                    character.SetRagdoll(false);
                    break;
            }
        }

        [Button(ButtonSizes.Large)]
        public void Hand()
        {
            character = GetComponent<Character>();
            animator = character.GetCharacterAnimator();
            var handIK = animator.GetCharacterHandIK();
            
            switch (handAction)
            {
                case HandAction.Reach:
                    handIK.Reach(hand, reachTarget.transform, handDuration);
                    break;
                
                case HandAction.LetGo:
                    handIK.LetGo(hand, handDuration);
                    break;
            }
        }

        [Button(ButtonSizes.Large)]
        public void Gesture()
        {
            character = GetComponent<Character>();
            animator = character.GetCharacterAnimator();

            animator.CrossFadeGesture(clip, gestureSpeed, avatarMask, fadeIn, fadeOut);
        }
    }
}