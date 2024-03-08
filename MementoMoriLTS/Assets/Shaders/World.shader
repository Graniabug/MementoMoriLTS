Shader "MementoMori/World"
{
	Properties
	{
		[Toggle(HARD_OUTLINES_ENABLED)] _HardOutlines("Use Hard Outlines", Float) = 1
		_Amount("Fresnel Amount", Range(0,0.5)) = 0
	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
			Tags {
				"LightMode" = "ForwardBase"
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma shader_feature HARD_OUTLINES_ENABLED

            #include "UnityCG.cginc"

			struct MeshData 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators 
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 wPos : TEXCOORD2;
			};

			float _Amount;

			Interpolators vert(MeshData v) 
			{
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.wPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			float Abs(float val)
			{
				if (val >= 0)
				{
					//initialValue is already not negative, so return itself
					return val;
				}
				else
				{
					return val - (val * 2);
				}
			}

			float percentDifferent(float v1, float v2)
			{
				return Abs(v1 - v2) / ((v1 + v2) / 2);
			}

			float gradient(float normal)
			{

				return lerp(_Amount, (0.5 + _Amount), normal);
				
				if (normal <= (0.5 - _Amount))
				{
					return 0;
				}
				else if (normal >= (0.5 + _Amount))
				{
					return 1;
				}
				else
				{
					return lerp(_Amount, (0.5 + _Amount), normal);
				}
			}

			float4 frag(Interpolators i) : SV_Target
            { 
				float3 N = normalize(i.normal);
				float NdotL = dot(_WorldSpaceLightPos0, N);
				float lightIntensity;

				#if HARD_OUTLINES_ENABLED
					lightIntensity = NdotL > 0 ? 1 : 0;
				#else
					lightIntensity = gradient(NdotL);
				#endif

					return float4(lightIntensity, lightIntensity, lightIntensity, 1);
            }

            ENDCG
        }

		Pass
        {
			Tags {
				"LightMode" = "ForwardAdd"
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma shader_feature HARD_OUTLINES_ENABLED
			#pragma multi_compile DIRECTIONAL POINT

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct MeshData 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators 
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 wPos : TEXCOORD2;
			};

			float _Amount;

			Interpolators vert(MeshData v) 
			{
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.wPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			float Abs(float val)
			{
				if (val >= 0)
				{
					//initialValue is already not negative, so return itself
					return val;
				}
				else
				{
					return val - (val * 2);
				}
			}

			float percentDifferent(float v1, float v2)
			{
				return Abs(v1 - v2) / ((v1 + v2) / 2);
			}

			float gradient(float normal)
			{

				return lerp(_Amount, (0.5 + _Amount), normal);
				
				if (normal <= (0.5 - _Amount))
				{
					return 0;
				}
				else if (normal >= (0.5 + _Amount))
				{
					return 1;
				}
				else
				{
					return lerp(_Amount, (0.5 + _Amount), normal);
				}
			}

			UnityLight CreateLight (Interpolators i) {
				UnityLight light;
				light.dir = normalize(_WorldSpaceLightPos0.xyz - i.wPos);
				UNITY_LIGHT_ATTENUATION(attenuation, 0, i.wPos);
				light.color = _LightColor0.rgb;
				light.ndotl = DotClamped(normalize(i.normal), light.dir);
				return light;
			}

			float4 frag(Interpolators i) : SV_Target
            { 
				UnityLight currentLight = CreateLight(i);
				float3 N = normalize(i.normal);
				//float NdotL = dot(CreateLight(i).dir, N);
				float lightIntensity;

				#if HARD_OUTLINES_ENABLED
					lightIntensity = currentLight.ndotl > 0 ? 1 : 0;
				#else
					lightIntensity = gradient(currentLight.ndotl);
				#endif

					return float4(lightIntensity, lightIntensity, lightIntensity, 1);
            }

            ENDCG
        }
    }
}
