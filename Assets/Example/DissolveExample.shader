Shader "Unlit/DissolveExample"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        _Scale("Scale", Float) = 4
        _ColorR ("ColorR", float) = 0
        _Dissolve ("Dissolve", float) = 0
        _ColorGB ("ColorGB", vector) = (0 , 0 , 0 , 0)
        _DissolveTexture("Texture", 2D) = "white" {}
        _Speed("Speed", Float) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry" }

        Cull Off
        ZWrite On
        ZTest LEqual

        Pass
        {
            ZTest Less
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _DissolveTexture;

            float _Speed;
            float _Scale;
            float _ColorR;
            float2 _ColorGB;

            float _Dissolve;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                /*o.vertex.y *= abs(sin(_Time.y));
                o.vertex.x *= abs(sin(_Time.x));*/
                //v.uv = mul(v.uv, float2x2(_Scale, 0, 0, _Scale));
                //o.uv += fixed2(_Speed * _Time.y, 0);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                float dissolve = tex2D(_DissolveTexture , i.uv).r;
                _Dissolve *= 0.2;
                clip(dissolve - _Dissolve);
                
                return col;
            }
            ENDCG
        }
    }
    
    fallback "Universal Render Pipeline/Unlit"
}
