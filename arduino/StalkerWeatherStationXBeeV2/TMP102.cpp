/*
 * Temperature sensor TMP102 Interface code
 */
#include "TMP102.h"

void TMP102::readTemperatureRegister(void)
{
	unsigned char MSB,LSB;
	
	Wire.beginTransmission(TMP102_ADDRESS); //TMP102 Device (I2C Slave) address
	Wire.write(0x00); //Temperature registers starts at address 0x00
	Wire.endTransmission();
	
	Wire.requestFrom(TMP102_ADDRESS, 2);  
	
	MSB = Wire.read();  //Read MSB of Temperature Register
	LSB = Wire.read();  //Read LSB of Temperature Register
	
	/*
	Temperature Register Format:
	
	MSB:  T11 T10 T9 T8 T7 T6 T5 T4
	LSB:  T3  T2  T1 T0 0  0  0  0
	*/
	
	temperature = (MSB << 8 )  | LSB; 
	temperature >>= 4 ;   // Value is left shifted in above table. Hence Right shift.
	
	if(temperature & 0b100000000000) //T11 indicates sign (either +ve or -ve)
	{
		//Negative temperature 
		//Negative numbers are represented in binary two's complement format according to data sheet
		temperature = temperature - 1;
		temperature ^= 0xFFF;
		temperatureCelsius = temperature * 0.0625 * (-1); //Each bit is 0.0625 deg Celsius
	}
	else
	{
		//Positive temperature 
		temperatureCelsius = temperature * 0.0625;
	}
}

//Return temperature (each bit indicates 0.0625 deg celsius)
int TMP102::getTemperature(void)
{
	readTemperatureRegister();
	return temperature;
}

//Return temperature in celsius
float TMP102::getTemperatureInCelsius(void)
{
	readTemperatureRegister();
	return temperatureCelsius;
}


TMP102 Tmp102; //Create object





