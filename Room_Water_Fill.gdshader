shader_type canvas_item;
render_mode blend_mix;

global uniform vec4 Water_Color : source_color = vec4(0.000000, 0.000000, 1.000000, 0.498039);
uniform float FillAmount : hint_range(0, 1) = 0.0;



void fragment() {
	float WaterHeight = clamp(UV.g + FillAmount, 0.0, 1.0);
	float WaterHeightAlpha = Water_Color.a * floor(WaterHeight);
	vec3 WaterColorMultipliedAlpha = Water_Color.xyz * vec3(WaterHeightAlpha);
	//vec3 WaterColorMultipliedAlpha = vec3(UV,0.0) * vec3(WaterHeightAlpha);
	COLOR = vec4(WaterColorMultipliedAlpha,WaterHeightAlpha);
}
