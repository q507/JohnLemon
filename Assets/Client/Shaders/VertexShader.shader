Shader "Unlit/VertexShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _item("item", Range(0, 1)) = 1
        _Color("Coloe(RGBA)", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _item;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                _item = abs(sin(_Time.y));
                o.vertex.y += o.vertex.y * _item * 0.05;
                o.vertex.x += o.vertex.x * _item * 0.05;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                _Color.g += abs(sin(_Time.y)) * 0.2;
                return col * _Color;
            }
            ENDCG
        }
    }
}
