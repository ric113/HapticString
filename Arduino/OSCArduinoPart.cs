/*
WiFi UDP Send and Receive String
This sketch wait an UDP packet on localPort using a WiFi shield.
When a packet is received an Acknowledge packet is sent to the client on port remotePort
Circuit:
* WiFi shield attached
created 30 December 2012
by dlf (Metodo2 srl)
*/

#include <SPI.h>
#include <WiFi101.h>
#include <WiFiUdp.h>
#include <OSCMessage.h>
#include <OSCBundle.h>

int ledPin = 10;
int status = WL_IDLE_STATUS;
char ssid[] = "NextInterfaces Lab"; // your network SSID (name)
char pass[] = "nextinterfaces"; // your network password (use for WPA, or use as key for WEP)
int keyIndex = 0; // your network key Index number (needed only for WEP)
IPAddress sendToUnityPC_Ip(10, 0, 1, 2); // UnityPC's IP
unsigned int sendToUnityPC_Port = 8000; // UnityPC's listening port
unsigned int listenPort = 9000; // local port to listen on
char packetBuffer[255]; //buffer to hold incoming packet
char ReplyBuffer[] = "acknowledged"; // a string to send back
WiFiUDP Udp_send;
WiFiUDP Udp_listen;

void setup() 
{
	//Configure pins for Adafruit ATWINC1500 Feather
	WiFi.setPins(8,7,4,2);
	//Initialize serial and wait for port to open:
	Serial.begin(9600);
	while (!Serial) 
	{
		; // wait for serial port to connect. Needed for native USB port only
	}
	// check for the presence of the shield:
	if (WiFi.status() == WL_NO_SHIELD) 
	{
		Serial.println("WiFi shield not present");
		// don't continue:
		while (true);
	}
	// attempt to connect to Wifi network:
	while ( status != WL_CONNECTED) 
	{
		Serial.print("Attempting to connect to SSID: ");
		Serial.println(ssid);
		// Connect to WPA/WPA2 network. Change this line if using open or WEP network:
		status = WiFi.begin(ssid, pass);
		// wait 10 seconds for connection:
		delay(10000);
	}
	Serial.println("Connected to wifi");
	printWifiStatus();
	Serial.println("\nStarting connection to server...");
	// if you get a connection, report back via serial:
	Udp_send.begin(sendToUnityPC_Port);
	Udp_listen.begin(listenPort);
}

void loop() 
{
	// Write
	OSCMessage msg("/1/fader1");
	msg.add((int)analogRead(0));
	// msg.add(sensorValue);
	Udp_send.beginPacket(sendToUnityPC_Ip, sendToUnityPC_Port);
	msg.send(Udp_send);
	Udp_send.endPacket();
	msg.empty();
	delay(10);

	// Read
	OSCMessage messageIn;
	int size;
	if( (size = Udp_listen.parsePacket())>0)
	{
		while(size--)
			messageIn.fill(Udp_listen.read());
		if(!messageIn.hasError())
		{ 
			int data = messageIn.getInt(0); 
			Serial.println(messageIn.size());
			Serial.println(data);
			// setting intensity of the LED
			int fadeValue = data; 
			analogWrite(ledPin, fadeValue);
		}
	}
}

void printWifiStatus() 
{
	
	// print the SSID of the network you're attached to:
	Serial.print("SSID: ");
	Serial.println(WiFi.SSID());
	
	// print your WiFi shield's IP address:
	IPAddress ip = WiFi.localIP();
	Serial.print("IP Address: ");
	Serial.println(ip);
	
	// print the received signal strength:
	long rssi = WiFi.RSSI();
	Serial.print("signal strength (RSSI):");
	Serial.print(rssi);
	Serial.println(" dBm");
}