using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Controls the steps in the in coaching card.
    /// </summary>
    public class CoachingCard_StepManager : MonoBehaviour
    {
        [Serializable]
        class Step
        {
            [SerializeField]
            public GameObject stepObject;
        }

        [SerializeField]
        List<Step> m_StepList = new List<Step>();

        int m_CurrentStepIndex = 0;

        public void Update()
        {
            if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                ReturnToMenu();
            }
        }

        void OnEnable()
        {
            GameEventsManager.instance.inputEvents.onYButtonPressed += ReturnToMenu;
        }


        void OnDisable()
        {
            GameEventsManager.instance.inputEvents.onXButtonPressed -= ReturnToMenu;
        }


        public void Next()
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = (m_CurrentStepIndex + 1) % m_StepList.Count;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }

        public void ReturnToMenu(InputAction.CallbackContext context)
        {
            if (m_CurrentStepIndex != 0)
            {
                //Try and abandon/restart all tasks - must restart as button press is step 0 on all tasks.
                if (GameObject.Find("Tutorial PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Tutorial_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Tutorial_Task");
                }
                if (GameObject.Find("Glove Hygiene PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Glove_Hygiene_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Glove_Hygiene_Task");
                }
                if (GameObject.Find("Glassware Use PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Glassware_Use_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Glassware_Use_Task");
                }
                if (GameObject.Find("Chemical Change PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Chemical_Change_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Chemical_Change_Task");
                }


                //Reset coaching card
                m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
                m_CurrentStepIndex = 0;
                m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
            }
        }

        //This one has no input
        public void ReturnToMenu()
        {
            if (m_CurrentStepIndex != 0)
            {
                //Try and abandon/restart all tasks - must restart as button press is step 0 on all tasks.
                if (GameObject.Find("Tutorial PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Tutorial_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Tutorial_Task");
                }
                if (GameObject.Find("Glove Hygiene PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Glove_Hygiene_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Glove_Hygiene_Task");
                }
                if (GameObject.Find("Glassware Use PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Glassware_Use_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Glassware_Use_Task");
                }
                if (GameObject.Find("Chemical Change PopUp") != null)
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Chemical_Change_Task");
                    GameEventsManager.instance.taskEvents.StartTask("Chemical_Change_Task");
                }


                //Reset coaching card
                m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
                m_CurrentStepIndex = 0;
                m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
            }
        }
    }
}
