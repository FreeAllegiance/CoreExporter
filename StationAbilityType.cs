using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreExporter
{
    internal enum StationAbilityType
    {
        c_sabmUnload                = 0,      //Ability to offload mined minerals
        c_sabmStart                 = 1,      //           start the game at this station
        c_sabmRestart               = 2,    //           restart after dying
        c_sabmRipcord               = 3,    //           teleport to the station
        c_sabmCapture               = 4,    //           be captured
        c_sabmLand                  = 5,    //           land at
        c_sabmRepair                = 6,    //           get repaired
        c_sabmRemoteLeadIndicator   = 7,    //           shows up in the loadout menu of stations
        c_sabmReload                = 8,    //           free fuel and ammo on launch
        c_sabmFlag                  = 9,    //           counts for victory
        c_sabmPedestal              = 10,    //           be a pedestal for a flag
        c_sabmTeleportUnload        = 11,    //           be a pedestal for a flag
        c_sabmCapLand               = 12,    //           land capital ships
        c_sabmRescue                = 13,    //           rescue pods
        c_sabmRescueAny             = 14     //           not used (but reserved for pods)
    }
}
