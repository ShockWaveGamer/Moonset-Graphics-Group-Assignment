Shader "Custom/Palette"
{
    Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _MainTex;
			static const float3 PALETTE_RGB[70] =
{
float3(23,	32,	56),
float3(37,	58,	94),
float3(60,	94,	139),
float3(79,	143,	186),
float3(115,	190,	211),
float3(164,	221,	219),

float3(25, 51,	45),
float3(37,	86,	46),
float3(70,	130,	50),
float3(117,	167,	67),
float3(168,	202,	88),
float3(208,	218,	145),
float3(77,	43,	50),
float3(122,	72,	65),

float3(173,	119,	87),
float3(192,	148,	115),
float3(215,	181,	148),
float3(231,	213,	179),
float3(52,	28,	39),
float3(96,	44,	44),
float3(136,	75,	43),
float3(190,	119,	43),

float3(222,	158,	65),
float3(232,	193,	112),
float3(36,	21,	39),
float3(65,	29,	49),
float3(117,	36,	56),
float3(165,	48,	48),
float3(207,	87,	60),
float3(218,	134,	62),

float3(30,	29,	57),
float3(64,	39,	81),
float3(122,	54,	123),
float3(162,	62,	140),
float3(198,	81,	151),
float3(223,	132,	165),
float3(9,	10,	20),
float3(16,	20,	31),

float3(21,	29,	40),
float3(32,	46,	55),
float3(57,	74,	80),
float3(87,	114,	119),
float3(129,	151,	150),
float3(168,	181,	178),
float3(199,	207,	204),
float3(235,	237,	233),

float3(254,	224,	15), // Promise Yellow | ffe01a
float3(222,	242,	246), // Moonglow Blue | def2f6
float3(10,	13,	22), // Pelagic Blue | 0a0d16
float3(212,	235,	220), // Catalytic Green | d3eede
float3(222,	219,	220), // Etherfill Pink | de7778
float3(179,	214,	118), // Holograss Green | b3d676

float3(52, 49, 28), //Dark Brown | 34311c
float3(105, 98, 80), //Light Brown | 696250
float3(180, 150, 108), //Brown | b4966c
float3(227, 198, 154), //Beige | e3c69a
float3(220, 213, 214), //Snow Blue | dcd5d6
float3(194, 206, 197), //Light Clay | c2cec5
float3(150, 156, 167), //Smooth Blue | 969ca7
float3(75, 86, 39), //Camo Green | 4b5627
float3(84, 83, 58), //Puke Green | 54533a
float3(102, 120, 47), //Leaf Green | 66782f
float3(126, 140, 71), //Happy Green | 7e8c47
float3(143, 161, 62), //Bright Green | 8fa13e
float3(148, 150, 149), //Gray Blue | 949695
float3(134, 153, 167), //Sea Bue | 8699a7
float3(64, 96, 101), //Dark Teal | 406065
float3(86, 87, 88), //Gray | 565758
float3(154, 141, 109), //Beige Brown | 9a8d6d

float3(0,0,0)//black
};
			static const float3 PALETTE[16] =
            {
                float3(0, 0, 0),
                float3(256, 256, 256),
                float3(0, 255, 255),
                float3(255, 255, 0),
                float3(255, 0, 255),
                float3(0, 0, 255),
                float3(0, 255, 0),
                float3(255, 0, 0),
                float3(30, 30, 30),
                float3(100, 100, 100),

                float3(254, 224, 15), // Promise Yellow | ffe01a
                float3(222, 242, 246), // Moonglow Blue | def2f6
                float3(10, 13, 22), // Pelagic Blue | 0a0d16
                float3(212, 235, 220), // Catalytic Green | d3eede
                float3(222, 219, 220), // Etherfill Pink | de7778
                float3(179, 214, 118), // Holograss Green | b3d676
            };
			
            v2f vert (appdata v) // Vertex Shader
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target { // Fragment Shader
                fixed4 color_tex = tex2D(_MainTex, i.uv);
				// compare color_tex to the Palette using manhattan distance and return the closest color
				fixed4 color_out = fixed4(0,0,0,1);
				float min_dist = 1000000; // very large number
				int best_color = 0;
				for (int i = 0; i < 69; i++)
				{
				    float dist = abs(color_tex.r - PALETTE_RGB[i].r/256) + abs(color_tex.g - PALETTE_RGB[i].g/256) + abs(color_tex.b - PALETTE_RGB[i].b/256);
				    if (dist < min_dist)
				    {
				        min_dist = dist;
						best_color = i;
					}
                }
				color_out = fixed4(PALETTE_RGB[best_color].r/256,PALETTE_RGB[best_color].g/256,PALETTE_RGB[best_color].b/256,1);
				return color_out;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

