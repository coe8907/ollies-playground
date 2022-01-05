#pragma once
#include <string>
using namespace std;
class Object
{
private:
	
	int objectID = -1;
	string name = "null";
public:
	int get_id() { return objectID; }
	void set_id(int new_id) { objectID = new_id; }
	string get_name() { return name; }
	void set_name(string new_name) { name = new_name; }
	
	float x_pos, y_pos, z_pos, x_rot, y_rot, z_rot;
};

