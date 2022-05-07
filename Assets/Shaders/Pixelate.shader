Shader "Hidden/Xam/Pixelate"
{
	HLSLINCLUDE
	
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	
	TEXTURE2D_SAMPLER2D( _MainTex, sampler_MainTex );
	float2 _ScreenResolution;
	float2 _CellSize;

	float4 Frag( VaryingsDefault i ) : SV_Target
	{
		float2 uv = i.texcoord;

		_CellSize.x = max( 1, _CellSize.x );
		_CellSize.y = max( 1, _CellSize.y );

		float pixelX = _ScreenResolution.x / _CellSize.x;
		float pixelY = _ScreenResolution.y / _CellSize.y;

		float2 texcoord = float2(floor( pixelX * uv.x ) / pixelX, floor( pixelY * uv.y ) / pixelY);
		return SAMPLE_TEXTURE2D( _MainTex, sampler_MainTex, texcoord );
	}

	ENDHLSL

	SubShader
	{
		Cull Off 
		ZWrite Off 
		ZTest Always

		Pass
		{
			HLSLPROGRAM
		
			#pragma vertex VertDefault
			#pragma fragment Frag
		
			ENDHLSL
		}
	}
}