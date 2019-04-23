// START LoRa includes
#include <RadioHead.h>
#include <RHCRC.h>
#include <RHDatagram.h>
#include <RHEncryptedDriver.h>
#include <RHGenericDriver.h>
#include <RHGenericSPI.h>
#include <RHHardwareSPI.h>
#include <RHMesh.h>
#include <RHNRFSPIDriver.h>
#include <RHReliableDatagram.h>
#include <RHRouter.h>
#include <RHSoftwareSPI.h>
#include <RHSPIDriver.h>
#include <RHTcpProtocol.h>
#include <RH_ASK.h>
#include <RH_CC110.h>
#include <RH_E32.h>
#include <RH_MRF89.h>
#include <RH_NRF24.h>
#include <RH_NRF51.h>
#include <RH_NRF905.h>
#include <RH_RF22.h>
#include <RH_RF24.h>
#include <RH_RF69.h>
#include <RH_RF95.h>
#include <RH_Serial.h>
#include <RH_TCP.h>
// END LoRa includes

// START pin-mapping
#define GreenButton 0
#define GreenLED 1
#define RedButton 3
#define RedLED 4
// END pin-mapping

// VARIABLES
int greenButtonState;             // the current reading from the input pin
int redButtonState;             // the current reading from the input pin
int lastGreenButtonState = LOW;   // the previous reading from the input pin
int lastRedButtonState = LOW;   // the previous reading from the input pin

int positive = LOW; // trigger for LoRa communication
int negative = LOW; // trigger for LoRa communication

unsigned long lastGreenDebounceTime = 0;  // the last time the output pin was toggled
unsigned long lastRedDebounceTime = 0;  // the last time the output pin was toggled
unsigned long debounceDelay = 50;    // the debounce time; increase if the output flickers

void sendPositive(){
  
}

void sendNegative(){
  
}

void setup() {
  // put your setup code here, to run once:
  pinMode(GreenButton, INPUT);
  pinMode(GreenLED, OUTPUT);
  pinMode(RedButton, INPUT);
  pinMode(RedLED, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:

  // START INPUT & DEBOUNCING

  // read the state of the buttons into local variables
  int readingGreen = digitalRead(GreenButton);
  int readingRed = digitalRead(RedButton);

  // if button state changes
  if (readingGreen != lastGreenButtonState){
    // reset the debouncing timer
    lastGreenDebounceTime = millis();
  }
  // if button state changes
  if (readingRed != lastRedButtonState){
    // reset the debouncing timer
    lastRedDebounceTime = millis();
  }

  if ((millis() - lastGreenDebounceTime) > debounceDelay){
    // the reading has been there longer than the debounce delay, so assume it as the actual current state

    // if button state has changed
    if (readingGreen != greenButtonState){
      greenButtonState = readingGreen;

      //only sendPositive if button state is HIGH
      if (greenButtonState == HIGH){
        positive = HIGH;
      }
    }
  }

  if ((millis() - lastRedDebounceTime) > debounceDelay){
    // the reading has been there longer than the debounce delay, so assume it as the actual current state

    // if button state has changed
    if (readingRed != redButtonState){
      redButtonState = readingRed;

      //only sendNegative if button state is HIHG
      if (redButtonState == HIGH){
        negative = HIGH;
      }
    }
  }

  // save the reading. Next time through the loop, it'll be lastColourButtonState
  lastGreenButtonState = readingGreen;
  lastRedButtonState = readingRed;

  // END INPUT & DEBOUNCING

  // only use LoRa communication at the end of the loop to prevent delays and problems with input reading and input debouncing
  if(positive){
    sendPositive()
    positive = LOW;
  }
  if(){
    sendNegative()
    negative = LOW;
  }
}
