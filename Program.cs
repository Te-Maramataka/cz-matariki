using System;

namespace Matariki {

    class Matariki {
        public static void Main() {
            uint year = input(); // input method asks for input then checks validity of input.
            var date = new DateTime(2022,7,23);
            var closestFriday = new DateTime();
            closestFriday = findFriday(date); // findFriday method returns the closest Friday.
            Console.WriteLine(closestFriday);
            Console.WriteLine("Year: " + year);
        }
        public static DateTime findFriday(DateTime inputDate) {
            
            var outputDate = new DateTime(); //Creates the output date variable for the friday.         
            int dayOfWeek = (int)inputDate.DayOfWeek; //Converts the first day of the four day period into a day of the week
            
            switch (dayOfWeek) {
                case 0: //Sunday 
                    outputDate = inputDate.AddDays(-2);
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

        public static uint input(){
            
            string inputYearString = "";
            uint inputYearInt = 0;

            Console.WriteLine("Please enter a year between 0001 and 9999: ");
            inputYearString = Console.ReadLine();

            while (uint.TryParse(inputYearString, out inputYearInt) == false || inputYearInt < 1 || inputYearInt > 9999) {
                Console.WriteLine("Invalid year, please enter a valid year between 0001 and 9999: ");
                inputYearString = Console.ReadLine();
            }

            return inputYearInt;
        }
    }
}
