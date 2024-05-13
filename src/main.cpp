#include <Arduino.h>
#include <LiquidCrystal_I2C.h>
#include <sstream>
#include <string>
#include <iostream>
#include <WiFi.h>
#include <WiFiUdp.h>
using namespace std;

// Digital Twin
const char SSID_NAME[] = "HotspotName";
const char SSID_PASSWORD[] = "mobilesoftware6";
WiFiUDP Udp;
char UDPPacketBuffer[255];

IPAddress RECEIVER_IP(192, 168, 125, 60); // 192.168.125.216
const int RECEIVER_PORT = 50195;
const int LOCAL_PORT = 3002;

char potUDPString[10] = {};
char keyUDPString[8] = {};

const int P_POT = 34;
const int DOOR_OPEN = 12;
const int DOOR_CLOSE = 27;

const int OPEN_LIMIT = 4095;
const int CLOSE_LIMIT = 500;

const int DOOR_LIMIT_BUFFER = 100;

int isOpen = 1;
int taskRunning = 0;

constexpr byte ZERO_PIN = 16;
constexpr byte ONE_PIN = 17;
constexpr byte TWO_PIN = 5;
constexpr byte THREE_PIN = 14;
constexpr byte FOUR_PIN = 23;
constexpr byte FIVE_PIN = 32;
constexpr byte SIX_PIN = 19;
constexpr byte SEVEN_PIN = 33;
constexpr byte EIGHT_PIN = 18;
constexpr byte NINE_PIN = 15;
constexpr byte RESET_PIN = 13;
constexpr byte ENTER_PIN = 26;

bool zeroPinState;
bool onePinState;
bool twoPinState;
bool threePinState;
bool fourPinState;
bool fivePinState;
bool sixPinState;
bool sevenPinState;
bool eightPinState;
bool ninePinState;
bool resetPinState;
bool enterPinState;

TaskHandle_t motor_handler;

int lcdColumns = 16;
int lcdRows = 2;

String inputString = String("");
const String password = String("1111");

const uint32_t KEYPAD_DELAY_TIME = 225;

LiquidCrystal_I2C lcd(0x27, lcdColumns, lcdRows);
void sendUdpData(String Data);
void sendMotorData(uint16_t motorVal);
void sendKeyData(int key);
void receiveUDPMessage();
void motorToggle(void *pvParameters);
void handleNumberInputs();
void setDoorState();
void keypadNumber(int key);
void sendKeyData(int key);
void handleEnter();
void handleReset();



void setup()
{
  Serial.begin(9600);

  pinMode(ZERO_PIN, INPUT_PULLUP);
  pinMode(ONE_PIN, INPUT_PULLUP);
  pinMode(TWO_PIN, INPUT_PULLUP);
  pinMode(THREE_PIN, INPUT_PULLUP);
  pinMode(FOUR_PIN, INPUT_PULLUP);
  pinMode(FIVE_PIN, INPUT_PULLUP);
  pinMode(SIX_PIN, INPUT_PULLUP);
  pinMode(SEVEN_PIN, INPUT_PULLUP);
  pinMode(EIGHT_PIN, INPUT_PULLUP);
  pinMode(NINE_PIN, INPUT_PULLUP);
  pinMode(RESET_PIN, INPUT_PULLUP);
  pinMode(ENTER_PIN, INPUT_PULLUP);
  pinMode(DOOR_OPEN, OUTPUT);
  pinMode(DOOR_CLOSE, OUTPUT);

  lcd.init();
  lcd.backlight();

  WiFi.begin(SSID_NAME, SSID_PASSWORD);
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.println("Trying WiFi");
    delay(1000);
  }
  Udp.begin(LOCAL_PORT);

  setDoorState();
}

void loop()
{
  lcd.setCursor(0, 0);
  lcd.print("Enter Password: ");
  lcd.setCursor(0, 1);

  resetPinState = digitalRead(RESET_PIN);
  enterPinState = digitalRead(ENTER_PIN);

  if (enterPinState == LOW)
  {
    handleEnter();
  }

  else if (resetPinState == LOW)
  {
    handleReset();
  }
  else
  {
    handleNumberInputs();
  }
  receiveUDPMessage();
}

void handleEnter()
{
  sendKeyData(-1);
  if (inputString == password)
  {
    lcd.clear();
    lcd.print("Correct");
    if (taskRunning == 1)
    {
      vTaskDelete(motor_handler);
    }
    xTaskCreate(motorToggle, "Toggle1", 4000, NULL, 1, &motor_handler);
    delay(3000);
  }
  else
  {
    lcd.clear();
    lcd.print("Wrong");
    delay(3000);
  }
  inputString = "";
  lcd.clear();
  lcd.print(inputString);
  delay(KEYPAD_DELAY_TIME);
}

void handleReset()
{
  sendKeyData(-2);
  inputString = "";
  lcd.clear();
  lcd.print(inputString);
  delay(KEYPAD_DELAY_TIME);
}

void handleUDPKeypadNumber(int key)
{
  inputString += key;
  lcd.print(inputString);
  delay(KEYPAD_DELAY_TIME);
}

void handleNumberInputs()
{
  zeroPinState = digitalRead(ZERO_PIN);
  onePinState = digitalRead(ONE_PIN);
  twoPinState = digitalRead(TWO_PIN);
  threePinState = digitalRead(THREE_PIN);
  fourPinState = digitalRead(FOUR_PIN);
  fivePinState = digitalRead(FIVE_PIN);
  sixPinState = digitalRead(SIX_PIN);
  sevenPinState = digitalRead(SEVEN_PIN);
  eightPinState = digitalRead(EIGHT_PIN);
  ninePinState = digitalRead(NINE_PIN);

  if (zeroPinState == LOW)
  {
    keypadNumber(0);
  }
  else if (onePinState == LOW)
  {
    keypadNumber(1);
  }

  else if (twoPinState == LOW)
  {
    keypadNumber(2);
  }
  else if (threePinState == LOW)
  {
    keypadNumber(3);
  }

  else if (fourPinState == LOW)
  {
    keypadNumber(4);
  }
  else if (fivePinState == LOW)
  {
    keypadNumber(5);
  }

  else if (sixPinState == LOW)
  {
    keypadNumber(6);
  }
  else if (sevenPinState == LOW)
  {
    keypadNumber(7);
  }

  else if (eightPinState == LOW)
  {
    keypadNumber(8);
  }
  else if (ninePinState == LOW)
  {
    keypadNumber(9);
  }
}

void keypadNumber(int key)
{
  inputString += key;
  sendKeyData(key);
  lcd.print(inputString);
  delay(KEYPAD_DELAY_TIME);
}

void sendKeyData(int key)
{
  sprintf(keyUDPString, "key|%d", key);
  sendUdpData(keyUDPString);
}

void sendMotorData(uint16_t motorVal)
{
  float potPercentage = ((float)motorVal) / 4095;
  sprintf(potUDPString, "pot|%.2f", potPercentage);
  sendUdpData(potUDPString);
}


void motorToggle(void *pvParameters)
{
  taskRunning = 1;
  int target;
  int to_run;
  int not_run;

  if (isOpen == 1)
  {
    target = CLOSE_LIMIT;
    to_run = DOOR_OPEN;
    not_run = DOOR_CLOSE;
  }
  else
  {
    target = OPEN_LIMIT;
    to_run = DOOR_CLOSE;
    not_run = DOOR_OPEN;
  }

  digitalWrite(not_run, LOW);
  digitalWrite(to_run, HIGH);

  uint16_t x = analogRead(P_POT);

  while (abs(target - x) > DOOR_LIMIT_BUFFER)
  {
    x = analogRead(P_POT);
    sendMotorData(x);
    vTaskDelay(50);
  }
  digitalWrite(to_run, LOW);

  setDoorState();

  taskRunning = 0;
  vTaskDelete(motor_handler);
}

void setDoorState()
{
  uint16_t x = analogRead(P_POT);
  if (x > OPEN_LIMIT - DOOR_LIMIT_BUFFER)
  {
    isOpen = 1;
  }
  else if (x < CLOSE_LIMIT + DOOR_LIMIT_BUFFER)
  {
    isOpen = 0;
  }
}
void sendUdpData(String Data)
{
  Udp.beginPacket(RECEIVER_IP, RECEIVER_PORT);
  Udp.print(Data);
  Udp.endPacket();
}
//Receive UDP DataString from Unity
void receiveUDPMessage() {
  if (Udp.parsePacket()) {
    int length = Udp.read(UDPPacketBuffer, 255);
    if (length > 0) {
      UDPPacketBuffer[length] = 0;
    }

    // Parse the message 
    int value;
 
    if (UDPPacketBuffer != NULL) {
      value = atoi(UDPPacketBuffer); // Convert string to integer

      if(value<0){
        switch (value)
        {
        case -1:
          handleEnter();
          break;
        case -2:
          handleReset();
          break;
        default:
          handleReset();
          break;
        }
      }
      else {
        handleUDPKeypadNumber(value);
      }
    }
  }
}