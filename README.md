# Online Auction System

This project is a microservice-based **Online Auction System** designed for real-time bidding and auction management. Each service handles a distinct functionality to ensure a seamless auction experience, from bid placement to payment and invoicing.

## Services

### 1. Room Service
- **Functionality**: Handles the creation of auctions, room management, and bid updates.
- **Communication**: Interacts with the `BiddingService` via NATS server to notify when a user enters an auction room or updates a bid.
- **Background Task**: Periodically communicates with the `NotificationService` to generate invoices for the highest bidder through the `InvoiceService`.

### 2. Bidding Service
- **Functionality**: Listens for events from the `RoomService` when users enter a room.
- **Notifications**: Sends updates to the `NotificationService` to notify all users about the highest bid and bidder.

### 3. Notification Service
- **Functionality**: Receives updates from `BiddingService` and sends notifications (emails) to all participants using Brevo.

### 4. Invoice Service
- **Functionality**: Generates invoices for the highest bidder after an auction concludes.
  
### 5. Payment Service
- **Integration**: Uses Paystack to process payments from the winning bidder after invoice generation.

### 6. Auth Service
- **Implementation**: Identity user authentication system to handle user login, registration, and management.

## Technologies Used
- **Microservice Communication**: NATS server for event-driven communication between services.
- **Payment Gateway**: Paystack for handling payments.
- **Emailing Service**: Brevo for sending bid and auction-related notifications.
- **ORM**: Entity Framework Core (EFCore) for database management and interactions.
- **ASP.NET Core**: Backend framework for building services.
- **Docker**: Containerization of services.

## Project Structure
This system is divided into multiple services, each responsible for a specific domain within the auction process:
- **RoomService**: Auction room management.
- **BiddingService**: Real-time bid processing.
- **NotificationService**: User notifications.
- **InvoiceService**: Invoice generation for auction winners.
- **PaymentService**: Payment processing.
- **AuthService**: User authentication.

## Running the Project
To run this project locally:
1. Clone the repository:
   ```bash
   git clone https://github.com/Ambaaq-Ajibike/Online-Auction-System
