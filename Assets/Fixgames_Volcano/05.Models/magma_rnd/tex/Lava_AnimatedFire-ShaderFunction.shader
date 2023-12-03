// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Lava_AnimatedFire-ShaderFunction"
{
	Properties
	{
		//_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 12.03
		//_TessMin( "Tess Min Distance", Float ) = 20
		//_TessMax( "Tess Max Distance", Float ) = 25
		_Cutoff( "Mask Clip Value", Float ) = 0.6
		_FireTexture("Fire Texture", 2D) = "white" {}
		_emiss_2_3("emiss_2_3", 2D) = "white" {}
		_lava_mask_02_01("lava_mask_02_01", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		_Float1("Float 1", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		BlendOp Add
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		//#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		//#pragma target 4.6
		#pragma target 3.5
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample2;
		uniform sampler2D _emiss_2_3;
		uniform float4 _emiss_2_3_ST;
		uniform sampler2D _FireTexture;
		uniform sampler2D _TextureSample4;
		uniform float _Float1;
		uniform sampler2D _lava_mask_02_01;
		uniform float4 _lava_mask_02_01_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _Cutoff = 0.6;
		/*
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}
		*/

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_TexCoord20 = i.uv_texcoord * float2( 3,3 );
			o.Normal = UnpackNormal( tex2D( _TextureSample0, uv_TexCoord20 ) );
			o.Albedo = tex2D( _TextureSample2, uv_TexCoord20 ).rgb;
			float2 uv_emiss_2_3 = i.uv_texcoord * _emiss_2_3_ST.xy + _emiss_2_3_ST.zw;
			float2 panner2_g1 = ( _Time.x * float2( 0,1 ) + uv_TexCoord20);
			float4 temp_output_9_0_g1 = ( tex2D( _emiss_2_3, uv_emiss_2_3 ) * tex2D( _FireTexture, panner2_g1 ) );
			float4 temp_output_55_0 = temp_output_9_0_g1;
			float4 _Color0 = float4(1,0.9972117,0,0);
			o.Emission = ( temp_output_55_0 * _Color0 ).rgb;
			o.Specular = tex2D( _TextureSample4, uv_TexCoord20 ).rgb;
			o.Smoothness = _Float1;
			float2 uv_lava_mask_02_01 = i.uv_texcoord * _lava_mask_02_01_ST.xy + _lava_mask_02_01_ST.zw;
			o.Alpha = tex2D( _lava_mask_02_01, uv_lava_mask_02_01 ).a;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			clip( tex2D( _TextureSample1, uv_TextureSample1 ).a - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc
		/*tessellate:tessFunction */

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma target 4.6
			#pragma target 3.5
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
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
Version=15401
947.3334;821.3334;880;557;1658.349;490.5959;2.154294;True;False
Node;AmplifyShaderEditor.RangedFloatNode;19;-1829.642,401.9764;Float;False;Property;_Float0;Float 0;10;0;Create;True;0;0;False;0;2;5;2;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-2296.932,112.6894;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;55;-1349.821,352.6743;Float;False;Burn Effect;6;;1;e412e392e3db9574fbf6397db39b4c51;0;2;12;FLOAT;500;False;14;FLOAT2;50,50;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;45;-1027.828,472.8419;Float;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;1,0.9972117,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-1418.833,-363.1105;Float;True;Property;_TextureSample2;Texture Sample 2;13;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-133.6309,534.2148;Float;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;ed6d0af896cbc9a4580cea9cd37a0c6b;a5b0380be60add04a958c5b9629278d2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-550.1702,525.0231;Float;True;Property;_lava_mask_02_01;lava_mask_02_01;11;0;Create;True;0;0;False;0;3002501f3367b9748b18cbd6095e40ff;3002501f3367b9748b18cbd6095e40ff;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;-1364.922,-109.3998;Float;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;False;0;None;11f03d9db1a617e40b7ece71f0a84f6f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-702.0787,171.3149;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-697.3698,387.9928;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;-1341.493,114.7487;Float;True;Property;_TextureSample4;Texture Sample 4;15;0;Create;True;0;0;False;0;None;6618005f6bafebf40b3d09f498401fba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-345.4676,368.3405;Float;False;Property;_Float1;Float 1;16;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;158.059,-298.8566;Float;False;True;6;Float;ASEMaterialInspector;0;0;StandardSpecular;Lava_AnimatedFire-ShaderFunction;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.6;True;True;0;True;TransparentCutout;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;12.03;20;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;5;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;55;12;19;0
WireConnection;55;14;20;0
WireConnection;24;1;20;0
WireConnection;21;1;20;0
WireConnection;54;0;55;0
WireConnection;54;1;45;0
WireConnection;44;0;55;0
WireConnection;44;1;45;0
WireConnection;26;1;20;0
WireConnection;0;0;24;0
WireConnection;0;1;21;0
WireConnection;0;2;44;0
WireConnection;0;3;26;0
WireConnection;0;4;27;0
WireConnection;0;9;25;4
WireConnection;0;10;22;4
ASEEND*/
//CHKSM=173263A274FA86CCEE4DA92709C68A60AEA4A811