// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Compass"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 11.1
		_Erosion("Erosion", Float) = 0
		_Opacity("Opacity", Float) = 1
		_Color("Color", Color) = (0,0,0,0)
		_ColorA("ColorA", Color) = (0,0,0,0)
		_ColorB("ColorB", Color) = (0,0,0,0)
		_Lerp("Lerp", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_Displacement("Displacement", Float) = 0.5
		_Noise("Noise", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _Displacement;
		uniform sampler2D _Noise;
		uniform sampler2D _Sampler012;
		uniform float4 _Noise_ST;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float _Lerp;
		uniform float4 _Color;
		uniform float _Erosion;
		uniform float _Smoothness;
		uniform float _Opacity;
		uniform float _TessValue;

		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 _Vector0 = float3(0,1,0);
			float4 transform28 = mul(unity_WorldToObject,float4( _Vector0 , 0.0 ));
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 tex2DNode5 = tex2Dlod( _Noise, float4( ( ( ase_screenPosNorm * float4( _Noise_ST.xy, 0.0 , 0.0 ) ) + float4( _Noise_ST.zw, 0.0 , 0.0 ) ).xy, 0, 0.0) );
			v.vertex.xyz += ( _Displacement * transform28 * pow( tex2DNode5 , 2.0 ) ).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float4 tex2DNode5 = tex2D( _Noise, ( ( ase_screenPosNorm * float4( _Noise_ST.xy, 0.0 , 0.0 ) ) + float4( _Noise_ST.zw, 0.0 , 0.0 ) ).xy );
			float4 lerpResult25 = lerp( _ColorA , _ColorB , saturate( ( tex2DNode5.r - _Lerp ) ));
			o.Albedo = lerpResult25.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV1, 0.5 ) );
			o.Emission = ( _Color * fresnelNode1 ).rgb;
			float4 temp_cast_5 = (_Erosion).xxxx;
			float4 temp_cast_6 = (( _Erosion + _Smoothness )).xxxx;
			float4 smoothstepResult14 = smoothstep( temp_cast_5 , temp_cast_6 , tex2DNode5);
			o.Alpha = ( smoothstepResult14 * _Opacity ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
0;96;1758;923;1103.47;430.0534;1.144525;True;True
Node;AmplifyShaderEditor.TextureTransformNode;12;-1351.169,383.3764;Float;False;5;1;0;SAMPLER2D;_Sampler012;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.ScreenPosInputsNode;6;-1346.631,136.6805;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1102.153,195.9761;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-883.7989,61.01932;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;5;-568.2291,133.406;Float;True;Property;_Noise;Noise;13;0;Create;True;0;0;False;0;None;8bbef6cac6d17324fbea7bd02afefe9d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-241.6426,-20.31335;Float;False;Property;_Lerp;Lerp;10;0;Create;True;0;0;False;0;0;-0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-5.744122,227.6511;Float;False;Property;_Erosion;Erosion;5;0;Create;True;0;0;False;0;0;-1.52;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;106.7948,363.3144;Float;False;Property;_Smoothness;Smoothness;11;0;Create;True;0;0;False;0;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;2;-595.6793,510.8468;Float;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;16;296.415,210.6932;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;29;-139.8406,25.69312;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;341.5089,-280.7376;Float;False;Property;_ColorA;ColorA;8;0;Create;True;0;0;False;0;0,0,0,0;1,0.8189512,0.3632073,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;18;-187.1211,180.723;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;23;-87.84816,-579.6628;Float;False;Property;_Color;Color;7;0;Create;True;0;0;False;0;0,0,0,0;0.8018868,0.5623186,0.3290761,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-513.3286,373.5955;Float;False;Property;_Displacement;Displacement;12;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;30;55.47327,20.21704;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;28;-241.3877,669.8591;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;14;404.3291,136.695;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;1;-495.235,-130.5957;Float;False;Standard;WorldNormal;ViewDir;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;685.2103,230.0601;Float;False;Property;_Opacity;Opacity;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;270.9816,-94.43917;Float;False;Property;_ColorB;ColorB;9;0;Create;True;0;0;False;0;0,0,0,0;0.3728639,0.3728639,0.5943396,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;719.2103,126.0601;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;189.9834,-335.0179;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;25;635.5941,-134.3603;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-926.579,265.6195;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;7;-325.393,472.746;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;10;-1155.136,5.919495;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-86.77328,419.0134;Float;False;3;3;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1080.026,-122.9156;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Compass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;11.1;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.1;0.1698113,0.1698113,0.1698113,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;6;0
WireConnection;11;1;12;0
WireConnection;9;0;11;0
WireConnection;9;1;12;1
WireConnection;5;1;9;0
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;29;0;5;1
WireConnection;29;1;31;0
WireConnection;18;0;5;0
WireConnection;30;0;29;0
WireConnection;28;0;2;0
WireConnection;14;0;5;0
WireConnection;14;1;15;0
WireConnection;14;2;16;0
WireConnection;21;0;14;0
WireConnection;21;1;20;0
WireConnection;22;0;23;0
WireConnection;22;1;1;0
WireConnection;25;0;26;0
WireConnection;25;1;27;0
WireConnection;25;2;30;0
WireConnection;13;0;10;0
WireConnection;13;1;12;1
WireConnection;7;0;2;0
WireConnection;3;0;4;0
WireConnection;3;1;28;0
WireConnection;3;2;18;0
WireConnection;0;0;25;0
WireConnection;0;2;22;0
WireConnection;0;9;21;0
WireConnection;0;11;3;0
ASEEND*/
//CHKSM=E19490FF5C09110D923781C5E0F4F16896BCC679