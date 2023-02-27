#ifndef ColourSplit_INCLUDED
#define ColourSplit_INCLUDED
static const float3 PALETTE_ALT[16] =
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

float3 rgb(float r,float g,float b)
{
    return float3(r,g,b);
}

void ColourRounding_float(float Steps,float3 In,out float3 Out) // Simple and fast implementation - Making a better one.
{
    float stepsize = (1 / Steps);
    Out = float3( round(In.x/stepsize) * stepsize, round(In.y/stepsize) * stepsize, round(In.z/stepsize) * stepsize);
}

float f3ManhattanDist(float3 p1, float3 p2)
{
    return abs(p1.x - p2.x) + abs(p1.y - p2.y) + abs(p1.z - p2.z);
}
void PaletteMatch_float(float3 pIn, float debugDiv, out float3 pOut)
{

	float3 BEST_MATCH = float3(0,256,256);
	float BEST_DIST = 9999;
	[unroll]
    for (int i = 0; i < 70; i++)
	{
        float distance = abs(pIn.x - PALETTE_RGB[i].x / debugDiv) + abs(pIn.y - PALETTE_RGB[i].y / debugDiv) + abs(pIn.z - PALETTE_RGB[i].z / debugDiv);
    if(distance <= BEST_DIST)
    {
        BEST_DIST = distance;
            BEST_MATCH = PALETTE_RGB[i];
        }
	}
	pOut = float3(BEST_MATCH.x/debugDiv,BEST_MATCH.y/debugDiv,BEST_MATCH.z/debugDiv);
}





#endif
