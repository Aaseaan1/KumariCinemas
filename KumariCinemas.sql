-- =====================================================
-- =     KUMARI CINEMAS - Movie Ticketing System.      =
-- =====================================================



-- ======================== 
--      TABLE CREATION.     
-- ======================== 

CREATE TABLE Users (
    UserID NUMBER PRIMARY KEY,
    Username VARCHAR2(50),
    Address VARCHAR2(100)
);

CREATE TABLE Movie (
    MovieID NUMBER PRIMARY KEY,
    Title VARCHAR2(100),
    Duration VARCHAR2(20),
    Language VARCHAR2(30),
    Genre VARCHAR2(30),
    ReleaseDate DATE
);

CREATE TABLE Theater_City_Hall (
    HallID NUMBER PRIMARY KEY,
    TheaterName VARCHAR2(100),
    TheaterCity VARCHAR2(50),
    HallCapacity NUMBER
);

CREATE TABLE Showtime (
    ShowTimeID NUMBER PRIMARY KEY,
    MovieID NUMBER NOT NULL,
    HallID NUMBER NOT NULL,
    ShowDate DATE,
    ShowTime VARCHAR2(20),
    TicketPrice NUMBER,
    CONSTRAINT fk_showtime_movie FOREIGN KEY (MovieID)
        REFERENCES Movie(MovieID),
    CONSTRAINT fk_showtime_hall FOREIGN KEY (HallID)
        REFERENCES Theater_City_Hall(HallID)
);

CREATE TABLE Ticket (
    TicketID NUMBER PRIMARY KEY,
    UserID NUMBER NOT NULL,
    ShowtimeID NUMBER NOT NULL,
    BookingStatus VARCHAR2(20),
    CONSTRAINT fk_ticket_user FOREIGN KEY (UserID)
        REFERENCES App_User(UserID),
    CONSTRAINT fk_ticket_showtime FOREIGN KEY (ShowtimeID)
        REFERENCES Showtime(ShowTimeID)
);

-- ======================== --
--    DATA INSERTION.       --
-- ======================== --

INSERT INTO App_User VALUES (1, 'Pratibha Gurung', 'Pokhara');


INSERT INTO Movie VALUES (
    1, 'Avatar: Fire and Ash', '3h', 'English', 'Fiction', DATE '2025-12-19'
);

INSERT INTO Theater_City_Hall VALUES (
    1, 'Kumari Cinemas', 'Pokhara Cineplex', 326
);

INSERT INTO Showtime VALUES (
    1, 1, 1, DATE '2025-12-21', 'Evening', 390
);

INSERT INTO Ticket VALUES (1, 1, 1, 'Booked');


-- ======================== --
--    DATA VERIFICATION.    --
-- ======================== --

SELECT * FROM Users;
SELECT * FROM Movie;
SELECT * FROM Theater_City_Hall;
SELECT * FROM Showtime;
SELECT * FROM Ticket;

