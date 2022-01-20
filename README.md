# GPUBoids
Description:
  A implementation of boids using compute shaders, this allows for much better preformance and thus more boids than would be possible on the cpu

Design:
  The project is pretty simple, it contains a c# file, a compute shader and an instanced mesh shader.
  The c# updates the variables of the flock whenever they are updated within the editor.
  I decided to write the instanced mesh shader so I could draw the boids using a StructuredBuffer, this way I could avoid passing the buffer back and forth between the cpu and gpu
  as this was the main bottleneck at the time.
 
 
Result:
  I can simulate around 30k boids at 60+ fps on my RTX 3050, the weights on the steering behaviors can be edited at runtime in the editor.
  
Conclusion:
  Compute shaders can be used to exponantionally increase performance on certain tasks, boids on cpu that I made earlier this year went up to around 2000 at 60fps.
  
Future work:
  I found a technique to make the boids fly in the shape of a mesh, 
  but I couldn't find many resources on it and due to time constraints I wasn't able to make it, I'd like to implement this in my free time
 
