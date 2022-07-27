# cz-matariki

## About

cz-matariki is a program that predicts when the Matariki holiday will occur on a given year. This was written for the 2022 Macleans College Hackathon

## Video
[Youtube](https://youtu.be/0jXzWPd-UzQ)

## Installation

Clone the repo and run the Program.cs file with your preffered c# compiler 

## Running the program

Compile and run using your compiler. 

## How it works

The program will request that you input a valid year (an integer between 1-9999) and will output its predictions for when the matariki holiday will occur on that year. 

## Method

The matariki advisory group made a [report](https://www.mbie.govt.nz/assets/matariki-dates-2022-to-2052-matariki-advisory-group.pdf) on its method for identifying the date for the matariki holiday. It chooses the nearest friday to the Tangaroa Lunar Period, a 4 day period that begins on the maori equivalent of the last quarter lunar phase on the 5th lunar month. Hence, any predictions should be made using this method. 

To obtain the times of the last quarter lunar phases we use the method outlined in the [report](https://www.academia.edu/42333239/Calculating_the_phase_of_the_Moon_Andr%C3%A9s_Mej%C3%ADa_Valencia) by Andrés Mejía Valencia. It treats the phase as a “illuminated fraction ofthe Moon´s disk” that is calculated using the lunar phase angle. 

The phase angle is calculated using the mean elongation of the moon, the mean anomaly of the moon and the mean anomaly of the sun as follows. 

Time is represented as julian centuries, a time period of 36525 days, since epoch J2000.0. 

After implementing this we obtain the first day of the period that is always four days. 

For cases where the first day lies between Monday and Thursday, days are incremented to reach the next Friday.
For cases where the first day lies on the Friday, the date remains the same.
For cases where the first day lies on a Saturday, the date is incremented by -1.
For cases where the first day lies on a Sunday, the date is either incremented or reduced based on the approximate calculated time of the beginning of the Tangaroa Period, depending on whether or not it is before 6AM NZST (approx sunrise).

## Contributors

[@capybaraslap](https://github.com/capybaraslap) - Wrote lunar angle identifier. 

[@quantified-null](https://github.com/quantified-null) - Wrote method for obtaining closest friday. 
