using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR; //Stands for mixed reality

namespace BreadAndButter.VR
{
    //Used for switching
    public static class VRUtil
    {
        private static List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();




        public static void SetVREnabled(bool _enabled)
        {
            //Get all the connected XR devices
            SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);

            //Loop through all XR devices
            foreach (XRInputSubsystem subsystem in subsystems)
            {
                //
                if (_enabled)
                {
                    subsystem.Start();
                }
                else
                {
                    subsystem.Stop();
                }
            }
        }

        public static bool IsVREnabled()
        {
            SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);

            //Loop through all XR devices
            foreach (XRInputSubsystem subsystem in subsystems)
            {
                //
                if (subsystem.running)
                {
                    return true;
                }
            }
            return false;
        }
    }
}