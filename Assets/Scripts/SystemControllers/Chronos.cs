using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemControllers
{
    public class Chronos : MonoBehaviour
    {
        private const float max_tick = 2f;
        private int tick;
        private float timer;
        public static event EventHandler<TickEventDispatcher> OnTick;

        public class TickEventDispatcher : EventArgs
        {
            public int currentTick;
        }

        private void Start()
        {
            tick = 0;
        }

        public int GetCurrentTick()
        {
            return tick;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= max_tick)
            {
                timer -= max_tick;
                tick++;
                OnTick?.Invoke(this, new TickEventDispatcher
                {
                    currentTick = tick
                });
            }
        }
    }
}

