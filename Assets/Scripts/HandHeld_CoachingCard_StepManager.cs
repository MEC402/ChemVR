using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Controls the steps in the in coaching card.
    /// </summary>
    public class HandHeld_CoachingCard_StepManager : MonoBehaviour
    {
        [Serializable]
        class Step
        {
            [SerializeField]
            public GameObject stepObject;
        }

        [SerializeField]
        List<Step> m_StepList = new List<Step>();
        [SerializeField]
        GameObject chemicalOverview;
        [SerializeField]
        GameObject glasswareOverview;
        [SerializeField] 
        GameObject gloveOverview;
        [SerializeField] 
        GameObject tutorialOverview;


        int m_CurrentStepIndex = 0;
        Button yesButton;

        

        public void Update()
        {
            //Added for keyboard support
            if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                ShowMenu();
            }
            if (Keyboard.current.xKey.wasReleasedThisFrame)
            {
                HideMenu();
            }
        }

        void OnEnable()
        {
            GameEventsManager.instance.inputEvents.onYButtonPressed += ShowMenu;
            GameEventsManager.instance.inputEvents.onYButtonReleased += HideMenu;
            yesButton = GameObject.Find("VR Movement and Interaction/Complete XR Origin Set Up Variant/XR Origin (XR Rig)/Camera Offset/Left Controller/Left Hand/UI/Spatial Panel Manipulator Model (1)/CoachingCardRoot/Yes Button").GetComponent<Button>();
            yesButton.onClick.AddListener(StartAny);
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = 2;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }

        private void StartAny()
        {
            GameObject current = this.GetComponent<Toggle_Module_All>().current;
            Debug.LogWarning(current.name);
            //Try and abandon/restart all tasks - must restart as button press is step 0 on all tasks.
            if (GameObject.Find("Tutorial PopUp") != null)
            {
                if(!current.name.Equals("Tutorial"))
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Tutorial_Task");
                } else
                {
                    tutorialOverview.GetComponent<Tutorial_Overview>().restart();
                }
            }
            if (GameObject.Find("Glove Hygiene PopUp") != null)
            {
                if (!current.name.Equals("Glove Hygiene"))
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Glove_Hygiene_Task");
                }
                else
                {
                    gloveOverview.GetComponent<Glove_Hygiene_Overview>().restart();
                }
            }
            if (GameObject.Find("Glassware Use PopUp") != null)
            {
                if (!current.name.Equals("Glassware Use"))
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Glassware_Use_Task");
                }
                else
                {
                    glasswareOverview.GetComponent<Glassware_Use_Overview>().restart();
                }
            }
            if (GameObject.Find("Chemical Change PopUp") != null)
            {
                if (!current.name.Equals("Chemical Change"))
                {
                    GameEventsManager.instance.taskEvents.AbandonTask("Chemical_Change_Task");
                }
                else
                {
                    chemicalOverview.GetComponent<Chemical_Change_Overview>().restart();
                }
            }
            this.GetComponent<Toggle_Module_All>().Show();

        }

        void OnDisable()
        {
            GameEventsManager.instance.inputEvents.onYButtonPressed -= ShowMenu;
            GameEventsManager.instance.inputEvents.onYButtonReleased -= HideMenu;
            yesButton.onClick.RemoveListener(StartAny);
        }


        public void Next()
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = (m_CurrentStepIndex + 1) % m_StepList.Count;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }
        public void Back()
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = (m_CurrentStepIndex - 1) % m_StepList.Count;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }

        private void ShowMenu(InputAction.CallbackContext obj)
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = 0;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }

        private void HideMenu(InputAction.CallbackContext obj)
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = 2;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }
        //below added for keyboard support
        private void ShowMenu()
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = 0;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }

        private void HideMenu()
        {
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
            m_CurrentStepIndex = 2;
            m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        }
    }
}
