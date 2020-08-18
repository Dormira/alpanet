Shader "Custom/DistanceSurfaceShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _CloseColor("Closer Color", Color) = (1, 0, 0)
        _FarColor("Farther Color", Color) = (0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert vertex:myVert //fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #pragma enable_d3d11_debug_symbols

        sampler2D _MainTex;
        float3 _CloseColor;
        float3 _FarColor;

        struct Input
        {
            float localDist;
            float2 uv_MainTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


        void myVert(inout appdata_full v, out Input data) {
            UNITY_INITIALIZE_OUTPUT(Input, data);//fancy
            data.localDist = distance(v.vertex.xyz, float3(0, 0, 0));
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            float3 color = (IN.localDist * _FarColor) + ((1 / IN.localDist) * _CloseColor);

            o.Albedo = _CloseColor;
            o.Emission = normalize(color);
        }
        ENDCG
    }

    FallBack "Diffuse"
}
