struct Boid
{
    float3 position;
    float3 velocity;
};


// https://gamedev.stackexchange.com/questions/149137/pseudo-random-number-generation-in-compute-shader
float3 RandomNumbers(float3 pos) // Not super great randomness but only really needs to look random
{
    float3 output = (float3) 0;
    output.x = (sin(pos.x * 71.01) * 500461564);
    output.y = (cos(pos.y * 53.7) * 1023467329);
    output.z = (sin(pos.z * 98.23) * 500461564);
    return output;

}

// Wander brokey, please do the fixxy
float3 Wander(Boid boid, float wanderSphereSize, float wanderSphereDistance)
{
    float3 randomNumbers = RandomNumbers(boid.position);
    float speed = length(boid.velocity);
    if(speed == 0) // speed can be zero at the first frame, if we don't check for this we could divide by 0
    {
        return float3(0, 0, 0);
    }
    
    float3 forward = boid.velocity / speed;
    // this method of generating a random point is not great, we essentially turn a random point in a cube 
    // into a random point on a sphere. More points can be generated in the edges of the cube causing a higher
    // chance for the boid goes that direction 
    float3 spherePos = normalize(randomNumbers) * wanderSphereSize;
    float3 desiredPos = spherePos + (forward * wanderSphereDistance) + boid.position;
    
    return normalize(desiredPos - boid.position);
}

float3 Seperate(Boid boid, Boid neighbor, float neighborhoodRad)
{
    float3 vecFromNeighbor = boid.position - neighbor.position;
    float dist = length(vecFromNeighbor);
    return (vecFromNeighbor / dist) * (1 - (dist / neighborhoodRad));
}


// It feels pointless to make this last one a function but for the sake of consitency
float3 Allign(Boid neighbor)
{
    return neighbor.velocity;
}
