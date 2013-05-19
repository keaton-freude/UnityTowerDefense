//Light_Flicker.js

var time : float = .2;
var min : float = .5;
var max : float = 5;

function Start () {
	if(light){
		InvokeRepeating("OneLightChange", time, time);
	}
	else{
		print("Please add a light component for light flicker");
	}
}

function OneLightChange () {
	light.range = Random.Range(min,max);
}
