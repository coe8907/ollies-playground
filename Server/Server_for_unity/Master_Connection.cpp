#include "Master_Connection.h"


void Master_Connection::new_client(sockaddr_in  add)
{
	Client* c = new Client(&add);
	clients.insert(client_map::value_type(add, c));
	all_addresses.push_back(add);
	cout << "new client" << endl;
}

bool Master_Connection::offload_message(sockaddr_in  add2, string message)
{
	for (auto it = clients.begin(); it != clients.end(); ++it)
	{
		sockaddr_in add = it->first;
		string s_add = inet_ntop(AF_INET, &add.sin_addr, ipinput, INET_ADDRSTRLEN);
		Client* client = it->second;
		string a = inet_ntop(AF_INET, &add2.sin_addr, ipinput, INET_ADDRSTRLEN);
		if (a == s_add) {
			client->process_message(message);
			return true;
		}
	}
	return false;
}

bool Master_Connection::send_message(sockaddr_in  address, string message)
{
	memset(buf, '\0', BUFLEN);
	strcpy_s(buf, message.c_str());
	printf("sending sneding \n");
	if (sendto(s, buf, recv_len, 0, (struct sockaddr*) &address, slen) == SOCKET_ERROR)
	{
		
		printf("sendto() failed with error code : %d", WSAGetLastError());
		return false;
		exit(EXIT_FAILURE);
	}
	return true;
}

void Master_Connection::process_messages()
{
	if (!locked) {
		int m_l = messages.size();
		if (m_l > 0) {
			for (int count = 0; count < m_l; count++) {
				
				vector<string> words = splitstring(messages.at(count), ":");
				
				if (words.size() > 0) {
					cout << words.at(0);
					if (words.at(0) == "NEWClient") {
						new_client(addresses.at(count));
					}
					else {
						offload_message(addresses.at(count), messages.at(count));
					}
				}
			}
			messages.clear();
			addresses.clear();
		}
		else {
			locked = true;
			return;
		}
	}
	for (auto it = clients.begin(); it != clients.end(); ++it)
	{
		sockaddr_in add = it->first;
		string s_add = inet_ntop(AF_INET, &add.sin_addr, ipinput, INET_ADDRSTRLEN);
		Client * client = it->second;
		vector<string>* to_send = client->get_messages();
		while (to_send->size() > 0) {
			
				string m = to_send->at(to_send->size()-1);
				for (int j = 0; j < all_addresses.size(); j++) {
					sockaddr_in t_add = all_addresses.at(j);
					string s_add1 = inet_ntop(AF_INET, &t_add.sin_addr, ipinput, INET_ADDRSTRLEN);
					//if (s_add != s_add1) {
						send_message(t_add,m);
					//}
				}
			
			to_send->pop_back();
		}

	}
}

void Master_Connection::listen()
{
	vector<string> t_messages;
	vector<sockaddr_in> t_addresses;
	while (1)
	{
		char ipinput[INET_ADDRSTRLEN];
		//printf("Waiting for data...");
		fflush(stdout);

		//clear the buffer by filling null, it might have previously received data
		memset(buf, '\0', BUFLEN);

		//try to receive some data, this is a blocking call
		if ((recv_len = recvfrom(s, buf, BUFLEN, 0, (struct sockaddr *) &si_other, &slen)) == SOCKET_ERROR)
		{
			printf("recvfrom() failed with error code : %d", WSAGetLastError());
		}
		t_addresses.push_back(si_other);
		t_messages.push_back((string)buf);

		string add = inet_ntop(AF_INET, &si_other.sin_addr, ipinput, INET_ADDRSTRLEN);
		//printf("Received packet from %s:%d\n", add, ntohs(si_other.sin_port));
		printf("Data: %s\n", buf);

		if (locked) {
			messages.insert(messages.end(),t_messages.begin(), t_messages.end());
			addresses.insert(addresses.end(), t_addresses.begin(), t_addresses.end());
			t_messages.clear();
			t_addresses.clear();
			locked = false;
		}

	}
}

Master_Connection::Master_Connection()
{
	slen = sizeof(si_other);

	//Initialise winsock
	printf("\nInitialising Winsock...");
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
	{
		printf("Failed. Error Code : %d", WSAGetLastError());
		exit(EXIT_FAILURE);
	}
	printf("Initialised.\n");

	//Create a socket
	if ((s = socket(AF_INET, SOCK_DGRAM, 0)) == INVALID_SOCKET)
	{
		printf("Could not create socket : %d", WSAGetLastError());
	}
	printf("Socket created.\n");

	//Prepare the sockaddr_in structure
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;
	server.sin_port = htons(PORT);

	//Bind
	if (::bind(s, (struct sockaddr *)&server, sizeof(server)) == SOCKET_ERROR)
	{
		printf("Bind failed with error code : %d", WSAGetLastError());
		exit(EXIT_FAILURE);
	}
	
	puts("Bind done");

	thread t(&Master_Connection::listen, this);
	while (true) {
		process_messages();
	}
	t.join();

	closesocket(s);
	WSACleanup();
}
