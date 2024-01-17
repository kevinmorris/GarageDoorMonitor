#include <Arduino.h>
#include <WiFiS3.h>
#include "app_settings.h"

const char* networkName = WIFI_NETWORK_NAME;
const char* networkPassword = WIFI_PASSWORD;
int status = WL_IDLE_STATUS;

WiFiClient client;

float voltage = -1.0f;
int configCheck = 0;
int voltageEnabled = 0;
double voltageThreshold = DEFAULT_VOLTAGE_THRESHOLD;

float getVoltage(pin_size_t pin);
bool postIsOpen(bool isOpen);
bool postVoltage(float voltage);
char* getConfig(char* name);

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
    Serial.print("Voltage: ");
    Serial.println(currentVoltage);

    if(configCheck == 0) {
        configCheck = CONFIG_CHECK_CYCLE;
        voltageEnabled = atoi(getConfig("voltageEnabled"));
        voltageThreshold = atof(getConfig("voltageThreshold"));
    } else {
        configCheck -= 1;
    }

    if(voltageEnabled > 0) {
        postVoltage(currentVoltage);
    }

    if(currentVoltage > voltageThreshold &&
        (voltage <= voltageThreshold || voltage == -1) &&
       postIsOpen(false)) {

        voltage = currentVoltage;

    } else if(currentVoltage <= voltageThreshold &&
               (voltage > voltageThreshold || voltage == -1) &&
              postIsOpen(true)) {

        voltage = currentVoltage;
    }

    delay(10000);
}

float getVoltage(pin_size_t pin) {

    int sensorValue = analogRead(pin);
    return sensorValue * (5.0f / 1023.0f);
}

bool postIsOpen(bool isOpen) {

    Serial.print("Posting isOpen: ");
    Serial.println(isOpen);

    bool success = false;
    if(client.connect(DOMAIN_PROXY, PORT1)) {
        Serial.println("Connected to server");
        Serial.print("POST /garage-door/1?isOpen=");
        Serial.print(isOpen ? 1 : 0);
        Serial.println(" HTTP/1.1");
        Serial.print("Host: ");
        Serial.println(DOMAIN);
        Serial.print("Authorization: Basic ");
        Serial.println("##############################################");
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

        success = true;
    }

    return success;
}

bool postVoltage(float voltage) {

    Serial.print("Posting voltage: ");
    Serial.println(voltage);

    bool success = false;
    if(client.connect(DOMAIN_PROXY, PORT1)) {
        Serial.println("Connected to server");
        Serial.print("POST /voltage?value=");
        Serial.print(voltage);
        Serial.println(" HTTP/1.1");
        Serial.print("Host: ");
        Serial.println(DOMAIN);
        Serial.print("Authorization: Basic ");
        Serial.println("##############################################");
        Serial.println("Connection: close");
        Serial.println();

        client.print("POST /voltage?value=");
        client.print(voltage);
        client.println(" HTTP/1.1");
        client.print("Host: ");
        client.println(DOMAIN);
        client.print("Authorization: Basic ");
        client.println(AUTHORIZATION_HEADER);
        client.println("Connection: close");
        client.println();

        success = true;
    }

    return success;
}

char* getConfig(char* name) {

    Serial.print("Getting config: ");
    Serial.println(name);

    if(client.connect(DOMAIN_PROXY, PORT1)) {
        Serial.println("Connected to server");
        Serial.print("GET /config/");
        Serial.print(name);
        Serial.println(" HTTP/1.1");
        Serial.print("Host: ");
        Serial.println(DOMAIN);
        Serial.print("Authorization: Basic ");
        Serial.println(AUTHORIZATION_HEADER);
        Serial.println();

        client.print("GET /config/");
        client.print(name);
        client.println(" HTTP/1.1");
        client.print("Host: ");
        client.println(DOMAIN);
        client.print("Authorization: Basic ");
        client.println(AUTHORIZATION_HEADER);
        client.println("Connection: close");
        client.println();

        delay(5000);

        Serial.println("Writing config response");

        while(client.available()) {
            char c = client.read();

            //Crude json body parsing

            //Look for the body opening brace and start accumulating
            if(c == '{') {
                static char body[128];
                int index = 1;

                body[0] = c;

                while(index < 128 && (c = client.read()) != '\r') {
                    body[index] = c;
                    index += 1;
                }

                while(index < 128) {
                    body[index] = '\0';
                    index += 1;
                }

                Serial.println(body);

                //pull out the value json attribute
                char* value = strstr(body, "\"value\"");
                if(value == NULL) {
                    return NULL;
                }

                value += 9;
                char* current = value;
                while(*current != '\"') {
                    current += 1;
                }

                *current = '\0';
                Serial.print("Config result: ");
                Serial.println(value);
                return value;
            }
        }
        Serial.println("Completed config response");
    }

    return NULL;
}