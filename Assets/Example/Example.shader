Shader "Unlit/Example"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        _ColorR ("ColorR", float) = 0
        _ColorGB ("ColorGB", vector) = (0 , 0 , 0 , 0)
        _ColorA ("ColorA" , float) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+100" }

        Cull Off
        ZWrite On
        ZTest LEqual

        Pass
        {
            //ZTest Less
            
            HLSLPROGRAM
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
            
            float _ColorR;
            float _ColorA;
            float2 _ColorGB;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                col.r *= _ColorR;
                col.gb *= _ColorGB;
                col.a *= _ColorA;
                return col;
            }
            ENDHLSL
        }
    }
    
    fallback "Universal Render Pipeline/Unlit"
}
