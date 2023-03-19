using UnityEngine;

using VirtualEscapes.Common.Jitter;

namespace VirtualEscapes.Common.JitterExample
{
    /// <summary>
    /// Jitter instantiator.
    /// Demonstrates how to instantiate Jitter from a script at runtime, rather than have it attached to a gameobject/prefab.
    /// </summary>
    public class JitterInstantiator : MonoBehaviour
    {
        private const float SHAKE_DURATION = 2.0f;

        private JitterPosition mJitterPosition;
        private JitterRotation mJitterRotation;
        private JitterScale mJitterScale;

        private float mfShakeTimerStart;
        private bool mbIsPaused;

        void Start()
        {
            mJitterPosition = gameObject.AddComponent<JitterPosition>();
            mJitterPosition.Initialise(1, -1, 1, 0.15f, AxisMasks.X | AxisMasks.Y | AxisMasks.Z, false);

            mJitterRotation = gameObject.AddComponent<JitterRotation>();
            mJitterRotation.Initialise(1, -45, 45, 0.25f, AxisMasks.X | AxisMasks.Y | AxisMasks.Z, false);

            mJitterScale = gameObject.AddComponent<JitterScale>();
            //if initialise is not called then default values will be used

            //but jitter properties can also be set/get individually
            mJitterScale.seed = 12345;
            mJitterScale.RandomizeSeed();
            mJitterScale.frequency = 0.25f;
            mJitterScale.amplitudeRange.start = 0;
            mJitterScale.amplitudeRange.end = 0.5f;
            mJitterScale.axisMask = AxisMasks.X | AxisMasks.Y | AxisMasks.Z;
            mJitterScale.axisLocked = true;

            mfShakeTimerStart = Time.time - SHAKE_DURATION;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //make it shake
                mfShakeTimerStart = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                //toggle pausing of the Jitter
                mbIsPaused = !mbIsPaused;
                Time.timeScale = mbIsPaused ? 0 : 1;
            }

            setMagnitudes(Mathf.Clamp01(1.0f - (Time.time - mfShakeTimerStart) / SHAKE_DURATION));
        }

        private void setMagnitudes(float magnitude)
        {
            mJitterPosition.magnitude = mJitterRotation.magnitude = mJitterScale.magnitude = magnitude;
        }
    }
}