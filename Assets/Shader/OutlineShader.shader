Shader "Custom/OutlineShader"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        [Toggle] _OutlineEnabled ("Outline Enabled", Float) = 1
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };
        sampler2D _MainTex;
        float _OutlineEnabled;
        fixed4 _OutlineColor;

        void surf (Input IN, inout SurfaceOutput output)
        {
            fixed4 color = tex2D(_MainTex, IN.uv_MainTex);
            if (_OutlineEnabled == 1 && (IN.uv_MainTex.x < 0.05 || IN.uv_MainTex.x > 0.95 || IN.uv_MainTex.y < 0.05 || IN.uv_MainTex.y > 0.95)) {
                output.Albedo = _OutlineColor.rgb;
                output.Alpha = _OutlineColor.a;
            } else {
                output.Albedo = color.rgb;      
                output.Alpha = color.a;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
