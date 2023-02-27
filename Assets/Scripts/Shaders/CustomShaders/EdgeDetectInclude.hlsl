#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED
// I hate shader code with every fibre of my being. why, oh why, oh why.
// Thank you 'Ned Makes Games'. this is probably basically 100% copy pasted.

// Three. It's the magic number. yes it is. its the magic number.
// Therefore three by three is magic squared.
static float2 sobleSamplePoints[9] = {
  float2(-1,1),float2(0,1),float2(1,1),
  float2(-1,0),float2(0,0),float2(1,1),
  float2(-1,-1),float2(0,-1),float2(1,-1),
};

// ok but there's also x and y weights and uh hurr durr math go brr

static float sobelMatX[9] = {
  1,0,-1,
  2,0,-2,
  1,0,-1
};
static float sobelMatY[9] = {
  1,2,1,
  0,0,0,
  -1,-2,-1
};

// Do I understand what these do or why they're the numbers they are?
// no.

void DepthSobel_float(float2 UV, float Thickness, out float Out){ // Gee do I wish I had error checking for hlsl
  float2 sobel = 0;
  [unroll] for (int i = 0; i < 9; i++){ // please unroll my brain.
    float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobleSamplePoints[i] * Thickness); // Yikes.
    sobel += depth * float2(sobelMatX[i], sobelMatY[i]); // no idea why you do this.
  }
  Out = length(sobel);
}

void ColorSobel_float(float2 UV, float Thickness, out float Out){
  float2 sobelR = 0;
  float2 sobelG = 0;
  float2 sobelB = 0;

  [unroll] for (int i = 0; i < 9; i++){
    float3 rgb = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV + sobleSamplePoints[i] * Thickness);
    float2 kernel = float2(sobelMatX[i],sobelMatY[i]);
    sobelR += rgb.r * kernel;
    sobelG += rgb.g * kernel;
    sobelB += rgb.b * kernel;
  }
  Out = max(length(sobelR),max(length(sobelG), length(sobelB)));
}
#endif
