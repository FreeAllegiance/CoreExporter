using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreExporter
{
    // Aka "Features" in the exported Json.
    // Lining these up to ICE UI, otherwise check igc.h under ExpendableAbilityBitMask if there is trouble. 
    internal enum ExpendableAbilityType
    {
        c_eabmCapture = 0,
        c_eabmWarpBombDual = 1, // KGJV: both sides aleph rez
        c_eabmWarpBombSingle = 2, // KGJV: one side aleph rez
        //c_eabmWarpBomb = c_eabmWarpBombDual | c_eabmWarpBombSingle; // KGJV: both types into one for backward compatibility
        c_eabmQuickReady = 3,
        c_eabmWarnDrop = 4,
        c_eabmShootStations = 5,
        c_eabmShootShips = 6,
        c_eabmShootMissiles = 7,
        c_noop1 = 8,
        c_noop2 = 9,
        c_noop3 = 10,
        c_noop4 = 11,
        c_eabmShootOnlyTarget = 12,
        c_eabmRescue = 13,     //0x2000 Rescue lifepods that collide with it
        c_eabmRescueAny = 14,  //0x4000 Rescue any lifepod that collide with it
        c_noop5 = 15
    }
}
