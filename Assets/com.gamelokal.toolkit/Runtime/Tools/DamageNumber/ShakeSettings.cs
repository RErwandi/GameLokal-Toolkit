﻿using UnityEngine;

namespace GameLokal.Toolkit
{
    [System.Serializable]
    public struct ShakeSettings
    {
        public ShakeSettings(Vector2 customDefault)
        {
            offset = customDefault;
            frequency = 50f;
        }

        [Tooltip("Moves back and fourth from negative offset to positive offset.")]
        public Vector2 offset;
        [Tooltip("Changes the speed at which the number moves back and fourth.\nUsed in a sinus function.")]
        public float frequency;
    }

}
