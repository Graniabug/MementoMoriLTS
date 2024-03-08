Shader "MementoMori/Character" {
	Properties{

		_Color("Color", Color) = (1,1,1,1)
		_Amount("Fresnel Amount", Range(0,1)) = 1
		[Toggle(HARD_OUTLINES_ENABLED)] _HardOutlines("Use Hard Outlines", Float) = 0
		_RimAmount("Rim Amount", Range(0,1)) = 1
		_BlendHeight("Gradient Amount", Range(0,10)) = 0.5
	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Geometry" }

			Pass { //behind objects
				
				ZWrite Off
				ZTest Always

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma shader_feature HARD_OUTLINES_ENABLED

				#include "UnityCG.cginc"

				struct MeshData {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0; 
				};

				struct Interpolators {
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : TEXCOORD1;
					float3 wPos : TEXCOORD2;
				};

				float _Amount;
				float _HardOutlines;
				float _RimAmount;
				float4 _Color;
				float4 _GradientColor;
				float4 _ColorA;
				float _BlendHeight;

				Interpolators vert(MeshData v) {
					Interpolators o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = UnityObjectToWorldNormal(v.normal);
					o.wPos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				float4 frag(Interpolators i) : SV_Target 
				{

					float3 N = normalize(i.normal);
					float3 V = normalize(_WorldSpaceCameraPos - i.wPos);

					if (_Color.x == 0) 
					{
						_ColorA = (_Color + 1); //black
					}
					else
					{
						_ColorA = (_Color - 1); //white
					}

					_GradientColor = lerp(_ColorA, _Color, (i.wPos.y * _BlendHeight));

					return _GradientColor;	
				}
				ENDCG
			}

			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma shader_feature HARD_OUTLINES_ENABLED

				#include "UnityCG.cginc"

				struct MeshData {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0;
				};

				struct Interpolators {
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : TEXCOORD1;
					float3 wPos : TEXCOORD2;
				};

				float _Amount;
				float _HardOutlines;
				float _RimAmount;
				float4 _Color;

				Interpolators vert(MeshData v) {
					Interpolators o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.normal = UnityObjectToWorldNormal(v.normal);
					o.wPos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				float4 frag(Interpolators i) : SV_Target 
				{
					//float4 color = (_Color, _Color, _Color, 1);

					float3 N = normalize(i.normal);
					float3 V = normalize(_WorldSpaceCameraPos - i.wPos);

					if (_Amount <= 0)
					{
						return _Color;
					}

					float fresnel = _Amount - (dot(V, N));

					#if HARD_OUTLINES_ENABLED
						float rimIntensity = 0;
						if (_Color.x == 0) //black
						{
							rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, fresnel);
						}
						else
						{
							rimIntensity = -smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, fresnel);
					
						}
						return (_Color + rimIntensity);
					#endif

					if (_Color.x == 0) //black
					{
						return (_Color + fresnel); //white fresnel
					}
					else
					{
						return (_Color - fresnel); //black fresnel
					}
					
				}
				ENDCG
			}


		}
}
