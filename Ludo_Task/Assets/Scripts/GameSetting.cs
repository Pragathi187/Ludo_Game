using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{


    /*
     #region REd player
        public void SetRedHumanType(bool on)
        {
            if(on)
            {
                SaveSettings.players[0] = "HUMAN";     

             }
        }

        public void SetRedcpuType(bool on)
        {
            if (on)
            {
                SaveSettings.players[0] = "CPU";

            }
        }
        public void SetRedNoPlayerType(bool on)
        {
            if (on)
            {
                SaveSettings.players[0] = "NoPlayer";
                noPlayerCount++;
            }
        }
        #endregion
     #region Green Player
        public void SetGreenHumanType(bool on)
        {
            if (on)
            {
                SaveSettings.players[1] = "HUMAN";

            }
        }

        public void SetGreencpuType(bool on)
        {
            if (on)
            {
                SaveSettings.players[1] = "CPU";

            }
        }
        public void SetGreenNoPlayerType(bool on)
        {
            if (on)
            {
                SaveSettings.players[1] = "NoPlayer";
                noPlayerCount++;
            }
        }
        #endregion
        #region Yellow Player
        public void SetYellowHumanType(bool on)
        {
            if (on)
            {
                SaveSettings.players[2] = "HUMAN";

            }
        }

        public void SetYellowcpuType(bool on)
        {
            if (on)
            {
                SaveSettings.players[2] = "CPU";

            }
        }
        public void SetYellowNoPlayerType(bool on)
        {
            if (on)
            {
                SaveSettings.players[2] = "NoPlayer";
                noPlayerCount++;
            }
        }
        #endregion
        #region Blue Player
        public void SetBlueHumanType(bool on)
        {
            if (on)
            {
                SaveSettings.players[3] = "HUMAN";

            }
        }

        public void SetBluecpuType(bool on)
        {
            if (on)
            {
                SaveSettings.players[3] = "CPU";

            }
        }
        public void SetBlueNoPlayerType(bool on)
        {
            if (on)
            {
                SaveSettings.players[3] = "NoPlayer";
                noPlayerCount++;
            }
        }
        #endregion    */

        public Toggle redCpu, redHuman;
        public Toggle greenCpu, greenHuman; 
        public Toggle yellowCpu, yellowHuman, yellowNoPlayer;
        public Toggle blueCpu, blueHuman, blueNoPlayer;
       // public bool isCpuorHuman;

       // public bool isRedNoplayerOn, isGreenNoPlayerOn, isYellowNoPlayerOn, isBlueNoPlayerOn;

    public void ReadToggle()


    {
       
            //RED - 0 
            if (redCpu.isOn)
            {
           
            SaveSettings.players[0] = "CPU";
            }
            else if (redHuman.isOn)
            {
            
            SaveSettings.players[0] = "HUMAN";
            }
           
           else
           {

            SaveSettings.players[0] = "CPU";
           }


        //Green - 1
            if (greenCpu.isOn)
            {
           
            SaveSettings.players[1] = "CPU";
            }
            else if (greenHuman.isOn)
            {
              SaveSettings.players[1] = "HUMAN";
            }
          
           else
           {

            SaveSettings.players[1] = "CPU";
           }



        //Yellow - 2
        if (yellowCpu.isOn)
        {
            
            SaveSettings.players[2] = "CPU";
        }
        else if (yellowHuman.isOn)
        {
           
            SaveSettings.players[2] = "HUMAN";
        }
        else if (yellowNoPlayer.isOn)
        {
            
            SaveSettings.players[2] = "NoPlayer";
        }
        else
        {
            SaveSettings.players[2] = "NoPlayer"; ;
        }
        //Blue - 3
        if (blueCpu.isOn)
            {
            
            SaveSettings.players[3] = "CPU";
            }
        else if (blueHuman.isOn)
            {
            
            SaveSettings.players[3] = "HUMAN";
            }
        else if (blueNoPlayer.isOn)
        {
           
            SaveSettings.players[3] = "NoPlayer";
        }
        else
        {
            SaveSettings.players[3] = "NoPlayer"; ;
        }

    }
}

public static class SaveSettings
{
    //player order
    // 0   1      2     3
    //red green yellow blue

    public static string[] players = new string[4];

    public static string[] winners = new string[3] { string.Empty, string.Empty, string.Empty };



}
