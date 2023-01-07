float ModifyByTex(fixed4 modif, float noise)
{
	modif.b = (modif.b - 1) * 2;
	return lerp(noise, modif.r, modif.g) + (modif.b * modif.a);
}