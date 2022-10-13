# GPU Boids
I learned about compute shaders and learned they're a good way to offload work from your CPU whenever you have a highly parallelizable problem. I decided that making an implementation of boids in compute shaders seems like a good way to learn. I decided on using Unity because they have good support for compute shaders.

#### C# 
The first challenge was learning how to use compute shaders in unity, luckily the code is similar to DirectX with which I already have some experience. The C# side of the project is pretty easy. I made the boids customizable at runtime in the editor. 

![Editor variables](https://images2.imgbox.com/92/f3/QgEfKi01_o.png "Editor variables")

#### Compute shader
Because you can't use classes in compute shaders I had to go for a different approach to implement boids than I would usually use. I made seperate functions for the steering behaviours  and combine all the output from them in the main function of the shader. I also learned from https://github.com/Shinao/Unity-GPU-Boids that just using a brute force loop to check for neighbors is the fastest for GPU.

#### Instanced Meshes 
After I made the compute shader I learned that the biggest bottleneck was actually pushing the transforms of the boids to the GPU and then reading it back and transforming the boids on the CPU. Because of this I made a instanced mesh renderer. The positions of the boids get generated on the CPU when the program starts and gets pushed to the GPU once and is never accessed by the CPU again. This massively improved FPS.

#### Future work
- Shinao who I mentioned earlier made the boids fly within a mesh, I'd like to also make this because it looks really cool.
- Make a way to place waypoints and have the boids follow these.
- Make seperate flocks that stick together, and can follow seperate waypoint paths.


