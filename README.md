## Overview

This application is a basic blog platform built with .NET 8.0 Razor Pages
based on the instructions described here [readme_instructions.md](readme_instructions.md) file.
It allows authors to write posts which are then displayed to all visitors. 
The application has been designed with scalability and maintainability in mind, 
delegating business logic to dedicated services.

## Features

- **User Management:** Create and manage author profiles.
- **Post Management:** Authors can create, edit, and delete posts.
- **Time-Aware Posts:** Utilizing `DateTimeOffset` to capture post times relative to different time zones.
- **Responsive Design:** Built to work seamlessly on desktops and mobile devices.
- Rate limiter feature to prefent band limiting attacks
- Caching News API to protect limit rating for API key (cost optimization), and for app performance

## Project Structure

- **Pages/**: Contains Razor Pages (.cshtml files) and their associated PageModel classes. This is where UI components and page-level logic reside.
- **wwwroot/**: Contains static files such as CSS, JavaScript, and images.
- **Models/**: Contains the data models (e.g., Post, Profile) used throughout the application.
- **Services/**: Contains the business logic and service classes that handle operations like user authentication, post creation, and validation.
- **Data/**: Contains data access logic and the database context for persistence.

## Business Logic

The core business logic of the application revolves around managing author profiles and blog posts. 

- **Post Handling:**  
  - Posts contain content, a timestamp (`DateTimeOffset`) to support multiple time zones, and are linked to an author.
  - Business rules ensure that posts are correctly created, edited, and displayed in proper order.

- **Profile Management:**  
  - Each author has a profile containing essential information (first name, last name, job title).
  - Additional computed properties, like the author's full name, are used to enhance display logic.

- **Service Layer:**  
  - All interactions between the user interface and the data layer are mediated by services. This encapsulation ensures that the Razor Pages remain clean and focused solely on presentation logic.
  - Error handling, input validation, and domain-specific rules are implemented within these services.

## Getting Started

1. **Prerequisites:**
   - .NET 8 SDK
   - Visual Studio 2022
  
2. **Build and Run:**
   - Open the solution in Visual Studio 2022.
   - Build the project.
   - Run the application and navigate to the default landing page.