Shader "Custom/TACT_Shader"
{
    Properties 
    {
	    // These Components are set by the terrain system
        [HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}
        [HideInInspector] _Splat3("Layer 3 (A)", 2D) = "grey" {}
        [HideInInspector] _Splat2("Layer 2 (B)", 2D) = "grey" {}
        [HideInInspector] _Splat1("Layer 1 (G)", 2D) = "grey" {}
        [HideInInspector] _Splat0("Layer 0 (R)", 2D) = "grey" {}
		
		_StoneAlbedo("Stone Texture",2D) = "grey" {}
		_Layer1Albedo("Stone Texture",2D) = "grey" {}
		
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" }

        Pass 
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _Control;
            sampler2D _Splat3;
            sampler2D _Splat2;
            sampler2D _Splat1;
            sampler2D _Splat0;

            v2f vert (appdata v) // Vertex Shader
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target { // Fragment Shader
			    return (0,0,0,0);
                //fixed4 c = tex2D (_Control, i.uv);
				//fixed4 map1 = tex2D (_Splat1, i.uv);
				//fixed4 map2 = tex2D (_Splat2, i.uv);
				//fixed4 map3 = tex2D (_Splat3, i.uv);
				
                //fixed4 color_tex = (Control.r * map1 + Control.g * map2 + Control.b * map3);
                //return color_tex;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
