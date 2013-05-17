using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UEx
{
    /// <summary>
    /// 
    /// </summary>
    public static class MonoBehaviourExtensions
    {
        /// <summary>
        /// disable the specified behaviour if the assertvalue is false, and throw a warning
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="assertValue"></param>
        /// <param name="message"></param>
        public static void Assert(this MonoBehaviour behaviour, bool assertValue, string message = "")
        {
            if (!assertValue)
            {
                Debug.LogWarning("Assert failed. " + message);
                behaviour.enabled = false;
            }
        }
    }
}
