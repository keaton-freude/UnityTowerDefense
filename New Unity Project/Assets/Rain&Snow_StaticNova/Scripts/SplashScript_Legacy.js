var splash: Transform;

function LateUpdate () 
{
	var theParticles = particleEmitter.particles;
	
	for(var i = 0; i < particleEmitter.particleCount;i++)
	{
		if(theParticles[i].energy > particleEmitter.maxEnergy)
		{
			var splashObj: Transform = Transform.Instantiate(splash, theParticles[i].position, Quaternion.identity);
			theParticles[i].energy = 0;
			Destroy(splashObj.gameObject, 0.5);
		}
	}	
	particleEmitter.particles = theParticles;
}