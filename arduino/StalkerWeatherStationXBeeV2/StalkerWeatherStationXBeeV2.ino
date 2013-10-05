/*
 * Stalker Weather Station for SeeedStudio Stalker board v2.0
 *    Stalker has XBee connector on board, RTC on board (based on R8025) and TMP102 based temperature sensor.
 *    Stalker board also has SD on board, but we are not using it here.
 * 
 * Complete description of project at:
 *      http://www.instructables.com/id/Wireless-outdoor-Arduino-weather-station-with-PC-l/
 * Downloading SW to Stalker board:
 *		The Stalker v2 is ATMega328P @ 8MHz!!!
 *              Choose board Arduino Pro/Pro Mini (3.3V, 8MHz) w/ ATmega32P
 *              If you choose 16MHz by mistake, the SW Serial will not work
*               (or work at 1/2 the speed you were setting - 4800 if you keep the defaults)
 * Wiring:
 *    Console is connected as SoftwareSerial, 9600 Tx on pin 11, Rx on pin 10
 *    XBee is connected to the default UART of the board
 *	  Xbee Sleep assert pin is connected to Digital 5 or Arduino
 *    DHT22 Temperature/Humidity sensor connected to Digital 3 of Arduino
 *    BMP025, Battery and TMP102 are on standard I2C of Arduino
 *
 */
#include <avr/sleep.h>
#include <Wire.h>
#include <SoftwareSerial.h>
#include <XBee.h>
#include <BMP085.h>

#include "TMP102.h"
#include "DHT.h"
#include "R8025.h"
#include "Battery.h"

/* ******************************************************************
	Constants for wiring of the circuit
   ****************************************************************** */
#define PIN_ONBOARD_LED		8
#define PIN_DHT22_SENSOR	2
#define PIN_XBEE_POWER		5
#define DEEP_SLEEP			1	// Sleep for long time including power saving, make it 0 for only 5 seconds sleep
#define SET_RTC_INIT            0
#define XBEE_BAUDRATE	     9600
#define CONSOLE_BAUDRATE     9600

#define COORDINATOR_SH 0x0013A200
#define COORDINATOR_SL 0x409E5682



//The following code is taken from sleep.h as Arduino Software v22 (avrgcc) in w32 does not have the latest sleep.h file
#ifndef sleep_bod_disable()
#define sleep_bod_disable() \
{ \
  uint8_t tempreg; \
  __asm__ __volatile__("in %[tempreg], %[mcucr]" "\n\t" \
                       "ori %[tempreg], %[bods_bodse]" "\n\t" \
                       "out %[mcucr], %[tempreg]" "\n\t" \
                       "andi %[tempreg], %[not_bodse]" "\n\t" \
                       "out %[mcucr], %[tempreg]" \
                       : [tempreg] "=&d" (tempreg) \
                       : [mcucr] "I" _SFR_IO_ADDR(MCUCR), \
                         [bods_bodse] "i" (_BV(BODS) | _BV(BODSE)), \
                         [not_bodse] "i" (~_BV(BODSE))); \
} 
#endif

/* ******************************************************************
	Special types we use in this program
   ****************************************************************** */
typedef struct tagDateTime
{
	unsigned char hour;
	unsigned char minute;
	unsigned char second;
	unsigned char week;
	unsigned char year;
	unsigned char month;
	unsigned char date; 
} CommDateTime;	// Holds all data on date and time

typedef struct tagWeatherStationSample
{
	// Battery and charger data
	int16_t 		bat_voltage;
	unsigned char	bat_status;
	unsigned char	charge_status;
	
	// TMP102 built in temperature sensor on Stalker
	int16_t 			tmp102Temp;
	
	// BMP085 (I2C temperature and barrometric sensor)
	int16_t			bmp085Temp;
	int32_t 		bmp085Pressure;
	int16_t 		bmp085Altitude;
	int16_t 		bmp085RealAltitude;
	
	// DHT 22 temperature and humidty sensor
	int16_t 		dht22Temp;
	int16_t 		dht22Humid;

	// Date and time
	CommDateTime		dt;

}  WeatherStationSample;	// All the data we will be sending over XBee

typedef union tagPayloadUnion
{
  WeatherStationSample sample;
  CommDateTime  dt;
} PayloadUnion;

typedef struct tagXbeeCommBuffer
{
	unsigned char command;
		#define WS_CMD_READING	'R'	// We are sending a WeatherStationSample data, normal case
		#define WS_CMD_GETTIME	'T'	// We are sending DateTime of current time and ask for 
									// DateTime to set local Rtc
		#define WS_CMD_POWER	'P'	// Power alert is set to ask for charging remote sensor.
									// Data is still WeatherStationSample.
		#define WS_CMD_HEAT		'H'	// Excessive heat, if temperature is above 50 degrees.
									// Data is still WeatherStationSample.
    unsigned char length;
	PayloadUnion payload;
} XbeeCommBuffer;
 


/* ******************************************************************
	Global variables
   ****************************************************************** */
// Battery and charger status from Stalker
unsigned int bat_read;			// Analog read battery voltage
float bat_voltage;				// Battery voltage
unsigned char charge_status;	// Battery charge status
unsigned char battery_status;	// If charging too low, high or OK 
   
// Stalker on-board RX8025 Real-Time-Clock
R8025 Rtc; //Create the R8025 object

static float tmp102Temp;

// DHT22 sensor variables
//static DHT dht22(PIN_DHT22_SENSOR);
static DHT dht22;
static float tempDht22;
static float humidDht22;

// BMP085 temperature and barrometer sensor variables
static BMP085 bmp085;
static float tempBmp085;
static int32_t presBmp085;
static float altBmp085;
static float realAltBmp085;

// XBee and communication buffers
static XbeeCommBuffer xcbBuffer;
static XBeeAddress64 addr64 = XBeeAddress64(COORDINATOR_SH, COORDINATOR_SL);
static ZBTxStatusResponse txStatus = ZBTxStatusResponse();
static ZBTxRequest zbTx = ZBTxRequest(addr64, (uint8_t *)&xcbBuffer, sizeof(xcbBuffer) );
static XBee	xbee = XBee();

SoftwareSerial NSerial(10, 11); // Rx, Tx
#define Console  NSerial

volatile bool wokeUpISR     = false;
static long lastSleepMillis = 0;


void setup () 
{
#if DEEP_SLEEP
	/*Initialize INT0 for accepting interrupts */
    PORTD |= 0x04; 
    DDRD &=~ 0x04;
#endif

    Serial.begin(XBEE_BAUDRATE);
    NSerial.begin(CONSOLE_BAUDRATE);
    Console.println("Starting...");

    SetupIOPorts(OUTPUT);
    digitalWrite(PIN_ONBOARD_LED, HIGH); // Turn on the on-board LED
    Wire.begin();
    Rtc.begin();
    
#if DEEP_SLEEP
    attachInterrupt(0, INT0_ISR, LOW); //Only LOW level interrupt can wake up from PWR_DOWN
    set_sleep_mode(SLEEP_MODE_PWR_DOWN);
#endif

    Console.println("\tXBee");
    Console.println("\t\tPower on...");
    digitalWrite(PIN_XBEE_POWER, LOW);	// Set the XBee sleep to low
    delay(1000); // Wait for XBee to be on
    Console.println("\t\tInitializing XBee");
    xbee.begin(Serial);
    Console.println("\tXBee init done");

    // Initialize the different sensors and RTC
    Console.println("\tBattery");
    Battery_init();
    Console.println("\tBMP085");
    bmp085.init();
    
    Console.println("\tTime and sleep");
#if SET_RTC_INIT
    // Set the data and time the first time...
    DateTime dt = DateTime(2011, 9, 3, 1, 02, 30, 7);
    Rtc.adjust(dt);
#endif
	
#if DEEP_SLEEP
    //Enable Interrupt
    Rtc.enableINTA_Interrupts(EveryMinute); //interrupt at  EverySecond, EveryMinute, EveryHour or EveryMonth
#endif
	
    Console.println("setup complete");
    digitalWrite(PIN_ONBOARD_LED, LOW); // Turn off on-boardLED
}


void loop () 
{
#if DEEP_SLEEP
	Rtc.refreshINTA(); //This function call is  a must to bring /INTA pin HIGH after interrupt
#endif
    
    lastSleepMillis = millis();	// Preserve millis of when we started
    digitalWrite(PIN_XBEE_POWER, LOW);	// Set the XBee sleep to low (turn on)
    digitalWrite(PIN_ONBOARD_LED, HIGH);//LED pin set to OUTPUT 

    Console.println("> In loop");
    ReadAllData();	// Read data from sensors
    PrintAllData(); // Show sensors data in text on console
	
    SendOnXBee();	// XBee module should return from hibernate by now

    // Prepare to sleep
    digitalWrite(PIN_ONBOARD_LED, LOW);
    digitalWrite(PIN_XBEE_POWER, HIGH);	// Ask the XBee to hibernate
 #if DEEP_SLEEP
    Rtc.refreshINTA(); //This function call is  a must to bring /INTA pin HIGH after interrupt
    attachInterrupt(0, INT0_ISR, LOW); 
    //Rtc.disableINTA_Interrupts(); //uncomment this if required
    char msgBuff[50];
    sprintf(msgBuff, "< Awake for: %ld mSec\r\n\t> Sleeping\r\n", millis() - lastSleepMillis);
    Console.print(msgBuff);

    //Shut down all peripherals like ADC before sleep. Refer Atmega328 manual
    
    //Power Down routine
    SetupIOPorts(INPUT);
    cli();
    sleep_enable();      // Set sleep enable bit
    //sleep_bod_disable(); // Disable brown out dection during sleep. Saves more power
    sei();
    delay(10); 			// This delay is required to allow print to complete
    sleep_cpu();        // Sleep the CPU as per the mode set earlier(power down)  
    sleep_disable();    // Wakes up sleep and clears enable bit. Before this ISR would have executed
    sei();
    delay(10); 			// This delay is required to allow CPU to stabilize
        
    Console.println("\t< Woke up from sleep\n");
    SetupIOPorts(OUTPUT);
#else
    delay(5000);
#endif
} 

void SetupIOPorts(int _io)
{
	pinMode(PIN_ONBOARD_LED, _io);	// LED pin set to OUTPUT
	pinMode(PIN_XBEE_POWER, _io);	// XBee pin9 sleep 
}
 
  
//Interrupt service routine for external interrupt on INT0 pin conntected to /INTA
void INT0_ISR()
{
	//Keep this as short as possible. Possibly avoid using function calls
    detachInterrupt(0); 
    Console.println("<< I'm ISR >>");
    wokeUpISR = true;
}

/* ******************************************************************
	Sensors Reading functions
   ****************************************************************** */
void   ReadDHT22(void)
{
	int dht22Error;
	bool cont = true;
	int i = 0;

    Console.println("\tDHT");  
	for (i=0, cont = true; cont && (i<3); i++)
	{
        Console.println("\t\tread");
		dht22Error = dht22.read22(PIN_DHT22_SENSOR);
		if ( dht22Error == 0)
		{
			tempDht22  = dht22.temperature;
			humidDht22 = dht22.humidity;
			cont = false;
		}
		else
		{
			delay(2100);
			char msg[30];
			sprintf(msg, "\t\tDHT22 Error: 0x%x", dht22Error);
			Console.println(msg);
		}
	}

  if (dht22Error != 0) // if in error clear the values
  {
        tempDht22 = 0;
        humidDht22 = 0;
   }
   else
     Console.println("\t\tOK");
}
//======================================  
  
void ReadMBP085(void)
{
  int32_t t = 0L, p = 0L, a = 0L;
  
  // Read from BMP085 (Barrometer) sensor
  bmp085.getTemperature(&t);
  bmp085.getPressure(&p);
  bmp085.getAltitude(&a);
  tempBmp085    = (float)t / 10.0;
  presBmp085    = p;
  altBmp085  	  = a;
  realAltBmp085 = a;
}

//================================= 
void ReadAllData()
{
    // Read all the sensors
    tmp102Temp = Tmp102.getTemperatureInCelsius();
    Battery_charge_status();
    Battery_voltage_read();
    ReadDHT22();
    ReadMBP085(); 
}


/* ******************************************************************
	Display functions for the various sensors data we have
   ****************************************************************** */
void Print_tmp102_data(void)
{
    Console.print("\tTMP102\t");
    Console.print(tmp102Temp);
    Console.println("*C");
}
//==================================


void Print_Battery_data(void)
{
	Console.println("\tBat");
        Console.print("\t\tStatus = ");
	switch (charge_status)
	{
		case 0x01:
		  Console.println("Sleeping");
		  break;
		case 0x02:
		  Console.println("Complete");
		  break;
		case 0x04:
		  Console.println("Charging");
		  break;
		case 0x08:
		  Console.println("No Battery");
		  break;
	}
	Console.print("\t\tVoltage = ");
	Console.print(bat_voltage);
	Console.println("V");
}
//==============================================


void Print_RX8025_time(void)
{
	static char weekDay[][4] = {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
	DateTime now = Rtc.now();
        int year;
	
	// Copy time to communication buffer
	xcbBuffer.payload.sample.dt.hour 	= now.hour();
	xcbBuffer.payload.sample.dt.minute	= now.minute();
	xcbBuffer.payload.sample.dt.second	= now.second();
	xcbBuffer.payload.sample.dt.week	= now.dayOfWeek();
    year = now.year();
    if (year > 2000)
       year -= 2000;
	xcbBuffer.payload.sample.dt.year	= year;
	xcbBuffer.payload.sample.dt.month	= now.month();
	xcbBuffer.payload.sample.dt.date	= now.date();

	char msgBuff[50];
	sprintf(msgBuff, "\tRX8025\t%s. %02d/%02d/%d\t%d:%02d:%02d",
		weekDay[now.dayOfWeek()],
		now.date(), now.month(), now.year(),
		now.hour(), now.minute(), now.second() );
	Console.println(msgBuff);
}
//======================================

void Print_DH22_Sensor(void)
{
	Console.print("\tDHT22\tT = ");
	Console.print(tempDht22);
	Console.print("*C\tH = ");
	Console.print(humidDht22);
	Console.println("%");
}
//======================================

void Print_BMP085_Sensor(void)
{
  int hex;
  
    Console.println("\tBMP085");
    
    Console.print("\t\tT = ");
    Console.print(tempBmp085);
    Console.println("*C");
    
    Console.print("\t\tP = ");
    Console.print(presBmp085);
    Console.println("Pa");
    
    // Calculate altitude assuming 'standard' barometric
    // pressure of 1013.25 millibar = 101325 Pascal
    Console.print("\t\tA = ");
    Console.print(altBmp085);
    Console.println("m");

  // you can get a more precise measurement of altitude
  // if you know the current sea level pressure which will
  // vary with weather and such. If it is 1015 millibars
  // that is equal to 101500 Pascals.
    Console.print("\t\tReal altitude = ");
    Console.print(realAltBmp085);
    Console.println("m");
}
//======================================

void PrintAllData()
{
	Print_RX8025_time();
	Print_tmp102_data();
	Print_DH22_Sensor();
	Print_BMP085_Sensor();
	Print_Battery_data();
}


/* ******************************************************************
	Communication functions on the XBee
   ****************************************************************** */
static void SendOnXBee(void)
{
	GetWeatherSample();		// Fill the communication buffer
    xbee.getNextFrameId();
	Console.println("\tXBee");
    Console.println("\t\tSending over XBee");
	xbee.send(zbTx);				// Send via the XBee
	
	// after sending a tx request, we expect a status response
    // wait up to half second for the status response 
    Console.println("\t\tReading packet...");

	if (xbee.readPacket(5000))
	{
		// got a response! should be a znet tx status
    	if (xbee.getResponse().getApiId() == ZB_TX_STATUS_RESPONSE)
		{
    	   xbee.getResponse().getZBTxStatusResponse(txStatus);
           Console.println("\t\tGot response");
    	   // get the delivery status, the fifth byte
           if ( txStatus.getDeliveryStatus() == SUCCESS )
			   Console.println("\t\t\tGot success"); // success.  time to celebrate
		   else
               Console.println("\t\t\tgot failue. is remore unit on?"); // the remote XBee did not receive our packet. is it powered on?
        }
		else
			Console.println("\t\t\tWhat I got is not a response");
    }
	else
	{
		// readPacket returned 0
		if (xbee.getResponse().isError())
		{
			Console.print("\t\tError reading packet.  Error code: ");        
			Console.println(xbee.getResponse().getErrorCode());
		}
		else
		{
			// local XBee did not provide a timely TX Status Response -- should not happen
			// Should we reset the XBee module?
			Console.println("\t\tlocal XBee did not provide TX Status, should not happen");
		}
    }
} 

#define FloatToInt16(f) (round((double)(f) * (double)100.0))

static void GetWeatherSample (void)
{
  	xcbBuffer.command = WS_CMD_READING;
    xcbBuffer.length = sizeof(WeatherStationSample);

	if (tempBmp085 >= 50.0)
		xcbBuffer.command = WS_CMD_HEAT;
        
    if (battery_status == BAT_TOO_WEEK)
		xcbBuffer.command = WS_CMD_POWER;
        
	// Battery and charger
	xcbBuffer.payload.sample.bat_voltage = FloatToInt16(bat_voltage );
	xcbBuffer.payload.sample.bat_status = battery_status;
	xcbBuffer.payload.sample.charge_status = charge_status;
	
	// TMP102
	xcbBuffer.payload.sample.tmp102Temp = FloatToInt16(tmp102Temp);

	// BMP085
	xcbBuffer.payload.sample.bmp085Temp = FloatToInt16(tempBmp085); 
	xcbBuffer.payload.sample.bmp085Pressure = presBmp085;
	xcbBuffer.payload.sample.bmp085Altitude = FloatToInt16(altBmp085);
	xcbBuffer.payload.sample.bmp085RealAltitude = FloatToInt16(realAltBmp085);

	// DHT 22
	xcbBuffer.payload.sample.dht22Temp = FloatToInt16(tempDht22);
	xcbBuffer.payload.sample.dht22Humid = FloatToInt16(humidDht22);

}
//======================================
