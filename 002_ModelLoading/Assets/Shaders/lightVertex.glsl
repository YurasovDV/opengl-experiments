uniform mat4 u_MVPMatrix;
attribute vec3 a_Position;   
 
void main()                   
{                             
   gl_Position =  u_MVPMatrix * vec4(a_Position, 1);  
   gl_PointSize = 5.0;
}                             