using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WeatherStationMan
{
    #region Stalker and Application data
    public struct StalkerDateTime
    {
        public byte hour;
        public byte minute;
        public byte second;
        public byte week;
        public byte year;
        public byte month;
        public byte date;
    };

    public struct WeatherStationData
    {
        // Battery and charger data
        public float bat_voltage;
        public byte bat_status;
        public byte charge_status;
        // TMP102
        public float tmp102Temp;
        // BMP085
        public float bmp085Temp;
        public Int32 bmp085Pressure;
        public float bmp085Altitude;
        public float bmp085RealAltitude;
        // DHT 22
        public float dht22Temp;
        public float dht22Humid;
        // Date and time
        public StalkerDateTime dt;

    };
        
    public struct WeatherStationCommandBuffer
    {
        public byte command;
        /* Possible values
         * #define WS_CMD_READING	'R'	// We are sending a WeatherStationSample data, normal case
         * #define WS_CMD_GETTIME	'T'	// We are sending DateTime of current time and ask for DateTime to set local RTC
         * #define WS_CMD_POWER	'P'	// Power alert is set to ask for charging remote sensor. Data is still WeatherStationSample.
         * #define WS_CMD_HEAT		'H'	// Excessive heat, if temperature is above 50 degrees. Data is still WeatherStationSample.
         */
        public byte length;
        public WeatherStationData sample;
    };
    #endregion

    #region FormWindow
    public partial class mainWindow : Form
    {
        #region Variables
        SerialPort myPort = new SerialPort();	//sets up the serial port
        string myActiveSerialPortName;      // Once we have selection by double-click port name will be stored here.
		byte RxString;				        // each byte we will read will be stored here.
        bool lastMessageExist = false;      // If we got good packet from the serial port
        byte[] lastMessage = new byte[140]; // 110 is the frame of XBee plus some headers and spare...
        int posMessage = 0;                 // last populated index in the lastMessage array
        WeatherStationCommandBuffer wsCmdBuff;  // Data record of the sampling from weather station
        String wsError;                     // Either error string or packet completion time (Host based not weather station)
        static Timer commTimer = new System.Windows.Forms.Timer();
        CommStatusWatcher commWatcher = new CommStatusWatcher();
        #endregion

        #region Constructor
        public mainWindow(string serialCmdPortName)
        {
            bool cmdNameFound = false;
            int portIndex;

            InitializeComponent();
            this.receivedData.UseVisualStyleBackColor = false; // Trying to solve the bug packet button sometimes remains green

            // Populate list box with all available serial port names
            SerialPortsListBox.BeginUpdate();
            string[] allSerialPorts = SerialPort.GetPortNames();
            foreach (string port in allSerialPorts)
            {
                portIndex = SerialPortsListBox.Items.Add(port); // add it to the list
                if (serialCmdPortName != null)
                    if (port.Equals(serialCmdPortName))
                    {
                        cmdNameFound = true;
                        SerialPortsListBox.SetSelected(portIndex, true);
                    }
            }
            SerialPortsListBox.EndUpdate();
            if ((serialCmdPortName != null) && (cmdNameFound == false))
                // We did not find the command line argument in the serial ports list
                MessageBox.Show("Please Select an Available Port");
            else
                if (serialCmdPortName != null) // if we have a command line and it is one of the existing serial ports
                    openComPort(serialCmdPortName);

            commWatcher.Reset();
            commTimer.Tick += new EventHandler(TimerEventHandler);
            commTimer.Interval = 1000 * 10;

            myPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceivedEventHandler);
        }
        #endregion

        #region SerialHandler
        public void SerialDataReceivedEventHandler(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
            // Constants related to XBee interface
            const byte StartByte = 0x7E;
            const byte EscapeByte = 0x7D;
            const byte ApiIdIndex = 3;
            const int MaxFrameDataSize = 110;

            bool continueRead = true;
            bool hadEscape = false;
            int bytesRead = 0;
            int packetLegnth = 0;
            //int bytesToRead = 0;
            byte _checksum = 0;
            byte _msb = 0, _lsb = 0;
            byte _apiId;

            
            this.receivedData.BackColor = Color.LawnGreen;

            // Read all the bytes that exist on the serial port for one packet
            while (/*(myPort.BytesToRead > 0) && */ continueRead)
            {
                int fromBuffer = myPort.ReadByte();
                RxString = Convert.ToByte(fromBuffer);

                if ((posMessage > 0) && (RxString == StartByte))
                {
                    // New packet start before previous packet completed discard previous and start over
                    posMessage = 0;
                    continueRead = false;
                    lastMessageExist = false;
                    wsError = "Unexpected start byte while reading XBee data";
                    continue;
                }

                if ((posMessage > 0) && RxString == EscapeByte)
                {
                    if (myPort.BytesToRead > 0)
                    {
                        fromBuffer = myPort.ReadByte();
                        RxString = Convert.ToByte(fromBuffer);
                        RxString ^= (byte)0x20;
                    }
                    else
                    {
                        // escape byte. Next byte will be the real one
                        hadEscape = true;
                        continue;
                    }
                }

                if (hadEscape)
                {
                    RxString ^= (byte)0x20;
                    hadEscape = false;
                }

                if (posMessage >= ApiIdIndex)
                    _checksum += RxString;
                switch (posMessage)
                {
                    case 0:
                        if (RxString == StartByte)
                        {
                            lastMessage[posMessage] = RxString;
                            posMessage++;
                        }
                        else
                        {
                            int temp = RxString;
                            wsError = "Waiting for start byte (" + myPort.BytesToRead.ToString() + ") " + DateTime.Now.ToLongTimeString() + " b = " + temp.ToString();
                        }
                        break;
                    case 1:
                        _msb = RxString;
                        lastMessage[posMessage] = RxString;
                        posMessage++;
                        break;
                    case 2:
                        _lsb = (byte)RxString;
                        lastMessage[posMessage] = RxString;
                        posMessage++;
                        packetLegnth = (_msb << 8) | _lsb;
                        break;
                    case 3:
                        _apiId = RxString;
                        lastMessage[posMessage] = RxString;
                        posMessage++;
                        break;
                    default:
                        if (posMessage > MaxFrameDataSize)
                        {
                            // An error
                            continueRead = false;
                            lastMessageExist = false;
                            wsError = "More data than max packet size on XBee";
                            continue;
                        }
                        if (posMessage == (packetLegnth + 3))
                        {
                            continueRead = false;
                            if (_checksum == 0xFF)
                            {
                                lastMessageExist = true;
                                wsError = "Got frame: " + DateTime.Now.ToString();
                            }
                            else
                                wsError = "Bad Checksum for frame " + DateTime.Now.ToString();

                            // reset counters
                            bytesRead = posMessage;
                            posMessage = 0;
                        }
                        else
                        {
                            // this is the normal case
                            lastMessage[posMessage] = RxString;
                            posMessage++;
                        }
                        break;
                } // close the switch
            } // close the while

            this.receivedData.BackColor = Color.LightGray;
            this.Invalidate(); // This line suppose to fix issue of "Packet" button remaining green after packet received.

            if (lastMessageExist) // If we are done with a packet
            {
                if (ParseAndSaveXbeePacket(lastMessage, bytesRead) == 0)
                {
                    this.Invoke(new EventHandler(DisplayOnScreen)); // UPDATING On-Screen data:
                    this.Invoke(new EventHandler(SaveToTextFile));  // Write to CSV file
                }
                else
                {
                    wsError = "Error parsing packets";
                    this.Invoke(new EventHandler(DisplayErrorOnScreen));
                }
            }
            else
                this.Invoke(new EventHandler(DisplayErrorOnScreen));
            lastMessageExist = false;
            posMessage = 0;

        }//SerialDataReceivedEventHandler
        #endregion

        

        #region DisplayFunctions
        private void CommWatcher_Show(Object sender, EventArgs e)
        {
            CommStatus state = (CommStatus)commWatcher.GetCommStatus();
            
            switch (state)
            {
                case CommStatus.Lost:
                    commWatchStatus.Text = "Lost";
                    commWatchStatus.BackColor = Color.Red;
                    this.GraphsButton.Enabled = false;
                    FlashWindow.Flash(this);
                    break;
                case CommStatus.Silent:
                    commWatchStatus.Text = "Waiting";
                    commWatchStatus.BackColor = Color.Yellow;
                    this.GraphsButton.Enabled = false;
                    FlashWindow.Stop(this);
                    break;
                case CommStatus.Working:
                    commWatchStatus.Text = "Receiving";
                    commWatchStatus.BackColor = Color.LawnGreen;
                    this.GraphsButton.Enabled = true;
                    FlashWindow.Stop(this);
                    break;
            }
        }  // CommWather_Show

        
        private void DisplayOnScreen(object sender, EventArgs e)
        {
            String fixedPointFormat = "F2"; // Most values are with 2 dots after the decimal point
            String pressureFormat = "N0"; // Comma separation as altitude and pressure are / can be long numbers
            String celsius = " °C";
            String meter = " m";
            String precent = " %";

            textBox3.Text = wsCmdBuff.sample.dht22Humid.ToString(fixedPointFormat) + precent;
            textBox4.Text = wsCmdBuff.sample.bmp085Altitude.ToString(fixedPointFormat) + meter;
            textBox5.Text = wsCmdBuff.sample.bmp085RealAltitude.ToString(fixedPointFormat) + meter;
            textBox6.Text = wsCmdBuff.sample.bmp085Pressure.ToString(pressureFormat) + "Pa";
            textBox7.Text = wsCmdBuff.sample.bmp085Temp.ToString(fixedPointFormat) + celsius;
            textBox8.Text = wsCmdBuff.sample.bat_voltage.ToString(fixedPointFormat) + "V";
            textBox9.Text = wsCmdBuff.sample.dht22Temp.ToString(fixedPointFormat) + celsius;
            textBox10.Text = wsCmdBuff.sample.tmp102Temp.ToString(fixedPointFormat) + celsius;
            textBox11.Text = wsError;
            switch (wsCmdBuff.sample.charge_status)
            {
                case 0x01:
                    Charge.BackColor = Color.Gray;
                    Charge.Text = "No Charge";
                    break;
                case 0x02:
                    Charge.BackColor = Color.LawnGreen;
                    Charge.Text = "Complete";
                    break;
                case 0x04:
                    Charge.BackColor = Color.Yellow;
                    Charge.Text = "Charging";
                    break;
                case 0x08:
                    Charge.BackColor = Color.Red;
                    Charge.Text = "No Bat.";
                    break;
                default:
                    Charge.BackColor = Color.Pink;
                    Charge.Text = "Unknown";
                    break;
            }
            switch (wsCmdBuff.sample.bat_status)
            {
                case 0:
                    BatteryPower.BackColor = Color.LawnGreen;
                    BatteryPower.Text = "OK";
                    break;
                case 0x01:
                    BatteryPower.BackColor = Color.Red;
                    BatteryPower.Text = "Over";
                    break;
                case 0x02:
                    BatteryPower.BackColor = Color.Orange;
                    BatteryPower.Text = "Weak";
                    break;
                default:
                    BatteryPower.BackColor = Color.Pink;
                    BatteryPower.Text = "Unknown";
                    break;
            }
            DateTime dt = new DateTime(2000 + wsCmdBuff.sample.dt.year, wsCmdBuff.sample.dt.month, wsCmdBuff.sample.dt.date,
                wsCmdBuff.sample.dt.hour, wsCmdBuff.sample.dt.minute, wsCmdBuff.sample.dt.second);
            WeatherStationTime.Text = dt.ToString();
            this.Invoke(new EventHandler(CommWatcher_Show));
        } // DisplayOnScreen

        
        private void DisplayErrorOnScreen(object sender, EventArgs e)
        {
            textBox11.Text = wsError;
        } // DisplayErrorOnScreen
        #endregion

        #region ParsingDeserialization
        private float TwoBytesToFloat(byte b1, byte b2)
        {
            float f;
            double d;
            short r;

            r = b2;
            r <<= 8;
            r |= (short)b1;
            d = (float)r;
            f = (float)(d / 100.0);
            return f;
        } // TwoBytesToFloat


        private Int32 FourBytesToInt32(byte b1, byte b2, byte b3, byte b4)
        {
            Int32 r, temp;

            temp = (Int32)b4;
            r = temp << 24;
            temp = (Int32)b3;
            r += temp << 16;
            temp = (Int32)b2;
            r += temp << 8;
            r += b1;

            return r;
        } // FourBytesToInt32


        private int ParseAndSaveXbeePacket(byte[] packet, int packetSize)
        {
            const int xbeeApiModePacketHeaderLenght = 0x15/*15*/; // Change made to adjust to Explicit AO (AO=1) mode of XBee
            int pos;
            int rc;

            if (packetSize == 50/*44*/) // Change to adjust to Explicit AO (AO=1) mode of XBee
            {
                commWatcher.GotPacket();

                // Deserialize all the packet
                pos = xbeeApiModePacketHeaderLenght;

                wsCmdBuff.command = packet[pos++];
                wsCmdBuff.length = packet[pos++];
                wsCmdBuff.sample.bat_voltage = TwoBytesToFloat(packet[pos], packet[pos + 1]);
                pos += 2;
                wsCmdBuff.sample.bat_status = packet[pos++];
                wsCmdBuff.sample.charge_status = packet[pos++];

                wsCmdBuff.sample.tmp102Temp = TwoBytesToFloat(packet[pos], packet[pos + 1]);
                pos += 2;

                wsCmdBuff.sample.bmp085Temp = TwoBytesToFloat(packet[pos], packet[pos + 1]);
                pos += 2;

                wsCmdBuff.sample.bmp085Pressure = FourBytesToInt32(packet[pos], packet[pos + 1],
                    packet[pos + 2], packet[pos + 3]);
                pos += 4;

                wsCmdBuff.sample.bmp085Altitude = TwoBytesToFloat(packet[pos], packet[pos + 1]);
                pos += 2;

                wsCmdBuff.sample.bmp085RealAltitude = TwoBytesToFloat(packet[pos], packet[pos + 1]);
                pos += 2;

                wsCmdBuff.sample.dht22Temp = TwoBytesToFloat(packet[pos], packet[pos + 1]);
                pos += 2;
                wsCmdBuff.sample.dht22Humid = TwoBytesToFloat(packet[pos], packet[pos + 1]);
                pos += 2;

                wsCmdBuff.sample.dt.hour = packet[pos++];
                wsCmdBuff.sample.dt.minute = packet[pos++];
                wsCmdBuff.sample.dt.second = packet[pos++];
                wsCmdBuff.sample.dt.week = packet[pos++];
                wsCmdBuff.sample.dt.year = packet[pos++];
                wsCmdBuff.sample.dt.month = packet[pos++];
                wsCmdBuff.sample.dt.date = packet[pos++];

                rc = 0;
            }
            else
                rc = -1;

            return rc;
            // Now we should have the wsCmdBuff filled with data from serial port
        } // ParseAndSaveXbeePacket
        #endregion

        #region StorageFunctions
        private void SaveToTextFile(object sender, EventArgs e)
		{
            const string fileName = WeatherStationMan.WeatherStationManConstants.csvFileName;
            const string twoPos = "D2";
            DateTime fileTime;
            fileTime = File.GetLastAccessTime(fileName);

            TextWriter csvDataFile = new StreamWriter(fileName, true);
            if (fileTime.Year < 2000) // File did not exist before, write the header line of CSV file
                //csvDataFile.WriteLine(";ComputerDate,ComputerTime,Date,Time,year,month,day,hour,minute,second,voltage,charger,battery_status,temperature102,temperatureDHT22,temperatureBMP085,humidty,pressure");
                csvDataFile.WriteLine(";ComputerDate,ComputerTime,voltage,charger,battery_status,temperature102,temperatureDHT22,temperatureBMP085,humidty,pressure");
            
            int year = wsCmdBuff.sample.dt.year + 2000; // Fix wrong year in WeatherStation RTC until I reset it to right setting
            
            csvDataFile.WriteLine(
                // Computer date and time "yyyy/mm/dd","hh:mm",
                "\"" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString(twoPos) + "/" + DateTime.Now.Day.ToString(twoPos) + "\",\"" +
                DateTime.Now.Hour.ToString(twoPos) + ":" + DateTime.Now.Minute.ToString(twoPos) + "\"," +
                // Voltage and battery
                wsCmdBuff.sample.bat_voltage.ToString() + "," +
                wsCmdBuff.sample.charge_status.ToString() + "," +
                wsCmdBuff.sample.bat_status.ToString() + "," +
                // Temperatures
                wsCmdBuff.sample.tmp102Temp.ToString() + "," +
                wsCmdBuff.sample.dht22Temp.ToString() + "," +
                wsCmdBuff.sample.bmp085Temp.ToString() + "," +
                // Humidity and Pressure
                wsCmdBuff.sample.dht22Humid.ToString() + "," +
                wsCmdBuff.sample.bmp085Pressure.ToString());
            csvDataFile.Close();
			return;
		}// SaveToTextFile
        #endregion

        #region TimerFunctions
        private void TimerEventHandler(Object sender, EventArgs e)
        {
            commWatcher.GotTimerTick();
            commTimer.Enabled = true;

            // Now display
            this.Invoke(new EventHandler(CommWatcher_Show));
        } // TimerEventHandler
        #endregion

        #region SerialPortSelection
        private void openComPort(string portName)
        {
            try //handles the cases when you try to connect but the xbee is not plugged in 
            {
                    myPort.PortName = portName;
                    myPort.Open();
                    myPort.DiscardOutBuffer();
                    OpenCloseComPort_button.Text = "Close " + portName;
                    commWatcher.Reset();
                    commTimer.Start();
                }//try
                catch (IOException)
                {
                    MessageBox.Show("Sorry Your Port is not available,\nPlease Check your Connections");
                }//catch
                catch (System.ArgumentException)
                {
                    MessageBox.Show("Please Select an Available Port");
             }//catch
            
        } //OpenCloseComPort
        
        private void OpenCloseComPort_button_Click(object sender, EventArgs e)
        {
            if (!myPort.IsOpen)
            {
                myActiveSerialPortName = SerialPortsListBox.SelectedItem.ToString();
                openComPort(myActiveSerialPortName);
                this.textBox11.Text = "Waiting for data";
            }
            else
            {
                commTimer.Stop();
                myPort.Close();
                this.Charge.BackColor = Color.LightGray; // After closing, turn all indicators on screen to gray
                this.commWatchStatus.BackColor = Color.LightGray;
                this.receivedData.BackColor = Color.LightGray;
                this.BatteryPower.BackColor = Color.LightGray;
                this.textBox11.Text = "Serial port closed";
                OpenCloseComPort_button.Text = "Open Port";
            }//else
        } // OpenCloseComPortButton_Click

        private void SerialPortsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        } // SerialPortsListBox_SelectedIndexChanged


        private void SerialPortsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (myPort.IsOpen) // if port is open one must first close it
                return;
            this.Invoke(new EventHandler(OpenCloseComPort_button_Click));
        } // SerialPortsListBox_DoubleClick

        
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        #endregion




        #region mainWindow Buttons Click
        private void CloseButton_Click(object sender, EventArgs e) { this.Close(); }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(WeatherStationMan.WeatherStationManConstants.aboutMessage, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GraphsButton_Click(object sender, EventArgs e)
        {
            WeatherDataAnalyzer.WeatherDataReport reporter = new WeatherDataAnalyzer.WeatherDataReport();
            reporter.Show();
            reporter = null;
        }
        #endregion

    }  // Form1 class
    #endregion


    #region CommunicationStatus
    public enum CommStatus : int { Working, Silent, Lost };
    
    public class CommStatusWatcher
    {
        // Variables
        public bool hadComm = false;
        public bool commInLast10Seconds = false;
        public bool commLost = false;
        public int intervalCounter = 0;

        // Functions
        public void Reset()
        {
            commInLast10Seconds = false;
            commLost = false;
            hadComm = false;
            intervalCounter = 0;
        } // Reset

        public void GotPacket()
        {
            commInLast10Seconds = true;
            commLost = false;
            intervalCounter = 0;
            hadComm = true;
        } // GotPacket

        public void GotTimerTick()
        {
            if (commInLast10Seconds)
            {
                commInLast10Seconds = false;
            }
            else
                intervalCounter++;
            if (intervalCounter > 18)
                commLost = true;
        } // GotTimer

        public int GetCommStatus()
        {
            if (commInLast10Seconds)
                return (int)CommStatus.Working;
            if (commLost)
                return (int)CommStatus.Lost;
            if (hadComm && (intervalCounter < 5)) // Turn yellow 10s before remote unit needs to send data
                return (int)CommStatus.Working;
            return (int)CommStatus.Silent;
        } // GetCommStatus

    } // CommStatusWatcher Class
    #endregion
        
}