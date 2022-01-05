#pragma once
#include <vector>
#include "Client.h"
using namespace std;
class Master_Connection
{
private:
	vector<Client> clients;
	bool offloadmessage(string message);
	bool sendmessage(struct sockaddr* address,string message);
public:

};

