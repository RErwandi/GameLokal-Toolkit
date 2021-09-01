using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLokal.Toolkit
{
    public class MenuExamples : MonoBehaviour
    {
        public void OpenMenu()
        {
            TestMenu.Open();
            TestMenu.onClose.AddListener(OnMenuClosed);
        }

        private void OnMenuClosed()
        {
            Debug.Log("Menu just closed");
            TestMenu.onClose.RemoveListener(OnMenuClosed);
        }
    }
}