// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Grass" {
    Properties{
       _WindTex("Wind Texture", 2D) = "white" {}
       _ScaleX("Scale X", Float) = 1
       _ScaleY("Scale Y", Float) = 1
       _Color1("First Color", Color) = (1,1,1,1)
    }
        SubShader{
           Blend SrcAlpha OneMinusSrcAlpha
           Pass {
              CGPROGRAM

              #pragma vertex vert  
              #pragma fragment frag
              #pragma multi_compile_instancing
              #include "UnityCG.cginc"

           // https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33
           // Matrix equivalent of rotating angle amount around axis
           // Made it return a 4x4
           float4x4 AngleAxis4x4(float angle, float3 axis)
       {
           float c, s;
           sincos(angle, s, c);

           float t = 1 - c;
           float x = axis.x;
           float y = axis.y;
           float z = axis.z;

           return float4x4(
               t * x * x + c, t * x * y - s * z, t * x * z + s * y, 0,
               t * x * y + s * z, t * y * y + c, t * y * z - s * x, 0,
               t * x * z - s * y, t * y * z + s * x, t * z * z + c, 0,
               0, 0, 0, 1
               );
       }

       // User-specified uniforms            
       uniform sampler2D _WindTex;
       uniform float _ScaleX;
       uniform float _ScaleY;
       uniform float4 _Color1;

       struct vertexInput {
          float4 vertex : POSITION;
          float2 tex : TEXCOORD0;
          UNITY_VERTEX_INPUT_INSTANCE_ID
       };
       struct vertexOutput {
          float4 pos : SV_POSITION;
          float2 tex : TEXCOORD0;
       };

       //This block defines the set of properties that are UNIQUE to each blade of grass
       UNITY_INSTANCING_BUFFER_START(Props)
           //UNITY_DEFINE_INSTANCED_PROP(float4, _Color) 
       UNITY_INSTANCING_BUFFER_END(Props)

       vertexOutput vert(vertexInput input)
       {
          UNITY_SETUP_INSTANCE_ID(input);
          vertexOutput output;

          //Second get the xyz location of the grass's root
          float3 objectXYZ = float3(unity_ObjectToWorld[0][3], unity_ObjectToWorld[1][3], unity_ObjectToWorld[2][3]);
          //Get the x and y position of our uv map
          float windX = (objectXYZ.x + objectXYZ.z) % 128;
          float windY = (objectXYZ.y + objectXYZ.z + _Time.y/8) % 128;

          float2 windStrength = ((tex2Dlod(_WindTex, float4(windX, windY, 0, 0)).xy)*2-1)/2;
          float3 windDirection = normalize(float3(windStrength.x, windStrength.y, 0));
          float4x4 windRotation = AngleAxis4x4(UNITY_PI * windStrength, windDirection);

          /*
          //Billboarding, I don't use it here but I like having it around. You know, for the future.
          float4 scale = float4(_ScaleX, _ScaleY, 1.0, 1.0);
          float4 position = mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1));
          float4 shearScaleRotate = mul(unity_ObjectToWorld, float4(input.vertex.xyz, 0.0)*scale);
          */

          float4 scale = float4(_ScaleX, _ScaleY, 1.0, 1.0);
          float4 scaledPosition = input.vertex * scale;
          float4x4 rotation = unity_ObjectToWorld;
          rotation[0][3] = 0;
          rotation[1][3] = 0;
          rotation[2][3] = 0;

          rotation = mul(rotation, windRotation);

          output.pos = mul(UNITY_MATRIX_VP, mul(rotation, scaledPosition) + float4(objectXYZ, 0));

          output.tex = input.tex;

          return output;
       }

       float4 frag(vertexOutput input) : COLOR
       {
           //float4 rawTexel = tex2D(_MainTex, float2(input.tex));

           return _Color1;// * tex2D(_MainTex, float2(input.tex));
       }

       ENDCG
    }
       }
}