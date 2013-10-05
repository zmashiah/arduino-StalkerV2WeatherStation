#ifndef Battery_h
#define Battery_h

//**********************************************
#define CH_Status 6
#define OK_Status 7
#define BAT_voltage 7
//==============================================
extern unsigned int bat_read;
extern float bat_voltage;
extern unsigned char charge_status;
extern unsigned char battery_status;
#  define BAT_TOO_WEEK 2
# define BAT_TOO_HIGH 1
# define BAT_OK 0

//==============================================
void Battery_init(void);
void Battery_voltage_read(void);
void Battery_charge_status(void);
//**********************************************

#endif
