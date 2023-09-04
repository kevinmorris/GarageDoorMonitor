#include <Arduino.h>
#include <SPI.h>
#include <WiFi.h>
#include "app_settings.h"

const char* networkName = WIFI_NETWORK_NAME;
const char* networkPassword = WIFI_PASSWORD;
int status = WL_IDLE_STATUS;

float voltage = 0.0f;

float getVoltage(pin_size_t pin);
void postIsOpen(bool isOpen);

void setup() {

    Serial.begin(9600);
    while(!Serial);

    Serial.println("Starting...");
    // check for the WiFi module:
    if (WiFi.status() == WL_NO_MODULE) {
        Serial.println("Communication with WiFi module failed");
        while (true);
    }

    while(status != WL_CONNECTED) {

        Serial.print("Connecting to ");
        Serial.println(networkName);

        status = WiFi.begin(networkName, networkPassword);
        Serial.println(status);
        delay(5000);
    }

    Serial.println("Connected");
}

void loop() {

    float currentVoltage = voltage > VOLTAGE_THRESHOLD ? 0.0f : 2.5f;
    if(currentVoltage > VOLTAGE_THRESHOLD && voltage <= VOLTAGE_THRESHOLD) {
        Serial.println("Door is closed");
        voltage = currentVoltage;
        postIsOpen(false);
    } else if(currentVoltage <= VOLTAGE_THRESHOLD && voltage > VOLTAGE_THRESHOLD){
        Serial.println("Door is open");
        voltage = currentVoltage;
        postIsOpen(true);
    }

    delay(10000);
}

float getVoltage(pin_size_t pin) {

    int sensorValue = analogRead(pin);
    return sensorValue * (5.0f / 1023.0f);
}

void postIsOpen(bool isOpen) {

    Serial.print("Posting isOpen: ");
    Serial.println(isOpen);

    IPAddress addr;
//    WiFi.hostByName(DOMAIN, addr);
//    Serial.print("IP address: ");
//    Serial.println(addr);
//    if(client.connect(DOMAIN, 80)) {
//        Serial.println("Connected to server");
//        Serial.print("POST /garage-door/1?isOpen=");
//        Serial.print(isOpen ? 1 : 0);
//        Serial.println(" HTTP/1.1");
//        Serial.print("Host: ");
//        Serial.println(DOMAIN);
//        Serial.print("Authorization: Basic ");
//        Serial.println(AUTHORIZATION_HEADER);
//        Serial.println("Connection: close");
//        Serial.println();
//    }
}
