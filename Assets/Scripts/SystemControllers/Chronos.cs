using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemControllers
{
    public class Chronos : MonoBehaviour
    {
        // Private variable declaration
        // MaxTick refers to the max amount of real-life time that can pass before an in-game tick is experienced.
        private const float MaxTick = 2f;
        private int _tick;
        private float _timer;
        private float _clock;
        
        // Public dispatch delegate declaration
        public static event EventHandler<TickEventDispatcher> OnTick;

        // Declaration of the dispatched tick event object itself
        public class TickEventDispatcher : EventArgs
        {
            public int CurrentTick;
        }

        private void Start()
        {
            _tick = 0;
            _clock = 0;
        }

        public float GetCurrentDuration()
        {
            return _clock;
        }

        private void Update()
        {
            // Add the real-world time experienced between frames
            _timer += Time.deltaTime;
            // Add the timer value to the clock, as this is the same as the time-experienced
            // This is purely an optimisation
            _clock += _timer;
            if (_timer >= MaxTick)
            {
                // Decrement or "reset" the timer
                _timer -= MaxTick;
                // Increment the total ticks experienced
                _tick++;
                // Dispatch an event to update all listeners of the new tick value
                OnTick?.Invoke(this, new TickEventDispatcher
                {
                    CurrentTick = _tick
                });
            }
        }
        
        public int GetCurrentTick()
        {
            return _tick;
        }
    }
}

