Shader "Custom/HPBarShader" 
{

Properties 
{
	_MainTex("Texture", 2D) = ""
}

SubShader
{
	Ztest Always
	Tags {Queue = Transparent}
	Pass
	{
		SetTexture[_MainTex] {Combine texture Double}
	}
}

}