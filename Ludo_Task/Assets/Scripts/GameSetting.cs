using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    
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
    #endregion
#region Yellow Player
    public void SetYellowGreeenHumanType(bool on)
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
    #endregion    

}

public static class SaveSettings
{
    //player order
    // 0   1      2     3
    //red green yellow blue

    public static string[] players = new string[4];

    public static string[] winners = new string[3] { string.Empty, string.Empty, string.Empty };



}
