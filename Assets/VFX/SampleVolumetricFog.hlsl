#ifndef SAMPLE_VOLUMETRIC_FOG
#define SAMPLE_VOLUMETRIC_FOG

#ifndef UNITY_DECLARE_OPAQUE_TEXTURE_INCLUDED
TEXTURE2D(_CameraOpaqueTexture);
SAMPLER(sampler_CameraOpaqueTexture);
float3 SampleSceneColor_SVF(float2 uv) {
	return SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv).rgb;
}
#else
float3 SampleSceneColor_SVF(float2 uv) {
	return SampleSceneColor(uv);
}
#endif

#ifndef UNITY_DECLARE_DEPTH_TEXTURE_INCLUDED
TEXTURE2D_FLOAT(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);
float SampleSceneDepth_SVF(float2 uv)
{
    return SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
}
#else
float SampleSceneDepth_SVF(float2 uv)
{
    return SampleSceneDepth(uv);
}
#endif

half GetScreenSpaceAmbientOcclusion_SVF(float2 normalizedScreenSpaceUV){
    #if defined(_SCREEN_SPACE_OCCLUSION) && !defined(_SURFACE_TYPE_TRANSPARENT)
    return SampleAmbientOcclusion(normalizedScreenSpaceUV);
	#else
    return 1.0;
    #endif
}

#if !defined(SHADERGRAPH_PREVIEW)
half3 SubtractDirectMainLightFromLightmap_SVF(half shadowAttenuation, half3 bakedGI) {
    half3 estimatedLightContributionMaskedByInverseOfShadow = _MainLightColor.rgb * (1.0 - shadowAttenuation);
	half3 subtractedLightmap = bakedGI - estimatedLightContributionMaskedByInverseOfShadow;

	half3 realtimeShadow = max(subtractedLightmap, _SubtractiveShadowColor.xyz);
    realtimeShadow = lerp(bakedGI, realtimeShadow, _MainLightShadowParams.x);
	
    return min(bakedGI, realtimeShadow);
}

half MainLightRealtimeShadow_SVF(float4 shadowCoord) {
	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	return SampleShadowmap(TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowCoord, shadowSamplingData, _MainLightShadowParams, false);
}

half MainLightShadow_SVF(float4 shadowCoord, float3 positionWS, half4 shadowMask, half4 occlusionProbeChannels) {
    half realtimeShadow = MainLightRealtimeShadow_SVF(shadowCoord);

#ifdef CALCULATE_BAKED_SHADOWS
    half bakedShadow = BakedShadow(shadowMask, occlusionProbeChannels);
#else
    half bakedShadow = half(1.0);
#endif

    half shadowFade = GetMainLightShadowFade(positionWS);

    return MixRealtimeAndBakedShadows(realtimeShadow, bakedShadow, shadowFade);
}

int GetAdditionalLightsCount_SVF() {
    // TODO: we need to expose in SRP api an ability for the pipeline cap the amount of lights
    // in the culling. This way we could do the loop branch with an uniform
    // This would be helpful to support baking exceeding lights in SH as well
    return int(min(_AdditionalLightsCount.x, unity_LightData.y));
}

half4 CalcAdditionalLight_SVF(uint lightIndex, float3 worldPos, float bias) {
#if USE_FORWARD_PLUS
    int perObjectLightIndex = lightIndex;
#else
    int perObjectLightIndex = GetPerObjectLightIndex(lightIndex);
#endif

#if USE_STRUCTURED_BUFFER_FOR_LIGHT_DATA
	float4 lightPositionWS = _AdditionalLightsBuffer[perObjectLightIndex].position;
	half3 color = _AdditionalLightsBuffer[perObjectLightIndex].color.rgb;
    half4 distanceAndSpotAttenuation = _AdditionalLightsBuffer[perObjectLightIndex].attenuation;
    half4 spotDirection = _AdditionalLightsBuffer[perObjectLightIndex].spotDirection;
    half4 occlusionProbeChannels = _AdditionalLightsBuffer[perObjectLightIndex].occlusionProbeChannels;
#else
    float4 lightPositionWS = _AdditionalLightsPosition[perObjectLightIndex];
    half3 color = _AdditionalLightsColor[perObjectLightIndex].rgb;
    half4 distanceAndSpotAttenuation = _AdditionalLightsAttenuation[perObjectLightIndex];
    half4 spotDirection = _AdditionalLightsSpotDir[perObjectLightIndex];
    half4 occlusionProbeChannels = _AdditionalLightsOcclusionProbes[perObjectLightIndex];
#endif

	float3 lightVector = lightPositionWS.xyz - worldPos * lightPositionWS.w;
	float distanceSqr = max(dot(lightVector, lightVector), HALF_MIN);

    half3 lightDirection = half3(lightVector * rsqrt(distanceSqr));

    float attenuation = DistanceAttenuation(distanceSqr, distanceAndSpotAttenuation.xy) * AngleAttenuation(spotDirection.xyz, lightDirection, distanceAndSpotAttenuation.zw);

	attenuation *= AdditionalLightShadow(perObjectLightIndex, worldPos + lightDirection * bias, lightDirection, half4(1.0, 1.0, 1.0, 1.0), occlusionProbeChannels);

#if defined(_LIGHT_COOKIES)
    real3 cookieColor = SampleAdditionalLightCookie(perObjectLightIndex, worldPos);
    color *= cookieColor;
#endif

	return half4(color, attenuation);
}

#endif

half4 LightingAtWorldPos_SVF(half3 baseColor, float3 worldPos, float bias) {
#if defined(SHADERGRAPH_PREVIEW)
	return half4(baseColor, 1.0);
#else
	const half4 shadowMask = half4(1.0, 1.0, 1.0, 1.0);

	float2 normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(TransformWorldToHClip(worldPos));

	float3 biasedWorldPos = worldPos + _MainLightPosition.xyz * bias;
	half attenuation = MainLightShadow_SVF(TransformWorldToShadowCoord(biasedWorldPos), biasedWorldPos, shadowMask, _MainLightOcclusionProbes);
	half directAmbientOcclusion = lerp(half(1.0), GetScreenSpaceAmbientOcclusion_SVF(normalizedScreenSpaceUV), _AmbientOcclusionParam.w);

	half3 color = baseColor * SubtractDirectMainLightFromLightmap_SVF(attenuation, half3(1.0, 1.0, 1.0));

	color += baseColor * attenuation * (_MainLightColor.rgb * directAmbientOcclusion);

    uint lightCount = GetAdditionalLightsCount();

#if USE_FORWARD_PLUS
	for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
    {
        FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK

		half4 light = CalcAdditionalLight_SVF(lightIndex, worldPos, bias);
        color += baseColor * light.rgb * light.a;
		attenuation += light.a;
    }

	{
		uint lightIndex;
		ClusterIterator _urp_internal_clusterIterator = ClusterInit(normalizedScreenSpaceUV, worldPos, 0);
		[loop] while (ClusterNext(_urp_internal_clusterIterator, lightIndex)) {
			lightIndex += URP_FP_DIRECTIONAL_LIGHTS_COUNT;
			FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK

#elif !_USE_WEBGL1_LIGHTS
    for (uint lightIndex = 0u; lightIndex < lightCount; ++lightIndex) {

#else
    // WebGL 1 doesn't support variable for loop conditions
    for (int lightIndex = 0; lightIndex < _WEBGL1_MAX_LIGHTS; ++lightIndex) {
        if (lightIndex >= (int)lightCount) break;

#endif

		half4 light = CalcAdditionalLight_SVF(lightIndex, worldPos, bias);
        color += baseColor * light.rgb * light.a;
		attenuation += light.a;

#if USE_FORWARD_PLUS
		}
#endif
	}

	return half4(color, attenuation);
#endif
}

void SampleVolumetricFog_half(float2 uv, float fogDensity, half3 fogColor, out half4 output) {
	output = half4(0.0, 0.0, 0.0, 0.0);//SampleSceneColor_SVF(uv);
	
	float depth = SampleSceneDepth_SVF(uv);

	float3 sampleWorldPos = ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);

	const half depthStart = 2.5;

	float3 cameraRayDir = normalize(sampleWorldPos - _WorldSpaceCameraPos);
	float3 cameraRayStart = ComputeWorldSpacePosition(uv, 1.0, UNITY_MATRIX_I_VP) + cameraRayDir;


	half sceneDepth;
	if (unity_OrthoParams.w == 1.0)
		sceneDepth = LinearEyeDepth(sampleWorldPos, UNITY_MATRIX_V);
	else
		sceneDepth = LinearEyeDepth(depth, _ZBufferParams);

	sceneDepth = min(sceneDepth, 10.0);

	const uint steps = 128;

	float stepSize = sceneDepth / steps;
	half stepAlpha = saturate(fogDensity * stepSize);

	half attenuation = 1.0;

	for (uint i = 1; i < steps; i++) {
		float stepDepth = sceneDepth - stepSize * i;
		half4 lighting;

		if(stepDepth < depthStart) {
			lighting = half4(0.5, 0.5, 0.5, 0.5);
		} else {
			float3 worldPos = cameraRayStart + (cameraRayDir * stepDepth);
			lighting = LightingAtWorldPos_SVF(fogColor, worldPos, 0.0);
		}
		
		attenuation = (1.0 - stepAlpha) * attenuation + stepAlpha * lighting.a;
		output = (1.0 - stepAlpha) * output + stepAlpha * half4(lighting.rgb, 1.0);
	}

	attenuation = saturate(attenuation);
	output.rgb = output.rgb * attenuation * attenuation * attenuation;
}

void LightSprite_half(half4 color, float3 worldPos, out half3 outputColor, out float alpha) {
	// color = color * 0.70710678118654752440084436210485;

	half4 lighting = LightingAtWorldPos_SVF(color.rgb, worldPos, 0.03);

	outputColor = lighting.rgb
#if defined(SHADERGRAPH_PREVIEW)
	;
#else
	+ color.rgb * GetScreenSpaceAmbientOcclusion_SVF(GetNormalizedScreenSpaceUV(TransformWorldToHClip(worldPos)));
#endif

	alpha = color.a;
}

#endif // SAMPLE_VOLUMETRIC_FOG