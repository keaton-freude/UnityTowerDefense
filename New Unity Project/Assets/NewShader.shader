Shader "Custom/Offset/No Offset" {

Properties
{
	_MainTex("Texture", 2D) = ""
}


	SubShader
	{
		Pass
		{
			SetTexture[_MainTex] {Combine texture Double}
		}
	}
}