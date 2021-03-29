using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace BreadAndButter.VR
{
    [RequireComponent(typeof(SteamVR_Behaviour_Pose))]
    [RequireComponent(typeof(VRControllerInput))]
    public class VRController : MonoBehaviour
    {
        //Actual updating of the contorller's position and movement in worldspace, get peak velocity, 
        private SteamVR_Behaviour_Pose pose; 
        private VRControllerInput input;

        public VRControllerInput Input => input;
        /// <summary>
        /// How fast the controller is moving in worldspace
        /// </summary>
        public Vector3 velocity => pose.GetVelocity();
        /// <summary>
        /// How fast the controller is rotating and in which direction.
        /// </summary>
        public Vector3 AngularVelocity => pose.GetAngularVelocity();

        /// <summary>
        /// Note that SteamVR_Input_Source (holds the source you're getting input from) is different from SteamVR_Input_Sources (holds a variety of sources
        /// </summary>
        public SteamVR_Input_Sources InputSource => pose.inputSource;

        public void Initialise ()
        {
        //Behaviour pose: the postion of hte controller in space
            pose = gameObject.GetComponent<SteamVR_Behaviour_Pose>();
            input = gameObject.GetComponent<VRControllerInput>();

            input.Initialise(this);
        }

    }
}
