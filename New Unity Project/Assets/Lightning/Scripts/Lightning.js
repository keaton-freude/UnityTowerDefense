//Lightning.js
var meshDetail : int = 5;
var random : float = 1;
var smoothness : float = 30;
var lightningThickness : float = 1.5;


var lightningColor : Color = Color.cyan; 
var lightningGlowObject : LineRenderer;

var endPoint : Transform;
var glowTextures : Texture2D[];
var lightObjects : Transform[];
var rayCastEndPoint = false;
var oneShot = false;

var noHitLightningDistance : float = 200;
var oneShotFadeSpeed : float = .1;
var oneShotLightFadeSpeed : float = .2;

private var detail : int = 20;
private var startDestroy = false;
private var firstPosSet = false;

function Start () {
	endPoint.transform.parent = null;
	if(oneShot == true){
		startDestroy = true;
	}
}

function Update () {
	var hit : RaycastHit;
	
	if(Physics.Raycast(transform.position, transform.forward, hit)){
		if(rayCastEndPoint == true){
			endPoint.position = hit.point;
		}
		transform.LookAt(endPoint);
	}
	else {
		if(rayCastEndPoint == true){
			endPoint.transform.localPosition = transform.TransformDirection(Vector3(0,0,noHitLightningDistance));
		}
	}
	
	var dist = Vector3.Distance(transform.position, endPoint.position);
	detail = dist * meshDetail / 40;
	var modColor = lightningColor;
	
	if(lightningColor.r < 0.5){
		modColor.r = .5;
	}
	
	if(lightningColor.g < 0.5){
		modColor.g = .5;
	}
	
	if(lightningColor.b < 0.5){
		modColor.b = .5;
	}
	
	renderer.material.SetColor("_TintColor", modColor);
	lightningGlowObject.material.SetColor("_TintColor", lightningColor);
	
	if(lightObjects.length > 0){
		for(i = 0; i < lightObjects.length; i++){
			lightObjects[i].light.color = lightningColor;
			if(firstPosSet == false){
				lightObjects[i].localPosition.z = Random.Range(1, dist);
			}
			if(startDestroy == true){
				lightObjects[i].light.intensity -= oneShotLightFadeSpeed;
			}
		}
	}
	
	var GlowWidthRandom = Random.Range(9, 13);
	
	if(firstPosSet == false){
		lightningGlowObject.material.mainTexture = glowTextures[Random.value * glowTextures.length];
		lightningGlowObject.SetWidth(GlowWidthRandom, GlowWidthRandom);
	}
	
	var render : LineRenderer = GetComponent(LineRenderer);
	
	render.SetWidth(lightningThickness, lightningThickness);
	
	
		
	if(detail > 0){		
		render.SetVertexCount(detail + 1);
		lightningGlowObject.SetVertexCount(2);
		if(firstPosSet == false){
			renderer.material.mainTextureOffset.x = Random.value;
		}
		renderer.material.mainTextureScale.x = dist/smoothness;
		
		positionDistance = dist / detail;
	}
	for(i = 1; i < detail + 1; i ++){
		var randomPos = Vector3(Random.Range(-random,random),Random.Range(-random,random),i * positionDistance);
		if(i == detail){
			var pos = Vector3(0, 0, dist);
			render.SetPosition(i, pos);
			lightningGlowObject.SetPosition(1, pos);
		}
		else{
			if(firstPosSet == false){
				render.SetPosition(i, randomPos);
			}
		}
	}
		
	if(oneShot == true){
		if(startDestroy == true){
			lightningColor.a -= oneShotFadeSpeed;
		}
		firstPosSet = true;
	}
	
}