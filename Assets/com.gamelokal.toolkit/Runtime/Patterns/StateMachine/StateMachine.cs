using UnityEngine;
using System;

namespace GameLokal.Toolkit.Pattern
{
	public struct StateChangeEvent<T> where T: struct, IComparable, IConvertible, IFormattable
	{
		public GameObject target;
		public StateMachine<T> targetStateMachine;
		public T newState;
		public T previousState;

		public StateChangeEvent(StateMachine<T> stateMachine)
		{
			target = stateMachine.target;
			targetStateMachine = stateMachine;
			newState = stateMachine.CurrentState;
			previousState = stateMachine.PreviousState;
		}
	}
	
	public interface IStateMachine
	{
		bool TriggerEvents { get; set; }
	}
	
	public class StateMachine<T> : IStateMachine where T : struct, IComparable, IConvertible, IFormattable
	{
		public GameObject target;
		
		public bool TriggerEvents { get; set; }
		public T CurrentState { get; protected set; }
		public T PreviousState { get; protected set; }

        public delegate void OnStateChangeDelegate();
        public OnStateChangeDelegate onStateChange;
        
        public StateMachine(GameObject target, bool triggerEvents)
		{
			this.target = target;
			this.TriggerEvents = triggerEvents;
		} 
        
		public virtual void ChangeState(T newState)
		{
			// if the "new state" is the current one, we do nothing and exit
			if (newState.Equals(CurrentState))
			{
				return;
			}

			// we store our previous character movement state
			PreviousState = CurrentState;
			CurrentState = newState;

            onStateChange?.Invoke();

            if (TriggerEvents)
			{
				EventManager.TriggerEvent (new StateChangeEvent<T> (this));
			}
		}
		
		public virtual void RestorePreviousState()
		{
			// we restore our previous state
			CurrentState = PreviousState;

            onStateChange?.Invoke();

            if (TriggerEvents)
			{
				EventManager.TriggerEvent (new StateChangeEvent<T> (this));
			}
		}	
	}
}