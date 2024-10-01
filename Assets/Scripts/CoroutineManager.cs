using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    static CoroutineManager m_Instance = null;

    public static CoroutineManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<CoroutineManager>();
                if (m_Instance == null)
                    m_Instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
            }

            return m_Instance;
        }
    }
}