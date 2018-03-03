#version 330

attribute vec2 vPosition;
attribute vec2 vTexCoordinate; 

out vec2 texCoords;

void main()
{
    gl_Position = vec4(vPosition.x, vPosition.y, 0f, 1.0f);

	texCoords = vTexCoordinate;
}