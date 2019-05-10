// START LoRa includes
#include <LoRa.h>
#include <SPI.h>
// END LoRa includes

// START pin-mapping
#define GreenButton 5
#define GreenLED 1
#define RedButton 3
#define RedLED 4
// END pin-mapping

#define DEVICE_ID "KdG"

// VARIABLES
int greenButtonState;             // the current reading from the input pin
int redButtonState;             // the current reading from the input pin
int lastGreenButtonState = LOW;   // the previous reading from the input pin
int lastRedButtonState = LOW;   // the previous reading from the input pin

int positive = LOW; // trigger for LoRa communication
int negative = LOW; // trigger for LoRa communication

int count = 0;

unsigned long lastGreenDebounceTime = 0;  // the last time the output pin was toggled
unsigned long lastRedDebounceTime = 0;  // the last time the output pin was toggled
unsigned long debounceDelay = 50;    // the debounce time; increase if the output flickers

void sendPositive() {
  Serial.print("Sending packet ");
  Serial.print(count);
  Serial.println("... ");

  int status = LoRa.beginPacket();
  if (status) {
    LoRa.print("<");
    LoRa.print(DEVICE_ID);
    LoRa.print(">");
    unsigned long m = millis();
    LoRa.print(m);
    LoRa.print(",");
    LoRa.print(count++);
    LoRa.endPacket();
    Serial.println("Packet sent");
  } else {
    Serial.println("Error sending packet");
  }
  
}

void sendNegative() {
  Serial.println("Red");
  digitalWrite(RedLED, HIGH);
  delay(200);
  digitalWrite(RedLED, LOW);
}

void setup() {
  Serial.begin(9600);
  Serial.println("Setup LoRa shield...");
  if (!LoRa.begin(868100000)) {
    Serial.println("Starting LoRa failed!");
    while(1);
  }
  LoRa.setSyncWord(0x34);
  Serial.println("LoRa shield initialized");
  
  // put your setup code here, to run once:
  pinMode(GreenButton, INPUT);
  pinMode(GreenLED, OUTPUT);
  pinMode(RedButton, INPUT);
  pinMode(RedLED, OUTPUT);

  for(int i=0; i<3; i++){
    digitalWrite(GreenLED, HIGH);
    digitalWrite(RedLED, HIGH);
    delay(400);
    digitalWrite(GreenLED, LOW);
    digitalWrite(RedLED, LOW);
    delay(400);
  }  
  Serial.println("Ready!");
}

void loop() {
  // put your main code here, to run repeatedly:

  // START INPUT & DEBOUNCING

  // read the state of the buttons into local variables
  int readingGreen = digitalRead(GreenButton);
  int readingRed = digitalRead(RedButton);

  // if button state changes
  if (readingGreen != lastGreenButtonState) {
    // reset the debouncing timer
    lastGreenDebounceTime = millis();
  }
  // if button state changes
  if (readingRed != lastRedButtonState) {
    // reset the debouncing timer
    lastRedDebounceTime = millis();
  }

  if ((millis() - lastGreenDebounceTime) > debounceDelay) {
    // the reading has been there longer than the debounce delay, so assume it as the actual current state

    // if button state has changed
    if (readingGreen != greenButtonState) {
      greenButtonState = readingGreen;

      //only sendPositive if button state is HIGH
      if (greenButtonState == HIGH) {
        positive = HIGH;
      }
    }
  }

  if ((millis() - lastRedDebounceTime) > debounceDelay) {
    // the reading has been there longer than the debounce delay, so assume it as the actual current state

    // if button state has changed
    if (readingRed != redButtonState) {
      redButtonState = readingRed;

      //only sendNegative if button state is HIHG
      if (redButtonState == HIGH) {
        negative = HIGH;
      }
    }
  }

  // save the reading. Next time through the loop, it'll be lastColourButtonState
  lastGreenButtonState = readingGreen;
  lastRedButtonState = readingRed;

  // END INPUT & DEBOUNCING

  // only use LoRa communication at the end of the loop to prevent delays and problems with input reading and input debouncing
  if (positive) {
    sendPositive();
    positive = LOW;
  }
  if (negative) {
    sendNegative();
    negative = LOW;
  }
}
