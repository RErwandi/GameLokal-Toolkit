using UnityEngine;
using UnityEngine.Events;

namespace GameLokal.Toolkit
{
    public class Timer : MonoBehaviour
    {
        public bool startOnEnable = false;
        
        public uint hours = 0;
        public uint minutes = 1;
        public uint seconds = 30;
        public uint milliseconds = 0;

        private float timer = 0f;

        public UnityEvent onTimerStart;
        public UnityEvent onTimerFinished;
        public UnityEvent onTimerInterrupt;

        private void OnEnable()
        {
            if (startOnEnable)
            {
                StartTimer();
            }
            else
            {
                timer = 0f;
            }
        }

        private void Update()
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    onTimerFinished?.Invoke();
                }
            }
        }

        public void StartTimer()
        {
            timer = hours * 3600 + minutes * 60 + seconds + milliseconds * 0.001f;
            onTimerStart?.Invoke();
        }

        public void Interrupt()
        {
            if (timer > 0f)
            {
                timer = 0f;
                onTimerInterrupt?.Invoke();
            }
        }
    }
}