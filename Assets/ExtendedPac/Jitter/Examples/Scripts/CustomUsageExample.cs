using UnityEngine;

using VirtualEscapes.Common.Jitter;

namespace VirtualEscapes.Common.JitterExample
{
    /// <summary>
    /// Jitter instantiator.
    /// Demonstrates how to instantiate Jitter from a script at runtime, rather than have it attached to a gameobject/prefab.
    /// </summary>
    public class CustomUsageExample : MonoBehaviour
    {
        private JitterCustom jitter => GetComponent<JitterCustom>();

        void Start()
        {

        }

        void Update()
        {
            float value = jitter.jitterValue;

            transform.localPosition = new Vector3(0, value, 0);
            transform.localScale = new Vector3(1, 1, value * 4);
        }

    }
}