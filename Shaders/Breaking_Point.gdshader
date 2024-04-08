shader_type canvas_item;


uniform float PixelWidth = 2.f;
uniform int PointCount;
uniform vec2 Points[256];

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
	for (int i = 0; i < PointCount; i++) {
		vec2 PointOne = Points[i];
		vec2 PointTwo = vec2(0);
		float closestdst = 1000000.f;
		for (int j = 0; j < PointCount; j++) {
			if (j == i) {
				continue;
			}
			vec2 PointCheck = Points[j];
			float dst = distance(PointOne, PointCheck);
			if (dst < closestdst) {
				PointTwo = PointCheck;
				closestdst = dst;
			}
		}
		
		float line_dist = sdSegment(2.f*UV-vec2(1,1),PointOne,PointTwo);
		min_line_dist = min(min_line_dist, line_dist);
	}
	float line_thickness = length(TEXTURE_PIXEL_SIZE)*PixelWidth;
	min_line_dist = min_line_dist - line_thickness;
	float color = min_line_dist < 0.f ? 1.f : 0.f;
	COLOR = vec4(vec3(color),1);
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
