struct Boid
{
    float3 position;
    float3 velocity;
};


// https://gamedev.stackexchange.com/questions/149137/pseudo-random-number-generation-in-compute-shader
int3 RandomNumbers(uint3 id) // Not super great randomness but only really needs to look random
{
    int3 output = (float3) 0;
    output.x = (int) (sin(id.x * 71.01) * 500461564);
    output.y = (int) (cos(id.y * 53.7) * 1023467329);
    output.z = (int) (sin(id.z * 98.23) * 500461564);
    return output;

}

// Wander brokey, please do the fixxy
float3 Wander(Boid boid, float wanderCubeSize, float wanderCubeDistance, uint3 id)
{
    uint3 randomNumbers = RandomNumbers(id);
    float3 wanderPos = boid.position;
    wanderPos.x = ((randomNumbers.x % wanderCubeSize * 2) - wanderCubeSize) / wanderCubeSize;
    wanderPos.y = ((randomNumbers.y % wanderCubeSize * 2) - wanderCubeSize) / wanderCubeSize;
    wanderPos.z = ((randomNumbers.z % wanderCubeSize * 2) - wanderCubeSize) / wanderCubeSize;
    float oriSpeed = length(boid.velocity);
    float3 wander = (float3) 0;
    if (oriSpeed != 0)
    {
        wanderPos += boid.position + (boid.velocity / oriSpeed) * wanderCubeDistance;
        return normalize(wanderPos - boid.position);
    }
    return (float3) 0;

}