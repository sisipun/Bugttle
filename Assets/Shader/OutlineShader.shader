Shader "Unlit/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle] _OutlineEnabled ("Outline Enabled", Float) = 1
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(1, 10)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Trasparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;

            float _OutlineEnabled;
            fixed4 _OutlineColor;
            float _OutlineWidth;

            static float D = 0.71;
            static float2 _dirs[8] = { 
                float2(1, 0), float2(-1, 0), 
                float2(0, 1), float2(0, -1), 
                float2(D, D), float2(-D, D), 
                float2(D, -D), float2(-D, -D)
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float getNeighborMaxAlpha(float2 uv)
            {
                float result = 0;
                for (int i = 0; i < 8; i++)
                {
                    float2 deltaUv = uv + _dirs[i] * _OutlineWidth * float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
                    result = max(result, tex2D(_MainTex, deltaUv).a);
                }
                return result;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                if (_OutlineEnabled == 1) {
                   col.rgb = lerp(_OutlineColor, col.rgb, col.a);
                   col.a = getNeighborMaxAlpha(i.uv);
                }
                return col;
            }
            ENDCG
        }
    }

    FALLBACK "Diffuse"
}
