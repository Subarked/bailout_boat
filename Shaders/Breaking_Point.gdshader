shader_type canvas_item;


uniform float PixelWidth = 2.f;
uniform int PointCount;
uniform vec2 Points[128];
//TODO: add JointCount uniform to make it so you can have more than 2 points per arm
uniform float Damage : hint_range(0,1) = 0.f;
void vertex() {
	// Called for every vertex the material is visible on.
}

float sdSegment( in vec2 p, in vec2 a, in vec2 b )
{
    vec2 pa = p-a, ba = b-a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h );
}

void fragment() {
	float min_line_dist = 100000.f;
	for (int i = 0; i < PointCount-1; i++) {
		bool whichPoint = i % 2 == 0; //true = first, false = second
		if (Damage < 0.5f && !whichPoint) {
			continue;
		}
		vec2 PointTwo = whichPoint ? Points[0] : Points[i];
		PointTwo = PointTwo * clamp(Damage*2.f,0.f,1.f);
		vec2 PointOne = (whichPoint ? Points[i+1] * clamp(Damage*2.f,0.f,1.f) : Points[i+1] * clamp(Damage*2.f-1.f,0.f,1.f))+PointTwo;
		
		float line_dist = sdSegment(2.f*UV-vec2(1,1),PointOne,PointTwo);
		min_line_dist = min(min_line_dist, line_dist);
	}
	float line_thickness = length(TEXTURE_PIXEL_SIZE)*PixelWidth;
	min_line_dist = min_line_dist - line_thickness;
	bool in_line = min_line_dist < 0.f;
	COLOR = in_line ? vec4(0,0,0,1) : vec4(0,0,0,0);
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
