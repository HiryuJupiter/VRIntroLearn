using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadAndButter.VR
{
    public class VRRig : MonoBehaviour
    {
        public static VRRig instance = null;

        [SerializeField] private VRController leftController; //Try making this of type VRController
        [SerializeField] private VRController rightController;
        [SerializeField] private Transform headset;
        [SerializeField] private Transform playerArea;
        public VRController LeftController => leftController;
        public VRController RightController => rightController;
        public Transform Headset => headset;
        public Transform PlayerArea => playerArea;

        #region Mono
        private void Awake()
        {
            SingletonValidation();
        }

        void Start()
        {
            //Validate all the transform ocmponents
            ValidateComponent(LeftController);
            ValidateComponent(RightController);
            ValidateComponent(Headset);
            ValidateComponent(PlayerArea);

            leftController.Initialise();
            rightController.Initialise();
        }
        #endregion

        #region Minor methods
        // This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)
        private void ValidateComponent<T>(T _component) where T : Component
        {
            //Of tje cp,[pmemt os mi;; tjem ;pg pit tje ma,e pf jt epc,[pmemt om am errpr
            if (_component == null)
            {
                Debug.LogError($"Component {nameof(_component)} is null. This has to be set.");
                Debug.LogError($"Component {_component} is null. This has to be set.");

#if UNITY_EDITOR
                //The component was null and we are in the editor so stop the editor from playing.
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        private void SingletonValidation ()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        
        #endregion
    }
}


/*
         private void OnValidate()
        {
            //Check if the set object isn't a VRController, if it isn't, unset it 
            if(leftController != null && leftController.GetComponent<VRController>() == null)
            {
                // THe object set to this variable is not of type VRConytroller
                leftController = null;
                Debug.LogWarning("The objec tyou are trying to set to leftController does not have VrController component on it");
            }

            if (rightController != null && rightController.GetComponent<VRController>() == null)
            {
                // THe object set to this variable is not of type VRConytroller
                rightController = null;
                Debug.LogWarning("The objec tyou are trying to set to rightController does not have VrController component on it");
            }
        }

 */

/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadAndButter.VR
{
    public class VRRig : MonoBehaviour
    {
        public static VRRig instance = null;

        public VRController LeftController => leftController;
        public VRController RightController => rightController;
        public Transform Headset => headset;
        public Transform PlayerArea => playerArea;

        [SerializeField] private VRController leftController; //Try making this of type VRController
        [SerializeField] private VRController rightController;
        [SerializeField] private Transform headset;
        [SerializeField] private Transform playerArea;

        // This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)

        private void ValidateComponent <T> (T _component) where T: Component
        {
            //Of tje cp,[pmemt os mi;; tjem ;pg pit tje ma,e pf jt epc,[pmemt om am errpr
            if (_component == null)
            {
                Debug.LogError($"Component {nameof(_component)} is null. This has to be set.");
                Debug.LogError($"Component {_component} is null. This has to be set.");

#if UNITY_EDITOR
                //The component was null and we are in the editor so stop the editor from playing.
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            //Validate all the transform ocmponents
            ValidateComponent(LeftController);
            ValidateComponent(RightController);
            ValidateComponent(Headset);
            ValidateComponent(PlayerArea);

            //Get the VrContorllerComponents from the relevant controllers
            left = leftController.GetComponent<VRController>();
            right = rightController.GetComponent<VRController>();
        }
    }
}


*/