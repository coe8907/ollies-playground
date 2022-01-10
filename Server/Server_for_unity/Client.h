#pragma once
#include <vector>
#include "Object.h"
#include<stdio.h>
#include<winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include <iostream>

using namespace std;
class Client
{
private:
	const int MAXNUMEROFOBJECTS = 100;
	vector<Object*> objects;
	vector<string> messages;
	struct sockaddr_in* address;
	void add_message(string message) {
		messages.push_back(message);
	}
	vector<string> splitstring(string text, string spliter) {
		vector<string> words{};
		size_t pos = 0;
		while ((pos = text.find(spliter)) != string::npos) {
			words.push_back(text.substr(0, pos));
			text.erase(0, pos + spliter.length());
		}
		return words;
	}
	int ID = 0;
public:
	void Set_id(int id) {
		ID = id * MAXNUMEROFOBJECTS;
	}

	Client(sockaddr_in * add) { address = add; };
	void add_object(Object * object) { objects.push_back(object); };

	bool remove_object(Object * object);
	bool remove_object(int id);

	Object* get_object(int id);
	int get_object_id(Object * object);
	
	vector<string>* get_messages() {
		return &messages;
	}
	void clear_messages() {
		messages.clear();
	}
	void process_message(string message);

	
};

