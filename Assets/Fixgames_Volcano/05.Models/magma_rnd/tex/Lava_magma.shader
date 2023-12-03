// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Lava_magma"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.6
		_FireTexture("Fire Texture", 2D) = "white" {}
		_emiss_2_3("emiss_2_3", 2D) = "white" {}
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		_RockSediment_mask3("RockSediment_mask3", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "bump" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float1("Float 1", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		//#pragma target 4.6
		#pragma target 3.5
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample3;
		uniform sampler2D _TextureSample4;
		uniform sampler2D _emiss_2_3;
		uniform float4 _emiss_2_3_ST;
		uniform sampler2D _FireTexture;
		uniform sampler2D _TextureSample0;
		uniform float _Float1;
		uniform sampler2D _RockSediment_mask3;
		uniform float4 _RockSediment_mask3_ST;
		uniform float _Cutoff = 0.6;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 5,5 );
			o.Normal = UnpackNormal( tex2D( _TextureSample3, uv_TexCoord1 ) );
			o.Albedo = tex2D( _TextureSample4, uv_TexCoord1 ).rgb;
			float2 uv_emiss_2_3 = i.uv_texcoord * _emiss_2_3_ST.xy + _emiss_2_3_ST.zw;
			float2 panner2_g2 = ( _Time.x * float2( 0,-2 ) + uv_TexCoord1);
			float4 temp_output_9_0_g2 = ( tex2D( _emiss_2_3, uv_emiss_2_3 ) * tex2D( _FireTexture, panner2_g2 ) );
			o.Emission = temp_output_9_0_g2.rgb;
			o.Specular = tex2D( _TextureSample0, uv_TexCoord1 ).rgb;
			o.Smoothness = _Float1;
			o.Alpha = 1;
			float2 uv_RockSediment_mask3 = i.uv_texcoord * _RockSediment_mask3_ST.xy + _RockSediment_mask3_ST.zw;
			clip( tex2D( _RockSediment_mask3, uv_RockSediment_mask3 ).a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
947.3334;821.3334;880;557;56.01874;494.5914;2.015091;False;False
Node;AmplifyShaderEditor.RangedFloatNode;2;-121.0379,530.718;Float;False;Property;_Float0;Float 0;5;0;Create;True;0;0;False;0;2;5;2;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-489.5278,129.631;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;1353.655,345.9537;Float;True;Property;_RockSediment_mask3;RockSediment_mask3;7;0;Create;True;0;0;False;0;eee1a3e7e1bcad6488092f2d69a2b733;eee1a3e7e1bcad6488092f2d69a2b733;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;465.911,131.6902;Float;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;False;0;None;6618005f6bafebf40b3d09f498401fba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;21;425.0024,397.7849;Float;False;Burn Effect 1;1;;2;248e5bb2ae607b0469073ca9f46356db;0;2;12;FLOAT;5;False;14;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;9;388.571,-346.1689;Float;True;Property;_TextureSample4;Texture Sample 4;6;0;Create;True;0;0;False;0;None;7130c16fd8005b546b111d341310a9a4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;442.4821,-92.45826;Float;True;Property;_TextureSample3;Texture Sample 3;8;0;Create;True;0;0;False;0;None;11f03d9db1a617e40b7ece71f0a84f6f;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;969.3105,436.6428;Float;False;Property;_Float1;Float 1;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1737.037,-197.627;Float;False;True;6;Float;ASEMaterialInspector;0;0;StandardSpecular;Lava_magma;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.6;True;True;0;True;TransparentCutout;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;1;1;0
WireConnection;21;12;2;0
WireConnection;21;14;1;0
WireConnection;9;1;1;0
WireConnection;7;1;1;0
WireConnection;0;0;9;0
WireConnection;0;1;7;0
WireConnection;0;2;21;0
WireConnection;0;3;3;0
WireConnection;0;4;8;0
WireConnection;0;10;10;4
ASEEND*/
//CHKSM=4724E6AC4555D60F1CF99720646DEE452E6836F2