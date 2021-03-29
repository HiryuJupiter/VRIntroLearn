using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Serializable = System.SerializableAttribute;

namespace BreadAndButter.VR
{
    //This is how you create a custom Unity event
    [Serializable]
    public class VRInputEvent : UnityEvent<InputEventArgs> {}

    //This handles button, touch pad, all the input actions
   [Serializable]
    public class InputEventArgs
    {
        //The controller firing the events
        public VRController controller;
        //Input sources
        public SteamVR_Input_Sources sources;
        //The position the player is touching the touchpad on.
        public Vector2 touchpadAxis;

        public InputEventArgs(VRController controller, SteamVR_Input_Sources sources, Vector2 touchpadAxis)
        {
            this.controller = controller;
            this.sources = sources;
            this.touchpadAxis = touchpadAxis;
        }
    }
}