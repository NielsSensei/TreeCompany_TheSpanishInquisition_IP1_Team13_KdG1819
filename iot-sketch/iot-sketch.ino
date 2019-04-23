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

void setup() {
  // put your setup code here, to run once:
  pinMode(GreenButton, INPUT);
  pinMode(GreenLED, OUTPUT);
  pinMode(RedButton, INPUT);
  pinMode(RedLED, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:

  if(digitalRead(GreenButton)){
    //SEND POSITIVE TO LORA
    
  }
  if(digitalRead(RedButton)){
    //SEND NEGATIVE TO LORA
    
  }
}
