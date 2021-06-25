using UnityEngine;
using UnityEngine.Events;

namespace GameLokal.Toolkit
{
    public class Timer : MonoBehaviour
    {
        public float duration = 5f;
        public bool startOnEnable = false;

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
            timer = duration;
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