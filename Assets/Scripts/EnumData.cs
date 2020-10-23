using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class EnumData
    {
        public enum PlatformerType { Normal, Breakable, Ground, Final};
        
        public enum currentPlayerMoveStatus { Normal, BackJump, InAir}
        public enum GameStatus { MainMenu, Preparing, GameStarted, Die, Finished}

        public enum JumpScoreType { Good, Long, Perfect, LongPerfect}

        public enum SoundEnum { 
            JumpSFX, 
            GoodJumpSfx, 
            PerfectJumpSFX, 
            LongJumpSFX, 
            WowJumpSFX, 
            DieInWaterSFX, 
            WinSFX,
            WinSFX2,
            MainMenuMusic,
            GameplayMusic,
            UIClickSFX,
            UIHeavyClickSFX,
        }

       
       

    }
}
