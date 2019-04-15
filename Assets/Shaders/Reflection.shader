Shader "GalssBird/Reflection"
{
	Properties
	{
		_MainTint("Diffuse Tint", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BaseColor("BaseColorLeve", Range(0,1)) = 1
		_NormalMap("Normal Map", 2D) = "bump" {}
		_Cubemap("Cubemap", CUBE) = ""{}
		_offset_X("offset_X", Range(-1,1)) = 0.0
		_offset_Y("offset_Y", Range(-1,1)) = 0.0
		_offset_Z("offset_Z", Range(-1,1)) = 0.0
		_ReflAmount("Reflection Amount", Range(0,2)) = 0.5
		_Glossiness("Smoothness", Range(0,1)) = 0.0
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Occlusion("Occlusion", Range(0,2)) = 1.0
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

		samplerCUBE _Cubemap;
	sampler2D _MainTex;
	sampler2D _NormalMap;
	float4 _MainTint;
	half _BaseColor;
	half _ReflAmount;
	float _offset_X;
	float _offset_Z;
	float _offset_Y;
	half _Glossiness;
	half _Metallic;
	half _Occlusion;

	struct Input
	{
		float2 uv_MainTex;
		float2 uv_NormalMap;
		float3 worldRefl;
		INTERNAL_DATA
	};

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		half4 c = tex2D(_MainTex, IN.uv_MainTex);

		float3 normals = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap)).rgb;
		o.Normal = normals;

		float3 offset;
		offset.x = _offset_X;
		offset.y = _offset_Y;
		offset.z = _offset_Z;
		o.Emission = texCUBE(_Cubemap, WorldReflectionVector(IN, o.Normal) + offset).rgb * _ReflAmount;
		o.Albedo = c.rgb * _MainTint * _BaseColor;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Occlusion = _Occlusion;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}