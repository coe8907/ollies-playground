#pragma once
#include <vector>
#include <map>
#include "Client.h"
#include <iostream>
#include<stdio.h>
#include<winsock2.h>
#include <ws2tcpip.h>
#include <string>
#include<thread>
#pragma comment(lib,"ws2_32.lib")

using namespace std;

#define BUFLEN 100	//Max length of buffer
#define PORT 25565	//The port on which to listen for incoming data

class Master_Connection
{
private:
	//vars for networking connection //
	char ipinput[INET_ADDRSTRLEN];
	SOCKET s;
	struct sockaddr_in server, si_other;
	int slen, recv_len;
	char buf[BUFLEN];
	WSADATA wsa;

	//compartions
	struct cmpByAdd {
		bool operator()(const sockaddr_in& a, const sockaddr_in& b) const {
			return true;
			/*char ipinput[INET_ADDRSTRLEN];
			string add = inet_ntop(AF_INET, &a.sin_addr, ipinput, INET_ADDRSTRLEN);
			char ipinput2[INET_ADDRSTRLEN];
			string add1 = inet_ntop(AF_INET, &b.sin_addr, ipinput2, INET_ADDRSTRLEN);
			if (add == add1) {
				return true;
			}
			return false;
			*/
		}
	};
	vector<string> splitstring(string text, string spliter) {
		vector<string> words{};
		size_t pos = 0;
		while ((pos = text.find(spliter)) != string::npos) {
			words.push_back(text.substr(0, pos));
			text.erase(0, pos + spliter.length());
		}
		return words;
	}
	


	// messages vars
	vector<string> messages;
	vector<sockaddr_in> addresses;
	vector<sockaddr_in> all_addresses;
	bool locked = true;
	//
	typedef map<sockaddr_in, Client*, cmpByAdd> client_map;
	client_map clients;
	void new_client(sockaddr_in add);
	bool offload_message(sockaddr_in add, string message);
	bool send_message(sockaddr_in address,string message);
	void process_messages();
	void listen();
public:
	Master_Connection();
};

