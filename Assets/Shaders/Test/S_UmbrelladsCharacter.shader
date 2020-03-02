Shader "Umbrellads/Character"
{
    Properties
    {
		[Header(Color)]
        /*[PerRendererData]*/ _Color ("Color", Color) = (1,1,1,1)
        [NoScaleOffset]_MainTex ("ID Tex", 2D) = "white" {}
		[PerRendererData] _GradientTex ("Gradient Tex", 2D) = "white" {}
		[PerRendererData] _GradientY("Gradient Y", Range(0,1)) = 0

		[Header(Shading)]
		_ShadowColor("Shadow Color", Color) = (0.2, 0.2, 0.2, 1)
		_SharpnessShadow ("Sharpness Shadow", Range(0,1)) = 0.5
		_FresnelShadow ("Fresnel Shadow", Range(0,1)) = 0.5

		[Header(Shadow Outline)]
		_ShadowOutlineWidth ("Shadow Outline Width", Range(0,1)) = 0.5
		_ShadowOutlineSmoothness ("Shadow Outline Smoothness", Range(0,1)) = 0.5
		_ShadowOutlineColor("Shadow Outline Color", Color) = (0,0,0,0)

		[Header(Smoothness)]
		_Smoothness ("Smoothness ", Range(0,1)) = 0.5
		_SmoothnessSharp ("Smoothness Sharp", Range(0,1)) = 0.5

		[Header(SSS)]
		[HDR]_SSSColor("SSS Color", Color) = (1,1,1,1)
		_SSSSharpness ("SSS Sharpness", Range(-1,1)) = 0.5
		_SSSSThreshold("SSS Threshold", Range(0,1)) = 0.5

		[Header(Rim Light)]
		[HDR] _RimLightColor("Rim Light color", Color) = (1,1,1,1)
		_RimLightThreshold("Rim Light Threshold", Range(0,1)) = 0.5
		_RimLightSharpness("Rim Light Sharpness", Range(0,1)) = 0.5

		[Header(Fresnel)]
		[Toggle(USE_FRESNEL)] _UseFresnel("UseFresnel", float) = 0
		[HDR]_FresnelColor("Fresnel Color", Color) = (0,0,0,0)
		_FresnelPower("Fresnel Power", float) = 0
		_FresnelStrength("Fresnel Strength", float) = 0

		[Header(Face)]
		[Toggle(USE_FACE)] _UseFace("UseFace", float) = 0
		_RemoveBackFace("Remove Back Face", Range(-1,1)) = 0.5
		_FaceOffsetX("Face Offset X", Range(-1,1)) = 1.25
		_FaceOffsetY("Face Offset Y", Range(0,0.2)) = 0.1
		_LineSharpness("Line Sharpness", Range(0,0.005)) = 0

		[Header(Eyes)]
		[PerRendererData] _EyeSample("Eye Sample Amount",  Range(1, 10)) = 3
		[PerRendererData] _EyeColor("Eye Color", Color) = (0,0,0,1)
		[PerRendererData] _EyeSize("Eye Size", Range(0, 4)) = 0.2
		[PerRendererData] _EyeHeightOffset("Eye Height Offset", Range(0, 4)) = 0
		[PerRendererData] _EyeHeight("Eye Height", Range(0, 4)) = 0
		[PerRendererData] _EyeDistance("Eye Distance", Range(0,0.2)) = 0.5
		[PerRendererData] _EyeOffsetX("Eye Sample Offset X", Range(-0.04,0.04)) = 1
		[PerRendererData] _EyeOffsetY("Eye Sample Offset Y", Range(-0.04,0.04)) = 0
		[PerRendererData] _EyeCurve("Eye Curve", Range(-0.02, 0.02)) = 0
		
		[Header(Eyebrows)]
		[PerRendererData] _EyebrowSample("Eyebrow Sample Amount", Range(1, 10)) = 3
		[PerRendererData] _EyebrowColor("Eyebrow Color", Color) = (0,0,0,1)
		 _EyebrowSize("Eyebrow Size", Range(0, 4)) = 0.1
		[PerRendererData] _EyebrowHeight("Eyebrow Height", Range(-0.1, 0.1)) = 0
		[PerRendererData] _EyebrowDistance("Eyebrow Distance", Range(-0.2, 0.2)) = 0.01
		[PerRendererData] _EyebrowOffsetX("Eyebrow Sample Offset X", Range(-0.04,0.04)) = 0.001
		[PerRendererData] _EyebrowOffsetY("Eyebrow Sample Offset Y", Range(-0.02,0.02)) = 0
		[PerRendererData] _EyebrowCurve("Eyebrow Curve", Range(-0.02, 0.02)) = 0

		[Header(Mouth)]
		[PerRendererData] _MouthSample("Mouth Sample", Range(0, 5)) = 2
		[PerRendererData] _MouthSize("Mouth Size", Range(0, 0.05)) = 0.01
		[PerRendererData] _MouthHeight("Mouth Height", Range(-0.2, 0.2)) = 0
		[PerRendererData] _MouthCurve("Mouth Curve", Range(-0.02, 0.02)) = 0
		[PerRendererData] _MouthOffsetX("Mouth Sample Offset X", Range(-0.04,0.04)) = 1
		[PerRendererData] _MouthOffsetY("Mouth Sample Offset X", Range(-0.04,0.04)) = 1		
		[PerRendererData] _MouthColor("Mouth Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off

        CGPROGRAM
		#include "UnityPBSLighting.cginc"
        #pragma surface surf Toon fullforwardshadows vertex:vert

        #pragma target 5.0
		#pragma shader_feature_local USE_FACE
		#pragma shader_feature_local USE_FRESNEL
			
        sampler2D _MainTex;
        sampler2D _GradientTex;
        fixed4 _Color;
        fixed4 _ShadowColor;
        fixed4 _SSSColor;
        fixed4 _RimLightColor;

		float _GradientY;
		float _SharpnessShadow;
		float _Smoothness;
		float _SmoothnessSharp;
		float _FresnelShadow;

		float _SSSSharpness;
		float _SSSSThreshold;

		float _RimLightThreshold;
		float _RimLightSharpness;

		float _ShadowOutlineWidth;
		float _ShadowOutlineSmoothness;
		fixed4 _ShadowOutlineColor;

		fixed _RandomGradient[30];

		float _FaceOffsetX;
		float _FaceOffsetY;
		float _RemoveBackFace;
		float _LineSharpness;

		fixed4 _EyeColor;
		float _EyeSize;
		float _EyeHeight;
		float _EyeHeightOffset;
		float _EyeDistance;
		int _EyeSample;
		float _EyeOffsetX;
		float _EyeOffsetY;
		float _EyeCurve;

		fixed4 _EyebrowColor;
		float _EyebrowSize;
		float _EyebrowHeight;
		float _EyebrowDistance;
		int _EyebrowSample;
		float _EyebrowOffsetX;
		float _EyebrowOffsetY;
		float _EyebrowCurve;

		float _MouthSize;
		int _MouthSample;
		float _MouthHeight;
		float _MouthCurve;
		float _MouthOffsetX;
		float _MouthOffsetY;
		fixed4 _MouthColor;

		fixed4 _FresnelColor;
		float _FresnelPower;
		float _FresnelStrength;

		float _UseFace;
		float _UseFresnel;
		
        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
			float3 localPos;
			float3 worldNormal;
			float4 color : COLOR;
        };

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
		}

		float4 LightingToon(SurfaceOutputStandard s, half3 lightDir, half3 viewDir, half atten)
		{
			// SPECULAR
			half3 h = normalize(lightDir + viewDir);
			float nDotL = dot(normalize(s.Normal), normalize(lightDir));
			half diff = max(0, nDotL);
			float nh = max(0, dot(normalize(s.Normal), h));
			float spec = pow(nh, lerp(0.2, 100, _Smoothness)) * smoothstep(-0.2, 1, _Smoothness);
			float smoothSpec = smoothstep(_SmoothnessSharp / 2, 1 - _SmoothnessSharp / 2, spec);
			float fresnel = dot(normalize(s.Normal), normalize(viewDir));

			//SHADING
			float remappedDp = (nDotL + 1) / 2;
			float sharpness = _SharpnessShadow / 2;
			float fresnelShadow = saturate(fresnel + (1 - _FresnelShadow));
			float shadow = smoothstep(sharpness, 1 - sharpness, remappedDp * atten) * fresnelShadow;

			//OUTLINE SHADOW
			float shadowOutline = smoothstep(_ShadowOutlineWidth, _ShadowOutlineSmoothness, remappedDp * atten);
			
			float outl = (1 - shadowOutline) * shadow;

			float4 col;
			float3 shadowCol = _ShadowColor * s.Albedo * _LightColor0.rgb;
			float3 shadowAndOutline = lerp(shadowCol, _ShadowOutlineColor , outl) * atten;
			float3 litCol = (s.Albedo * _LightColor0.rgb * _Color + smoothSpec * _LightColor0.rgb);
			col.rgb = lerp(shadowAndOutline, litCol, 1 - saturate((1 - shadow) + outl));
			col.a = 1;
			//return 1;
			return col;
		}
		
		float2 faceOffset()
		{
			return float2(_FaceOffsetX, _FaceOffsetY);
		}

		float2 xyPos(Input IN)
		{
			return IN.uv_MainTex;
			return float2 (IN.localPos.x, IN.localPos.y);
		}
		
		float drawLine(Input IN, float sampleAmnt, float2 dist, float2 offset, float size, float2 constantOffset, float curveStrength)
		{		
			float initLine = 1;
			for (int i = 0; i < sampleAmnt - 1 ; i++)
			{
				float2 offsetSample = lerp(0, offset, (i / (sampleAmnt - 1)));

				float evaluate = (i / (sampleAmnt - 1)) * 3.1415;
				float curve = log(sin(evaluate));
				float2 distCompare =  faceOffset() + dist + offsetSample + constantOffset + float2(0, curve * curveStrength);
				float newLine = distance(xyPos(IN), distCompare);
				float sharp = smoothstep(size, size + _LineSharpness, newLine);
				initLine *= sharp;
			}

			return initLine;
		}

		float drawPoint(Input IN, float2 dist, float size, float2 constantOffset, float curve)
		{
			return drawLine(IN, 2, dist, 0, size, constantOffset, curve);
		}

		float eye(int mul, Input IN)
		{
			float eyes = 1;
			float2 eyeDist = float2(_EyeDistance, 0) * mul;
			float eyeSize = _EyeSize / 100;
			float2 offset = float2(_EyeOffsetX * mul, _EyeOffsetY);
			
			eyes = drawLine(IN, _EyeSample, eyeDist, offset, eyeSize, float2(0,_EyeHeight + _EyeHeightOffset), _EyeCurve);
			return eyes;
		}

		float eyebrow(int mul, Input IN)
		{
			float eyebrows = 1;
			float2 eyeDist = float2(_EyeDistance + _EyebrowDistance, 0) * mul;
			float size = _EyebrowSize / 100;
			float2 offset = float2(_EyebrowOffsetX * mul, _EyebrowOffsetY);

			eyebrows = drawLine(IN, _EyebrowSample, eyeDist, offset, size, float2(0, _EyebrowHeight + _EyeHeight), _EyebrowCurve);
			return eyebrows;
		}

		float eyes(Input IN)
		{
			float left = eye(-1, IN);
			float right = eye(1, IN);
			float eyes = left * right;

			return eyes;
		}

		float eyebrows(Input IN)
		{
			float leftEyebrow = eyebrow(-1, IN);
			float rightEyebrow = eyebrow(1, IN);
			float eyebrows = leftEyebrow * rightEyebrow;
			return eyebrows;
		}

		float mouth(Input IN)
		{
			float2 offset = float2(_MouthOffsetX, _MouthOffsetY);
			float m = drawLine(IN, _MouthSample, 0, offset, _MouthSize, float2(-_MouthOffsetX / 2, _MouthHeight), _MouthCurve);
			return m;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;
            fixed4 c = tex2D (_MainTex, uv);

			//RANDOM TEXTURE
			fixed greyscale = c.r;

			float2 gCoords = float2(greyscale, _GradientY);
            fixed4 g = tex2D (_GradientTex, gCoords);


			float3 light = normalize(_WorldSpaceLightPos0);
			float3 view = normalize(IN.viewDir);
			float dotViewLight = 1 - ((dot(light, view) + 1) / 2);

			///SSS
			float facingLightThreshold = smoothstep(_SSSSThreshold, 1.0, dotViewLight);
			float fresnel = dot(normalize(IN.worldNormal), view);
			float fresnelSSS = ((fresnel + 1 ) / 2) * facingLightThreshold;
			float smoothstepSSS = smoothstep(_SSSSharpness / 2, 1 - _SSSSharpness / 2, fresnelSSS);


			///RIM SSS
			float rim = dotViewLight * (1 - fresnel);
			float rimSharp = smoothstep(_RimLightThreshold, _RimLightThreshold + (1 - _RimLightSharpness), rim);
			fixed4 rimColor = facingLightThreshold * rimSharp * _RimLightColor;

			//SECONDARY FRESNEL
			#if USE_FRESNEL
				float powFresnel = pow(1 - fresnel, _FresnelPower);
				fixed4 secondaryFresnel = powFresnel * _FresnelColor;
				rimColor += secondaryFresnel * _FresnelStrength;
			#endif

			float removeBf = 1 - step(_RemoveBackFace, IN.localPos.z);

			fixed4 applySSS = lerp(g, _SSSColor, smoothstepSSS);
			fixed4 applyFace = applySSS;
			#if USE_FACE
				fixed4 applyEye = lerp(_EyeColor, applySSS, saturate(eyes(IN) + removeBf));
				fixed4 applyEyebrow = lerp(_EyebrowColor, applyEye, saturate(eyebrows(IN) + removeBf));
				fixed4 applyMouth = lerp(_MouthColor, applyEyebrow, saturate(mouth(IN) + removeBf));
				applyFace = applyMouth;
			#endif
						
			o.Albedo = applyFace /* _Color*/ * IN.color;
			//o.Albedo = IN.color;
			o.Occlusion = 0.0;
			o.Emission = rimColor;
			o.Metallic = 1;
			o.Smoothness = _Smoothness;
			clip(c.a - 0.5);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
