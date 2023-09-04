#include <Arduino.h>
#include <WiFiS3.h>
#include "app_settings.h"

const char* networkName = WIFI_NETWORK_NAME;
const char* networkPassword = WIFI_PASSWORD;
int status = WL_IDLE_STATUS;

WiFiClient client;

float voltage = 0.0f;

float getVoltage(pin_size_t pin);
void postIsOpen(bool isOpen);

void setup() {

    Serial.begin(9600);
    while(!Serial);

    Serial.println("Starting...");
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

    float currentVoltage = getVoltage(A0);
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

    if(client.connect(DOMAIN1, 8888)) {
        Serial.println("Connected to server");
        Serial.print("POST /garage-door/1?isOpen=");
        Serial.print(isOpen ? 1 : 0);
        Serial.println(" HTTP/1.1");
        Serial.print("Host: ");
        Serial.println(DOMAIN);
        Serial.print("Authorization: Basic ");
        Serial.println(AUTHORIZATION_HEADER);
        Serial.println("Connection: close");
        Serial.println();

        client.print("POST /garage-door/1?isOpen=");
        client.print(isOpen ? 1 : 0);
        client.println(" HTTP/1.1");
        client.print("Host: ");
        client.println(DOMAIN);
        client.print("Authorization: Basic ");
        client.println(AUTHORIZATION_HEADER);
        client.println("Connection: close");
        client.println();
    }
}
