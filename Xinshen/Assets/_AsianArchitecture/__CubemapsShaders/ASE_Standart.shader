// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE/ASE_Standart"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoColor("AlbedoColor", Color) = (1,1,1,1)
		_EmissionMap("EmissionMap", 2D) = "white" {}
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionMultiplayer("EmissionMultiplayer", Float) = 0
		_NormalMap("NormalMap", 2D) = "bump" {}
		[Toggle]_SmoothFromMapSwitch("SmoothFromMapSwitch", Float) = 1
		[Toggle]_EmissionSwitch("EmissionSwitch", Float) = 0
		[Toggle]_SmoothRough("Smooth/Rough", Float) = 0
		_SmoothnessMap("SmoothnessMap", 2D) = "white" {}
		_MetallicMap("MetallicMap", 2D) = "white" {}
		_ParalaxOffset("ParalaxOffset", Float) = 0.001
		_Snoothness("Snoothness", Float) = 1
		_Metallic("Metallic", Float) = 1
		_NormalMapDepth("NormalMapDepth", Float) = 1
		_Tiling("Tiling", Float) = 1
		_MetalicBrightnes("MetalicBrightnes", Range( 0 , 1)) = 0
		_HeightMap("HeightMap", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
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
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			float3 worldNormal;
			float3 worldPos;
		};

		uniform sampler2D _NormalMap;
		uniform float _Tiling;
		uniform sampler2D _HeightMap;
		uniform float _ParalaxOffset;
		uniform float4 _HeightMap_ST;
		uniform float _NormalMapDepth;
		uniform sampler2D _Albedo;
		uniform float4 _AlbedoColor;
		uniform float4 _EmissionColor;
		uniform float _EmissionMultiplayer;
		uniform float _EmissionSwitch;
		uniform sampler2D _EmissionMap;
		uniform float _Metallic;
		uniform sampler2D _MetallicMap;
		uniform float _MetalicBrightnes;
		uniform float _SmoothRough;
		uniform float _SmoothFromMapSwitch;
		uniform sampler2D _SmoothnessMap;
		uniform float _Snoothness;


		inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv, int index )
		{
			float3 result = 0;
			int stepIndex = 0;
			int numSteps = ( int )lerp( (float)maxSamples, (float)minSamples, saturate( dot( normalWorld, viewWorld ) ) );
			float layerHeight = 1.0 / numSteps;
			float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
			uvs.xy += refPlane * plane;
			float2 deltaTex = -plane * layerHeight;
			float2 prevTexOffset = 0;
			float prevRayZ = 1.0f;
			float prevHeight = 0.0f;
			float2 currTexOffset = deltaTex;
			float currRayZ = 1.0f - layerHeight;
			float currHeight = 0.0f;
			float intersection = 0;
			float2 finalTexOffset = 0;
			while ( stepIndex < numSteps + 1 )
			{
			 	currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
			 	if ( currHeight > currRayZ )
			 	{
			 	 	stepIndex = numSteps + 1;
			 	}
			 	else
			 	{
			 	 	stepIndex++;
			 	 	prevTexOffset = currTexOffset;
			 	 	prevRayZ = currRayZ;
			 	 	prevHeight = currHeight;
			 	 	currTexOffset += deltaTex;
			 	 	currRayZ -= layerHeight;
			 	}
			}
			int sectionSteps = 2;
			int sectionIndex = 0;
			float newZ = 0;
			float newHeight = 0;
			while ( sectionIndex < sectionSteps )
			{
			 	intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
			 	finalTexOffset = prevTexOffset + intersection * deltaTex;
			 	newZ = prevRayZ - intersection * layerHeight;
			 	newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
			 	if ( newHeight > newZ )
			 	{
			 	 	currTexOffset = finalTexOffset;
			 	 	currHeight = newHeight;
			 	 	currRayZ = newZ;
			 	 	deltaTex = intersection * deltaTex;
			 	 	layerHeight = intersection * layerHeight;
			 	}
			 	else
			 	{
			 	 	prevTexOffset = finalTexOffset;
			 	 	prevHeight = newHeight;
			 	 	prevRayZ = newZ;
			 	 	deltaTex = ( 1 - intersection ) * deltaTex;
			 	 	layerHeight = ( 1 - intersection ) * layerHeight;
			 	}
			 	sectionIndex++;
			}
			return uvs.xy + finalTexOffset;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord17 = i.uv_texcoord * temp_cast_0;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float2 OffsetPOM98 = POM( _HeightMap, uv_TexCoord17, ddx(uv_TexCoord17), ddy(uv_TexCoord17), ase_worldNormal, ase_worldViewDir, i.viewDir, 8, 8, ( 0.001 * _ParalaxOffset ), 0.5, _HeightMap_ST.xy, float2(0,0), 0 );
			o.Normal = UnpackScaleNormal( tex2D( _NormalMap, OffsetPOM98 ), _NormalMapDepth );
			float4 temp_output_3_0 = ( tex2D( _Albedo, OffsetPOM98 ) * _AlbedoColor );
			o.Albedo = temp_output_3_0.rgb;
			float4 tex2DNode33 = tex2D( _EmissionMap, OffsetPOM98 );
			o.Emission = ( _EmissionColor * ( _EmissionMultiplayer * (( _EmissionSwitch )?( ( temp_output_3_0 * tex2DNode33.a ) ):( tex2DNode33 )) ) ).rgb;
			float4 tex2DNode5 = tex2D( _MetallicMap, OffsetPOM98 );
			float3 linearToGamma105 = LinearToGammaSpace( tex2DNode5.rgb );
			float lerpResult93 = lerp( linearToGamma105.x , 1.0 , _MetalicBrightnes);
			o.Metallic = ( _Metallic * lerpResult93 );
			float3 linearToGamma103 = LinearToGammaSpace( tex2D( _SmoothnessMap, OffsetPOM98 ).rgb );
			o.Smoothness = ( (( _SmoothRough )?( ( 1.0 - (( _SmoothFromMapSwitch )?( linearToGamma103.z ):( tex2DNode5.a )) ) ):( (( _SmoothFromMapSwitch )?( linearToGamma103.z ):( tex2DNode5.a )) )) * _Snoothness );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
Version=18712
72.66667;324.6667;1718;1055;1680.563;1517.095;1.797401;True;False
Node;AmplifyShaderEditor.RangedFloatNode;64;-3545.771,133.1267;Float;False;Constant;_ParalaxDepthCorrection;ParalaxDepthCorrection;20;0;Create;True;0;0;0;False;0;False;0.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3309.308,265.2168;Float;False;Property;_ParalaxOffset;ParalaxOffset;11;0;Create;True;0;0;0;False;0;False;0.001;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2881.67,-1132.751;Float;False;Property;_Tiling;Tiling;15;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-3003.232,200.3235;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;101;-2447.69,-820.036;Float;True;Property;_HeightMap;HeightMap;17;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;19;-2793.595,-491.6602;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-2552.477,-1265.36;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;98;-2003.916,-746.5908;Inherit;False;0;8;False;-1;16;False;-1;2;0.02;0.5;False;1,1;False;0,0;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;6;-1961.9,260.7534;Inherit;True;Property;_SmoothnessMap;SmoothnessMap;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LinearToGammaNode;103;-1611.315,288.9816;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;1;-403.4567,-799.6359;Inherit;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;0;False;0;False;-1;None;393c444aebbd8e046b21ba46a951db6b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-406.1573,-598.7033;Float;False;Property;_AlbedoColor;AlbedoColor;1;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;104;-1383.809,277.0616;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;5;-1775.516,-38.90791;Inherit;True;Property;_MetallicMap;MetallicMap;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-483.3045,-288.8951;Inherit;True;Property;_EmissionMap;EmissionMap;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;0.9208729,-670.8475;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-4.894272,-515.6068;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LinearToGammaNode;105;-1487.305,24.36759;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;2;-1130.743,191.3664;Float;False;Property;_SmoothFromMapSwitch;SmoothFromMapSwitch;6;0;Create;True;0;0;0;False;0;False;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;32;319.2034,-471.3975;Float;False;Property;_EmissionSwitch;EmissionSwitch;7;0;Create;True;0;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;106;-1207.845,1.661224;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;95;-1387.871,-249.1329;Float;False;Property;_MetalicBrightnes;MetalicBrightnes;16;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1389.108,-135.8561;Float;False;Constant;_Float0;Float 0;21;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;274.8227,-694.9818;Float;False;Property;_EmissionMultiplayer;EmissionMultiplayer;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;12;-750.9462,311.9731;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1536.795,-552.9112;Float;False;Property;_NormalMapDepth;NormalMapDepth;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-461.2012,333.9779;Float;False;Property;_Snoothness;Snoothness;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-644.9746,-14.00033;Float;False;Property;_Metallic;Metallic;13;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;93;-879.5681,-29.05231;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;11;-515.8754,163.1064;Float;False;Property;_SmoothRough;Smooth/Rough;8;0;Create;True;0;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;476.2137,-1228.58;Float;False;Property;_EmissionColor;EmissionColor;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;709.4679,-581.6841;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-196.8749,160.8066;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-1234.694,-697.3605;Inherit;True;Property;_NormalMap;NormalMap;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-293.0748,8.706559;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;923.8754,-706.2381;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1165.502,-341.0357;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ASE/ASE_Standart;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;63;0;64;0
WireConnection;63;1;18;0
WireConnection;17;0;67;0
WireConnection;98;0;17;0
WireConnection;98;1;101;0
WireConnection;98;2;63;0
WireConnection;98;3;19;0
WireConnection;6;1;98;0
WireConnection;103;0;6;0
WireConnection;1;1;98;0
WireConnection;104;0;103;0
WireConnection;5;1;98;0
WireConnection;33;1;98;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;34;0;3;0
WireConnection;34;1;33;4
WireConnection;105;0;5;0
WireConnection;2;0;5;4
WireConnection;2;1;104;2
WireConnection;32;0;33;0
WireConnection;32;1;34;0
WireConnection;106;0;105;0
WireConnection;12;0;2;0
WireConnection;93;0;106;0
WireConnection;93;1;97;0
WireConnection;93;2;95;0
WireConnection;11;0;2;0
WireConnection;11;1;12;0
WireConnection;39;0;38;0
WireConnection;39;1;32;0
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;13;1;98;0
WireConnection;13;5;66;0
WireConnection;7;0;8;0
WireConnection;7;1;93;0
WireConnection;37;0;36;0
WireConnection;37;1;39;0
WireConnection;0;0;3;0
WireConnection;0;1;13;0
WireConnection;0;2;37;0
WireConnection;0;3;7;0
WireConnection;0;4;10;0
ASEEND*/
//CHKSM=ACCFA0C20E8A9E3EFA012BA8D0B3A3C7A2B182B6