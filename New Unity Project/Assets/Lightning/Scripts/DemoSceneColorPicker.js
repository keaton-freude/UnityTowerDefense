var lightningObject : Lightning;
private var defaultColor : Color;

function Start () {
	defaultColor = lightningObject.lightningColor;
}

function OnGUI () {
	var col = lightningObject.lightningColor; 
	col.r = GUI.HorizontalSlider (Rect ( ( Screen.width/2 - 100 ), 30, 200, 10 ), col.r, 0.0, 1.0);
	col.g = GUI.HorizontalSlider (Rect ( ( Screen.width/2 - 100 ), 50, 200, 10 ), col.g, 0.0, 1.0);
	col.b = GUI.HorizontalSlider (Rect ( ( Screen.width/2 - 100 ), 70, 200, 10 ), col.b, 0.0, 1.0);
	
	if(GUI.Button(Rect ( Screen.width/2 - 50, 90, 100, 20), "Reset Color")){
		lightningObject.lightningColor = defaultColor;
	}
	else{
		lightningObject.lightningColor = col;
	}
}