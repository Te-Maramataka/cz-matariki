using System;

namespace Matariki {

    class Matariki {

        //Accuracy, scope and purpose constants
        public const double phaseEqualityPrecision = 0.001; // How close a double has to be to be considered equal to another double
        public const double timeZoneOffset = 12.0; // The time zone offset

        public static void Main() {
            uint year = input(); // input method asks for input then checks validity of input.
            var date = getMatarikiDate(year);
            var closestFriday = new DateTime();
            closestFriday = findFriday(date); // findFriday method returns the closest Friday.
            Console.WriteLine("The matariki public holiday will be on " + closestFriday.ToString("MMMM dd, yyyy"));
        }
        public static DateTime findFriday(DateTime inputDate) {

            var outputDate = new DateTime(); // Creates the output date variable for the friday.         
            int dayOfWeek = (int)inputDate.DayOfWeek; // Converts the first day of the four day period into a day of the week

            switch (dayOfWeek) {
                case 0: //Sunday 
                    var morn = new TimeSpan(6,0,0);
                    if (inputDate.TimeOfDay >= morn) {
                        outputDate = inputDate.AddDays(-2);
                    } else {
                        outputDate = inputDate.AddDays(5);
                    }
                    break;
                case 1: // Monday
                    outputDate = inputDate.AddDays(4);
                    break;
                case 2: // Tuesday
                    outputDate = inputDate.AddDays(3);
                    break;
                case 3: // Wednesday
                    outputDate = inputDate.AddDays(2);
                    break;
                case 4: // Thursday
                    outputDate = inputDate.AddDays(1);
                    break;
                case 5: // Friday
                    outputDate = inputDate;
                    break;
                case 6: // Saturday
                    outputDate = inputDate.AddDays(-1);
                    break;
            }
            return outputDate;
        }

        public static uint input() {
            string inputYearString = "";
            uint inputYearInt = 0;

            Console.WriteLine("Please enter a year between 0001 and 9999: "); //Asks user to input a string
            inputYearString = Console.ReadLine(); //Reads the line

            while (uint.TryParse(inputYearString, out inputYearInt) == false || inputYearInt < 1 || inputYearInt > 9999) //Requires re-input until the input is valid (ie it is an integer)
            {
                Console.WriteLine("Invalid year, please enter a valid year between 0001 and 9999: ");
                inputYearString = Console.ReadLine();
            }

            return inputYearInt;
        }

        public static double DegToRad(double degrees) //Converts a double from degrees to radians
        {
            double radians = degrees * (Math.PI / 180);
            return radians;
        }

        public static double NormalizeDeg(double degrees) //Normalizes a degree double value
        {
            double normalizedDegrees = degrees;
            if (degrees < 0) {
                normalizedDegrees += 360;
                return NormalizeDeg(normalizedDegrees);
            }
            return normalizedDegrees %= 360; ;
        }

        public static bool IsEquivalent(double num1, double num2) //Checks if two doubles are close enough to each other and close enough to being equivalent
        {
            if (Math.Abs(num2 - num1) <= phaseEqualityPrecision) {
                return true;
            } else {
                return false;
            }
        }

        public static DateTime getMatarikiDate(uint year) {
            DateTime j2000 = new DateTime(2000, 1, 1, 12, 0, 0); // The date of epoch J2000.0
            DateTime currentTimeUTC; // The internal date that is iterated through, in UTC
            DateTime currentTimeLocal; // The local date

            //A boolean recording whether the last phase was a full moon or a new moon; its initial assignment is arbitrary as the program will always iterate through at least one full or empty moon before reaching the beginning of the tangaroa lunar period
            bool previouslyFull = true;

            //Variables that are involved int the calculation of the illuminated fraction ofthe Moon´s disk
            double meanElongationMoon, meanAnomalyMoon, meanAnomalySun, timeJulianCenturies, lunarPhaseAngle, illuminatedFraction;

            currentTimeUTC = new DateTime((int)year, 1, 1, 0, 0, 0);//Sets the time to 00:00 of January the first on the target year.
            currentTimeLocal = currentTimeUTC.AddHours(timeZoneOffset);
            illuminatedFraction = 0.0;

            Console.WriteLine("Getting Date...");

            //Code will iterate until the last phase that was either a full or empty moon was a full moon, the current illuminated fraction is approximately equal to 0.5 and the date is suitable for matariki
            while (!(previouslyFull && IsEquivalent(illuminatedFraction, 0.5) && ((currentTimeLocal.Month == 6 && currentTimeLocal.Day >= 19) || currentTimeLocal.Month == 7))) {
                //Get the time since epoch J2000.0 in julian centuries
                timeJulianCenturies = (currentTimeUTC - j2000).TotalDays / 36525;

                //Obtain Mean elongation of the Moon, Mean anomaly of the Moon and Mean anomaly of the Sun using the given methods
                meanElongationMoon = 297.85019 + 445267.1114034 * timeJulianCenturies;
                meanAnomalyMoon = 134.96340 + 477198.8675055 * timeJulianCenturies;
                meanAnomalySun = 357.52911 + 35999.0502909 * timeJulianCenturies;

                //Normalize the obtained constants
                meanElongationMoon = NormalizeDeg(meanElongationMoon);
                meanAnomalyMoon = NormalizeDeg(meanAnomalyMoon);
                meanAnomalySun = NormalizeDeg(meanAnomalySun);

                //Calculate the lunar phase angle using the optained above constants
                lunarPhaseAngle = 180 - meanElongationMoon - 6.289 * Math.Sin(DegToRad(meanAnomalyMoon)) + 2.100 * Math.Sin(DegToRad(meanAnomalySun)) - 1.274 * Math.Sin(DegToRad(2 * meanElongationMoon - meanAnomalyMoon) - 0.658 * Math.Sin(2 * meanElongationMoon) - 0.214 * Math.Sin(DegToRad(2 * meanAnomalyMoon)) - 0.110 * Math.Sin(DegToRad(meanElongationMoon)));

                //Normalize the lunar phase angle
                lunarPhaseAngle = NormalizeDeg(lunarPhaseAngle);

                //Calculate the illuminated fraction of the moon using the lunar phase angle
                illuminatedFraction = (1 + Math.Cos(DegToRad(lunarPhaseAngle))) / 2;

                //If the illuminated fraction is equal to 1, then the previous full or new moon was full
                if (IsEquivalent(illuminatedFraction, 1)) {
                    previouslyFull = true;
                }

                //Likewise, if the illuminated fraction is equal to 0, then the previous full or new moon was new
                if (IsEquivalent(illuminatedFraction, 0)) {
                    previouslyFull = false;
                }

                //Increment the checked time by one minute
                currentTimeUTC = currentTimeUTC.AddMinutes(1);
                currentTimeLocal = currentTimeUTC.AddHours(timeZoneOffset);
            }
            //Offset time by 1 hour because of some unknown issue
            currentTimeLocal = currentTimeLocal.AddHours(-1);
            return currentTimeLocal;
        }
    }
}
