using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace BreadAndButter.VR.Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class InteractableObject : MonoBehaviour
    {
        public Rigidbody Rigidbody => rigidbody;
        public Collider Collider => collider;
        public Transform AttachPoint => attachPoint;

        [SerializeField] private bool isGrabbable = true;
        [SerializeField] private bool isTouchable = false;
        [SerializeField] private bool isUsable = false;
        [SerializeField] private SteamVR_Input_Sources allowedSource = SteamVR_Input_Sources.Any; //Do we want an item can only be picked up by the right controller.

        [Space]
        [SerializeField, Tooltip("The point on the inneractable object we actually want to grab, if not set, will use origin")]
        private Transform attachPoint;

        [Space]
        public InteractionEvent onGrabbed = new InteractionEvent();
        public InteractionEvent onReleased = new InteractionEvent();
        public InteractionEvent onTouched = new InteractionEvent();
        public InteractionEvent onStopTouching = new InteractionEvent();
        public InteractionEvent onUsed = new InteractionEvent();
        public InteractionEvent onStopUsing = new InteractionEvent();

        private new Collider collider; //Being specific that this override the inherited collider
        private new Rigidbody rigidbody;


        void Start()
        {
            collider = gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                collider = gameObject.AddComponent<BoxCollider>();
                Debug.LogError($"Object {name} does not have a collider, adding BoxCollider", gameObject);
            }

            rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        private InteractEventArgs GenerateArgs(VRController _controller)
            => new InteractEventArgs(_controller, rigidbody, collider);

        public void OnObjectGrabbed (VRController _controller)
        {
            if (isGrabbable && IsInputSourceAllowed(_controller))
            {
                onGrabbed.Invoke(GenerateArgs(_controller));
            }
        }

        public void OnObjectReleased(VRController _controller)
        {
            if (isGrabbable && IsInputSourceAllowed(_controller))
            {
                onReleased.Invoke(GenerateArgs(_controller));
            }
        }

        public void OnObjectTouched(VRController _controller)
        {
            if (isTouchable && IsInputSourceAllowed(_controller))
            {
                onTouched.Invoke(GenerateArgs(_controller));
            }
        }

        public void OnObjectStopTouching(VRController _controller)
        {
            if (isTouchable && IsInputSourceAllowed(_controller))
            {
                onStopTouching.Invoke(GenerateArgs(_controller));
            }
        }

        public void OnObjectUsed(VRController _controller)
        {
            if (isUsable && IsInputSourceAllowed(_controller))
            {
                onUsed.Invoke(GenerateArgs(_controller));
            }
        }

        public void OnObjectStopUsing(VRController _controller)
        {
            if (isUsable && IsInputSourceAllowed(_controller))
            {
                onStopUsing.Invoke(GenerateArgs(_controller));
            }
        }

        bool IsInputSourceAllowed(VRController _controller) => _controller.InputSource == allowedSource || AllowAnyInputSource;
        bool AllowAnyInputSource => allowedSource == SteamVR_Input_Sources.Any;

    }
}