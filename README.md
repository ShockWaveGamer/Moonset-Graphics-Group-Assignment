# Moonset Graphics Group Assignment
 Daniel Fiuk - 100834886 / Constantine 100822644

[Backup Of The Standard RP Shaders We Wrote](https://github.com/ShockWaveGamer/Group-Assignment-Back-Up.git)
Unfortunatly we were unable to port the individual assignment shaders from SRP to URP. This is a backup project featuring them.

# Statement of Contributions
## Constantine Pallas:
- [x] Individual Assignment Shaders (Secondary Project)
- [x] Particle System (Assets/Particle)
- [x] Palette Shader (Assets/Shaders/Outline And Palette)
## Daniel Fiuk:
- [x] Individual Assignment Shaders (Secondary Project)
- [x] Water Shader (Assets/Shaders/DanielsGroupAssignmentShaders/Water)
- [x] Decal Shader  (Assets/Shaders/DanielsGroupAssignmentShaders/Decals)
- [x] Fog Post Process (Assets/Shaders/DanielsGroupAssignmentShaders/Fog)

note: our post-processing effects use an external package called Blit Material Feature to work. All this does in our case is restore a feature removed in URP called Custom Post-Process [Cyanilux/URP_BlitRenderFeature](https://github.com/Cyanilux/URP_BlitRenderFeature)

### Particle System

 A prefab of a particle system configured is included in the same directory as the project, including the custom texture and material/shader. To see in-game, proceed to any checkpoint.
 
### Palette Shader
 
 A material with this shader is applied to a Blit Material Feature which applies to the default URP renderer asset (visible at  the directory Assets/Plugins/URP/Settings/URP-HighFidelity-Renderer.asset). This shader applies to all gameObjects and Cameras, as well as the scene view.
 
### Water Shader
 
 Apply the water material to a flat plane. The more vertecies that are a part of the plane the smoother the wave effect will apear. In the inspector add a blue tinted toon ramp texture 
 
### Decal Shader
 
 Apply the decal material on a mesh and choose a transparent image to overlay ontop of the main texture. Recolor and manipulate the values to fit the porpose.
 
### Fog Shader
 
 Intensity scales inversly. Simply manpulate the values of the fog material to change the density and color of the fog.
