Shader "Hidden/PostProcessing/Desaturate"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
    TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);

    #if SOURCE_GBUFFER
        TEXTURE2D_SAMPLER2D(_CameraGBufferTexture2, sampler_CameraGBufferTexture2);
    #endif

    float _Intensity;

    float3 SampleDepthNormal(float2 uv)
    {
    #if SOURCE_GBUFFER
        // GBUFFER is available in deferred automatically. It is not available in forward.
        float3 normal = SAMPLE_TEXTURE2D(_CameraGBufferTexture2, sampler_CameraGBufferTexture2, uv);
        return mul((float3x3)unity_WorldToCamera, normal);
    #else
        // _CameraDepthNormalsTexture is available in forward or deferred but we need to tell the camera to generate it
        float4 encodedNormal = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, uv);
        return DecodeViewNormalStereo(encodedNormal) * float3(1.0, 1.0, -1.0);
    #endif
    }

    float4 NormalFrag(VaryingsDefault i) : SV_TARGET
    {        
        return float4(SampleDepthNormal(i.texcoord.xy), 1);
    }

    float4 DepthFrag(VaryingsDefault i) : SV_TARGET
    {
        float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord.xy);

        return float4(depth, depth, depth, 1);
    }

    float4 DesaturateFrag(VaryingsDefault i) : SV_TARGET
    {
        float3 pixelColour = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb;

        pixelColour = lerp(pixelColour, dot(pixelColour, float3(0.3, 0.59, 0.11)), _Intensity);

        return float4(pixelColour, 1);
    }

    ENDHLSL

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

            #pragma vertex VertDefault
            #pragma fragment DesaturateFrag

            ENDHLSL
        }
    }
}
