#pragma once
#include <vector>
#include "Object.h"
#include<stdio.h>
#include<winsock2.h>
#include <ws2tcpip.h>
#include <string>
using namespace std;
class Client
{
private:
	vector<Object*> objects;
	vector<string> messages;
	struct sockaddr_in address;
public:
	Client(sockaddr_in add) { address = add; };
	void add_object(Object * object) { objects.push_back(object); };

	bool remove_object(Object * object);
	bool remove_object(int id);

	Object* get_object(int id);
	int get_object_id(Object * object);
	
	vector<string>* get_messages() {
		return &messages;
	}
	void add_message(string message) {
		messages.push_back(message);
	}
	void process_message(string message);

};

