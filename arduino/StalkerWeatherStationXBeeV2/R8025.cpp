// R8025 Class is by Seeed Technology Inc(http://www.seeedstudio.com) and used
// in Seeeduino Stalker v2 for battery management(MCU power saving mode)
// & to generate timestamp for data logging. DateTime Class is a modified
// version supporting day-of-week.

// Original DateTime Class and its utility code is by Jean-Claude Wippler at JeeLabs
// http://jeelabs.net/projects/cafe/wiki/RTClib 
// Released under MIT License http://opensource.org/licenses/mit-license.php

#include <Wire.h>
#include <avr/pgmspace.h>
#include "R8025.h"
#include <Arduino.h>

#define SECONDS_PER_DAY 86400L

////////////////////////////////////////////////////////////////////////////////
// utility code, some of this could be exposed in the DateTime API if needed

static uint8_t daysInMonth [] PROGMEM = { 31,28,31,30,31,30,31,31,30,31,30,31 };

// number of days since 2000/01/01, valid for 2001..2099
static uint16_t date2days(uint16_t y, uint8_t m, uint8_t d) {
    if (y >= 2000)
        y -= 2000;
    uint16_t days = d;
    for (uint8_t i = 1; i < m; ++i)
        days += pgm_read_byte(daysInMonth + i - 1);
    if (m > 2 && y % 4 == 0)
        ++days;
    return days + 365 * y + (y + 3) / 4 - 1;
}

static long time2long(uint16_t days, uint8_t h, uint8_t m, uint8_t s) {
    return ((days * 24L + h) * 60 + m) * 60 + s;
}

static uint8_t conv2d(const char* p) {
    uint8_t v = 0;
    if ('0' <= *p && *p <= '9')
        v = *p - '0';
    return 10 * v + *++p - '0';
}

////////////////////////////////////////////////////////////////////////////////
// DateTime implementation - ignores time zones and DST changes
// NOTE: also ignores leap seconds, see http://en.wikipedia.org/wiki/Leap_second

DateTime::DateTime (long t) {
    ss = t % 60;
    t /= 60;
    mm = t % 60;
    t /= 60;
    hh = t % 24;
    uint16_t days = t / 24;
    uint8_t leap;
    for (yOff = 0; ; ++yOff) {
        leap = yOff % 4 == 0;
        if (days < 365 + leap)
            break;
        days -= 365 + leap;
    }
    for (m = 1; ; ++m) {
        uint8_t daysPerMonth = pgm_read_byte(daysInMonth + m - 1);
        if (leap && m == 2)
            ++daysPerMonth;
        if (days < daysPerMonth)
            break;
        days -= daysPerMonth;
    }
    d = days + 1;
}

DateTime::DateTime (uint16_t year, uint8_t month, uint8_t date, uint8_t hour, uint8_t min, uint8_t sec, uint8_t wd) {
    if (year >= 2000)
        year -= 2000;
    yOff = year;
    m = month;
    d = date;
    hh = hour;
    mm = min;
    ss = sec;
    wday = wd;
}

// A convenient constructor for using "the compiler's time":
//   DateTime now (__DATE__, __TIME__);
// NOTE: using PSTR would further reduce the RAM footprint
DateTime::DateTime (const char* date, const char* time) {
    // sample input: date = "Dec 26 2009", time = "12:34:56"
    yOff = conv2d(date + 9);
    // Jan Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec 
    switch (date[0]) {
        case 'J': m = date[1] == 'a' ? 1 : m = date[2] == 'n' ? 6 : 7; break;
        case 'F': m = 2; break;
        case 'A': m = date[2] == 'r' ? 4 : 8; break;
        case 'M': m = date[2] == 'r' ? 3 : 5; break;
        case 'S': m = 9; break;
        case 'O': m = 10; break;
        case 'N': m = 11; break;
        case 'D': m = 12; break;
    }
    d = conv2d(date + 4);
    hh = conv2d(time);
    mm = conv2d(time + 3);
    ss = conv2d(time + 6);
}

long DateTime::get() const {
    uint16_t days = date2days(yOff, m, d);
    return time2long(days, hh, mm, ss);
}

static uint8_t bcd2bin (uint8_t val) { return val - 6 * (val >> 4); }
static uint8_t bin2bcd (uint8_t val) { return val + 6 * (val / 10); }

////////////////////////////////////////////////////////////////////////////////
// RTC RX8025 implementation

uint8_t R8025::begin(void) {
  unsigned char ctReg1=0, ctReg2=0;
  Wire.beginTransmission(RX8025_ADDRESS);
  Wire.write(RX8025_CTRL1_REG);     //CTRL1 Address
  // set the clock to 24hr format  
  ctReg1 |= RX8025_CTRL1_1224_BIT; 
  Wire.write(ctReg1);
  //Set VDSL and XST bits. Also clear PON (power on reset) flag. 
  Wire.write(RX8025_CTRL2_VDSL_BIT | RX8025_CTRL2_XST_BIT );   
  Wire.endTransmission();
  delay(10);

  return 1; 
  // This implementation can be changed to return 0 when power on reset is
  // deteted to instruct user to adjust current date-time.
}

//Adjust the time-date specified in DateTime format
//writing any non-existent time-data may interfere with normal operation of the RTC
void R8025::adjust(const DateTime& dt) {

  Wire.beginTransmission(RX8025_ADDRESS);
  Wire.write(RX8025_SEC_REG);  //beginning from SEC Register address

  Wire.write(bin2bcd(dt.second())); 
  Wire.write(bin2bcd(dt.minute()));
  Wire.write(bin2bcd(dt.hour()));
  Wire.write(dt.dayOfWeek());
  Wire.write(bin2bcd(dt.date()));
  Wire.write(bin2bcd(dt.month()));
  Wire.write(bin2bcd(dt.year() - 2000));  

  Wire.endTransmission();

}

//Read the current time-date and return it in DateTime format
DateTime R8025::now() {
  Wire.beginTransmission(RX8025_ADDRESS);
  Wire.write(0x00);	
  Wire.endTransmission();
  
  Wire.requestFrom(RX8025_ADDRESS, 8);
  Wire.read(); //ignore this data read from 0xF. Refer app note. Only Mode 3 Read operation
                  // is supported by wire library. i.e 0xF->0x0->0x2
  uint8_t ss = bcd2bin(Wire.read());
  uint8_t mm = bcd2bin(Wire.read());
  uint8_t hh = bcd2bin(Wire.read());
  uint8_t wd =  Wire.read();
  uint8_t d = bcd2bin(Wire.read());
  uint8_t m = bcd2bin(Wire.read());
  uint16_t y = bcd2bin(Wire.read()) + 2000;
  
  return DateTime (y, m, d, hh, mm, ss, wd);
}

//Enable periodic interrupt at /INTA pin. Supports only the level interrupt
//for consistency with other /INTA interrupts. All interrupts works like single-shot counter
//Use refreshINTA() to re-enable interrupt.
void R8025::enableINTA_Interrupts(uint8_t periodicity)
{
  intType=PERIODIC_INTERRUPT; //Record interrupt type and periodicity for use of refreshINTA() method
  intPeriodicity=periodicity;

  uint8_t ctReg1= periodicity + 4; 
  /*
  The above statement maps the periodicity defines from 0-3 into actual register value to be set for that mode.
  This arithmetic operation is simpler and faster than switch-case or if-else... Hence,
 
  EverySecond maps to RX8025_CTRL1_CT_PER_SEC     
  EveryMinute maps to RX8025_CTRL1_CT_PER_MIN     
  EveryHour  maps to RX8025_CTRL1_CT_PER_HOUR    
  EveryMonth maps to RX8025_CTRL1_CT_PER_MONTH   

*/
 //Maintain the clock in 24 Hour format
  ctReg1 |= RX8025_CTRL1_1224_BIT ;  
  Wire.beginTransmission(RX8025_ADDRESS);
  Wire.write(RX8025_CTRL1_REG); 
  Wire.write(ctReg1);
  Wire.endTransmission();
  delay(10);

  //set XST and VDSL flags.Clear all other flags
  Wire.beginTransmission(RX8025_ADDRESS);
  uint8_t ctReg2 = RX8025_CTRL2_XST_BIT | RX8025_CTRL2_VDSL_BIT;
  Wire.write(RX8025_CTRL2_REG); 
  Wire.write(ctReg2);
  Wire.endTransmission();
  delay(10);
}

//Enable HH/MM interrupt on /INTA pin. All interrupts works like single-shot counter
//Use refreshINTA() to re-enable interrupt. 
void R8025::enableINTA_Interrupts(uint8_t hh24, uint8_t mm)
{
   // hh24 is hours in 24 format (0-23). mm is in minutes(0 - 59)
   intType=HM_INTERRUPT; //Record interrupt type, hh24 and mm for use of refreshINTA() method
   intHH24 = hh24;
   intMM = mm;

   Wire.beginTransmission(RX8025_ADDRESS);
   Wire.write(RX8025_CTRL1_REG); //CTRL1 Address
  //Set DALE bit to 0 and take care of other bits of CTRL1
   Wire.write(RX8025_CTRL1_1224_BIT);
   Wire.endTransmission();
 
   Wire.beginTransmission(RX8025_ADDRESS);
   Wire.write(RX8025_ALDMIN_REG); //ALD Minute register Address
   Wire.write(bin2bcd(mm));
   Wire.write(bin2bcd(hh24));
   Wire.endTransmission();
  
   Wire.beginTransmission(RX8025_ADDRESS);
   Wire.write(RX8025_CTRL2_REG); //CTRL2 Address
   //Set DAFG bit to 0 and take care of other bits of CTRL2
   Wire.write(RX8025_CTRL2_VDSL_BIT | RX8025_CTRL2_XST_BIT );   
   Wire.endTransmission();

   Wire.beginTransmission(RX8025_ADDRESS);
   Wire.write(RX8025_CTRL1_REG); //CTRL1 Address
  //Set DALE bit to 1 and take care of other bits of CTRL1
   Wire.write(RX8025_CTRL1_1224_BIT | RX8025_CTRL1_DALE_BIT);
   Wire.endTransmission();

}

//Disable Interrupts. This is equivalent to begin() method.
void R8025::disableINTA_Interrupts()
{
   begin(); //Restore the to initial value.
}

//This method must be called after an interrupt is detected. This refreshes the interrupt.
void R8025::refreshINTA()
{

 if(intType==PERIODIC_INTERRUPT)
 {
 //It is essential to clear CTFG flag to make /INTA high. Otherwise next interrupt will not be 
 //detected by MCU. This is mentioned in page-19 timing-diagram of RX8025 App manual
 enableINTA_Interrupts(intPeriodicity); //Writing to CTRL2 is sufficient. But calling this method
                                        //is equilvalent for cleatring flag and setting Level interrupt.
 }
 else if(intType==HM_INTERRUPT)
 {
  enableINTA_Interrupts(intHH24, intMM);
  }
}

// Adjustment RTC Precision . Refer sec 8.3 of Application Manual
// This could be used only when required.
void R8025::changeOffset(uint8_t digitalOffset)
{
   // 'digitalOffset' can be any 7bit number
   Wire.beginTransmission(RX8025_DIGOFF_REG);
   Wire.write((digitalOffset & 0x7F));
   Wire.endTransmission();
}


