#include "Client.h"

bool Client::remove_object(Object * object)
{
	return false;
}

bool Client::remove_object(int id)
{
	return false;
}

Object * Client::get_object(int id)
{
	return nullptr;
}

int Client::get_object_id(Object * object)
{
	return 0;
}

void Client::process_message(string message)
{
	vector<string> words = splitstring(message, ":");
	int newid = stoi(words.at(0));
	newid += ID;
	string newmessage = to_string(newid);
	for (int i = 1; i < words.size(); i++) {
		newmessage.append(":");
		newmessage.append(words.at(i));
	}
	add_message(newmessage);

}
