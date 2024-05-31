# Car Simulator

## Overview

Car Simulator is a console-based simulation that allows users to interactively control a car and driver in various simulated activities. 

The project is built using C# 12, .NET 8.0 and demonstrates several key software engineering principles and design patterns, including SOLID, DRY, and Separation of Concerns (SoC).

The application uses MS Tests and NUnit for unit and integration testing different scenarios such as:
* Fatigue
* Fuel
* Direction
* Menu choices
* Integration test for API
* Hunger (Tests added but feature not implemented)

## Features

* Autofac for Dependency Injection

* Fetch Driver Details: Retrieve a random driver from an API.

* Enter Car Details: Select a car brand and driving direction.

* Interactive Car Control: Drive, turn, refuel, and rest the driver.

* Display Menus: Provide user-friendly menu interfaces for interaction.

## Architecture and Design Principles

### SOLID Principles

#### Single Responsibility Principle (SRP):

* Each service class (MainMenuService, RandomUserService, CarService, etc.) has a single responsibility.

* The ActionMenu and MainMenu classes handle user interactions and delegate tasks to respective services.

#### Open/Closed Principle (OCP):

* Services and interfaces can be extended with additional functionalities without modifying existing code.

* New services can be added without changing the existing structure.

#### Liskov Substitution Principle (LSP):

* The application relies on interfaces for dependency injection, ensuring that derived classes can be used interchangeably.

#### Interface Segregation Principle (ISP):

* Interfaces are specific to the functionality they provide (ICarService, IDriverService, IFoodService, etc.), ensuring clients only need to implement methods they use.

#### Dependency Inversion Principle (DIP):

* High-level modules depend on abstractions (IRandomUserService, IMainMenuService, etc.) rather than concrete implementations.

### DRY (Don't Repeat Yourself)

* Reusable services and interfaces are used across the application to prevent code duplication.

* Common functionalities are encapsulated within service classes.

### Separation of Concerns (SoC)

Clear separation between different concerns:

* Data Retrieval: RandomUserService handles data fetching from the API.

* User Interaction: MainMenu, ActionMenu manage user input and display.

* Business Logic: Individual services (CarService, DriverService, etc.) handle specific business logic.

## Dependencies

The application utilizes the following models, enums, and services:

### Enums

* CarBrand

* Direction

* Fatigue

* Fuel

* Hunger

### Models

* Car

* Driver

* Name

* RandomUserResponse

* Result

## Services and Interfaces

* ICarService: Manages car operations (drive, turn, get status).

* IDriverService: Manages driver-related operations (rest, check fatigue).

* IFoodService: Manages food-related operations (eat, check hunger).

* IFuelService: Manages fuel-related operations (refuel).

* IInputService: Manages user input.

* IMainMenuService: Manages main menu operations (fetch driver details, enter car details).

* IMenuDisplayService: Manages display operations for different menus.

* IRandomUserService: Fetches random driver details from an API.

* IActionService: Manages execution of menu actions.

* IActionServiceFactory: Creates instances of IActionService.

* IConsoleService: Manages console operations (write, read, clear, etc.).

* ISimulationSetupService: Manages simulation setup operations (fetch driver details, enter car details).

## Usage

### AppStart: Entry point of the application.

* Initializes required services and dependencies.

* Starts the main menu.

### MainMenu: Handles the main menu interactions.

* Fetches driver details.

* Enters car details.

* Transitions to ActionMenu.

### ActionMenu: Handles the action menu interactions.

* Executes various actions such as driving, turning, refueling, and resting.

* Displays the status menu after each action.

## Running the Application

### Setup and Initialization:

* Clone the repository or download the source code.

* Open the solution in Visual Studio.

* Restore the NuGet packages.

### Run the Application:

* Set CarSimulator as the startup project.

* Press F5 or click on Start to run the application.

* Follow the on-screen instructions to interact with the simulation.

## Conclusion
* Car Simulator 2.0 is a comprehensive example of a console application that adheres to key software engineering principles and design patterns.

* By following SOLID, DRY, and SoC principles, the application ensures maintainability, scalability, and ease of testing.

* The use of interfaces and dependency injection further enhances the flexibility and modularity of the codebase.
