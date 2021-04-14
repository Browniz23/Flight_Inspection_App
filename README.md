# Flight_Inspection_App

This is the first milestone of our 'Advanced Programming 2' course. This app enables the user to visualize flight data using a flight simulator and analyze it.

![FlightGearApp GUI](https://github.com/Browniz23/Flight_Inspection_App/blob/0f2cb39bb84fe93ac7fced8bc744d17682814ae4/Flight_Inspection_App/images/example_flight.jpg)

One main achievement of this task is to use the MVVM architecture. Therefore, the project can be divided into three main parts:

**Model** - The 'mind' behind the program. This part is responsible for making all calculations and keeping the data.

**View** - This part is in charge of representing the data to the user.

**View Model** - The link between the other two parts. One main idea of adding this part is that the **View** and the **Model** don't have to know each other and can be programmed independently.

## Folder Structure

Inside our main folder you can find 2 important folders:

Controls - Which contains the **View** part.

ViewModel - In which the **View Model** part sits.

Outside the folders you can see the **Model** part.

## Running

In order to run the project you should download FlightGear simulator and clone the project to VisualStudio 2019.
Then you should open the FlightGear app with the following settings:
![FlightGear settings](https://raw.githubusercontent.com/Browniz23/Flight_Inspection_App/master/Flight_Inspection_App/images/Additional%20Settings.jpg)

Now you can run the program on VS.

## Links

FlightGear simulator - https://www.flightgear.org/

UML of the main classes in the program - https://github.com/Browniz23/Flight_Inspection_App/blob/master/Flight_Inspection_App/images/FlightgearProject.pdf

Short video presenting the progaram - https://youtu.be/Uw_x4Johu7A


## Last Important Notes:

- In CSV files the first row should always be the titles of the chunks.
- DLL files should be in c#, with a class named 'anommalies'. The class should have to methods: 'detect' and 'draw'.
