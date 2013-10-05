// R8025 Class is by Seeed Technology Inc(http://www.seeedstudio.com) and used
// in Seeeduino Stalker v2 for battery management(MCU power saving mode)
// & to generate timestamp for data logging. DateTime Class is a modified
// version supporting day-of-week.

// Original DateTime Class and its utility code is by Jean-Claude Wippler at JeeLabs
// http://jeelabs.net/projects/cafe/wiki/RTClib 
// Released under MIT License http://opensource.org/licenses/mit-license.php

#ifndef R8025_H
#define R8025_H


#define RX8025_ADDRESS	      0x32 //I2C Slave address

/* RX8025 Registers. Refer Sec 8.2 of application manual */
#define RX8025_SEC_REG        0x00  
#define RX8025_MIN_REG        0x10  
#define RX8025_HOUR_REG       0x20
#define RX8025_WDAY_REG       0x30
#define RX8025_MDAY_REG       0x40
#define RX8025_MONTH_REG      0x50
#define RX8025_YEAR_REG       0x60
#define RX8025_DIGOFF_REG     0x70
#define RX8025_ALWMIN_REG     0x80
#define RX8025_ALWHOUR_REG    0x90
#define RX8025_ALWWDAY_REG    0xA0
#define RX8025_ALDMIN_REG     0xB0
#define RX8025_ALDHOUR_REG    0xC0
#define RX8025_RESERVED_REG   0xD0
#define RX8025_CTRL1_REG      0xE0
#define RX8025_CTRL2_REG      0xF0
	
#define RX8025_CTRL1_CT_BIT      (7 << 0)

/* periodic level interrupts used for powersaving mode of MCU*/
#define RX8025_CTRL1_CT_PER_OFF     0
#define RX8025_CTRL1_CT_PER_SEC     4
#define RX8025_CTRL1_CT_PER_MIN     5
#define RX8025_CTRL1_CT_PER_HOUR    6
#define RX8025_CTRL1_CT_PER_MONTH   7

/*Periodicity */
#define EverySecond   0
#define EveryMinute   1
#define EveryHour     2
#define EveryMonth    3

//CTRL1 and CTLR2 register bits

#define RX8025_CTRL1_TEST_BIT    (1 << 3)
#define RX8025_CTRL1_1224_BIT    (1 << 5)
#define RX8025_CTRL1_DALE_BIT    (1 << 6)
#define RX8025_CTRL1_WALE_BIT    (1 << 7)
	
#define RX8025_CTRL2_DAFG_BIT    (1 << 0)
#define RX8025_CTRL2_WAFG_BIT    (1 << 1)
#define RX8025_CTRL2_CTFG_BIT    (1 << 2)
#define RX8025_CTRL2_CLEN1_BIT   (1 << 3)
#define RX8025_CTRL2_PON_BIT     (1 << 4)
#define RX8025_CTRL2_XST_BIT     (1 << 5)
#define RX8025_CTRL2_VDET_BIT    (1 << 6)
#define RX8025_CTRL2_VDSL_BIT    (1 << 7)


#define PERIODIC_INTERRUPT	0x00
#define HM_INTERRUPT		0x01


// Simple general-purpose date/time class (no TZ / DST / leap second handling!)
class DateTime {
public:
    DateTime (long t =0);
    DateTime (uint16_t year, uint8_t month, uint8_t date,
              uint8_t hour, uint8_t min, uint8_t sec, uint8_t wday);
    DateTime (const char* date, const char* time);

    uint8_t second() const      { return ss; }
    uint8_t minute() const      { return mm; } 
    uint8_t hour() const        { return hh; }
 
    uint8_t date() const        { return d; }
    uint8_t month() const       { return m; }
    uint16_t year() const       { return 2000 + yOff; }

    uint8_t dayOfWeek() const   { return wday;}  /*Su=0 Mo=1 Tu=3 We=4 Th=5 Fr=6 Sa=7 */

    // 32-bit time as seconds since 1/1/2000
    long get() const;   

protected:
    uint8_t yOff, m, d, hh, mm, ss, wday;
};

// RTC RX8025 chip connected via I2C and uses the Wire library.
// Only 24 Hour time format is supported in this implementation
class R8025 {
public:
    uint8_t begin(void);
    void adjust(const DateTime& dt);  //Changes the date-time
    static DateTime now();            //Gets the current date-time

    //Decides the /INTA pin's output setting
    //periodicity cn be any of following defines: EverySecond, EveryMinute, EveryHour or EveryMonth 
    void enableINTA_Interrupts(uint8_t periodicity);
    void enableINTA_Interrupts(uint8_t hh24, uint8_t mm);
    void disableINTA_Interrupts();
    void refreshINTA();
    void changeOffset(uint8_t digitalOffset);
protected:
    uint8_t intType, intPeriodicity, intHH24, intMM;
};

#endif