﻿Shader "BalloonShaderCG"{
	//show values to edit in inspector
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		_SphereThickness("Sphere Thickness", float) = 1.0
		_ParticleSize("Particle Size", float) = 1.0
	}

	SubShader{
		//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
		Tags{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha
		//ZWrite Off
		Cull Off

		Pass{
			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader functions
			#pragma vertex vert
			#pragma fragment frag

			//texture and transforms of the texture
			sampler2D _MainTex;
			float4 _MainTex_ST;

			//tint of the texture
			fixed4 _Color;

			float _SphereThickness;
			float _ParticleSize;

			//the mesh data thats read by the vertex shader
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			//the data thats passed from the vertex to the fragment shader and interpolated by the rasterizer
			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			//the vertex shader function
			v2f vert(appdata v) {
				v2f o;
				//convert the vertex positions from object space to clip space so they can be rendered correctly
				o.position = UnityObjectToClipPos(v.vertex);
				//apply the texture transforms to the UV coordinates and pass them to the v2f struct
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			//the fragment shader function
			fixed4 frag(v2f i) : SV_TARGET{
				//read the texture color at the uv coordinate
				fixed4 col = tex2D(_MainTex, i.uv);
				//multiply the texture color and tint color
				//col *= _Color;
				//col *= i.color;

				float dist = length(i.uv - float2 (0.5f, 0.5f)); //get the distance form the center of the point-sprite
				float alpha = saturate(sign(0.5f - dist));
				float sphereDepth = cos(dist * 3.14159) * _SphereThickness * _ParticleSize; //calculate how thick the sphere should be

				float depth = saturate(sphereDepth + i.color.w); //input.color.w represents the depth value of the pixel on the point-sprite
				col *= float4 (depth.xxx, alpha); //or anything else you might need in future passes

				//return the final color to be drawn on screen
				return col;
			}

			ENDCG
		}
	}
	Fallback "VertexLit"
}
