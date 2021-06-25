using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameLokal.Toolkit.Pattern
{
    public class StateMachine : MonoBehaviour
    {
        [ValueDropdown("AvailableStates"), HorizontalGroup("TestState"), ShowInInspector, DisableInEditorMode, PropertyOrder(10)]
        private string testState = "";
        
        [ValueDropdown("AvailableStates"), Required]
        public string defaultState;
        public State[] states = new State[0];
        
        public State CurrentState { get; private set; }
        
        [Button(ButtonSizes.Large), HideInPlayMode]
        private void AddNewState()
        {
            var newState = new GameObject($"State {states.Length}");
            var state = newState.AddComponent<State>();
            newState.transform.parent = transform;
            newState.transform.localPosition = Vector3.zero;
            newState.transform.localRotation = Quaternion.identity;
            newState.transform.localScale = Vector3.one;
            states = states.Concat(new[] { state }).ToArray();

            if (CurrentState == null)
                CurrentState = state;
        }

        [Button(ButtonSizes.Small), HorizontalGroup("TestState"), DisableInEditorMode, PropertyOrder(11)]
        private void ChangeState()
        {
            SetState(testState);
        }

        private void Start()
        {
            foreach (var state in states)
            {
                if(state.gameObject.activeSelf)
                    state.gameObject.SetActive(false);
            }
            
            SetState(defaultState);
        }

        private List<string> AvailableStates
        {
            get
            {
                return states.Select(state => state.StateName).ToList();
            }
        }

        /// <summary>
        /// Change state. this will call OnStateExit on the current state before calling OnStateEnter on the target state.
        /// </summary>
        /// <param name="stateName">Name of the state</param>
        public void SetState(string stateName)
        {
            var newState = states.FirstOrDefault(o => o.StateName == stateName);

            if(newState != null)
            {
                if (CurrentState != null)
                {
                    CurrentState.onStateExit?.Invoke();
                    CurrentState.gameObject.SetActive(false);
                }

                newState.gameObject.SetActive(true);
                CurrentState = newState;
                CurrentState.onStateEnter?.Invoke();
            }
            else
                Debug.Log($"{gameObject.name} : Trying to set unknown state {stateName}");
        }
    }
}

