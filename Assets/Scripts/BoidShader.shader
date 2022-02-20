
Shader "Instanced/InstancedSurfaceShader" {
    Properties{
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma enable_d3d11_debug_symbols
            // Physically based Standard lighting model
            #pragma surface surf Standard addshadow fullforwardshadows
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:setup

            sampler2D _MainTex;

            struct Input {
                float2 uv_MainTex;
            };

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            struct Boid
            {
                float3 position;
                float3 velocity;
            };
            StructuredBuffer<Boid> g_Boids;
        #endif

            void setup()
            {
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                float3 right = cross(float3(0, 1, 0), g_Boids[unity_InstanceID].velocity);
                float3 up = cross(g_Boids[unity_InstanceID].velocity, right);
                unity_ObjectToWorld._11_21_31_41 = float4(normalize(right), 0);
                unity_ObjectToWorld._12_22_32_42 = float4(normalize(up), 0);
                unity_ObjectToWorld._13_23_33_43 = float4(normalize(g_Boids[unity_InstanceID].velocity), 0);
                unity_ObjectToWorld._14_24_34_44 = float4(g_Boids[unity_InstanceID].position.xyz, 1);

                unity_WorldToObject = unity_ObjectToWorld;
                unity_WorldToObject._14_24_34 *= -1;
                unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;
            #endif
            }

            half _Glossiness;
            half _Metallic;

            void surf(Input IN, inout SurfaceOutputStandard o) {
                o.Albedo = float3(1, 1, 1);
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = 1;
            }
            ENDCG
        }
            FallBack "Diffuse"
}