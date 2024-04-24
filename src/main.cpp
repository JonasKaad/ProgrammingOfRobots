#include <Arduino.h>
#include <LiquidCrystal_I2C.h>
#include <sstream>
#include <string>
#include <iostream>
using namespace std;

// sets input pins
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

// pin state
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


// set the LCD number of columns and rows
int lcdColumns = 16;
int lcdRows = 2;

// set LCD address, number of columns and rows
// if you don't know your display address, run an I2C scanner sketch
LiquidCrystal_I2C lcd(0x27, lcdColumns, lcdRows);  



void setup(){
  // enable Serial
  Serial.begin(9600);

  // set pinMode
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

  // initialize LCD
  lcd.init();
  // turn on LCD backlight                      
  lcd.backlight();
}


String inputString = String("");
String password = String("7355608");

uint32_t KEYPAD_DELAY_TIME = 225;



void keypadNumber(int key) {
  Serial.print(key);
  Serial.println(" SWITCH IS DOWN");
  inputString += key;
  //Serial.println(inputString.length());
  //lcd.clear();
  lcd.print(inputString);
  delay(KEYPAD_DELAY_TIME);
}


void loop(){
  lcd.setCursor(0, 0);
  lcd.print("Enter Password: ");
  lcd.setCursor(0, 1);

  // read the switch
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
  resetPinState = digitalRead(RESET_PIN);
  enterPinState = digitalRead(ENTER_PIN);

  if (enterPinState == LOW){
    Serial.println("Password");
    if(inputString == password){
      lcd.clear();
      lcd.print("Correct");
      delay(3000);
    } else {
      lcd.clear();
      lcd.print("Wrong");
      delay(3000);
    }
    inputString = "";
    lcd.clear();
    lcd.print(inputString);
    delay(KEYPAD_DELAY_TIME);
  }
  
  else if (resetPinState == LOW){
    Serial.println("reset!");
    inputString = "";
    Serial.println(inputString.length());
    lcd.clear();
    lcd.print(inputString);
    delay(KEYPAD_DELAY_TIME);
  }

  // print if switch is down
  else if(zeroPinState == LOW) {
    keypadNumber(0);
  }
  else if(onePinState == LOW) {
    keypadNumber(1);
  }

  else if(twoPinState == LOW) {
    keypadNumber(2);
  }
  else if(threePinState == LOW) {
    keypadNumber(3);
  }

  else if(fourPinState == LOW) {
    keypadNumber(4);
  }
  else if(fivePinState == LOW) {
    keypadNumber(5);
  }

  else if(sixPinState == LOW) {
    keypadNumber(6);
  }
  else if(sevenPinState == LOW) {
    keypadNumber(7);
  }

  else if(eightPinState == LOW) {
    keypadNumber(8);
  }
  else if(ninePinState == LOW) {
    keypadNumber(9);
  }
 
}
