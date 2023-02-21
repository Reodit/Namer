Shader "Custom/2PassAlpha"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MetallicGlossMap ("Metallic", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        zwrite on
        ColorMask 0

        CGPROGRAM
        #pragma surface surf nolight noambient noforwardadd nolightmap novertexlights noshadow

        #pragma target 3.0

        struct Input
        {
            float4 color:COLOR;
            fixed3 Normal;
            fixed3 Emission;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
        }

        float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten){
            return float4(0, 0, 0, 0);
        }
        ENDCG

        // 2nd pass zwrite off, rendering on
        zwrite off

        CGPROGRAM
        #pragma surface surf Lambert keepalpha alpha:fade

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            fixed3 Normal;
            fixed3 Emission;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG

    }
    FallBack "Diffuse"
}