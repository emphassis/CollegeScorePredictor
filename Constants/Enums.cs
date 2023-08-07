namespace CollegeScorePredictor.Constants
{

    public static class Enums
    {
        public enum PlayType
        {
            EndPeriod = 2,                   //NEUTRAL
            PassIncompletion = 3,            //NEGATIVE OFFENSE
            Rush = 5,                        //POSITIVE OFFENSE
            Sack = 7,                        //NEGATIVE OFFENSE
            Penalty = 8,                     //NEUTRAL
            FumbleRecoveryOwn = 9,           //NEGATIVE OFFENSE
            KickoffReturn = 12,              //NEUTRAL
            TwoPointPass = 15,               //POSITIVE OFFENSE
            TwoPointRush = 16,               //POSITIVE OFFENSE
            BlockedPunt = 17,                //NEGATIVE OFFENSE
            BlockedFieldGoal = 18,           //NEGATIVE OFFENSE
            Safety = 20,                     //NEGATIVE OFFENSE
            Timeout = 21,                    //NEUTRAL
            PassReception = 24,              //POSITIVE OFFENSE
            PassInterceptionReturn = 26,     //NEGATIVE OFFENSE
            FumbleRecoveryOpponent = 29,     //NEGATIVE OFFENSE
            KickoffReturnTouchdown = 32,     //NEGATIVE OFFENSE
            PuntReturnTouchdown = 34,        //NEGATIVE OFFENSE
            InterceptionReturnTouchdown = 36,//NEGATIVE OFFENSE
            BlockedPuntTouchdown = 37,       //NEGATIVE OFFENSE
            BlockedFieldGoalTouchdown = 38,  //NEGATIVE OFFENSE
            FumbleReturnTouchdown = 39,      //NEGATIVE OFFENSE
            FieldGoalReturn = 40,            //NEGATIVE OFFENSE
            FieldGoalReturnTouchdown = 41,   //NEGATIVE OFFENSE
            Punt = 52,                       //NEGATIVE OFFENSE
            Kickoff = 53,                    //POSITIVE OFFENSE
            Defensive2PtConversion = 57,     //NEGATIVE OFFENSE
            FieldGoalGood = 59,              //POSITIVE OFFENSE
            FieldGoalMissed = 60,            //NEGATIVE OFFENSE
            Interception = 63,               //NEGATIVE OFFENSE
            EndofHalf = 65,                  //NEUTRAL
            EndofGame = 66,                  //NEUTRAL
            PassingTouchdown = 67,           //POSITIVE OFFENSE
            RushingTouchdown = 68,           //POSITIVE OFFENSE
            CoinToss = 70,                   //NEUTRAL
            EndOfRegulation = 79             //NEUTRAL
        }

        public enum Conferences
        {
            AmericanAthleticConference,
            AtlanticCoastConference,
            Big12Conference,
            BigTenConference,
            ConferenceUSA,
            FBSIndependents,
            MidAmericanConference,
            MountainWestConference,
            Pac12Conference,
            SoutheasternConference,
            SunBeltConference
        }

        public static class TeamStatistics
        {
            public static readonly string FirstDowns = "firstDowns"; //17
            public static readonly string ThirdDownEfficiency = "thirdDownEff"; //5-14
            public static readonly string FourthDownEfficiency = "fourthDownEff"; //0-0
            public static readonly string TotalYards = "totalYards"; //329
            public static readonly string NetPassingYards = "netPassingYards"; //278
            public static readonly string CompletionAttempts = "completionAttempts"; //20-30
            public static readonly string YardsPerPass = "yardsPerPass"; //9.3
            public static readonly string RushingYards = "rushingYards"; //51
            public static readonly string RushingAttempts = "rushingAttempts"; //35
            public static readonly string YardsPerRushAttempt = "yardsPerRushAttempt"; //1.5
            public static readonly string TotalPenaltiesYards = "totalPenaltiesYards"; //6-40
            public static readonly string Turnovers = "turnovers"; //1
            public static readonly string FumblesLost = "fumblesLost"; //1
            public static readonly string Interceptions = "interceptions"; //0
            public static readonly string PossessionTime = "possessionTime"; //31:15
        }

        public class PlayerStatistics
        {
            public readonly string Passing = "passing";
            public readonly string Rushing = "rushing";
            public readonly string Receiving = "receiving";
            public readonly string Fumbles = "fumbles";
            public readonly string Defensive = "defensive";
            public readonly string Interceptions = "interceptions";
            public readonly string KickReturns = "kickReturns";
            public readonly string PuntReturns = "puntReturns";
            public readonly string Kicking = "kicking";
            public readonly string Punting = "punting";
        }
    }
}

//InterceptionReturnTouchdown = 36,     //  1 -7
//FumbleReturnTouchdown = 39,           //  1 -7
//Safety = 20,                          //  2 -5
//PassInterceptionReturn = 26,          //  3 -4
//FumbleRecoveryOpponent = 29,          //  4 -3.5
//Interception = 63,                    //  4 -3
//Sack = 7,                             //  5 -2
//FumbleRecoveryOwn = 9,                //  5 -1
//PassIncompletion = 3,                 //  6 -.5
//Rush = 5,                             //  7 ..
//PassReception = 24,                   //  7 ..
//PassingTouchdown = 67,                //  8 6
//RushingTouchdown = 68,                //  8 6


//-----Special Teams-----               //
//BlockedPuntTouchdown = 37,            // 1 -7
//BlockedFieldGoalTouchdown = 38,       // 1 -7
//KickoffReturnTouchdown = 32,          // 1 -6
//PuntReturnTouchdown = 34,             // 1 -6
//FieldGoalReturn = 40,                 // 2 -5
//BlockedPunt = 17,                     // 3 -4
//BlockedFieldGoal = 18,                // 3 -4
//FieldGoalMissed = 60,                 // 4 -3
//KickoffReturn = 12,                   // 5 -2
//Punt = 52,                            // 6 -1
//Kickoff = 53,                         // 7 1
//TwoPointPass                          //
//Defensive2PtConversion = 57,          // 8 2
//FieldGoalGood = 59,                   // 9 3


//-----Don't matter-----                //
//CoinToss = 70,                        //
//EndOfRegulation = 79                  //
//EndPeriod = 2,                        //
//EndofHalf = 65,                       //
//EndofGame = 66,                       //
//Timeout = 21,                         //
//Penalty = 8,                          //  handled