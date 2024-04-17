#include <Arduino.h>
#include <LiquidCrystal_I2C.h>
#include <sstream>
#include <string>
#include <iostream>
using namespace std;

// sets input pins
constexpr byte ZERO_PIN = 16;
constexpr byte ONE_PIN = 17;
constexpr byte RESET_PIN = 13;
constexpr byte ENTER_PIN = 26;

// pin state
bool zeroPinState;
bool onePinState;
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
  pinMode(RESET_PIN, INPUT_PULLUP);
  pinMode(ENTER_PIN, INPUT_PULLUP);

  // initialize LCD
  lcd.init();
  // turn on LCD backlight                      
  lcd.backlight();
}


String inputString = String("");
String password = String("0011");

uint32_t KEYPAD_DELAY_TIME = 225;

void loop(){
  lcd.setCursor(0, 0);
  lcd.print("Enter Password: ");
  lcd.setCursor(0, 1);

  // read the switch
  zeroPinState = digitalRead(ZERO_PIN);
  onePinState = digitalRead(ONE_PIN);
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
  
  if (resetPinState == LOW){
    Serial.println("reset!");
    inputString = "";
    Serial.println(inputString.length());
    lcd.clear();
    lcd.print(inputString);
    delay(KEYPAD_DELAY_TIME);
  }

  // print if switch is down
  if(zeroPinState == LOW) {
    Serial.println("zero SWITCH IS DOWN");
    inputString += "0";
    Serial.println(inputString.length());
    //counter++;
    //lcd.clear();

    lcd.print(inputString);
    delay(KEYPAD_DELAY_TIME);
  }
  if(onePinState == LOW) {
    Serial.println("one SWITCH IS DOWN");
    inputString += "1";
    Serial.println(inputString.length());
    //lcd.clear();

    lcd.print(inputString);
    delay(KEYPAD_DELAY_TIME);
  }


 
}
