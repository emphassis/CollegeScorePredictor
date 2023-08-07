using Microsoft.AspNetCore.Components;
using System;

namespace CollegeScorePredictor
{
    public static class BetResultsHelper
    {
        public static string GetTeamName(string teamName)
        {
            switch (teamName)
            {
                case "Massachusetts Minutemen":
                    return "UMass Minutemen";
                case "Southern California Trojans":
                    return "USC Trojans";
                case "Central Florida Knights":
                    return "UCF Knights";
                case "Georgia St. Panthers":
                    return "Georgia State Panthers";
                case "Mississippi Rebels":
                    return "Ole Miss Rebels";
                case "Miami (Ohio) RedHawks":
                    return "Miami (OH) RedHawks";
                case "Southern Methodist Mustangs":
                    return "SMU Mustangs";
                case "Texas-San Antonio Roadrunners":
                    return "UTSA Roadrunners";
                case "Texas Christian Horned Frogs":
                    return "TCU Horned Frogs";
                case "Louisiana State Tigers":
                    return "LSU Tigers";
                case "Connecticut Huskies":
                    return "UConn Huskies";
                case "Arkansas St. Red Wolves":
                    return "Arkansas State Red Wolves";
                case "UL Lafayette Ragin' Cajuns":
                    return "Louisiana Ragin' Cajuns";
                case "Texas El Paso Miners":
                    return "UTEP Miners";
                case "San Jose State Spartans":
                    return "San José State Spartans";
                case "Hawaii Rainbow Warriors":
                    return "Hawai'i Rainbow Warriors";
                case "Brigham Young Cougars":
                    return "BYU Cougars";
                default:
                    return teamName;
            }
        }

        public static long GetTeamId(string teamName)
        {
            switch (teamName)
            {
                //AAC
                case "Cincinnati Bearcats":
                    return 2132;
                case "East Carolina Pirates":
                    return 151;
                case "Houston Cougars":
                    return 248;
                case "Memphis Tigers":
                    return 235;
                case "Navy Midshipmen":
                    return 2426;
                case "Southern Methodist Mustangs":
                    return 2567;
                case "South Florida Bulls":
                    return 58;
                case "Temple Owls":
                    return 218;
                case "Tulane Green Wave":
                    return 2655;
                case "Tulsa Golden Hurricane":
                    return 202;
                case "Central Florida Knights":
                    return 2116;

                //ACC
                case "Boston College Eagles":
                    return 103;
                case "Clemson Tigers":
                    return 228;
                case "Duke Blue Devils":
                    return 150;
                case "Florida State Seminoles":
                    return 52;
                case "Georgia Tech Yellow Jackets":
                    return 59;
                case "Louisville Cardinals":
                    return 97;
                case "Miami Hurricanes":
                    return 2390;
                case "NC State Wolfpack":
                    return 152;
                case "North Carolina Tar Heels":
                    return 153;
                case "Pittsburgh Panthers":
                    return 221;
                case "Syracuse Orange":
                    return 183;
                case "Virginia Cavaliers":
                    return 258;
                case "Virginia Tech Hokies":
                    return 259;
                case "Wake Forest Demon Deacons":
                    return 154;

                //Big 12
                case "Baylor Bears":
                    return 239;
                case "Iowa State Cyclones":
                    return 66;
                case "Kansas Jayhawks":
                    return 2305;
                case "Kansas State Wildcats":
                    return 2306;
                case "Oklahoma Sooners":
                    return 201;
                case "Oklahoma State Cowboys":
                    return 197;
                case "Texas Christian Horned Frogs":
                    return 2628;
                case "Texas Longhorns":
                    return 251;
                case "Texas Tech Red Raiders":
                    return 2641;
                case "West Virginia Mountaineers":
                    return 277;

                //Big 10
                case "Illinois Fighting Illini":
                    return 356;
                case "Indiana Hoosiers":
                    return 84;
                case "Iowa Hawkeyes":
                    return 2294;
                case "Maryland Terrapins":
                    return 120;
                case "Michigan State Spartans":
                    return 127;
                case "Michigan Wolverines":
                    return 130;
                case "Minnesota Golden Gophers":
                    return 135;
                case "Nebraska Cornhuskers":
                    return 158;
                case "Northwestern Wildcats":
                    return 77;
                case "Ohio State Buckeyes":
                    return 194;
                case "Penn State Nittany Lions":
                    return 213;
                case "Purdue Boilermakers":
                    return 2509;
                case "Rutgers Scarlet Knights":
                    return 164;
                case "Wisconsin Badgers":
                    return 275;

                //ConferenceUSA
                case "Charlotte 49ers":
                    return 2429;
                case "Florida Atlantic Owls":
                    return 2226;
                case "Florida International Panthers":
                    return 2229;
                case "Louisiana Tech Bulldogs":
                    return 2348;
                case "Middle Tennessee Blue Raiders":
                    return 2393;
                case "North Texas Mean Green":
                    return 249;
                case "Rice Owls":
                    return 242;
                case "UAB Blazers":
                    return 5;
                case "Texas El Paso Miners":
                    return 2638;
                case "Texas-San Antonio Roadrunners":
                    return 2636;
                case "Western Kentucky Hilltoppers":
                    return 98;

                //FBS Independents
                case "Army Black Knights":
                    return 349;
                case "Brigham Young Cougars":
                    return 252;
                case "Liberty Flames":
                    return 2335;
                case "New Mexico State Aggies":
                    return 166;
                case "Notre Dame Fighting Irish":
                    return 87;
                case "Connecticut Huskies":
                    return 41;
                case "Massachusetts Minutemen":
                    return 113;

                //MAC
                case "Akron Zips":
                    return 2006;
                case "Ball State Cardinals":
                    return 2050;
                case "Bowling Green Falcons":
                    return 189;
                case "Buffalo Bulls":
                    return 2084;
                case "Central Michigan Chippewas":
                    return 2117;
                case "Eastern Michigan Eagles":
                    return 2199;
                case "Kent State Golden Flashes":
                    return 2309;
                case "Miami (Ohio) RedHawks":
                    return 193;
                case "Northern Illinois Huskies":
                    return 2459;
                case "Ohio Bobcats":
                    return 195;
                case "Toledo Rockets":
                    return 2649;
                case "Western Michigan Broncos":
                    return 2711;

                //Mountainwest Conference
                case "Air Force Falcons":
                    return 2005;
                case "Boise State Broncos":
                    return 68;
                case "Colorado State Rams":
                    return 36;
                case "Fresno State Bulldogs":
                    return 278;
                case "Hawaii Rainbow Warriors":
                    return 62;
                case "Nevada Wolf Pack":
                    return 2440;
                case "New Mexico Lobos":
                    return 167;
                case "San Diego State Aztecs":
                    return 21;
                case "San Jose State Spartans":
                    return 23;
                case "UNLV Rebels":
                    return 2439;
                case "Utah State Aggies":
                    return 328;
                case "Wyoming Cowboys":
                    return 2751;

                //Pac 12
                case "Arizona State Sun Devils":
                    return 9;
                case "Arizona Wildcats":
                    return 12;
                case "California Golden Bears":
                    return 25;
                case "Colorado Buffaloes":
                    return 38;
                case "Oregon Ducks":
                    return 2483;
                case "Oregon State Beavers":
                    return 204;
                case "Stanford Cardinal":
                    return 24;
                case "UCLA Bruins":
                    return 26;
                case "Southern California Trojans":
                    return 30;
                case "Utah Utes":
                    return 254;
                case "Washington Huskies":
                    return 264;
                case "Washington State Cougars":
                    return 265;

                //SEC
                case "Alabama Crimson Tide":
                    return 333;
                case "Arkansas Razorbacks":
                    return 8;
                case "Auburn Tigers":
                    return 2;
                case "Florida Gators":
                    return 57;
                case "Georgia Bulldogs":
                    return 61;
                case "Kentucky Wildcats":
                    return 96;
                case "Louisiana State Tigers":
                    return 99;
                case "Mississippi State Bulldogs":
                    return 344;
                case "Missouri Tigers":
                    return 142;
                case "Mississippi Rebels":
                    return 145;
                case "South Carolina Gamecocks":
                    return 2579;
                case "Tennessee Volunteers":
                    return 2633;
                case "Texas A&M Aggies":
                    return 245;
                case "Vanderbilt Commodores":
                    return 238;

                //Sunbelt
                case "Appalachian State Mountaineers":
                    return 2026;
                case "Arkansas St. Red Wolves":
                    return 2032;
                case "Coastal Carolina Chanticleers":
                    return 324;
                case "Georgia Southern Eagles":
                    return 290;
                case "Georgia St. Panthers":
                    return 2247;
                case "James Madison Dukes":
                    return 256;
                case "UL Lafayette Ragin' Cajuns":
                    return 309;
                case "Marshall Thundering Herd":
                    return 276;
                case "Old Dominion Monarchs":
                    return 295;
                case "South Alabama Jaguars":
                    return 6;
                case "Southern Miss Golden Eagles":
                    return 2572;
                case "Texas State Bobcats":
                    return 326;
                case "Troy Trojans":
                    return 2653;
                case "UL Monroe Warhawks":
                    return 2433;
                default:
                    //Console.WriteLine("GetTeamId Error: Couldn't find " + teamName);
                    return 0;
            }
        }

        public static async Task<decimal> GetGradeAverageWithoutUsing(AppDbContext db, long teamId, byte week, int year)
        {
            //var teamName = await (from e in db.ConferenceTeamTable where e.TeamId == teamId select e.TeamName).FirstAsync();

            //var weekStart = week - 3 < 0 ? 0 : week - 3;

            //var grades = await (from e in db.GameAnalysisTable
            //                    where e.TeamId == teamId && e.Year == year && e.Week >= weekStart && e.Week <= week
            //                    select new
            //                    {
            //                        e.TeamGrade,
            //                        e.OpponentGrade,
            //                        e.TeamOffenseGrade,
            //                        e.TeamDefenseGrade,
            //                        e.TeamSpecialTeamsGrade,
            //                        e.OpponentDefenseGrade,
            //                        e.TeamWon,
            //                        e.TeamAveragePointsAllowed,
            //                        e.OpponentAveragePointsAllowed,
            //                        e.OpponentScore,
            //                        e.TeamScore
            //                    }).ToListAsync();

            //var gradeTotal = 0M;

            //foreach (var grade in grades)
            //{
            //    var oppGrade = grade.OpponentGrade + -100;
            //    var teamGrade = grade.TeamGrade + -100;
            //    var teamOffenseGrade = grade.TeamOffenseGrade - 100;
            //    var oppDefenseGrade = grade.OpponentDefenseGrade - 100;
            //    var offenseGrade = teamOffenseGrade - oppDefenseGrade;
            //    var combinedGrade = oppGrade + teamGrade;
            //    var teamWon = grade.TeamWon ? 10 : 0;
            //    var teamPoints = grade.TeamScore - grade.OpponentAveragePointsAllowed;
            //    var opponentPoints = grade.OpponentScore - grade.TeamAveragePointsAllowed;
            //    var pointsAllowed = teamPoints - opponentPoints;
            //    gradeTotal = gradeTotal + combinedGrade + teamWon - pointsAllowed;
            //    //if(teamId == 5 || teamId == 242)
            //    //{
            //    //    Console.WriteLine(teamName + ": " + oppGrade + " / " + teamGrade + " / " + combinedGrade);
            //    //}
            //}

            //var gradeAverage = gradeTotal / grades.Count;

            ////if (teamId == 5 || teamId == 242)
            ////{
            ////    Console.WriteLine(gradeTotal);
            ////    Console.WriteLine(grades.Count);
            ////    Console.WriteLine(gradeAverage);
            ////}

            ////Console.WriteLine(teamName + ": " + Math.Round(gradeAverage, 2));

            //return Math.Round(gradeAverage, 2);
            return 2M;
        }
    }
}
