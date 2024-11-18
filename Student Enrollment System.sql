create database student_enrollment_system

-- Create the Students Table
CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
	Gender NVARCHAR(10),
    DOB DATE,
   EnrollmentDate DATE
);

-- Create the Courses Table
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(100),
    Credits INT
);

-- Create the Enrollments Table (to associate Students with Courses)
CREATE TABLE StudentCourses (
    StudentID INT,
    CourseID INT,
    EnrollmentDate DATE,
    PRIMARY KEY (StudentId, CourseId),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT,
    Amount DECIMAL(10, 2),
    PaymentDate DATE,
    PaymentStatus NVARCHAR(50),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);

CREATE TABLE AuditLogs (
    LogId INT PRIMARY KEY IDENTITY(1,1),
    ActionType NVARCHAR(50),
    TableName NVARCHAR(50),
    RecordId INT,
    UserName NVARCHAR(50),
    Timestamp DATETIME
);




ALTER TABLE Payments
ADD  Status VARCHAR(255);

ALTER TABLE Payments
ADD PaymentMethod VARCHAR(255);




select *from Students;




ALTER TABLE Students
ADD CourseId INT;

--  to add a foreign key constraint
ALTER TABLE Students
ADD CONSTRAINT FK_Students_Courses FOREIGN KEY (CourseId)
REFERENCES Courses(CourseId);

--  to add the missing column ActionMessage to the AuditLogs table
ALTER TABLE AuditLogs
ADD ActionMessage NVARCHAR(255);  


