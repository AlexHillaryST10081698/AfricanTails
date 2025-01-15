African Tails Project
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Overview

This was a complete application aimed at the NGO to manage animal relevant data with ease. It has the functionality of recording, editing and managing information on which animals, their records, adoptions, foster details, etc. The system itself was built in WPF and SQLite for the frontend and backend with a very user friendly interface and has very powerful data managing features.

Features

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Frontend
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
User-Centered Design: Allows users to use it with minimal training.

Responsive UI: Adapts seamlessly to different screen sizes.

Consistent Theme: Used Resource Dictionary in App.xaml to keep the look and feel consistent all across the application.

Interactive DataGrid: Displays data and gives you options to edit, then delete records directly.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Backend
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SQLite Database: For simplicity and reliability, local storage for animal related data.

Database Handler Class: Provides methods for:

                        Inserting new records.

                        Retrieving and displaying data.

                        Updating and deleting records.

Normalization: Ensures data integrity and reduces redundancy.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Key Functionalities
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Animal Management

Adding Animals: We allow users to add new animals to the database including the name, breed and attributes such as color, sex, the microchip ID, status and dates (date of birth, adoption etc).

Editing Animals: An edit window is dedicated to modify existing records.

Deleting Animals: Remove records with confirmation prompts to prevent accidental deletions.

Adoptions and Fostering

Adoption Details: Record and manage adoption details for animals.

Fostering Details: Record and manage fostering details.

Filtering and Sorting: Data can be sorted alphabetically or by other criteria.

Dashboard

Animal Statistics: Display key animal statistics and related data.

Search Functionality: Easily locate records with a powerful search bar.

Data Management: Seamlessly integrates data from multiple database tables.
