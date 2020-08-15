geometry.cs
	Contains a few basic geometry functions for triangles in 3D space

	* planeEquation(Vector3[] triangle)
		Takes a list of Unity Vector3s that represent the three vertices of a triangle in 3D space
		Returns a Vector4 of the form [a, b, c, d]
		This represents a function of the form ax+by+cz+d = 0 which represents the plane that the given triangle exists on
	* pointsOnSameSide(Vector3 pointA, Vector3 pointB, Vector4 planeEquation
		This function takes two Unity Vector3s which represent points in 3D space, 
		as well as a Unity Vector4 which represents the equation of a plane.
		This function returns True if both given points exist on the same side of the given plane, and False otherwise.
	* triangleNonintersectCheck(Vector3[] triangleA, Vector3[] triangleB)
		This function applies the Moller triangle-triangle intersection algorithm to two given Unity Vector3[]s which represent two triangles in 3D space
		It's not exactly Moller's because I introduce exceptions to intersection when two triangles share vertices, 
		which is not only normal but expected for a well formed 3D mesh, but the general idea is Moller's.
		Returns true if the triangles are not intersecting, false if they are
meshSerializer.cs
	Functions for serializing a Unity Mesh, 
	because Unity does not mark their classes as serializable which prevents me from saving certain assets to disk.

	The names of the functions are generally self explanatory.
	* serializableVector3ToVector3(SerializableVector3 svec3)
	* vector3ToSerializableVector3(Vector3 vec3)
	* serializableVector4ToVector4(SerializableVector4 svec4)
	* serializableVector4ToColor(SerializableVector4 svec4)
	* vector4ToSerializableVector4(Vector4 vec4)
	* serializeMesh(Mesh mesh, string name)
		This converts the given mesh into a serializableMesh object and saves it to disk
	* deserializeMesh(string name)
		This searches for the file indicated by the string name and attempts to deserialize it into a Unity Mesh
		If for whatever reason it can't find the file, it loads up a placeholder cube and returns that instead

	We also introduce new structures
	* SerializableVector3
		A Unity Vector3 is essentially just a set of three floating point values, 
		so here we store those values in a serializable struct.
	* SerializableVector4
		Similar to SerializableVector4
	* SerializableMesh
		This contains a SerializableVector3[] vertices which represent the vertices of our model
		An int[] triangles which represent indices into our SerializableVector3[] and from which we can construct the triangles of our model
		And a SerializableVector4[] colors which represent the colors of each vertex on our model, 
		which I currently use for rendering instead of textures.