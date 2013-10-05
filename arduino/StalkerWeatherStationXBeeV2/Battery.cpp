
#include "Battery.h"
#include <Arduino.h>
#include <SoftwareSerial.h>

extern SoftwareSerial NSerial;
#define Console NSerial

//**********************************
/*
void Battery_init(void)
 init Battery IOs and ADC7
 */
void Battery_init(void)
{
	analogReference(INTERNAL);//internal 1.1v in Atmega328
	pinMode(CH_Status,INPUT);
	pinMode(OK_Status,INPUT);
	digitalWrite(CH_Status,HIGH);//pull high
	digitalWrite(OK_Status,HIGH);//pull high
	Battery_charge_status();
	Battery_voltage_read();
}

//====================================
void Battery_charge_status(void)
{
	boolean OK = digitalRead(OK_Status);
	boolean CH = digitalRead(CH_Status);
	if(OK&&CH)//Charger is sleeping, will auto start charge battery when FB voltage less than 3.0v
	{
		charge_status = 0x01;
	}
	else if(CH)//Charge complete
	{
		charge_status = 0x02;
	}
	else if(OK)//Charging
	{
		charge_status = 0x04;
	}
	else//USB power exsit, battery is not exist
	{
		charge_status = 0x08;
		//Serial.println("Bad Battery or Battery is not exist");
	}
	
	Console.print("\tBattery Charger\tOK=");
        Console.print(OK ? "T" : "F");
        Console.print(" CH=");
        Console.println(CH ? "T" : "F");
}

//=====================================
void Battery_voltage_read(void)
{
	bat_read = analogRead(BAT_voltage);
	
	//  (1/1024)*6=0.0064453125,
	bat_voltage = (float)bat_read * 0.0064453125;
	
	//Serial.print(" -->Battery data = ");
	//Serial.println(bat_read);
	
	if(bat_read<570)// less than 3.48v
	{
		battery_status = BAT_TOO_WEEK;
	}
	else if(bat_read>680)// more than 4.22 v
	{
		battery_status = BAT_TOO_HIGH;
	}
	else
	{
		battery_status = BAT_OK;
	}
}














