using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Serializable = System.SerializableAttribute; 

namespace BreadAndButter.VR.Interaction
{
    //References 3 things: to controller, rb beign interacted with (not the controller), and the controller
    [Serializable]
    public class InteractionEvent : UnityEvent<InteractEventArgs> { }

    [Serializable]
    public class InteractEventArgs
    {
        public VRController controller; //Controll that initiated the interaction event
        public Rigidbody rigidbody; //Rb of the interactable object we are interacting with
        public Collider collider; //Col of the interactable object we are interacting with.

        public InteractEventArgs(VRController _controller, Rigidbody _rigidbody, Collider _collider)
        {
            controller = _controller;
            rigidbody = _rigidbody;
            collider = _collider;
        }
    }
}
