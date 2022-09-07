using System;
using UnityEngine;

[ExecuteAlways]
public class Example : MonoBehaviour
{
    public Material m_ExampleMaterial = null;

    public Texture m_MainTexture = null;

    public float m_ColorR = 1;

    public Vector2 m_ColorGB = Vector2.one;

    private float m_Dissolve = 0;
    
    private void Update()
    {
        if (m_ExampleMaterial == null)
        {
            return;
        }
        
        m_ExampleMaterial.SetTexture("_MainTex" , m_MainTexture);
        m_ExampleMaterial.SetFloat("_ColorR" , m_ColorR);
        m_ExampleMaterial.SetVector("_ColorGB" , m_ColorGB);

        if (m_Dissolve < 1)
        {
            m_Dissolve += 0.001f;
            m_ExampleMaterial.SetFloat("_Dissolve" , m_Dissolve);
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("启动"))
        {
            m_Dissolve = 0;
        }
    }
}
