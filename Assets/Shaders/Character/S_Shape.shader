// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shape"
{
	Properties
	{
		_Distortion("Distortion", Float) = 0.2
		[PerRendererData]_ColorA("ColorA", Color) = (1,1,1,0)
		[PerRendererData]_ColorB("ColorB", Color) = (1,1,1,0)
		_Perlin("Perlin", 2D) = "white" {}
		_Lerp("Lerp", Float) = 0
		_Highlight("Highlight", Float) = 0
		_Displace("Displace", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 viewDir;
			float3 worldPos;
			float3 worldNormal;
			float4 vertexColor : COLOR;
			float4 screenPos;
			INTERNAL_DATA
		};

		uniform float _Displace;
		uniform sampler2D _Perlin;
		uniform sampler2D _Sampler045;
		uniform float4 _Perlin_ST;
		uniform float _Highlight;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float _Lerp;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion;
		uniform sampler2D _CameraDepthTexture;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 panner46 = ( 1.0 * _Time.y * _Perlin_ST.zw + ( ase_screenPosNorm * float4( _Perlin_ST.xy, 0.0 , 0.0 ) ).xy);
			float4 tex2DNode40 = tex2Dlod( _Perlin, float4( panner46, 0, 0.0) );
			float4 temp_cast_2 = (0.5).xxxx;
			float4 temp_output_41_0 = ( tex2DNode40 - temp_cast_2 );
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( _Displace * temp_output_41_0 * float4( ase_vertexNormal , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult59 = normalize( ( i.viewDir + ase_worldlightDir ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float dotResult60 = dot( normalizeResult59 , ase_normWorldNormal );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 panner46 = ( 1.0 * _Time.y * _Perlin_ST.zw + ( ase_screenPosNorm * float4( _Perlin_ST.xy, 0.0 , 0.0 ) ).xy);
			float4 tex2DNode40 = tex2D( _Perlin, panner46 );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 normalizeResult28 = normalize( ( ase_vertex3Pos + float3( 0,0.5,0 ) ) );
			float fresnelNdotV26 = dot( normalize( normalizeResult28 ), ase_worldViewDir );
			float fresnelNode26 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV26, 2.0 ) );
			float4 lerpResult33 = lerp( _ColorA , _ColorB , ( tex2DNode40 * saturate( fresnelNode26 ) ));
			float4 temp_cast_2 = (0.5).xxxx;
			float4 temp_output_41_0 = ( tex2DNode40 - temp_cast_2 );
			float4 screenColor37 = tex2D( _GrabTexture, ( ase_screenPosNorm + ( _Distortion * temp_output_41_0 ) ).xy );
			float4 temp_output_48_0 = ( ( ( i.vertexColor * lerpResult33 ) + ( _Lerp * screenColor37 ) ) * 1.0 );
			float screenDepth85 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth85 = saturate( abs( ( screenDepth85 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 0.2 ) ) );
			o.Emission = ( ( step( ( 1.0 - dotResult60 ) , _Highlight ) + temp_output_48_0 ) * saturate( ( distanceDepth85 + 0.4 ) ) ).rgb;
			o.Smoothness = 1.0;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 screenPos : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15900
0;94;1758;887;129.6247;820.6813;1.49999;True;True
Node;AmplifyShaderEditor.TextureTransformNode;45;-192.4617,819.1761;Float;False;40;1;0;SAMPLER2D;_Sampler045;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.ScreenPosInputsNode;38;-305.1475,435.4393;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-20.93059,530.8065;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PosVertexDataNode;27;-624.4248,-96.68362;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;46;264.7471,525.3611;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-340.7636,-84.49514;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0.5,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;40;472.065,493.5859;Float;True;Property;_Perlin;Perlin;3;0;Create;True;0;0;False;0;None;8031679cb224dd346ab13f403adac6dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;781.1007,699.8458;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;28;-101.0168,66.71361;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FresnelNode;26;137.0879,79.26428;Float;False;Standard;WorldNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;885.6745,537.7435;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.5;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;43;1093.148,564.4291;Float;False;Property;_Distortion;Distortion;0;0;Create;True;0;0;False;0;0.2;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;1031.328,309.787;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;58;1946.792,650.1792;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;56;1842.995,420.0838;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;35;392.5812,24.04315;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;538.4679,-9.747658;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;34;517.6943,-245.3181;Float;False;Property;_ColorB;ColorB;2;1;[PerRendererData];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;25;559.3674,-445.9629;Float;False;Property;_ColorA;ColorA;1;1;[PerRendererData];Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;57;2088.792,489.1792;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;1013.732,115.3023;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalizeNode;59;2197.792,520.1792;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;33;897.4498,-249.7339;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;66;952.1682,31.64655;Float;False;Property;_Lerp;Lerp;4;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;92;866.3688,-456.1836;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;61;2289.792,755.1792;Float;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScreenColorNode;37;1168.283,147.6843;Float;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;1117.734,36.04407;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;60;2387.792,614.1792;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;1046.368,-346.6843;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;70;2641.651,347.7187;Float;False;Property;_Highlight;Highlight;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;1296.341,-204.1047;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;85;3449.845,-85.4252;Float;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;72;2677.001,536.6761;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;1318.25,-341.775;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;62;2795.915,326.6822;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.99;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1793.702,-263.1693;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;3739.359,-51.17928;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;3620.492,83.67921;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;90;3901.249,1.746277;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;84;1928.376,1441.825;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;83;1547.063,1298.498;Float;False;Property;_Displace;Displace;6;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;3753.753,243.4843;Float;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;1989.846,1193.431;Float;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;2128.506,-56.26488;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;52;1786.837,135.8932;Float;False;SF_Lighting;-1;;1;e0e44f51f8b410b4a95e500ff225fd18;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;3991.554,104.4841;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4221.397,185.1151;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shape;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.02;1,1,1,0;VertexScale;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;0;38;0
WireConnection;51;1;45;0
WireConnection;46;0;51;0
WireConnection;46;2;45;1
WireConnection;29;0;27;0
WireConnection;40;1;46;0
WireConnection;28;0;29;0
WireConnection;26;0;28;0
WireConnection;41;0;40;0
WireConnection;41;1;44;0
WireConnection;42;0;43;0
WireConnection;42;1;41;0
WireConnection;35;0;26;0
WireConnection;50;0;40;0
WireConnection;50;1;35;0
WireConnection;57;0;56;0
WireConnection;57;1;58;0
WireConnection;39;0;38;0
WireConnection;39;1;42;0
WireConnection;59;0;57;0
WireConnection;33;0;25;0
WireConnection;33;1;34;0
WireConnection;33;2;50;0
WireConnection;37;0;39;0
WireConnection;68;0;66;0
WireConnection;68;1;37;0
WireConnection;60;0;59;0
WireConnection;60;1;61;0
WireConnection;91;0;92;0
WireConnection;91;1;33;0
WireConnection;36;0;91;0
WireConnection;36;1;68;0
WireConnection;72;0;60;0
WireConnection;62;0;72;0
WireConnection;62;1;70;0
WireConnection;48;0;36;0
WireConnection;48;1;49;0
WireConnection;89;0;85;0
WireConnection;64;0;62;0
WireConnection;64;1;48;0
WireConnection;90;0;89;0
WireConnection;82;0;83;0
WireConnection;82;1;41;0
WireConnection;82;2;84;0
WireConnection;53;0;48;0
WireConnection;53;1;52;0
WireConnection;88;0;64;0
WireConnection;88;1;90;0
WireConnection;0;2;88;0
WireConnection;0;4;79;0
WireConnection;0;11;82;0
ASEEND*/
//CHKSM=7C2E402148F676E51AD69BC07079174FA77533FC