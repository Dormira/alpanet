Shader "Grass" {
    Properties{
       _MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
       _ScaleX("Scale X", Float) = 1.0
       _ScaleY("Scale Y", Float) = 1.0
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

              // User-specified uniforms            
              uniform sampler2D _MainTex;
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
                 

                 output.pos = mul(UNITY_MATRIX_P, 
                   mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1))
                   + mul(unity_ObjectToWorld, float4(input.vertex.xyz, 0.0))
                   * float4(_ScaleX, _ScaleY, 1.0, 1.0));

                 output.tex = input.tex;

                 return output;
              }

              float4 frag(vertexOutput input) : COLOR
              {
                  float4 rawTexel = tex2D(_MainTex, float2(input.tex));

                  return _Color1 * tex2D(_MainTex, float2(input.tex));
              }

              ENDCG
           }
       }
}