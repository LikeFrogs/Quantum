// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/QuantumObject" 
{
	Properties
	{
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_PerlinTex("Perlin Noise Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		Pass
		{
			CGPROGRAM

			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _PerlinTex;

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				//float3 localPosition : TEXCOORD0;
			};

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};


			Interpolators VertexProgram(VertexData v) {
				Interpolators i;
				//i.localPosition = position.xyz;
				i.position = UnityObjectToClipPos(v.position);
				//i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return i;
			}
			
			float4 FragmentProgram(Interpolators i) : SV_TARGET {
				//return float4(i.uv, 1, 1);
				float4 col = _Tint;
				//col.a = tex2D(_PerlinTex, i.uv);
				//return tex2D(_PerlinTex, i.uv) * _Tint;
				col = tex2D(_PerlinTex, i.uv);
				col = col + _Tint;
				col.a = 0.5;
				
				//col = float4(col.r, col.g, col.b, 0.5);



				//float edge = abs(0.5 - i.uv.x )  + abs(0.5 - i.uv.y);
				//float edge = (abs(0.5 - i.uv.x) + 0.5) * (abs(0.5 - i.uv.y) + 0.5f);
				float edge = abs(0.5 - i.uv.x) + abs(0.5 - i.uv.y);
				edge = edge * 2 - 2;



				col = col + float4(edge, edge, edge, 1.0);
				//col = float4(col.r * 0.25, col.g * 0.5, col.b, col.a);
				col.b = col.b + 0.4;
				//col = col + _Tint * 0.3;
				//clip(col.r * col.g * col.b );
				return col;
			}
			
			ENDCG
		}
	}
}
