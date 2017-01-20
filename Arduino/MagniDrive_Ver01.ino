/* 
This is a test sketch for the Adafruit assembled Motor Shield for Arduino v2
It won't work with v1.x motor shields! Only for the v2's with built in PWM
control

For use with the Adafruit Motor Shield v2 
---->  http://www.adafruit.com/products/1438
*/

/* Motor Control Part Lib */
#include <Wire.h>
#include <Adafruit_MotorShield.h>
#include "utility/Adafruit_MS_PWMServoDriver.h"
/* ---------------------------- */

/* OSC Part Lib */
#include <SPI.h>
#include <WiFi101.h>
#include <WiFiUdp.h>
#include <OSCMessage.h>
#include <OSCBundle.h>
/* ----------------------- */



/* Motor Part Variables */
// Create the motor shield object with the default I2C address
Adafruit_MotorShield AFMS = Adafruit_MotorShield(); 
// Or, create it with a different I2C address (say for stacking)
// Adafruit_MotorShield AFMS = Adafruit_MotorShield(0x61); 

// Select which 'port' M1, M2, M3 or M4. In this case, M1
Adafruit_DCMotor *myMotor = AFMS.getMotor(1);
// You can also make another motor on port M2
//Adafruit_DCMotor *myOtherMotor = AFMS.getMotor(2);
/* ------------------------------------------------------ */

/* OSC Part Variables */
int status = WL_IDLE_STATUS;
char ssid[] = "NextInterfaces Lab"; // your network SSID (name)
char pass[] = "nextinterfaces"; // your network password (use for WPA, or use as key for WEP)
int keyIndex = 0; // your network key Index number (needed only for WEP)
IPAddress sendToUnityPC_Ip(192, 168, 0, 105); // UnityPC's IP
unsigned int sendToUnityPC_Port = 8000; // UnityPC's listening port
unsigned int listenPort = 9000; // local port to listen on
char packetBuffer[255]; //buffer to hold incoming packet
char ReplyBuffer[] = "acknowledged"; // a string to send back
WiFiUDP Udp_send;
WiFiUDP Udp_listen;
/* ---------------------------------------- */

int value ;

void setup() 
{
  Serial.begin(9600);           // set up Serial library at 9600 bps

  
  AFMS.begin();  // create with the default frequency 1.6KHz
  //AFMS.begin(1000);  // OR with a different frequency, say 1KHz
  
  myMotor->run(FORWARD);
  Serial.setTimeout(10);   /* 加快timeout 判斷 (default : 1s => too slow)*/


  //Configure pins for Adafruit ATWINC1500 Feather
  WiFi.setPins(8,7,4,2);
  //Initialize serial and wait for port to open:
  //Serial.begin(9600);
  while (!Serial) 
  {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  // check for the presence of the shield:
  if (WiFi.status() == WL_NO_SHIELD) 
  {
    //Serial.println("WiFi shield not present");
    // don't continue:
    while (true);
  }
  // attempt to connect to Wifi network:
  while ( status != WL_CONNECTED) 
  {
    //Serial.print("Attempting to connect to SSID: ");
    //Serial.println(ssid);
    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:
    status = WiFi.begin(ssid, pass);
    // wait 10 seconds for connection:
    delay(10000);
  }
  //Serial.println("Connected to wifi");
  //printWifiStatus();
  //Serial.println("\nStarting connection to server...");
  // if you get a connection, report back via serial:
  Udp_send.begin(sendToUnityPC_Port);
  Udp_listen.begin(listenPort);
  
}

void loop() {

  /* Serial Port Version .
  while(1){
    if(Serial.available() > 0)
    {
      
    //  String pin_string = Serial.readStringUntil('.');
    //  if(pin_string!=""){
    //        String pwm_string = Serial.readStringUntil(':');
    //        int int_pwm = pwm_string.toInt();
    //
    //        Serial.println(int_pwm);
    //        myMotor->setSpeed(int_pwm);
    //   }
       
      
      value = Serial.parseInt();
      Serial.println(value);
      myMotor->setSpeed(value);
      Serial.parseInt();      // 把parseInt()後多出來的'0'讀掉 
    }
  */



  /* OSC Version */
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
      //Serial.println(messageIn.size());
      //Serial.println(data);
      // setting intensity of the LED
      //int fadeValue = data; 
      //analogWrite(ledPin, fadeValue);
      value = data ;
      //Serial.println(value);
      myMotor->setSpeed(value);

    }
  }

  

  //delay(20);  
  //delay(1000);
  
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


