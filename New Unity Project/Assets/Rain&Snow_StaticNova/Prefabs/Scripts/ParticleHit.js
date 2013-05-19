
function LateUpdate () 
{
	var theParticles = particleEmitter.particles;
	print("Particle" + 1 + "energy is" + theParticles[0].energy);
	
	if(theParticles[0].energy > particleEmitter.maxEnergy)
	{
		print("Hit Surface.");
	}
}