#ifndef TMP102_h
#define TMP102_h

//**********************************************
#include <Arduino.h>
#include <Wire.h>

#define TMP102_ADDRESS 72 // I2C device address (In Seeeduino Stalker V2 ADD0 is connected to ground) 

class TMP102 
{
public:
	int getTemperature(void);
	float getTemperatureInCelsius(void);

private:
	int temperature; 
	float temperatureCelsius;
	
	void readTemperatureRegister(void);
};

extern TMP102 Tmp102;

#endif



